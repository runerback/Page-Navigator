using System;

namespace PageNavigator.Business
{
	public static class ViewModels
	{
		public static object MenuContainerViewModel
		{
			get
			{
				return ViewModel.MenuContainerViewModel.Instance;
			}
		}

		public static object TabContainerViewModel
		{
			get
			{
				return ViewModel.TabContainerViewModel.Instance;
			}
		}

		public static object NavigatorViewModel
		{
			get
			{
				return ViewModel.NavigatorViewModel.Instance;
			}
		}
	}
}
