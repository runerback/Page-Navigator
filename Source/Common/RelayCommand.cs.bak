using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PageNavigator.Common
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;

		private readonly Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute)
			: this(execute, null) { }

		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if (this._canExecute != null)
				return this._canExecute(parameter);
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public void Execute(object parameter)
		{
			if (this._execute != null)
				this._execute(parameter);
		}
	}
}
