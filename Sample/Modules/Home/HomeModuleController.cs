using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Modules.Home
{
	public class HomeModuleController : PageNavigator.Business.ModuleControllerBase
	{
		internal HomeModuleController(PageNavigator.Model.ModuleData moduleData)
			: base(moduleData)
		{
			this.isHomePage = true;
		}

		protected override System.Windows.FrameworkElement setStartPage()
		{
			return new HomeView();
		}
	}
}
