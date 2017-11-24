using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PageNavigator.ViewModel
{
	internal class TabContainerViewModel : Common.ViewModelBase
	{
		protected TabContainerViewModel()
		{
			MenuContainerViewModel.Instance.ModuleOpened += this.onModuleOpened;

			this.navigateToModule = new Common.RelayCommand(this.onNavigateToModule);
			this.pinModule = new Common.RelayCommand(this.onPinModule);
			this.closeModule = new Common.RelayCommand(this.onCloseModule);
		}

		private static TabContainerViewModel instance = new TabContainerViewModel();
		public static TabContainerViewModel Instance
		{
			get { return instance; }
		}

		public ObservableCollection<Business.ModuleControllerBase> ModuleControllers
		{
			get { return Business.ModuleControllerSchedule.OpenedModuleControllerList; }
		}

		private void onModuleOpened(object sender, EventArgs e)
		{
			var module = sender as Model.ModuleData;
			if (module != null)
			{
				Business.ModuleControllerSchedule.Open(module);
				this.OnPropertyChanged("ModuleControllers");
				this.OnPropertyChanged("CurrentController");
			}
		}

		public Business.ModuleControllerBase CurrentController
		{
			get { return Business.ModuleControllerSchedule.CurrentModuleController; }
		}

		#region navigate to
		private Common.RelayCommand navigateToModule;
		public Common.RelayCommand NavigateToModule
		{
			get { return this.navigateToModule; }
		}

		private void onNavigateToModule(object obj)
		{
			var controller = obj as Business.ModuleControllerBase;
			if (controller != null)
			{
				Business.ModuleControllerSchedule.NavigateTo(controller);
				this.OnPropertyChanged("CurrentController");
			}
		}
		#endregion navigate to

		#region pin module
		private Common.RelayCommand pinModule;
		public Common.RelayCommand PinModule
		{
			get { return this.pinModule; }
		}

		private void onPinModule(object obj)
		{
			var controller = obj as Business.ModuleControllerBase;
			if (controller != null)
			{
				Business.ModuleControllerSchedule.UpdatePinnedState(controller);
				this.OnPropertyChanged("ModuleControllers");
			}
		}
		#endregion pin module

		#region close module
		private Common.RelayCommand closeModule;
		public Common.RelayCommand CloseModule
		{
			get { return this.closeModule; }
		}

		private void onCloseModule(object obj)
		{
			var controller = obj as Business.ModuleControllerBase;
			if (controller != null)
			{
				Business.ModuleControllerSchedule.Close(controller);
				this.OnPropertyChanged("ModuleControllers");
				this.OnPropertyChanged("CurrentController");
			}
		}
		#endregion close module

	}
}
