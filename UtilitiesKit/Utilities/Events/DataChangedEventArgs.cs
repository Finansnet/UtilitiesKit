namespace UtilitiesKit.Utilities.Events
{
	using System;

	public sealed class DataChangedEventArgs<T> : EventArgs
	{
		public T New { get; private set; }
		public T Old { get; private set; }

		public DataChangedEventArgs(T newValue, T oldValue)
		{
			New = newValue;
			Old = oldValue;
		}
	}
}
