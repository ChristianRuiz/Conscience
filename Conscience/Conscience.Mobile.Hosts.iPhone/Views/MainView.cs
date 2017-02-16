using System;
using Conscience.Mobile.Hosts.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace Conscience.Mobile.Hosts.iPhone
{
	public partial class MainView : MvxViewController<MainViewModel>
	{
		public MainView() : base("MainView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.CreateBinding(LblLongitude).To((MainViewModel vm) => vm.Longitude).Apply();
			this.CreateBinding(LblLatitude).To((MainViewModel vm) => vm.Latitude).Apply();
			this.CreateBinding(LblAccuracy).To((MainViewModel vm) => vm.Accuracy).Apply();
			this.CreateBinding(LblError).To((MainViewModel vm) => vm.Error).Apply();
			this.CreateBinding(LblBatteryStatus).To((MainViewModel vm) => vm.BatteryStatus).Apply();
			this.CreateBinding(LblBatteryLevel).To((MainViewModel vm) => vm.BatteryLevel).Apply();

			this.CreateBinding(BtnPlayNumber).To((MainViewModel vm) => vm.PlayRandomNumberCommand).Apply();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

