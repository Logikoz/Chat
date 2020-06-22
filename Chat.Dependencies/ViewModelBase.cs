using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat.Dependencies
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void Set<T>(ref T objectReferente, T newObjectValue, [CallerMemberName] string property = null)
		{
			objectReferente = newObjectValue;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
	}
}