using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PageNavigator.Business
{
	public static class ModuleControllerSchedule
	{
		///// <summary>
		///// opened controllers module name and origin title map, same module will be add only once.
		///// change current module title such as Module(1), Module(2).
		///// when module closed, keep its module name for cache.
		///// </summary>
		//private static Dictionary<string, string> moduleNameTitleMap = new Dictionary<string, string>();

		private static ObservableCollection<ModuleControllerBase> openedControllerList = new ObservableCollection<ModuleControllerBase>();
		internal static ObservableCollection<ModuleControllerBase> OpenedModuleControllerList
		{
			get { return openedControllerList; }
		}

		private static ConcurrentDictionary<Model.ModuleData, List<ModuleControllerBase>> moduleControllerMap =
			new ConcurrentDictionary<Model.ModuleData, List<ModuleControllerBase>>();
		/// <summary>
		/// controller to module indexes. when controller closed, search module here
		/// </summary>
		private static ConcurrentDictionary<ModuleControllerBase, Model.ModuleData> controllerModuleMap =
			new ConcurrentDictionary<ModuleControllerBase, Model.ModuleData>();

		private static int PinnedCount
		{
			get { return openedControllerList.Count(item => item.IsPinned); }
		}

		///// <summary>
		///// Home page count must be 0 or 1.
		///// </summary>
		//private static int homePageCount = 0;

		/// <summary>
		/// indicate whether home page has been created
		/// </summary>
		private static bool homePageCreated = false;

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
		/// find Controller bound to Module, then open its first page, and handle close event
		/// </summary>
		public static void Open(Model.ModuleData module)
		{
			//get controller instance
			ModuleControllerBase controller = moduleControllerMapping(module);
			if (controller == null)
				throw new InvalidOperationException("module controller can not be null.");

			if (controller.IsSingleMode)
			{
				List<ModuleControllerBase> controllers;
				if (moduleControllerMap.TryGetValue(module, out controllers))
				{
					var singleControllerInstance = controllers.SingleOrDefault();
					if (singleControllerInstance != null)
					{
						NavigateTo(singleControllerInstance);
						return;
					}
				}
			}
			//HomePage is Single too
			if (controller.IsHomePage)
			{
				if (homePageCreated)
					throw new InvalidOperationException("only can set one Home page.");
				homePageCreated = true;
				//homePageCount++;
				//if (homePageCount > 1)
				//	throw new InvalidOperationException("only can set one Home page.");
			}

			createModule(module, controller);
			currentModuleController = controller;
			updateControllerActivateState();
		}

		private static void createModule(Model.ModuleData module, ModuleControllerBase controller)
		{
			controller.Closed += onModuleClosed;

			var moduleControllers = moduleControllerMap.GetOrAdd(module, new List<ModuleControllerBase>());
			moduleControllers.Add(controller);
			openedControllerList.Insert(PinnedCount, controller);
			controllerModuleMap.TryAdd(controller, module);

			updateOpenedModuleTitle(module);
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

			Model.ModuleData module;
			if (!controllerModuleMap.TryGetValue(controller, out module))
				throw new InvalidOperationException("cannot find module data by controller");

			List<ModuleControllerBase> controllers;
			if (!moduleControllerMap.TryGetValue(module, out controllers))
				throw new InvalidOperationException("cannot find controller by module");
			controllers.Remove(controller);
			updateOpenedModuleTitle(module);
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

		private static void updateOpenedModuleTitle(Model.ModuleData module)
		{
			List<ModuleControllerBase> moduleControllers;
			if (moduleControllerMap.TryGetValue(module, out moduleControllers))
			{
				if (moduleControllers.Count > 0)
				{
					int index = 0;
					string moduleTitle = module.Title;
					foreach (var controller in moduleControllers.OrderBy(item => item.CreatedTime))
					{
						if (index++ > 0)
							controller.Header = string.Format("{0}({1})", moduleTitle, index);
						else
							controller.Header = moduleTitle;
					}
				}
			}
		}
	}
}
