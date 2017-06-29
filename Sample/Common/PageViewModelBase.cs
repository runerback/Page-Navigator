using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigatorSample.Common
{
	public class PageViewModelBase : PageNavigator.Common.ViewModelBase
	{
		protected PageViewModelBase()
		{
			this.navigateCommand = new PageNavigator.Common.RelayCommand(this.onNavigateCommand);
		}

		public event EventHandler Navigate;

		private void raiseNavigate()
		{
			if (this.Navigate != null)
			{
				this.Navigate(this, EventArgs.Empty);
			}
		}

		private PageNavigator.Common.RelayCommand navigateCommand;
		public PageNavigator.Common.RelayCommand NavigateCommand
		{
			get { return this.navigateCommand; }
		}

		private void onNavigateCommand(object obj)
		{
			raiseNavigate();
		}
	}
}
