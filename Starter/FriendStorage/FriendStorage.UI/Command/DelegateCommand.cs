using System;
using System.Windows.Input;

namespace FriendStorage.UI.Command
{
	public class DelegateCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			if (execute == null)
			{
				throw new ArgumentNullException(nameof(execute));
			}

			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

		public void Execute(object parameter) => _execute(parameter);

		public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
