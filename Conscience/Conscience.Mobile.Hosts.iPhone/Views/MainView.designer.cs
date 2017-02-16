// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Conscience.Mobile.Hosts.iPhone
{
    [Register ("MainView")]
    partial class MainView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BtnPlayNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblAccuracy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBatteryLevel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblBatteryStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblError { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblLatitude { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LblLongitude { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BtnPlayNumber != null) {
                BtnPlayNumber.Dispose ();
                BtnPlayNumber = null;
            }

            if (LblAccuracy != null) {
                LblAccuracy.Dispose ();
                LblAccuracy = null;
            }

            if (LblBatteryLevel != null) {
                LblBatteryLevel.Dispose ();
                LblBatteryLevel = null;
            }

            if (LblBatteryStatus != null) {
                LblBatteryStatus.Dispose ();
                LblBatteryStatus = null;
            }

            if (LblError != null) {
                LblError.Dispose ();
                LblError = null;
            }

            if (LblLatitude != null) {
                LblLatitude.Dispose ();
                LblLatitude = null;
            }

            if (LblLongitude != null) {
                LblLongitude.Dispose ();
                LblLongitude = null;
            }
        }
    }
}