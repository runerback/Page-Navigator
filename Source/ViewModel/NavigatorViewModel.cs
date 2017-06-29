﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigator.ViewModel
{
	internal class NavigatorViewModel : Common.ViewModelBase
	{
		private static NavigatorViewModel instance = new NavigatorViewModel();
		public static NavigatorViewModel Instance
		{
			get { return instance; }
		}

		protected NavigatorViewModel()
		{
			this.navigateBack = new Common.RelayCommand(this.onNavigateBack, this.canNavigateBack);
			this.navigateForward = new Common.RelayCommand(this.onNavigateForward, this.canNavigateForward);
		}

		#region naviage back
		private Common.RelayCommand navigateBack;
		public Common.RelayCommand NavigateBack
		{
			get { return this.navigateBack; }
		}

		private bool canNavigateBack(object obj)
		{
			var controller = TabContainerViewModel.Instance.CurrentController;
			if (controller != null)
			{
				return controller.CanNavigateBack;
			}
			return false;
		}

		private void onNavigateBack(object obj)
		{
			var controller = TabContainerViewModel.Instance.CurrentController;
			if (controller != null)
			{
				controller.NavigateBack();
				TabContainerViewModel.Instance.OnPropertyChanged("CurrentController");
			}
		}
		#endregion navigate back

		#region naviage forward
		private Common.RelayCommand navigateForward;
		public Common.RelayCommand NavigateForward
		{
			get { return this.navigateForward; }
		}

		private bool canNavigateForward(object obj)
		{
			var controller = TabContainerViewModel.Instance.CurrentController;
			if (controller != null)
			{
				return controller.CanNavigateForward;
			}
			return false;
		}

		private void onNavigateForward(object obj)
		{
			var controller = TabContainerViewModel.Instance.CurrentController;
			if (controller != null)
			{
				controller.NavigateForward();
				TabContainerViewModel.Instance.OnPropertyChanged("CurrentController");
			}
		}
		#endregion navigate forward
	}
}
