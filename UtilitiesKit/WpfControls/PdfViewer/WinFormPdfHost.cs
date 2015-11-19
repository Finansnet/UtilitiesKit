namespace UtilitiesKit.WpfControls.PdfViewer
{
	using System.Windows.Forms;

	public partial class WinFormPdfHost : UserControl
	{
		public WinFormPdfHost()
		{
			InitializeComponent();
			if (!DesignMode)
				axAcroPDF1.setShowToolbar(false);
		}

		public void LoadFile(string path)
		{
			axAcroPDF1.LoadFile(path);
			axAcroPDF1.src = path;
			axAcroPDF1.setViewScroll("FitH", 0);
			axAcroPDF1.setShowToolbar(false);
		}

		public void SetShowToolBar(bool on)
		{
			axAcroPDF1.setShowToolbar(on);
		}
	}
}
