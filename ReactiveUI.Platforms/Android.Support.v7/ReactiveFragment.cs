using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Splat;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace ReactiveUI.Android
{

    /// <summary>
    /// This is a SherlockFragment that is both an ReactiveSupportFragment and has ReactiveObject powers 
    /// (i.e. you can call RaiseAndSetIfChanged)
    /// </summary>
    public class ReactiveFragment<TViewModel> : ReactiveFragment, IViewFor<TViewModel>, ICanActivate
        where TViewModel : class, IReactiveNotifyPropertyChanged
    {
        protected ReactiveFragment() { }

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
    /// This is a SherlockFragment that is both an ReactiveSupportFragment and has ReactiveObject powers 
    /// (i.e. you can call RaiseAndSetIfChanged)
    /// </summary>
    public class ReactiveFragment : SupportFragment, IReactiveNotifyPropertyChanged, IReactiveObjectExtension, IHandleObservableErrors
    {
        protected ReactiveFragment() 
        {
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


        public override void OnPause()
        {
            base.OnPause();
            deactivated.OnNext(Unit.Default);
        }

        public override void OnDetach()
        {
            base.OnDetach();
            deactivated.OnNext(Unit.Default);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            activated.OnNext(Unit.Default);
        }

        public override void OnResume()
        {
            base.OnResume();
            activated.OnNext(Unit.Default);
        }

        
    }
}