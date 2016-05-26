using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using NationalInstruments.LabVIEW.DfirExport;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDfirBase;

namespace RibbonDemoApp
{
    /// <summary>
    /// This class exports a button to the ribbon UI. The button parses the IDfirRoot and writes the dotstring to a file.
    /// </summary>
    [Export(typeof(IDfirRootAcceptor))]
    [PartMetadata("ExportIdentifier", "ProductLevel:Base")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DotStringPrinter : IDfirRootAcceptor
    {
        #region IDfirRootAcceptor Members

        /// <summary>
        /// The method that is asynchronously called by the UI when a button is pressed. 
        /// </summary>
        /// <param name="dfirRoot">The <see cref="IDfirRoot"/> provided by the DfirExport tool.</param>
        public void AcceptDfirRoot(IDfirRoot dfirRoot)
        {
            // Open a file
            const string folderName = @"C:\MYFiles";
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var writer = new StreamWriter(Path.Combine(folderName, dfirRoot.Name + ".dot"));

            // Write the dotstring to the file
            writer.Write(dfirRoot.DotString);

            // Close the file
            writer.Close();

            // Open a new Windows Explorer window with the foldername as the current directory. 
            // Process.Start(folderName);
        }

        /// <inheritdoc />
        public string ButtonName
        {
            get { return "Print DotString"; }
        }

        /// <inheritdoc />
        public BitmapImage ButtonImage
        {
            get { return null; }
        }

        #endregion
    }
}