using System;
using System.Windows.Input;

namespace Chat.Client
{
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;

		public RelayCommand(Action<T> action) : this(action, null)
		{
		}

		public RelayCommand(Action<T> action, Func<T, bool> canExecute)
		{
			if (action is null)
				throw new NullReferenceException("action is null");

			_execute = action;

			if (canExecute != null)
				_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) =>
			_canExecute != null && _canExecute((T)parameter);

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

		public void Execute(object parameter) =>
			_execute((T)parameter);
	}
}