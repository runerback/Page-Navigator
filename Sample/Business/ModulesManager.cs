using System;
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
				PageNavigator.Business.ModuleControllerSchedule.CreateModule(
					new PageNavigator.Model.ModuleData("HomeModule", "Home"));
				initialized = true;
			}
		}

		private static PageNavigator.Business.ModuleControllerBase moduleControllerMapping(PageNavigator.Model.ModuleData moduleData)
		{
			switch (moduleData.Name)
			{
				case "HomeModule":
					return new Modules.Home.HomeModuleController(moduleData);
				case "ModuleAPart1":
					return new Modules.A.ModuleAPart1Controller(moduleData);
				case "ModuleAPart2":
					return new Modules.A.ModuleAPart2Controller(moduleData);
				default:
					throw new InvalidOperationException(
						string.Format("not support for Module \"{0}\"", moduleData.Name));
			}
		}
	}
}
