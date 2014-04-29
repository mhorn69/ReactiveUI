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
            return MonoTouch.UIKit.UIDevice.CurrentDevice.Orientation.ToString();
#else
            return null;
#endif
        }

        public static DeviceOrientation GetOrientationEnum()
        {
#if UIKIT
            switch (MonoTouch.UIKit.UIDevice.CurrentDevice.Orientation) 
            {
                case UIDeviceOrientation.Portrait:
                case UIDeviceOrientation.PortraitUpsideDown:
                case UIDeviceOrientation.FaceUp:
                case UIDeviceOrientation.FaceDown:
                    return DeviceOrientation.Portrait;
                case UIDeviceOrientation.LandscapeLeft:
                case UIDeviceOrientation.LandscapeRight:
                    return DeviceOrientation.Landscape;
                default :
                    return DeviceOrientation.None;
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

