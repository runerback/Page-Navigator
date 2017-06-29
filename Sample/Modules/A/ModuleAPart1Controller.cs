using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Modules.A
{
	public class ModuleAPart1Controller : PageNavigator.Business.ModuleControllerBase
	{
		internal ModuleAPart1Controller(PageNavigator.Model.ModuleData moduleData)
			: base(moduleData)
		{ }

		protected override System.Windows.FrameworkElement setStartPage()
		{
			return new Part1.Page();
		}
	}
}
