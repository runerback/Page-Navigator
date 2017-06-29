using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Modules.A
{
	public class ModuleAPart2Controller : PageNavigator.Business.ModuleControllerBase
	{
		internal ModuleAPart2Controller(PageNavigator.Model.ModuleData moduleData)
			: base(moduleData)
		{ }

		protected override System.Windows.FrameworkElement setStartPage()
		{
			var v = new Part2.Page1();
			var vm = new Part2.Page1ViewModel();
			vm.Navigate += this.onNavigateToPage2;
			v.DataContext = vm;
			return v;
		}

		private void onNavigateToPage2(object sender, EventArgs e)
		{
			this.OpenPage(new Part2.Page2());
		}
	}
}
