using System;
using Android.App;
using Android.Content;
using Android.Views;

namespace ReactiveUI.Android
{
    public class PlatformOperations : IPlatformOperations
    {
        public string GetOrientation()
        {
            var wm = Application.Context.GetSystemService(Context.WindowService) as IWindowManager;
            if (wm == null) return null;

            var disp = wm.DefaultDisplay;
            if (disp == null) return null;

            return disp.Rotation.ToString();
        }

        public DeviceOrientation GetOrientationEnum()
        {
            var wm = Application.Context.GetSystemService(Context.WindowService) as IWindowManager;
            if (wm == null) return DeviceOrientation.None;

            var disp = wm.DefaultDisplay;
            if (disp == null) return DeviceOrientation.None;

            if (disp.Orientation == 1)
            {
                return DeviceOrientation.Portrait;
            }
            if (disp.Orientation == 2)
            {
                return DeviceOrientation.Landscape;
            }
            return DeviceOrientation.None;
        }
    }
}

