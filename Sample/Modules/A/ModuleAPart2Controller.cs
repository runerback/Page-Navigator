using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Modules.A
{
	public class ModuleAPart2Controller : PageNavigator.Business.ModuleControllerBase
	{
		internal ModuleAPart2Controller()
		{
			var v = new Part2.Page1();
			var vm = new Part2.Page1ViewModel();
			vm.Navigate += this.onNavigateToPage2;
			v.DataContext = vm;

			this.startPage = v;
		}

		private System.Windows.FrameworkElement startPage;
		protected override System.Windows.FrameworkElement StartPage
		{
			get { return startPage; }
		}

		private void onNavigateToPage2(object sender, EventArgs e)
		{
			this.OpenPage(new Part2.Page2());
		}
	}
}
