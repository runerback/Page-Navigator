﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PageNavigatorSample.Controls
{
	/// <summary>
	/// Interaction logic for NavigatorView.xaml
	/// </summary>
	public partial class NavigatorView : UserControl
	{
		public NavigatorView()
		{
			InitializeComponent();

			DataContext = PageNavigator.Business.ViewModels.NavigatorViewModel;
		}
	}
}
