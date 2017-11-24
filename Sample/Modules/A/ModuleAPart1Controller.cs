using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Modules.A
{
	public class ModuleAPart1Controller : PageNavigator.Business.ModuleControllerBase
	{
		internal ModuleAPart1Controller()
		{
			this.isSingleMode = true;
			this.startPage = new Part1.Page();
		}

		private System.Windows.FrameworkElement startPage;
		protected override System.Windows.FrameworkElement StartPage
		{
			get { return startPage; }
		}
	}
}
