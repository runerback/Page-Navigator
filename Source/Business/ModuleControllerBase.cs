using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PageNavigator.Business
{
	public abstract class ModuleControllerBase : Common.ViewModelBase, IEquatable<ModuleControllerBase>
	{
		protected ModuleControllerBase(Model.ModuleData module)
		{
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			if (module.IsModuleSet)
			{
				throw new NotSupportedException(
					string.Format("not support container module \"{0}\"", module.Name));
			}
			this.module = module;
			this.CreatedDateTime = DateTime.Now;
		}

		/// <summary>
		/// module controller created time. Use to set index of same opened module.
		/// </summary>
		public DateTime CreatedDateTime { get; private set; }

		private bool isActivated;
		/// <summary>
		/// indicate whether current controller is activated.
		/// </summary>
		public bool IsActivated
		{
			get { return this.isActivated; }
			set
			{
				if (this.isActivated != value)
				{
					this.isActivated = value;
					this.OnPropertyChanged("IsActivated");
				}
			}
		}

		private bool isPinned;
		/// <summary>
		/// indicate whether current controller is pinned.
		/// </summary>
		public bool IsPinned
		{
			get
			{
				if (this.isHomePage)
				{
					return true;
				}
				return this.isPinned;
			}
			set
			{
				if (this.isPinned != value)
				{
					this.isPinned = value;
					this.OnPropertyChanged("IsPinned");
				}
			}
		}

		protected bool isHomePage;
		/// <summary>
		/// indicate whether current controller is Home page.
		/// this property can only set from derived controller class.
		/// Home page count must be 0 or 1.
		/// Home page will set IsPin to True and cannot changed.
		/// Home page has no Close and Pin tool.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool IsHomePage
		{
			get { return this.isHomePage; }
		}

		private FrameworkElement currentPage;
		/// <summary>
		/// Current Page of module
		/// </summary>
		public FrameworkElement CurrentPage
		{
			get { return this.currentPage; }
			private set
			{
				this.currentPage = value;
				if (this.currentPage != null)
				{
					this.OnPropertyChanged("CurrentPage");
					//focus to current page
					this.currentPage.BringIntoView();
					if (!this.currentPage.Focusable)
					{
						this.currentPage.Focusable = true;
					}
					this.currentPage.Focus();
				}
			}
		}

		private Model.ModuleData module;
		/// <summary>
		/// module data of current controller
		/// </summary>
		public Model.ModuleData Module
		{
			get { return this.module; }
		}

		/// <summary>
		/// open current module with start page
		/// </summary>
		public void Create()
		{
			FrameworkElement startPage = this.setStartPage();
			this.OpenPage(startPage);
		}

		/// <summary>
		/// specify module start page.
		/// </summary>
		protected abstract FrameworkElement setStartPage();

		/// <summary>
		/// store opened pages by order
		/// </summary>
		private Stack<FrameworkElement> navigateBackStack = new Stack<FrameworkElement>();

		/// <summary>
		/// store pages opened before naviage back operation happened.
		/// </summary>
		private Stack<FrameworkElement> navigateForwardStack = new Stack<FrameworkElement>();

		/// <summary>
		/// open new page in module.
		/// </summary>
		public void OpenPage(FrameworkElement page)
		{
			if (this.currentPage != null)
			{
				//push last page into stack
				this.navigateBackStack.Push(this.currentPage);
			}
			if (this.navigateForwardStack.Count > 0)
			{
				this.navigateForwardStack.Clear();
			}
			this.CurrentPage = page;
		}

		public bool CanNavigateBack
		{
			get { return this.navigateBackStack.Count > 0; }
		}

		/// <summary>
		/// back to last opened page.
		/// </summary>
		public void NavigateBack()
		{
			if (this.CanNavigateBack)
			{
				this.navigateForwardStack.Push(this.currentPage);
				this.CurrentPage = this.navigateBackStack.Pop();
			}
		}

		public bool CanNavigateForward
		{
			get { return this.navigateForwardStack.Count > 0; }
		}

		/// <summary>
		/// reopen last opened page before navigate back operation happened.
		/// </summary>
		public void NavigateForward()
		{
			if (this.CanNavigateForward)
			{
				var nextPage = this.navigateForwardStack.Pop();
				this.navigateBackStack.Push(this.currentPage);
				this.CurrentPage = nextPage;
			}
		}

		/// <summary>
		/// close current module
		/// </summary>
		public void Close()
		{
			this.CurrentPage = null;
			this.navigateBackStack.Clear();
			this.navigateForwardStack.Clear();

			if (this.Closed != null)
			{
				this.Closed(this, EventArgs.Empty);
			}
		}

		public event EventHandler Closed;

		public bool Equals(ModuleControllerBase other)
		{
			if (other == null) { return false; }
			if (this.CreatedDateTime == other.CreatedDateTime)
			{
				return this.module.Equals(other.module);
			}
			return false;
		}
	}
}
