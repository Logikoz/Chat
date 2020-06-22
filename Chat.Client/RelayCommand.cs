using System;
using System.Windows.Input;

namespace Chat.Client
{
	public class RelayCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public RelayCommand(Action action) : this(action, null)
		{
		}

		public RelayCommand(Action action, Func<bool> canExecute)
		{
			if (action is null)
				throw new ArgumentNullException("action is null");

			_execute = action;

			if (canExecute != null)
				_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) =>
			_canExecute == null || _canExecute();

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter) =>
			_execute();
	}
}