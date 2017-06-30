using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PageNavigator.Business
{
	public static class ModuleControllerSchedule
	{
		/// <summary>
		/// opened controllers module name and origin title map, same module will be add only once.
		/// change current module title such as Module(1), Module(2).
		/// when module closed, keep its module name for cache.
		/// </summary>
		private static Dictionary<string, string> moduleNameTitleMap = new Dictionary<string, string>();

		private static ObservableCollection<ModuleControllerBase> openedControllerList = new ObservableCollection<ModuleControllerBase>();
		internal static ObservableCollection<ModuleControllerBase> OpenedModuleControllerList
		{
			get { return openedControllerList; }
		}

		internal static int PinnedCount
		{
			get
			{
				return openedControllerList.Count(item => item.IsPinned);
			}
		}

		/// <summary>
		/// Home page count must be 0 or 1.
		/// </summary>
		private static int homePageCount = 0;

		/// <summary>
		/// get controller by module
		/// </summary>
		private static Func<Model.ModuleData, ModuleControllerBase> moduleControllerMapping;

		public static void SetModuleControllerMapping(Func<Model.ModuleData, ModuleControllerBase> mapping)
		{
			if (mapping == null)
				throw new ArgumentNullException("moduleControllerMapping");

			moduleControllerMapping = mapping;
		}

		private static ModuleControllerBase currentModuleController;
		internal static ModuleControllerBase CurrentModuleController
		{
			get { return currentModuleController; }
		}

		/// <summary>
		/// Binding Controller to Module, then open its first page, and handle close event
		/// </summary>
		public static void CreateModule(Model.ModuleData module)
		{
			//use new module not reference
			Model.ModuleData moduleCopy = Model.ModuleData.Copy(module);
			//get controller instance
			ModuleControllerBase controller = moduleControllerMapping(moduleCopy);
			if (controller == null)
			{
				throw new InvalidOperationException("module controller can not be null.");
			}
			if (controller.IsHomePage)
			{
				homePageCount++;
				if (homePageCount > 1)
				{
					throw new InvalidOperationException("only can set one Home page.");
				}
			}
			if (controller.IsSingleMode)
			{
				var singleControllerInstance = openedControllerList
					.FirstOrDefault(item => item.Module.Equals(module));
				if (singleControllerInstance != null)
				{
					NavigateTo(singleControllerInstance);
					return;
				}
			}

			createModule(controller);
			currentModuleController = controller;
			updateControllerActivateState();
		}

		private static void createModule(ModuleControllerBase controller)
		{
			if (!moduleNameTitleMap.ContainsKey(controller.Module.Name))
			{
				moduleNameTitleMap.Add(controller.Module.Name, controller.Module.Title);
			}
			controller.Closed += onModuleClosed;
			openedControllerList.Insert(PinnedCount, controller);
			updateOpenedModuleTitle(controller.Module.Name);
			controller.Create();
		}

		internal static void NavigateTo(Business.ModuleControllerBase moduleController)
		{
			if (openedControllerList.Contains(moduleController))
			{
				currentModuleController = moduleController;
				updateControllerActivateState();
			}
		}

		internal static void UpdatePinnedState(Business.ModuleControllerBase moduleController)
		{
			if (openedControllerList.Contains(moduleController))
			{
				int pinnedCount = PinnedCount;
				int currentIndex = openedControllerList.IndexOf(moduleController);
				if (moduleController.IsPinned)
				{
					openedControllerList.Move(currentIndex, pinnedCount - 1);
					moduleController.IsPinned = false;
				}
				else
				{
					openedControllerList.Move(currentIndex, pinnedCount);
					moduleController.IsPinned = true;
				}
			}
		}

		internal static void Close(Business.ModuleControllerBase moduleController)
		{
			if (openedControllerList.Contains(moduleController))
			{
				moduleController.Close();
				var nextController = openedControllerList.LastOrDefault();
				if (nextController != null)
				{
					nextController.IsActivated = true;
				}
				currentModuleController = nextController;
			}
		}

		private static void onModuleClosed(object sender, EventArgs e)
		{
			var controller = (ModuleControllerBase)sender;
			controller.Closed -= onModuleClosed;
			openedControllerList.Remove(controller);
			updateOpenedModuleTitle(controller.Module.Name);
		}

		private static void updateControllerActivateState()
		{
			if (currentModuleController != null)
			{
				foreach (var controller in openedControllerList)
				{
					controller.IsActivated = controller == currentModuleController;
				}
			}
		}

		private static void updateOpenedModuleTitle(string moduleName)
		{
			var controllers = openedControllerList
					   .Where(item => item.Module.Name == moduleName)
					   .OrderBy(item => item.CreatedDateTime)
					   .ToList();
			if (controllers.Count > 0)
			{
				string originTitle = moduleNameTitleMap[moduleName];
				controllers[0].Module.Title = originTitle;
				if (controllers.Count > 1)
				{
					for (int index = 1; index < controllers.Count; index++)
					{
						controllers[index].Module.Title = string.Format("{0}({1})", originTitle, index);
					}
				}
			}
		}
	}
}
