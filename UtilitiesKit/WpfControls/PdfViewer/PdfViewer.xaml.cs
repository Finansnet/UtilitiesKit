namespace UtilitiesKit.WpfControls.PdfViewer
{
	using System.Windows.Controls;
	using System.Linq;
	using System.Windows.Forms.Integration;
	using System;
	using System.Windows;

	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class PdfViewer : UserControl, IDisposable
	{
		public string FilePath
		{
			get { return (string)GetValue(FilePathProperty); }
			set { SetValue(FilePathProperty, value); }
		}
		public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(PdfViewer), new PropertyMetadata(null, FilePathChangedCallback));

		private static void FilePathChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PdfViewer viewer = (PdfViewer)d;
			string path = (string)e.NewValue;

			if (path == null)
				viewer.Clear();
			if (path != null)
				viewer.Load(path);
		}

		public PdfViewer()
		{
			InitializeComponent();
		}

		private void Load(string path)
		{
			WinFormPdfHost formsPdfHost;
			if (LayoutRoot.Children.Count == 0)
			{
				WindowsFormsHost formsHost = new WindowsFormsHost();
				formsPdfHost = new WinFormPdfHost();
				formsHost.Child = formsPdfHost;
				LayoutRoot.Children.Add(formsHost);
			}
			else
			{
				formsPdfHost = (WinFormPdfHost)((WindowsFormsHost)LayoutRoot.Children[0]).Child;
			}

			formsPdfHost.LoadFile(path);
		}

		private void Clear()
		{
			if (LayoutRoot.Children.Count > 0)
			{
				WindowsFormsHost formsHost = (WindowsFormsHost)LayoutRoot.Children[0];
				formsHost.Child.Dispose();
				formsHost.Dispose();

				LayoutRoot.Children.Clear();

				GC.Collect();
			}
		}

		#region Dispose pattern

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (LayoutRoot.Children.Count > 0)
					((WinFormPdfHost)((WindowsFormsHost)LayoutRoot.Children[0]).Child).Dispose();
			}
		}

		#endregion
	}
}
