namespace UtilitiesKit.Wpf
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Windows;

	public class BindableFocus
	{
		public static IBindableWithFocus GetBindable(DependencyObject obj)
		{
			return (IBindableWithFocus)obj.GetValue(BindableProperty);
		}

		public static void SetBindable(DependencyObject obj, IBindableWithFocus value)
		{
			obj.SetValue(BindableProperty, value);
		}

		// Using a DependencyProperty as the backing store for Bindable.
		public static readonly DependencyProperty BindableProperty = DependencyProperty.RegisterAttached("Bindable", typeof(IBindableWithFocus), typeof(BindableFocus), new PropertyMetadata(null, PropertyChangedCallback));

		private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IBindableWithFocus bindable = (IBindableWithFocus)e.NewValue;
			if (d != null)
				bindable.SetControl((UIElement)d);
		}
	}

	public interface IBindableWithFocus
	{
		void SetControl(UIElement uiElement);
	}

	public class BindableWithFocus<T> : Bindable<T>, IBindableWithFocus
	{
		private UIElement _UIElement = null;

		/// <summary>
		/// Flag allowing to wait with setting focus to the control until binding is done.
		/// </summary>
		private bool _DelayedFocus = false;
		private string _Name;

		public BindableWithFocus(BindablePropertyChangedDelegate guiActionCallback = null, BindablePropertyChangedDelegate setterCallback = null, string name = null)
			: base(guiActionCallback, setterCallback)
		{
			_Name = name;
		}

		void IBindableWithFocus.SetControl(UIElement uiElement)
		{
			if (_UIElement != null && _UIElement != uiElement)
				Debug.WriteLine("Warning: UIElement already set and will be replaced with new one. Current value: " + _UIElement.ToString());

			_UIElement = uiElement;
			if (_DelayedFocus)
			{
				_UIElement.Focus();
				_DelayedFocus = false;
			}
		}

		public void Focus()
		{
			if (_UIElement == null)
				_DelayedFocus = true;
			else
				OnFocus();
		}

		protected virtual void OnFocus()
		{
			bool focusResult = _UIElement.Focus();
			if (!focusResult)
			{
				Debug.WriteLine("ERROR: SetFocus to " + _UIElement.ToString() + " failed.");
				Debug.WriteLine(" - Focusable: " + _UIElement.Focusable);
				Debug.WriteLine(" - IsEnabled: " + _UIElement.IsEnabled);
				Debug.WriteLine(" - IsVisible: " + _UIElement.IsVisible);
			}
		}
	}
}
