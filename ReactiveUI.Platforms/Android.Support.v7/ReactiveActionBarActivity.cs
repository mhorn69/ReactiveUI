using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Splat;

namespace ReactiveUI.Android
{
    /// <summary>
    /// This is an SherlockFragmentActivity that is both an ActionBarActivity and has ReactiveObject powers 
    /// (i.e. you can call RaiseAndSetIfChanged)
    /// </summary>
    public class ReactiveActionBarActivity<TViewModel> : ReactiveActionBarActivity, IViewFor<TViewModel>, ICanActivate
        where TViewModel : class, IReactiveNotifyPropertyChanged
    {
        protected ReactiveActionBarActivity() { }

        TViewModel _ViewModel;
        public TViewModel ViewModel
        {
            get { return _ViewModel; }
            set { this.RaiseAndSetIfChanged(ref _ViewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = (TViewModel)value; }
        }
    }

    /// <summary>
    /// This is an SherlockFragmentActivity that is both an ActionBarActivity and has ReactiveObject powers 
    /// (i.e. you can call RaiseAndSetIfChanged)
    /// </summary>
    public class ReactiveActionBarActivity : ActionBarActivity, IReactiveObjectExtension, IReactiveNotifyPropertyChanged, IHandleObservableErrors
    {
        protected ReactiveActionBarActivity() 
        {
            RxApp.MainThreadScheduler = new WaitForDispatcherScheduler(() => new AndroidUIScheduler(this));
            this.setupReactiveExtension();
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void IReactiveObjectExtension.RaisePropertyChanging(PropertyChangingEventArgs args) 
        {
            var handler = PropertyChanging;
            if (handler != null) {
                handler(this, args);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void IReactiveObjectExtension.RaisePropertyChanged(PropertyChangedEventArgs args) 
        {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, args);
            }
        }

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be changed.         
        /// </summary>
        public IObservable<IObservedChange<object, object>> Changing {
            get { return this.getChangingObservable(); }
        }

        /// <summary>
        /// Represents an Observable that fires *after* a property has changed.
        /// </summary>
        public IObservable<IObservedChange<object, object>> Changed {
            get { return this.getChangedObservable(); }
        }

        /// <summary>
        /// When this method is called, an object will not fire change
        /// notifications (neither traditional nor Observable notifications)
        /// until the return value is disposed.
        /// </summary>
        /// <returns>An object that, when disposed, reenables change
        /// notifications.</returns>
        public IDisposable SuppressChangeNotifications()
        {
            return this.SuppressChangeNotifications();
        }

        public IObservable<Exception> ThrownExceptions { get { return this.getThrownExceptionsObservable(); } }

        readonly Subject<Unit> activated = new Subject<Unit>();
        public IObservable<Unit> Activated { get { return activated; } }

        readonly Subject<Unit> deactivated = new Subject<Unit>();
        public IObservable<Unit> Deactivated { get { return deactivated; } }

        protected override void OnPause()
        {
            base.OnPause();
            deactivated.OnNext(Unit.Default);
        }
                
        protected override void OnResume()
        {
            base.OnResume();
            activated.OnNext(Unit.Default);
        }

        readonly Subject<Tuple<int, Result, Intent>> activityResult = new Subject<Tuple<int, Result, Intent>>();
        public IObservable<Tuple<int, Result, Intent>> ActivityResult { 
            get { return activityResult; }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            activityResult.OnNext(Tuple.Create(requestCode, resultCode, data));
        }

        public Task<Tuple<Result, Intent>> StartActivityForResultAsync(Intent intent, int requestCode)
        {
            // NB: It's important that we set up the subscription *before* we
            // call ActivityForResult
            var ret = ActivityResult
                .Where(x => x.Item1 == requestCode)
                .Select(x => Tuple.Create(x.Item2, x.Item3))
                .FirstAsync()
                .ToTask();

            StartActivityForResult(intent, requestCode);
            return ret;
        }
                
        public Task<Tuple<Result, Intent>> StartActivityForResultAsync(Type type, int requestCode)
        {
            // NB: It's important that we set up the subscription *before* we
            // call ActivityForResult
            var ret = ActivityResult
                .Where(x => x.Item1 == requestCode)
                .Select(x => Tuple.Create(x.Item2, x.Item3))
                .FirstAsync()
                .ToTask();

            StartActivityForResult(type, requestCode);
            return ret;
        }

    }
}