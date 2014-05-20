using System;
using MonoTouch.UIKit;

namespace ReactiveUI.Cocoa
{
    /// <summary>
    /// Returns the current orientation of the device on iOS.
    /// </summary>
    public class PlatformOperations : IPlatformOperations
    {
        public static string GetOrientation()
        {
#if UIKIT
            //return MonoTouch.UIKit.UIDevice.CurrentDevice.Orientation.ToString();
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
            switch (orientation)
            {
                case UIInterfaceOrientation.LandscapeLeft :
                    return "LandscapeLeft";

                case UIInterfaceOrientation.LandscapeRight :
                    return "LandscapeRight";

                case UIInterfaceOrientation.Portrait :
                    return "Portrait";

                case UIInterfaceOrientation.PortraitUpsideDown :
                    return "PortraitUpsideDown";

                default :
                    return "Portrait";
            }

#else
            return null;
#endif
        }

        public static DeviceOrientation GetOrientationEnum()
        {
#if UIKIT
            //switch (MonoTouch.UIKit.UIDevice.CurrentDevice.Orientation) 
            //{
            //    case UIDeviceOrientation.Portrait:
            //    case UIDeviceOrientation.PortraitUpsideDown:
            //    case UIDeviceOrientation.FaceUp:
            //    case UIDeviceOrientation.FaceDown:
            //        return DeviceOrientation.Portrait;
            //    case UIDeviceOrientation.LandscapeLeft:
            //    case UIDeviceOrientation.LandscapeRight:
            //        return DeviceOrientation.Landscape;
            //    default :
            //        return DeviceOrientation.None;
            //}
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
            switch (orientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return DeviceOrientation.Landscape;

                case UIInterfaceOrientation.LandscapeRight:
                    return DeviceOrientation.Landscape;

                case UIInterfaceOrientation.Portrait:
                    return DeviceOrientation.Portrait;

                case UIInterfaceOrientation.PortraitUpsideDown:
                    return DeviceOrientation.Portrait;

                default:
                    return DeviceOrientation.Portrait;
            }
#else
            return DeviceOrientation.None;
#endif
            
        }

        DeviceOrientation IPlatformOperations.GetOrientationEnum()
        {
            return GetOrientationEnum();
        }

        string IPlatformOperations.GetOrientation()
        {
            return GetOrientation();
        }
    }
}

