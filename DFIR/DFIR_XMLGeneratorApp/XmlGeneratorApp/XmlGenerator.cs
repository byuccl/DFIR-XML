// This is an XML generator program which takes the DFIR from a
// LabVIEW Comms design and outputs an equivalent XML file.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using NationalInstruments.LabVIEW.DfirExport;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.IArray;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.IBoolean;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.IComparison;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.INumeric;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.INumeric.IComplex;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.INumeric.IConversion;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.INumeric.IDataManipulation;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDataTypes.IStreamManipulation;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDfirBase;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IDfirBase.ITypes;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IMath.ITrigonometric;
using NationalInstruments.LabVIEW.DfirExport.Interfaces.IProgramFlow;

namespace RibbonDemoApp
{
    /// <summary>
    /// This class exports a button to the ribbon UI. The button
    /// parses the IDfirRoot and XML generators the graph
    /// </summary>
    [Export(typeof(IDfirRootAcceptor))]
    [PartMetadata("ExportIdentifier", "ProductLevel:Base")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class XmlGenerator : IDfirRootAcceptor
    {
        //=====================================================================
        // This is the name of the location to save the resulting XML files.
        //=====================================================================
        const string folderName = @"Z:\Documents\NI_CSDS\Files";


        /// <summary>
        /// The method that is asynchronously called by the UI when a button is pressed. 
        /// </summary>
        /// <param name="dfirRoot">The <see cref="IDfirRoot"/> provided by the DfirExport tool.</param>
        public void AcceptDfirRoot(IDfirRoot dfirRoot)
        {
            // Open a file
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var writer = new StreamWriter(Path.Combine(folderName, dfirRoot.Name + ".xml"));

            // 	Call the XML generator with the top level block diagram
            // This is recursively call itself to print out the various structures found.	
            IDiagram diag = dfirRoot.BlockDiagram;
            writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xg(0, writer, (dynamic)diag, "");

            // Close the file handle 
            writer.Close();
        }

        // XML generator diagrams
        public void xg(int indnt, StreamWriter writer, IDiagram node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);

            writer.Write("<IDiagram NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId
                            + "\" DiagramIndex=\"" + node.Index + "\">\n");

            // A diagram contains a collection of nodes
            // Only XML generator those who are direct children of this diagram (the method
            // GetAllNodes() returns all nodes inside the diagram, not just direct children
            foreach (var n in node.GetAllNodes())
            {
                INode tmp = n.ParentNode;
                if (tmp != null && tmp == node)
                    xg(indnt + 1, writer, (dynamic)n, "");
            }
            indent(indnt, writer);
            writer.Write("</IDiagram>\n");
        }


        // Each of the versions of xg() below handle extended types of INodes

        // The default xg() for INode - when there is not a specialized version
        public void xg(int indnt, StreamWriter writer, INode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<INode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\" Mode=\"" + shortName(node.GetType().ToString()) + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</INode>\n");
        }

        // XML generator arithmetic nodes
        public void xg(int indnt, StreamWriter writer, ICompoundArithmeticNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);

            writer.Write("<ICompoundArithmeticNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\" Mode=\"" + node.Mode + "\">\n");
            PrintInvertedInputs(indnt + 1, writer, node);
            indent(indnt + 1, writer); writer.Write("<InvertedOutput>" + node.InvertedOutput + "</InvertedOutput>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer); writer.Write("</ICompoundArithmeticNode>\n");
        }

        public void PrintInvertedInputs(int indnt, StreamWriter writer, ICompoundArithmeticNode node)
        {
            indent(indnt, writer); writer.Write("<InvertedInputs>\n");
            foreach (var temp in node.InvertedInputs)
            {
                indent(indnt + 1, writer); writer.Write("<InvertedInput>" + temp + "</InvertedInput>\n");
            }
            indent(indnt, writer); writer.Write("</InvertedInputs>\n");
        }

        // XML generator data accessor nodes
        public void xg(int indnt, StreamWriter writer, IDataAccessor node, string msg)
        {
            indent(indnt, writer);
            ReadOnlyCollection<ITerminal> ct = node.Terminals;
            if (msg != "") writer.WriteLine(msg);

            writer.Write("<IDataAccessor NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<Name>" + node.Name + "</Name>\n");
            PrintDirection(indnt + 1, writer, ct);
            PrintTerminals(indnt + 1, writer, ct);
            indent(indnt, writer); writer.Write("</IDataAccessor>\n");
        }


        // XML generator constant nodes
        public void xg(int indnt, StreamWriter writer, IConstant node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IConstant NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<Value>" + node.Value + "</Value>\n");
            PrintDataType(indnt + 1, writer, node.DataType);
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer); writer.Write("</IConstant>\n");
        }

        public void xg(int indnt, StreamWriter writer, IWire node, string msg)
        {

        }

        // XML generator structures
        public void xg(int indnt, StreamWriter writer, IStructure node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IStructure NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");

            // A structure contains a list of border nodes on its periphery and a list of diagrams inside it
            foreach (var b in node.BorderNodes)
                xg(indnt + 1, writer, (dynamic)b, "");
            foreach (var dd in node.Diagrams)
                xg(indnt + 1, writer, (dynamic)dd, "");
            indent(indnt, writer);
            writer.Write("</IStructure>\n");
        }

        public void xg(int indnt, StreamWriter writer, IForLoop node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IForLoop NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");

            // A structure contains a list of border nodes on its periphery and a list of diagrams inside it
            foreach (var b in node.BorderNodes)
                xg(indnt + 1, writer, (dynamic)b, "");
            foreach (var dd in node.Diagrams)
                xg(indnt + 1, writer, (dynamic)dd, "");
            indent(indnt, writer);
            writer.Write("</IForLoop>\n");
        }

        public void xg(int indnt, StreamWriter writer, IWhileLoop node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IWhileLoop NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");

            // A structure contains a list of border nodes on its periphery and a list of diagrams inside it
            foreach (var b in node.BorderNodes)
                xg(indnt + 1, writer, (dynamic)b, "");
            foreach (var dd in node.Diagrams)
                xg(indnt + 1, writer, (dynamic)dd, "");
            indent(indnt, writer);
            writer.Write("</IWhileLoop>\n");
        }

        public void xg(int indnt, StreamWriter writer, ICaseStructure node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ICaseStructure NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");

            // A structure contains a list of border nodes on its periphery and a list of diagrams inside it
            foreach (var b in node.BorderNodes)
                xg(indnt + 1, writer, (dynamic)b, "");
            foreach (var dd in node.Diagrams)
                xg(indnt + 1, writer, (dynamic)dd, "");
            indent(indnt, writer);
            writer.Write("</ICaseStructure>\n");
        }


        public void xg(int indnt, StreamWriter writer, IBorderNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IBorderNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IBorderNode>\n");
        }

        public void xg(int indnt, StreamWriter writer, ITunnel node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ITunnel NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer);
            writer.Write("<IsInput>" + node.IsInput() + "</IsInput>\n");
            indent(indnt + 1, writer);
            writer.Write("<GetInnerTerminal TerminalId=\"" + node.GetInnerTerminal().UniqueId + "\"/>\n");
            indent(indnt + 1, writer);
            writer.Write("<GetOuterTerminal TerminalId=\"" + node.GetOuterTerminal().UniqueId + "\"/>\n");
            indent(indnt + 1, writer);
            writer.Write("<IndexingMode>" + node.IndexingMode + "</IndexingMode>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</ITunnel>\n");
        }

        public void xg(int indnt, StreamWriter writer, ILoopCondition node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ILoopCondition NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</ILoopCondition>\n");
        }

        public void xg(int indnt, StreamWriter writer, ILoopIndex node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ILoopIndex NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</ILoopIndex>\n");
        }

        public void xg(int indnt, StreamWriter writer, ILoopMax node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ILoopMax NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</ILoopMax>\n");
        }

        public void xg(int indnt, StreamWriter writer, ILeftShiftRegister node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ILeftShiftRegister NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<AssociatedRightShiftRegister NodeId=\"" + node.AssociatedRightShiftRegister.UniqueId + "\" ParentId=\"" + node.AssociatedRightShiftRegister.ParentNode.UniqueId + "\"/>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</ILeftShiftRegister>\n");
        }

        public void xg(int indnt, StreamWriter writer, IRightShiftRegister node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IRightShiftRegister NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<AssociatedLeftShiftRegister NodeId=\"" + node.AssociatedLeftShiftRegister.UniqueId + "\" ParentId=\"" + node.AssociatedLeftShiftRegister.ParentNode.UniqueId + "\"/>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IRightShiftRegister>\n");
        }


        public void xg(int indnt, StreamWriter writer, ICaseSelector node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<ICaseSelector NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<DefaultDiagramIndex DiagramIndex=\"" + node.DefaultDiagramIndex + "\"></DefaultDiagramIndex>\n");
            PrintCaseRanges(indnt + 1, writer, node.Ranges);
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</ICaseSelector>\n");
        }



        public void xg(int indnt, StreamWriter writer, IFeedbackInitNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IFeedbackInitNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<Direction>" + node.Direction.ToString().ToUpper() + "</Direction>\n");
            indent(indnt + 1, writer); writer.Write("<AssociatedFeedbackInputNode NodeId=\"" + node.AssociatedFeedbackInputNode.UniqueId + "\" ParentId=\"" + node.AssociatedFeedbackInputNode.ParentNode.UniqueId + "\"/>\n");
            indent(indnt + 1, writer); writer.Write("<AssociatedFeedbackOutputNode NodeId=\"" + node.AssociatedFeedbackOutputNode.UniqueId + "\" ParentId=\"" + node.AssociatedFeedbackOutputNode.ParentNode.UniqueId + "\"/>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IFeedbackInitNode>\n");
        }

        public void xg(int indnt, StreamWriter writer, IFeedbackInputNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IFeedbackInputNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            indent(indnt + 1, writer); writer.Write("<Delay>" + node.Delay + "</Delay>\n");


            if (node.FeedbackTerminal != null)
            {
                indent(indnt + 1, writer); writer.Write("<FeedbackTerminal TerminalId=\"" + node.FeedbackTerminal.UniqueId + "\"/>\n");
            }

            indent(indnt + 1, writer); writer.Write("<OutputNode NodeId=\"" + node.OutputNode.UniqueId + "\" ParentId=\"" + node.OutputNode.ParentNode.UniqueId + "\"/>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IFeedbackInputNode>\n");
        }

        public void xg(int indnt, StreamWriter writer, IFeedbackOutputNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IFeedbackOutputNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");

            if (node.InitTerminal != null)
            {
                indent(indnt + 1, writer); writer.Write("<InitTerminal TerminalId=\"" + node.InitTerminal.UniqueId + "\"/>\n");
            }
            indent(indnt + 1, writer); writer.Write("<InputNode NodeId=\"" + node.InputNode.UniqueId + "\" ParentId=\"" + node.InputNode.ParentNode.UniqueId + "\"/>\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IFeedbackOutputNode>\n");
        }

        public void xg(int indnt, StreamWriter writer, IBlackBoxNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IBlackBoxNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IBlackBoxNode>\n");
        }

        public void xg(int indnt, StreamWriter writer, IMethodCall node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IMethodCall NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");

            indent(indnt + 1, writer); writer.Write("<TargetName>" + node.TargetName + "</TargetName>\n");

            PrintSubVI(indnt + 1, writer, node);
            PrintTerminals(indnt + 1, writer, node.Terminals);
            CreatSubVIxml((dynamic)node.GetDfirRoot.BlockDiagram);
            indent(indnt, writer);
            writer.Write("</IMethodCall>\n");
        }

        public void PrintSubVI(int indnt, StreamWriter writer, IMethodCall node)
        {
            IEnumerable<INode> E = node.GetDfirRoot.BlockDiagram.GetAllNodes();
            indent(indnt, writer); writer.Write("<SubVI NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            foreach (var tmp in E)
            {
                
                if (shortName(tmp.GetType().ToString()) == "ExDataAccessor")
                {
                    IDataAccessor da = tmp as IDataAccessor;
                    ITerminal t = node.GetTerminalByUserVisibleName(da.Name, node.GetDfirRoot);
                    indent(indnt + 1, writer); writer.Write("<Pair TerminalId=\"" + t.UniqueId + "\" SubVIDataAccessorId=\"" + da.UniqueId + "\"/>\n");
                }
            }
            indent(indnt, writer); writer.Write("</SubVI>\n");
        }

        public void CreatSubVIxml(IDiagram diag)
        {
            // Open a file
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            var writer = new StreamWriter(Path.Combine(folderName, diag.DfirRoot.Name + ".xml"));

            // 	Call the XML generator with the top level block diagram
            // This is recursively call itself to print out the various structures found.	
            writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xg(0, writer, (dynamic)diag, "");

            // Close the file handle 
            writer.Close();
        }

        public string shortName(string s)
        {
            char[] delimiterChars = { '.' };
            string[] words = s.Split(delimiterChars);

            //return words[words.Length - 1].Remove(0,2).Remove(words.Length-9,9);
            //words.Length = words.Length - 9;
            //return words[words.Length - 1].Remove(0, 2);

            return words[words.Length - 1];
        }



        /// <inheritdoc />
        public string ButtonName
        {
            get { return "XmlGenerator"; }
        }

        /// <inheritdoc />
        public BitmapImage ButtonImage
        {
            get
            {
                // Use the default image
                return null;
            }
        }


        public void indent(int indnt, StreamWriter writer)
        {
            for (int i = 0; i < indnt; i++)
                writer.Write("  ");
        }



        public void PrintDataType(int indnt, StreamWriter writer, IDataType nodedatatype)
        {
            indent(indnt, writer); writer.Write("<IDataType>\n");
            PrintSubDataType(indnt + 1, writer, (dynamic)nodedatatype);
            indent(indnt, writer); writer.Write("</IDataType>\n");
        }

        //print data type in detail

        public void PrintSubDataType(int indnt, StreamWriter writer, IDataType nodedatatype)
        {

        }
        public void PrintSubDataType(int indnt, StreamWriter writer, ISignedInt s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<ISignedInt>\n");
                indent(indnt + 1, writer); writer.Write("<WordLength>" + s.WordLength + "</WordLength>\n");
                indent(indnt, writer); writer.Write("</ISignedInt>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IUnsignedInt s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IUnsignedInt>\n");

                indent(indnt + 1, writer); writer.Write("<WordLength>" + s.WordLength + "</WordLength>\n");
                indent(indnt, writer); writer.Write("</IUnsignedInt>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, ISignedFixedPoint s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<ISignedFixedPoint>\n");
                indent(indnt + 1, writer); writer.Write("<LeftLength>" + s.LeftLength + "</LeftLength>\n");
                indent(indnt + 1, writer); writer.Write("<RightLength>" + s.RightLength + "</RightLength>\n");
                indent(indnt, writer); writer.Write("</ISignedFixedPoint>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IUnsignedFixedPoint s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IUnsignedFixedPoint>\n");
                indent(indnt + 1, writer); writer.Write("<LeftLength>" + s.LeftLength + "</LeftLength>\n");
                indent(indnt + 1, writer); writer.Write("<RightLength>" + s.RightLength + "</RightLength>\n");
                indent(indnt, writer); writer.Write("</IUnsignedFixedPoint>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IArray s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IArray>\n");
                indent(indnt + 1, writer); writer.Write("<Dimensions>" + s.Dimensions + "</Dimensions>\n");
                indent(indnt + 1, writer); writer.Write("<Element>\n");
                PrintDataType(indnt + 2, writer, s.ElementDataType);
                indent(indnt + 1, writer); writer.Write("</Element>\n");
                indent(indnt, writer); writer.Write("</IArray>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IVariableSizedArray s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IVariableSizedArray>\n");
                indent(indnt + 1, writer); writer.Write("<Dimensions>" + s.Dimensions + "</Dimensions>\n");
                indent(indnt + 1, writer); writer.Write("<Element>\n");
                PrintDataType(indnt + 2, writer, s.ElementDataType);
                indent(indnt + 1, writer); writer.Write("</Element>\n");
                indent(indnt, writer); writer.Write("</IVariableSizedArray>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IComplex s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IComplex>\n");

                indent(indnt + 1, writer); writer.Write("<Element>\n");
                PrintDataType(indnt + 2, writer, s.ComplexElementDataType);
                indent(indnt + 1, writer); writer.Write("</Element>\n");
                indent(indnt, writer); writer.Write("</IComplex>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IFixedSizeArray s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IFixedSizeArray>\n");

                indent(indnt + 1, writer); writer.Write("<Dimensions>" + s.Dimensions + "</Dimensions>\n");
                indent(indnt + 1, writer); writer.Write("<ArraySize>" + s.ArraySize[0] + "</ArraySize>\n");
                indent(indnt + 1, writer); writer.Write("<Element>\n");
                PrintDataType(indnt + 2, writer, s.ElementDataType);
                indent(indnt + 1, writer); writer.Write("</Element>\n");
                indent(indnt, writer); writer.Write("</IFixedSizeArray>\n");
            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IBit s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IBit/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IBoolean s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IBoolean/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IDouble s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IDouble/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IIncorrect s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IIncorrect/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, ISingle s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<ISingle/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IString s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IString/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IUnknown s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IUnknown/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IUnsupported s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IUnsupported/>\n");

            }
        }

        public void PrintSubDataType(int indnt, StreamWriter writer, IVoid s)
        {

            if (s != null)
            {
                indent(indnt, writer); writer.Write("<IVoid/>\n");

            }
        }

        //print connections
        public void PrintConnections(int indnt, StreamWriter writer, ITerminal t)
        {

            if (t.IsConnected)
            {
                indent(indnt, writer);
                writer.Write("<Connections>\n");
                if (t.IsInput())
                {
                    ITerminal temp = t.GetImmediateSourceTerminal();
                    indent(indnt + 1, writer); writer.Write("<Connection TerminalId=\"" + temp.UniqueId + "\" NodeId=\"" + temp.ParentNode.UniqueId + "\"/>\n");
                }
                if (t.IsOutput())
                {
                    ReadOnlyCollection<ITerminal> tt = t.GetSinkTerminals();
                    foreach (var temp in tt)
                    {
                        indent(indnt + 1, writer); writer.Write("<Connection TerminalId=\"" + temp.UniqueId + "\" NodeId=\"" + temp.ParentNode.UniqueId + "\"/>\n");
                    }
                }
                indent(indnt, writer);
                writer.Write("</Connections>\n");
            }
            else{
                indent(indnt, writer);
                writer.Write("<Connections/>\n");
            }

        }



        //print terminal info
        public void PrintTerminal(int indnt, StreamWriter writer, ITerminal terminal)
        {
            indent(indnt, writer); writer.Write("<ITerminal TerminalId=\"" + terminal.UniqueId + "\" TerminalIndex=\"" + terminal.Index + "\">\n");
            PrintDataType(indnt + 1, writer, terminal.DataType);
            PrintConnections(indnt + 1, writer, terminal);
            indent(indnt, writer); writer.Write("</ITerminal>\n");
        }

        // print terminals
        public void PrintTerminals(int indnt, StreamWriter writer, ReadOnlyCollection<ITerminal> terminals)
        {


            bool flag1 = false, flag2 = false;
            foreach (var temp in terminals) if (temp.Direction.ToString() == "Output") flag1 = true;
            foreach (var temp in terminals) if (temp.Direction.ToString() == "Input") flag2 = true;

            if (flag2)
            {
                indent(indnt, writer); writer.Write("<InputTerminals>\n");
                foreach (var temp in terminals)
                {
                    if (temp.Direction.ToString() == "Input")
                    {
                        PrintTerminal(indnt + 1, writer, temp);
                    }

                }
                indent(indnt, writer); writer.Write("</InputTerminals>\n");
            }
            if (flag1)
            {
                indent(indnt, writer); writer.Write("<OutputTerminals>\n");
                foreach (var temp in terminals)
                {
                    if (temp.Direction.ToString() == "Output")
                        PrintTerminal(indnt + 1, writer, temp);
                }
                indent(indnt, writer); writer.Write("</OutputTerminals>\n");
            }



        }

        // print direction
        public void PrintDirection(int indnt, StreamWriter writer, ReadOnlyCollection<ITerminal> terminals)
        {
            indent(indnt, writer);
            if (terminals[0].IsConnected)
            {
                if (terminals[0].IsInput())
                    writer.Write("<Direction>OUTPUT</Direction>\n");
                if (terminals[0].IsOutput())
                    writer.Write("<Direction>INPUT</Direction>\n");
            }
        }

        public void PrintCaseRanges(int indnt, StreamWriter writer, ReadOnlyCollection<ICaseSelectorRange> ranges)
        {

            indent(indnt, writer); writer.Write("<Ranges>\n");
            foreach (var tmp in ranges)
            {
                PrintCaseRange(indnt + 1, writer, tmp);
            }
            indent(indnt, writer); writer.Write("</Ranges>\n");
        }

        public void PrintCaseRange(int indnt, StreamWriter writer, ICaseSelectorRange r)
        {

            indent(indnt, writer); writer.Write("<Range DiagramIndex=\"" + r.DiagramIndex + "\">\n");

            if (!r.Range.IsEmpty)
            {
                if (r.Range.IsSingleValue)
                {
                    indent(indnt + 1, writer);
                    writer.Write("<SingleValue>" + r.Range.LowValue + "</SingleValue>\n");
                }
                else if (r.Range.IsRange)
                {
                    indent(indnt + 1, writer);
                    writer.Write("<LowValue>" + r.Range.LowValue + "</LowValue>\n");
                    indent(indnt + 1, writer);
                    writer.Write("<HighValue>" + r.Range.HighValue + "</HighValue>\n");
                }
            }
            indent(indnt, writer); writer.Write("</Range>\n");
        }

        public void xg(int indnt, StreamWriter writer, IPrimitive node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IPrimitive NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\" Mode=\"" + shortName(node.GetType().ToString()) + "\">\n");

            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IPrimitive>\n");
        }

       
        public void xg(int indnt, StreamWriter writer, IArrayIndexNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IArrayIndexNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IArrayIndexNode>\n");
        }
        public void xg(int indnt, StreamWriter writer, IReplaceArraySubsetNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IReplaceArraySubsetNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IReplaceArraySubsetNode>\n");
        }
        public void xg(int indnt, StreamWriter writer, IInsertIntoArrayNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IInsertIntoArrayNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IInsertIntoArrayNode>\n");
        }
        public void xg(int indnt, StreamWriter writer, IDeleteFromArrayNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IDeleteFromArrayNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IDeleteFromArrayNode>\n");
        }
        public void xg(int indnt, StreamWriter writer, IInitializeArrayNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IInitializeArrayNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IInitializeArrayNode>\n");
        }
        public void xg(int indnt, StreamWriter writer, IBuildArrayNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IBuildArrayNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IBuildArrayNode>\n");
        }
        public void xg(int indnt, StreamWriter writer, IArraySubsetNode node, string msg)
        {
            indent(indnt, writer);
            if (msg != "") writer.WriteLine(msg);
            writer.Write("<IArraySubsetNode NodeId=\"" + node.UniqueId + "\" ParentId=\"" + node.ParentNode.UniqueId + "\">\n");
            PrintTerminals(indnt + 1, writer, node.Terminals);
            indent(indnt, writer);
            writer.Write("</IArraySubsetNode>\n");
        }

    }
}








