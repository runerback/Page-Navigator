using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Modules.Home
{
	public class HomeModuleController : PageNavigator.Business.ModuleControllerBase
	{
		internal HomeModuleController()
		{
			this.isHomePage = true;
			this.startPage = new HomeView();
		}

		private System.Windows.FrameworkElement startPage;
		protected override System.Windows.FrameworkElement StartPage
		{
			get { return startPage; }
		}
	}
}
