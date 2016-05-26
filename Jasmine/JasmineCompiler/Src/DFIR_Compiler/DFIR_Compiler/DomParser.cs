using System;
using System.Collections.Generic;
using System.Xml;


namespace DFIR_Compiler
{
    public class DomParser
    {
        //Parse IDataType
        public static IDataType GetIDataType(XmlNode n)
        {
            XmlElement e = (XmlElement)n;
            XmlNodeList nl = n.ChildNodes;
            string s = "";
            for (int i = 0; i < nl.Count; i++)
            {
                if (nl.Item(i).Name != "#text")
                {
                    s = nl.Item(i).Name;
                }
            }
            if (s == "")
            {
                Console.WriteLine("s is a null string");
            }

            BaseDataType _base = (BaseDataType)Enum.Parse(typeof(BaseDataType), s);
            IDataTypeBuilder builder = new IDataTypeBuilder(_base);

            if (s == "IBit" || s == "IBoolean" || s == "IDouble" || s == "IIncorrect" || s == "ISingle" || s == "IString" || s == "IUnknown" || s == "IUnsupported" || s == "IVoid")
            {
                return builder.GetResult();
            }
            else if (s == "ISignedInt" || s == "IUnsignedInt")
            {
                builder.SetWordLength(Convert.ToInt32(e.GetElementsByTagName("WordLength").Item(0).InnerText));
                return builder.GetResult();
            }
            else if (s == "ISignedFixedPoint" || s == "IUnsignedFixedPoint")
            {
                builder.SetLeftLength(Convert.ToInt32(e.GetElementsByTagName("LeftLength").Item(0).InnerText));
                builder.SetRightLength(Convert.ToInt32(e.GetElementsByTagName("RightLength").Item(0).InnerText));
                return builder.GetResult();
            }
            else if (s == "IArray" || s == "IVariableSizedArray")
            {
                builder.SetDimensions(Convert.ToInt32(e.GetElementsByTagName("Dimensions").Item(0).InnerText));
                XmlElement dataTypeElement = (XmlElement)e.GetElementsByTagName("Element").Item(0).FirstChild;
                builder.SetElementType(GetIDataType(dataTypeElement));
                return builder.GetResult();
            }
            else if (s == "IComplex")
            {
                XmlElement dataTypeElement = (XmlElement)e.GetElementsByTagName("Element").Item(0).FirstChild;
                builder.SetElementType(GetIDataType(dataTypeElement));
                return builder.GetResult();
            }
            else if (s == "IFixedSizeArray")
            {
                builder.SetDimensions(Convert.ToInt32(e.GetElementsByTagName("Dimensions").Item(0).InnerText));
                builder.SetArraySize(Convert.ToInt32(e.GetElementsByTagName("ArraySize").Item(0).InnerText));
                XmlElement dataTypeElement = (XmlElement)e.GetElementsByTagName("Element").Item(0).FirstChild;
                builder.SetElementType(GetIDataType(dataTypeElement));
                return builder.GetResult();
            }
            else
            {
                Console.WriteLine("IDataType Error!");
                return builder.GetResult();
            }
        }


        //Parse ITerminal
        public static ITerminal GetITerminal(XmlElement e)
        {
            XmlElement dataTypeElement = (XmlElement)e.GetElementsByTagName("IDataType").Item(0);
            IDataType datatype = GetIDataType(dataTypeElement);
            ITerminal iTerminal = new ITerminal(Convert.ToInt32(e.GetAttribute("TerminalId")), Convert.ToInt32(e.GetAttribute("TerminalIndex")), datatype);
            XmlElement connecetionsElement = (XmlElement)e.GetElementsByTagName("Connections").Item(0);

            if (connecetionsElement == null) { }
            else
            {
                XmlNodeList connectionList = connecetionsElement.GetElementsByTagName("Connection");
                for (int i = 0; i < connectionList.Count; i++)
                {
                    XmlElement temp = (XmlElement)connectionList.Item(i);
                    Connection connection = new Connection(Convert.ToInt32(temp.GetAttribute("TerminalId")), Convert.ToInt32(temp.GetAttribute("NodeId")));
                    iTerminal.AddConnection(connection);
                }
            }
            return iTerminal;
        }


        //Parse IDiagram
        public static IDiagram GetIDiagram(XmlNode n)
        {
            XmlElement iDiagramElement = (XmlElement)n;
            IDiagram iDiagram = new IDiagram(Convert.ToInt32(iDiagramElement.GetAttribute("NodeId")), Convert.ToInt32(iDiagramElement.GetAttribute("ParentId")), new List<ITerminal>(), new List<ITerminal>(), Convert.ToInt32(iDiagramElement.GetAttribute("DiagramIndex")));
            if (iDiagramElement.GetElementsByTagName("IComplex").Count != 0)
            {
                iDiagram.setHasComplex(true);
            }
            List<IConstant> IConstants = new List<IConstant>();
            List<IDataAccessor> IDataAccessors = new List<IDataAccessor>();
            List<ICompoundArithmeticNode> ICompoundArithmeticNodes = new List<ICompoundArithmeticNode>();
            List<IPrimitive> IPrimitives = new List<IPrimitive>();
            List<IForLoop> iForLoops = new List<IForLoop>();
            List<ICaseStructure> iCaseStructures = new List<ICaseStructure>();
            List<IFeedbackInputNode> iFeedbackInputNodes = new List<IFeedbackInputNode>();
            List<IFeedbackOutputNode> iFeedbackOutputNodes = new List<IFeedbackOutputNode>();

            if (iDiagramElement.GetElementsByTagName("IComplex").Count != 0)
            {
                iDiagram.setHasComplex(true);
            }

            XmlNodeList nList = iDiagramElement.GetElementsByTagName("IConstant");
            for (int temp = 0; temp < nList.Count; temp++)
            {
                XmlNode nNode = nList.Item(temp);

                Console.WriteLine("XmlElement :" + nNode.Name);

                if (nNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement eElement = (XmlElement)nNode;
                    if (eElement.ParentNode != n)
                    {
                        continue;
                    }
                    XmlNode dataTypeNode = eElement.GetElementsByTagName("IDataType").Item(0);

                    IDataType datatype = GetIDataType(dataTypeNode);
                    List<ITerminal> iConstantOutputTerminals = new List<ITerminal>();
                    XmlElement e = (XmlElement)eElement.GetElementsByTagName("OutputTerminals").Item(0);
                    XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                    for (int i = 0; i < eList.Count; i++)
                    {
                        XmlElement temp1 = (XmlElement)eList.Item(i);
                        ITerminal iTerminal = GetITerminal(temp1);
                        iConstantOutputTerminals.Add(iTerminal);
                    }
                    IConstant iConstant = new IConstant(Convert.ToInt32(eElement.GetAttribute("NodeId")), Convert.ToInt32(eElement.GetAttribute("ParentId")), new List<ITerminal>(), iConstantOutputTerminals, Convert.ToString(eElement.GetElementsByTagName("Value").Item(0).InnerText), datatype);
                    IConstants.Add(iConstant);
                }
            }
            iDiagram.setIConstants(IConstants);

            XmlNodeList IDataAccessorList = iDiagramElement.GetElementsByTagName("IDataAccessor");
            for (int temp = 0; temp < IDataAccessorList.Count; temp++)
            {
                XmlNode nNode = IDataAccessorList.Item(temp);

                Console.WriteLine("XmlElement :" + nNode.Name);

                if (nNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement eElement = (XmlElement)nNode;
                    if (eElement.ParentNode != n) continue;
                    List<ITerminal> iDataAccessorInputTerminals = new List<ITerminal>();
                    List<ITerminal> iDataAccessorOutputTerminals = new List<ITerminal>();

                    if (eElement.GetElementsByTagName("InputTerminals").Count > 0)
                    {
                        XmlElement e = (XmlElement)eElement.GetElementsByTagName("InputTerminals").Item(0);
                        XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                        for (int i = 0; i < eList.Count; i++)
                        {
                            XmlElement temp1 = (XmlElement)eList.Item(i);
                            ITerminal iTerminal = GetITerminal(temp1);
                            iDataAccessorInputTerminals.Add(iTerminal);
                        }
                    }
                    if (eElement.GetElementsByTagName("OutputTerminals").Count > 0)
                    {
                        XmlElement e = (XmlElement)eElement.GetElementsByTagName("OutputTerminals").Item(0);
                        XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                        for (int i = 0; i < eList.Count; i++)
                        {
                            XmlElement temp1 = (XmlElement)eList.Item(i);
                            ITerminal iTerminal = GetITerminal(temp1);
                            iDataAccessorOutputTerminals.Add(iTerminal);
                        }
                    }
                    IDataAccessor iDataAccessor = new IDataAccessor(Convert.ToInt32(eElement.GetAttribute("NodeId")), Convert.ToInt32(eElement.GetAttribute("ParentId")),
                                                                   iDataAccessorInputTerminals, iDataAccessorOutputTerminals, Convert.ToString(eElement.GetElementsByTagName("Name").Item(0).InnerText),
                                                                   (Direction)Enum.Parse(typeof(Direction), eElement.GetElementsByTagName("Direction").Item(0).InnerText));
                    IDataAccessors.Add(iDataAccessor);
                }
            }
            iDiagram.setIDataAccessors(IDataAccessors);

            XmlNodeList ICompoundArithmeticNodeList = iDiagramElement.GetElementsByTagName("ICompoundArithmeticNode");
            for (int temp = 0; temp < ICompoundArithmeticNodeList.Count; temp++)
            {
                XmlNode nNode = ICompoundArithmeticNodeList.Item(temp);
                Console.WriteLine("XmlElement :" + nNode.Name);

                if (nNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement eElement = (XmlElement)nNode;
                    if (eElement.ParentNode != n) continue;
                    XmlNodeList InvertedInputs = eElement.GetElementsByTagName("InvertedInput");                    
                    XmlNode InvertedOutput = eElement.GetElementsByTagName("InvertedOutput").Item(0);

                    List<ITerminal> iCompoundArithmeticNodeInputTerminals = new List<ITerminal>();
                    List<ITerminal> iCompoundArithmeticNodeOutputTerminals = new List<ITerminal>();

                    XmlElement e = (XmlElement)eElement.GetElementsByTagName("InputTerminals").Item(0);
                    XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                    for (int i = 0; i < eList.Count; i++)
                    {
                        XmlElement temp1 = (XmlElement)eList.Item(i);
                        ITerminal iTerminal = GetITerminal(temp1);
                        iCompoundArithmeticNodeInputTerminals.Add(iTerminal);
                    }
                    XmlElement e1 = (XmlElement)eElement.GetElementsByTagName("OutputTerminals").Item(0);
                    XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                    for (int i = 0; i < eList1.Count; i++)
                    {
                        XmlElement temp1 = (XmlElement)eList1.Item(i);
                        ITerminal iTerminal = GetITerminal(temp1);
                        iCompoundArithmeticNodeOutputTerminals.Add(iTerminal);
                    }
                    ICompoundArithmeticNode iCompoundArithmeticNode = new ICompoundArithmeticNode(Convert.ToInt32(eElement.GetAttribute("NodeId")), Convert.ToInt32(eElement.GetAttribute("ParentId")), iCompoundArithmeticNodeInputTerminals, iCompoundArithmeticNodeOutputTerminals, (ICompoundArithmeticNodeMode)Enum.Parse(typeof(ICompoundArithmeticNodeMode), eElement.GetAttribute("Mode")), InvertedInputs, InvertedOutput);
                    ICompoundArithmeticNodes.Add(iCompoundArithmeticNode);
                }
            }
            iDiagram.setICompoundArithmeticNodes(ICompoundArithmeticNodes);

            XmlNodeList IPrimitiveList = iDiagramElement.GetElementsByTagName("IPrimitive");
            for (int temp = 0; temp < IPrimitiveList.Count; temp++)
            {
                XmlNode nNode = IPrimitiveList.Item(temp);
                Console.WriteLine("XmlElement :" + nNode.Name);

                if (nNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement eElement = (XmlElement)nNode;
                    if (eElement.ParentNode != n) continue;
                    List<ITerminal> iPrimitiveInputTerminals = new List<ITerminal>();
                    List<ITerminal> iPrimitiveOutputTerminals = new List<ITerminal>();

                    XmlElement e = (XmlElement)eElement.GetElementsByTagName("InputTerminals").Item(0);
                    XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                    for (int i = 0; i < eList.Count; i++)
                    {
                        XmlElement temp1 = (XmlElement)eList.Item(i);
                        ITerminal iTerminal = GetITerminal(temp1);
                        iPrimitiveInputTerminals.Add(iTerminal);
                    }
                    XmlElement e1 = (XmlElement)eElement.GetElementsByTagName("OutputTerminals").Item(0);
                    XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                    for (int i = 0; i < eList1.Count; i++)
                    {
                        XmlElement temp1 = (XmlElement)eList1.Item(i);
                        ITerminal iTerminal = GetITerminal(temp1);
                        iPrimitiveOutputTerminals.Add(iTerminal);
                    }
                    IPrimitive iPrimitive = new IPrimitive(Convert.ToInt32(eElement.GetAttribute("NodeId")), Convert.ToInt32(eElement.GetAttribute("ParentId")), iPrimitiveInputTerminals, iPrimitiveOutputTerminals, (IPrimitiveMode)Enum.Parse(typeof(IPrimitiveMode), eElement.GetAttribute("Mode")));
                    IPrimitives.Add(iPrimitive);
                }
            }
            iDiagram.setIPrimitives(IPrimitives);

            XmlNodeList IForLoopList = iDiagramElement.GetElementsByTagName("IForLoop");
            if (iDiagramElement.GetElementsByTagName("IForLoop").Count != 0)
            {
                for (int temp = 0; temp < IForLoopList.Count; temp++)
                {
                    XmlNode nNode = IForLoopList.Item(temp);
                    Console.WriteLine("XmlElement :" + nNode.Name);

                    if (nNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement eElement = (XmlElement)nNode;
                        if (eElement.ParentNode != n)
                        {
                            continue;
                        }
                        XmlNodeList ITunnelList = eElement.GetElementsByTagName("ITunnel");
                        List<ITunnel> iTunnels = new List<ITunnel>();
                        XmlNodeList ILeftShiftRegisterList = eElement.GetElementsByTagName("ILeftShiftRegister");
                        List<ILeftShiftRegister> iLeftShiftRegisters = new List<ILeftShiftRegister>();
                        XmlNodeList IRightShiftRegisterList = eElement.GetElementsByTagName("IRightShiftRegister");
                        List<IRightShiftRegister> iRightShiftRegisters = new List<IRightShiftRegister>();
                        ILoopIndex finalILoopIndex = null;
                        ILoopMax finalILoopMax = null;

                        //Parse ILoopIndex
                        {
                            XmlNodeList iLoopMaxs = eElement.GetElementsByTagName("ILoopIndex");
                            for (int j = 0; j < iLoopMaxs.Count; j++)
                            {
                                XmlElement iLoopIndexElement = (XmlElement)eElement.GetElementsByTagName("ILoopIndex").Item(j);
                                if (iLoopIndexElement.ParentNode != nNode)
                                {
                                    continue;
                                }
                                List<ITerminal> iLoopIndexInputTerminals = new List<ITerminal>();
                                List<ITerminal> iLoopIndexOutputTerminals = new List<ITerminal>();
                                XmlElement e1 = (XmlElement)iLoopIndexElement.GetElementsByTagName("OutputTerminals").Item(0);
                                XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList1.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList1.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iLoopIndexOutputTerminals.Add(iTerminal);
                                }
                                ILoopIndex iLoopIndex = new ILoopIndex(Convert.ToInt32(iLoopIndexElement.GetAttribute("NodeId")), Convert.ToInt32(iLoopIndexElement.GetAttribute("ParentId")), iLoopIndexInputTerminals, iLoopIndexOutputTerminals);
                                finalILoopIndex = iLoopIndex;
                            }
                        }

                        //Parse ILoopMax
                        {
                            XmlNodeList iLoopMaxs = eElement.GetElementsByTagName("ILoopMax");
                            for (int j = 0; j < iLoopMaxs.Count; j++)
                            {
                                XmlElement iLoopMaxElement = (XmlElement)eElement.GetElementsByTagName("ILoopMax").Item(j);
                                if (iLoopMaxElement.ParentNode != nNode)
                                {
                                    continue;
                                }
                                List<ITerminal> iLoopMaxInputTerminals = new List<ITerminal>();
                                List<ITerminal> iLoopMaxOutputTerminals = new List<ITerminal>();
                                XmlElement e = (XmlElement)iLoopMaxElement.GetElementsByTagName("InputTerminals").Item(0);
                                XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iLoopMaxInputTerminals.Add(iTerminal);
                                }
                                XmlElement e1 = (XmlElement)iLoopMaxElement.GetElementsByTagName("OutputTerminals").Item(0);
                                XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList1.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList1.Item(i);
                                    if (temp1.GetElementsByTagName("Connections").Count != 0)
                                    {
                                        ITerminal iTerminal = GetITerminal(temp1);
                                        iLoopMaxOutputTerminals.Add(iTerminal);
                                    }
                                }
                                ILoopMax iLoopMax = new ILoopMax(Convert.ToInt32(iLoopMaxElement.GetAttribute("NodeId")), Convert.ToInt32(iLoopMaxElement.GetAttribute("ParentId")), iLoopMaxInputTerminals, iLoopMaxOutputTerminals);
                                finalILoopMax = iLoopMax;
                            }

                        }

                        //Parse ITunnel
                        {
                            for (int j = 0; j < ITunnelList.Count; j++)
                            {
                                XmlElement iTunnelElement = (XmlElement)ITunnelList.Item(j);
                                if (iTunnelElement.ParentNode != nNode) continue;
                                List<ITerminal> iTunnelInputTerminals = new List<ITerminal>();
                                List<ITerminal> iTunnelOutputTerminals = new List<ITerminal>();
                                XmlElement e = (XmlElement)iTunnelElement.GetElementsByTagName("InputTerminals").Item(0);
                                XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iTunnelInputTerminals.Add(iTerminal);
                                }
                                XmlElement e1 = (XmlElement)iTunnelElement.GetElementsByTagName("OutputTerminals").Item(0);
                                XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList1.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList1.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iTunnelOutputTerminals.Add(iTerminal);
                                }
                                XmlElement GetInnerTerminalElement = (XmlElement)iTunnelElement.GetElementsByTagName("GetInnerTerminal").Item(0);
                                XmlElement GetOuterTerminalElement = (XmlElement)iTunnelElement.GetElementsByTagName("GetOuterTerminal").Item(0);
                                ITunnel iTunnel = new ITunnel(Convert.ToInt32(iTunnelElement.GetAttribute("NodeId")), Convert.ToInt32(iTunnelElement.GetAttribute("ParentId")), iTunnelInputTerminals, iTunnelOutputTerminals, (IndexingMode)Enum.Parse(typeof(IndexingMode), iTunnelElement.GetElementsByTagName("IndexingMode").Item(0).InnerText), (IsInputMode)Enum.Parse(typeof(IsInputMode), iTunnelElement.GetElementsByTagName("IsInput").Item(0).InnerText), Convert.ToInt32(GetInnerTerminalElement.GetAttribute("TerminalId")), Convert.ToInt32(GetOuterTerminalElement.GetAttribute("TerminalId")));

                                iTunnels.Add(iTunnel);
                            }
                        }

                        //Parse ILeftShiftRegister
                        {
                            for (int j = 0; j < ILeftShiftRegisterList.Count; j++)
                            {
                                XmlElement iLeftShiftRegisterElement = (XmlElement)ILeftShiftRegisterList.Item(j);
                                if (iLeftShiftRegisterElement.ParentNode != nNode)
                                {
                                    continue;
                                }
                                XmlElement associated = (XmlElement)iLeftShiftRegisterElement.GetElementsByTagName("AssociatedRightShiftRegister").Item(0);
                                AssociatedRightShiftRegister associatedRightShiftRegister = new AssociatedRightShiftRegister(Convert.ToInt32(associated.GetAttribute("NodeId")), Convert.ToInt32(associated.GetAttribute("ParentId")));
                                List<ITerminal> iLeftShiftRegisterInputTerminals = new List<ITerminal>();
                                List<ITerminal> iLeftShiftRegisterOutputTerminals = new List<ITerminal>();
                                XmlElement e = (XmlElement)iLeftShiftRegisterElement.GetElementsByTagName("InputTerminals").Item(0);
                                XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iLeftShiftRegisterInputTerminals.Add(iTerminal);
                                }
                                XmlElement e1 = (XmlElement)iLeftShiftRegisterElement.GetElementsByTagName("OutputTerminals").Item(0);
                                XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList1.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList1.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iLeftShiftRegisterOutputTerminals.Add(iTerminal);
                                }
                                ILeftShiftRegister iLeftShiftRegister = new ILeftShiftRegister(Convert.ToInt32(iLeftShiftRegisterElement.GetAttribute("NodeId")), Convert.ToInt32(iLeftShiftRegisterElement.GetAttribute("ParentId")), iLeftShiftRegisterInputTerminals, iLeftShiftRegisterOutputTerminals, associatedRightShiftRegister);

                                iLeftShiftRegisters.Add(iLeftShiftRegister);
                            }
                        }

                        //Parse IRightShiftRegister
                        {
                            for (int j = 0; j < IRightShiftRegisterList.Count; j++)
                            {
                                XmlElement iRightShiftRegisterElement = (XmlElement)IRightShiftRegisterList.Item(j);
                                if (iRightShiftRegisterElement.ParentNode != nNode)
                                {
                                    continue;
                                }
                                XmlElement associated = (XmlElement)iRightShiftRegisterElement.GetElementsByTagName("AssociatedLeftShiftRegister").Item(0);
                                AssociatedLeftShiftRegister associatedLeftShiftRegister = new AssociatedLeftShiftRegister(Convert.ToInt32(associated.GetAttribute("NodeId")), Convert.ToInt32(associated.GetAttribute("ParentId")));
                                List<ITerminal> iRightShiftRegisterInputTerminals = new List<ITerminal>();
                                List<ITerminal> iRightShiftRegisterOutputTerminals = new List<ITerminal>();
                                XmlElement e = (XmlElement)iRightShiftRegisterElement.GetElementsByTagName("InputTerminals").Item(0);
                                XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iRightShiftRegisterInputTerminals.Add(iTerminal);
                                }
                                XmlElement e1 = (XmlElement)iRightShiftRegisterElement.GetElementsByTagName("OutputTerminals").Item(0);
                                XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList1.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList1.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iRightShiftRegisterOutputTerminals.Add(iTerminal);
                                }
                                IRightShiftRegister iRightShiftRegister = new IRightShiftRegister(Convert.ToInt32(iRightShiftRegisterElement.GetAttribute("NodeId")), Convert.ToInt32(iRightShiftRegisterElement.GetAttribute("ParentId")), iRightShiftRegisterInputTerminals, iRightShiftRegisterOutputTerminals, associatedLeftShiftRegister);
                                iRightShiftRegisters.Add(iRightShiftRegister);
                            }
                        }

                        IDiagram iForLoopDiagram = GetIDiagram(eElement.GetElementsByTagName("IDiagram").Item(0));
                        IForLoop iForLoop = new IForLoop(Convert.ToInt32(eElement.GetAttribute("NodeId")), Convert.ToInt32(eElement.GetAttribute("ParentId")), new List<ITerminal>(), new List<ITerminal>(), finalILoopIndex, finalILoopMax, iTunnels, iLeftShiftRegisters, iRightShiftRegisters, iForLoopDiagram);
                        iForLoops.Add(iForLoop);
                    }
                }
            }
            iDiagram.setIForLoops(iForLoops);


            //Parse ICaseStructure
            XmlNodeList ICaseStructureList = iDiagramElement.GetElementsByTagName("ICaseStructure");
            if (iDiagramElement.GetElementsByTagName("ICaseStructure").Count != 0)
            {
                for (int temp = 0; temp < ICaseStructureList.Count; temp++)
                {
                    XmlNode nNode = ICaseStructureList.Item(temp);
                    Console.WriteLine("XmlElement :" + nNode.Name);

                    if (nNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement eElement = (XmlElement)nNode;
                        if (eElement.ParentNode != n)
                        {
                            continue;
                        }
                        XmlNodeList ITunnelList = eElement.GetElementsByTagName("ITunnel");
                        List<ITunnel> iTunnels = new List<ITunnel>();
                        XmlNodeList IDiagramList = eElement.GetElementsByTagName("IDiagram");
                        List<IDiagram> iDiagrams = new List<IDiagram>();
                        ICaseSelector iCaseSelector = null;

                        //Parse ITunnel
                        {
                            for (int j = 0; j < ITunnelList.Count; j++)
                            {
                                XmlElement iTunnelElement = (XmlElement)ITunnelList.Item(j);
                                if (iTunnelElement.ParentNode != nNode)
                                {
                                    continue;
                                }
                                List<ITerminal> iTunnelInputTerminals = new List<ITerminal>();
                                List<ITerminal> iTunnelOutputTerminals = new List<ITerminal>();
                                XmlElement e = (XmlElement)iTunnelElement.GetElementsByTagName("InputTerminals").Item(0);
                                XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iTunnelInputTerminals.Add(iTerminal);
                                }
                                XmlElement e1 = (XmlElement)iTunnelElement.GetElementsByTagName("OutputTerminals").Item(0);
                                XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                                for (int i = 0; i < eList1.Count; i++)
                                {
                                    XmlElement temp1 = (XmlElement)eList1.Item(i);
                                    ITerminal iTerminal = GetITerminal(temp1);
                                    iTunnelOutputTerminals.Add(iTerminal);
                                }
                                XmlElement GetInnerTerminalElement = (XmlElement)iTunnelElement.GetElementsByTagName("GetInnerTerminal").Item(0);
                                XmlElement GetOuterTerminalElement = (XmlElement)iTunnelElement.GetElementsByTagName("GetOuterTerminal").Item(0);
                                ITunnel iTunnel = new ITunnel(Convert.ToInt32(iTunnelElement.GetAttribute("NodeId")), Convert.ToInt32(iTunnelElement.GetAttribute("ParentId")), iTunnelInputTerminals, iTunnelOutputTerminals, (IndexingMode)Enum.Parse(typeof(IndexingMode), (iTunnelElement.GetElementsByTagName("IndexingMode").Item(0).InnerText)), (IsInputMode)Enum.Parse(typeof(IsInputMode), (iTunnelElement.GetElementsByTagName("IsInput").Item(0).InnerText)), Convert.ToInt32(GetInnerTerminalElement.GetAttribute("TerminalId")), Convert.ToInt32(GetOuterTerminalElement.GetAttribute("TerminalId")));

                                iTunnels.Add(iTunnel);
                            }
                        }

                        //Parse IDiagrams
                        {
                            for (int j = 0; j < IDiagramList.Count; j++)
                            {
                                XmlElement iCaseDiagramElement = (XmlElement)IDiagramList.Item(j);
                                if (iCaseDiagramElement.ParentNode != nNode)
                                {
                                    continue;
                                }
                                IDiagram iCaseDiagram = GetIDiagram(iCaseDiagramElement);
                                iDiagrams.Add(iCaseDiagram);
                            }
                        }

                        //Parse ICaseSelector
                        {
                            XmlElement iCaseSelectorElement = (XmlElement)eElement.GetElementsByTagName("ICaseSelector").Item(0);
                            if (iCaseSelectorElement.ParentNode != nNode)
                            {
                                continue;
                            }
                            List<ITerminal> iCaseSelectorInputTerminals = new List<ITerminal>();
                            List<ITerminal> iCaseSelectorOutputTerminals = new List<ITerminal>();
                            List<Range> Ranges = new List<Range>();
                            XmlElement e0 = (XmlElement)iCaseSelectorElement.GetElementsByTagName("Ranges").Item(0);
                            XmlNodeList eList0 = e0.GetElementsByTagName("Range");
                            for (int i = 0; i < eList0.Count; i++)
                            {
                                XmlElement temp1 = (XmlElement)eList0.Item(i);
                                if (temp1.GetElementsByTagName("SingleValue").Count == 0)
                                {
                                    Ranges.Add(new Range(Convert.ToInt32(temp1.GetElementsByTagName("LowValue").Item(0).InnerText), Convert.ToInt32(temp1.GetElementsByTagName("HighValue").Item(0).InnerText), Convert.ToInt32(temp1.GetAttribute("DiagramIndex"))));
                                }
                                else
                                {
                                    if (temp1.GetElementsByTagName("SingleValue").Item(0).InnerText == "False")
                                    {
                                        Ranges.Add(new Range(false, Convert.ToInt32(temp1.GetAttribute("DiagramIndex"))));
                                    }
                                    else if (temp1.GetElementsByTagName("SingleValue").Item(0).InnerText == "True")
                                    {
                                        Ranges.Add(new Range(true, Convert.ToInt32(temp1.GetAttribute("DiagramIndex"))));
                                    }
                                    else
                                    {
                                        Ranges.Add(new Range(Convert.ToInt32(temp1.GetElementsByTagName("SingleValue").Item(0).InnerText), Convert.ToInt32(temp1.GetAttribute("DiagramIndex"))));
                                    }
                                }
                            }
                            XmlElement e = (XmlElement)iCaseSelectorElement.GetElementsByTagName("InputTerminals").Item(0);
                            XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                            for (int i = 0; i < eList.Count; i++)
                            {
                                XmlElement temp1 = (XmlElement)eList.Item(i);
                                ITerminal iTerminal = GetITerminal(temp1);
                                iCaseSelectorInputTerminals.Add(iTerminal);
                            }
                            XmlElement e1 = (XmlElement)iCaseSelectorElement.GetElementsByTagName("OutputTerminals").Item(0);
                            XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                            for (int i = 0; i < eList1.Count; i++)
                            {
                                XmlElement temp1 = (XmlElement)eList1.Item(i);
                                ITerminal iTerminal = GetITerminal(temp1);
                                iCaseSelectorOutputTerminals.Add(iTerminal);
                            }
                            XmlElement defaultDiagramIndexElement = (XmlElement)iCaseSelectorElement.GetElementsByTagName("DefaultDiagramIndex").Item(0);
                            iCaseSelector = new ICaseSelector(Convert.ToInt32(iCaseSelectorElement.GetAttribute("NodeId")), Convert.ToInt32(iCaseSelectorElement.GetAttribute("ParentId")), iCaseSelectorInputTerminals, iCaseSelectorOutputTerminals, Convert.ToInt32(defaultDiagramIndexElement.GetAttribute("DiagramIndex")), Ranges);
                        }


                        ICaseStructure iCaseStructure = new ICaseStructure(Convert.ToInt32(eElement.GetAttribute("NodeId")), Convert.ToInt32(eElement.GetAttribute("ParentId")), new List<ITerminal>(), new List<ITerminal>(), iCaseSelector, iTunnels, iDiagrams);
                        iCaseStructures.Add(iCaseStructure);
                    }
                }
            }
            iDiagram.setiCaseStructures(iCaseStructures);


            //Parse IFeedbackInputNode
            XmlNodeList IFeedbackInputNodeList = iDiagramElement.GetElementsByTagName("IFeedbackInputNode");
            if (iDiagramElement.GetElementsByTagName("IFeedbackInputNode").Count != 0)
            {
                for (int temp = 0; temp < IFeedbackInputNodeList.Count; temp++)
                {
                    XmlNode nNode = IFeedbackInputNodeList.Item(temp);
                    Console.WriteLine("XmlElement :" + nNode.Name);

                    if (nNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement iFeedbackInputNodeElement = (XmlElement)nNode;
                        if (iFeedbackInputNodeElement.ParentNode != n)
                        {
                            continue;
                        }
                        List<ITerminal> iFeedbackInputNodeInputTerminals = new List<ITerminal>();
                        XmlElement outputNodeElement = (XmlElement)iFeedbackInputNodeElement.GetElementsByTagName("OutputNode").Item(0);
                        OutputNode outputNode = new OutputNode(Convert.ToInt32(outputNodeElement.GetAttribute("NodeId")), Convert.ToInt32(outputNodeElement.GetAttribute("ParentId")));
                        XmlElement e = (XmlElement)iFeedbackInputNodeElement.GetElementsByTagName("InputTerminals").Item(0);
                        XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                        for (int i = 0; i < eList.Count; i++)
                        {
                            XmlElement temp1 = (XmlElement)eList.Item(i);
                            ITerminal iTerminal = GetITerminal(temp1);
                            iFeedbackInputNodeInputTerminals.Add(iTerminal);
                        }
                        IFeedbackInputNode iFeedbackInputNode = new IFeedbackInputNode(Convert.ToInt32(iFeedbackInputNodeElement.GetAttribute("NodeId")), Convert.ToInt32(iFeedbackInputNodeElement.GetAttribute("ParentId")), iFeedbackInputNodeInputTerminals, new List<ITerminal>(), Convert.ToInt32(iFeedbackInputNodeElement.GetElementsByTagName("Delay").Item(0).InnerText), outputNode);
                        iFeedbackInputNodes.Add(iFeedbackInputNode);
                    }

                }
            }
            iDiagram.setiFeedbackInputNodes(iFeedbackInputNodes);

            //Parse IFeedbackOutputNode
            XmlNodeList IFeedbackOutputNodeList = iDiagramElement.GetElementsByTagName("IFeedbackOutputNode");
            if (iDiagramElement.GetElementsByTagName("IFeedbackOutputNode").Count != 0)
            {
                for (int temp = 0; temp < IFeedbackOutputNodeList.Count; temp++)
                {
                    XmlNode nNode = IFeedbackOutputNodeList.Item(temp);
                    Console.WriteLine("\nCurrent Element :" + nNode.Name);

                    if (nNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement iFeedbackOutputNodeElement = (XmlElement)nNode;
                        if (iFeedbackOutputNodeElement.ParentNode != n)
                        {
                            continue;
                        }
                        List<ITerminal> iFeedbackOutputNodeInputTerminals = new List<ITerminal>();
                        List<ITerminal> iFeedbackOutputNodeOutputTerminals = new List<ITerminal>();
                        XmlElement inputNodeNodeElement = (XmlElement)iFeedbackOutputNodeElement.GetElementsByTagName("InputNode").Item(0);
                        InputNode inputNode = new InputNode(Convert.ToInt32(inputNodeNodeElement.GetAttribute("NodeId")), Convert.ToInt32(inputNodeNodeElement.GetAttribute("ParentId")));
                        XmlElement e = (XmlElement)iFeedbackOutputNodeElement.GetElementsByTagName("InputTerminals").Item(0);
                        XmlNodeList eList = e.GetElementsByTagName("ITerminal");
                        for (int i = 0; i < eList.Count; i++)
                        {
                            XmlElement temp1 = (XmlElement)eList.Item(i);
                            ITerminal iTerminal = GetITerminal(temp1);
                            iFeedbackOutputNodeInputTerminals.Add(iTerminal);
                        }
                        XmlElement e1 = (XmlElement)iFeedbackOutputNodeElement.GetElementsByTagName("OutputTerminals").Item(0);
                        XmlNodeList eList1 = e1.GetElementsByTagName("ITerminal");
                        for (int i = 0; i < eList1.Count; i++)
                        {
                            XmlElement temp1 = (XmlElement)eList1.Item(i);
                            ITerminal iTerminal = GetITerminal(temp1);
                            iFeedbackOutputNodeOutputTerminals.Add(iTerminal);
                        }
                        IFeedbackOutputNode iFeedbackOutputNode = new IFeedbackOutputNode(Convert.ToInt32(iFeedbackOutputNodeElement.GetAttribute("NodeId")), Convert.ToInt32(iFeedbackOutputNodeElement.GetAttribute("ParentId")), iFeedbackOutputNodeInputTerminals, iFeedbackOutputNodeOutputTerminals, inputNode);
                        iFeedbackOutputNodes.Add(iFeedbackOutputNode);
                    }

                }
            }
            iDiagram.setiFeedbackOutputNodes(iFeedbackOutputNodes);
            if (iFeedbackOutputNodes.Count != 0)
            {
                iDiagram.setHasFeedback(true);
            }



            return iDiagram;
        }
    }
}
