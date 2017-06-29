using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PageNavigator.ViewModel
{
	internal class MenuContainerViewModel : Common.ViewModelBase
	{
		private static MenuContainerViewModel instance = new MenuContainerViewModel();
		public static MenuContainerViewModel Instance
		{
			get { return instance; }
		}

		protected MenuContainerViewModel()
		{
			this.openModule = new Common.RelayCommand(this.onOpenModule);
		}

		private ObservableCollection<Model.ModuleData> moduleSets = new ObservableCollection<Model.ModuleData>();
		public ObservableCollection<Model.ModuleData> ModuleSets
		{
			get { return this.moduleSets; }
		}

		private Common.RelayCommand openModule;
		public Common.RelayCommand OpenModule
		{
			get { return this.openModule; }
		}

		public event EventHandler ModuleOpened;

		private void onOpenModule(object obj)
		{
			Model.ModuleData module = obj as Model.ModuleData;
			if (this.ModuleOpened != null)
			{
				this.ModuleOpened(module, EventArgs.Empty);
			}
		}
	}
}
