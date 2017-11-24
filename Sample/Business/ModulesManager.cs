using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Business
{
	public static class ModulesManager
	{
		private static bool initialized;
		public static bool Initialized
		{
			get { return initialized; }
		}

		public static void Initial()
		{
			if (!initialized)
			{
				PageNavigator.Business.ModulesManager.Load();
				PageNavigator.Business.ModuleControllerSchedule.SetModuleControllerMapping(moduleControllerMapping);

				Register("HomeModule", () => new Modules.Home.HomeModuleController());
				Register("ModuleAPart1", () => new Modules.A.ModuleAPart1Controller());
				Register("ModuleAPart2", () => new Modules.A.ModuleAPart2Controller());

				PageNavigator.Business.ModuleControllerSchedule.Open(
					new PageNavigator.Model.ModuleData("HomeModule", "Home"));
				initialized = true;
			}
		}

		private static PageNavigator.Business.ModuleControllerBase moduleControllerMapping(PageNavigator.Model.ModuleData module)
		{
			Func<PageNavigator.Business.ModuleControllerBase> controllerSelector;
			if (!moduleControllerMap.TryGetValue(module.Name, out controllerSelector))
				throw new InvalidOperationException(
					string.Format("not support for Module \"{0}\"", module.Name));
			return controllerSelector();
		}

		private static ConcurrentDictionary<string, Func<PageNavigator.Business.ModuleControllerBase>> moduleControllerMap =
			new ConcurrentDictionary<string, Func<PageNavigator.Business.ModuleControllerBase>>();

		private static void Register(string moduleName, Func<PageNavigator.Business.ModuleControllerBase> controllerSelector)
		{
			if (!moduleControllerMap.TryAdd(moduleName, controllerSelector))
				throw new ArgumentException(
					string.Format("module with name \"{0}\" already registered"), moduleName);
		}
	}
}
