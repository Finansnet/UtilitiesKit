namespace UtilitiesKit.Wpf
{
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// WPF notification automation.
	/// Experimental code.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Bindable<T> : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public delegate void BindablePropertyChangedDelegate(T oldValue, T newValue);

		private BindablePropertyChangedDelegate _GuiActionCallback;
		private BindablePropertyChangedDelegate _SetterCallback;

		/// <summary>
		/// Initializes new instance of <see cref="Bindable"/>.
		/// </summary>
		/// <param name="guiActionCallback">Executed only when Value property setter is called.</param>
		/// <param name="setterCallback">Executed when Value property setter is called or SetValueQuiet is called.</param>
		public Bindable(BindablePropertyChangedDelegate guiActionCallback = null, BindablePropertyChangedDelegate setterCallback = null)
		{
			_GuiActionCallback = guiActionCallback;
			_SetterCallback = setterCallback;
		}

		public T Value
		{
			get { return _Value; }
			set
			{
				if (!EqualityComparer<T>.Default.Equals(_Value, value))
				{
					T oldValue = _Value;
					_Value = value;

					if (_SetterCallback != null)
						_SetterCallback(oldValue, _Value);

					if (_GuiActionCallback != null)
						_GuiActionCallback(oldValue, _Value);

					RaisePropertyChanged("Value");
				}
			}
		}
		private T _Value;

		/// <summary>
		/// Sets the value without calling guiActionCallback.
		/// </summary>
		/// <param name="value"></param>
		public void SetValueQuiet(T value)
		{
			T oldValue = _Value;
			_Value = value;

			if (_SetterCallback != null)
				_SetterCallback(oldValue, _Value);

			RaisePropertyChanged("Value");
		}

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
