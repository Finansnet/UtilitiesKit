namespace PdfViewerDemo
{
	using System.IO;
	using System.Reflection;
	using System.Windows;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Load1(object sender, RoutedEventArgs e)
		{
			_PdfViewer.FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Files", "Sample1.pdf");
		}

		private void Button_Load2(object sender, RoutedEventArgs e)
		{
			_PdfViewer.FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Files", "Sample2.pdf");
		}

		private void Button_Unload(object sender, RoutedEventArgs e)
		{
			_PdfViewer.FilePath = null;
		}
	}
}