using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class CodeConverter
    {
        private List<string> headCode;
        private List<string> IPrimitiveFunctions;
        private static IDictionary<INode, string> INodeToName = new Dictionary<INode, string>();
        private static readonly IDictionary<IPrimitiveMode, string> IPrimitiveModeToString = new Dictionary<IPrimitiveMode, string>();
        private static readonly IDictionary<ICompoundArithmeticNodeMode, string> ICompoundArithmeticNodeModeToString = new Dictionary<ICompoundArithmeticNodeMode, string>();

        public CodeConverter()
        {
            headCode = new List<string>();
            IPrimitiveFunctions = new List<string>();

            //Convert the ICompoundArithemeticNode Mode into corresponding operator
            ICompoundArithmeticNodeModeToString.Add((ICompoundArithmeticNodeMode)Enum.Parse(typeof(ICompoundArithmeticNodeMode), "Add"), "+");
            ICompoundArithmeticNodeModeToString.Add((ICompoundArithmeticNodeMode)Enum.Parse(typeof(ICompoundArithmeticNodeMode), "Multiply"), "*");
            ICompoundArithmeticNodeModeToString.Add((ICompoundArithmeticNodeMode)Enum.Parse(typeof(ICompoundArithmeticNodeMode), "Or"), "|");
            ICompoundArithmeticNodeModeToString.Add((ICompoundArithmeticNodeMode)Enum.Parse(typeof(ICompoundArithmeticNodeMode), "And"), "&");
            ICompoundArithmeticNodeModeToString.Add((ICompoundArithmeticNodeMode)Enum.Parse(typeof(ICompoundArithmeticNodeMode), "Xor"), "^");

            //Convert the IPrimitive Mode into corresponding operator
            IPrimitiveModeToString.Add((IPrimitiveMode)Enum.Parse(typeof(IPrimitiveMode), "ExSubtractPrimitive"), "-");
            IPrimitiveModeToString.Add((IPrimitiveMode)Enum.Parse(typeof(IPrimitiveMode), "ExDividePrimitive"), "/");
        }


        //Process all the sorted nodes list in a diagram
        public virtual void Process(List<INode> sortednodes, List<string> resultlist, IDiagram iDiagram, int indnt, string codeFileName)
        {

            foreach (INode n in sortednodes)
            {
                if (!n.GetIsOccupied() && !n.GetIsDeclared() && IsDeclarationGroup(n))
                {
                    if (n.GetINodeType() == "IConstant")
                    {
                        IConstant c = (IConstant)n;
                        if (c.GetIDataType().GetBase() == BaseDataType.IArray || c.GetIDataType().GetBase() == BaseDataType.IVariableSizedArray || c.GetIDataType().GetBase() == BaseDataType.IFixedSizeArray)
                        {
                            resultlist.Add(AddSemicolon(indnt, Declaration(c, false, 0, "_" + c.GetName(), false) + "=" + c.GetValue()));
                            resultlist.Add(AddSemicolon(indnt, "Array<" + c.GetIDataType().GetDeclarationDataType() + ">" + ' ' + c.GetName() + "(" + "_" + c.GetName() + ")"));
                        }
                        else if (c.GetIDataType().GetBase() == BaseDataType.IComplex)
                        {
                            resultlist.Add(AddSemicolon(indnt, Declaration(c, false, 0, c.GetName(), false) + '(' + c.GetReal() + ',' + c.GetImag() + ')'));
                        }
                    }
                    else if (n.GetINodeType() == "IDataAccessor")
                    {
                        IDataAccessor da = (IDataAccessor)n;
                        Console.WriteLine(da.GetDirection());
                        resultlist.Add(AddSemicolon(indnt, Declaration(n, false, 0, n.GetName(), false)));

                        resultlist.Add(AddSemicolon(indnt, "scanf_s(\"" + Formatting(n) + "\", &" + n.GetName() + ")"));
                    }
                    n.SetIsDeclared(true);

                }
            }



            foreach (INode n in sortednodes)
            {

                if (n.GetINodeType() == "ITunnel")
                {
                    ITunnel t = (ITunnel)n;
                    if (t.IsOutputCaseITunnel() && !t.IsBorderNodeInside(iDiagram))
                    {
                        t.SetIsOccupied(true);
                    }
                }

                if (n.GetIsOccupied())
                {
                    if (n.GetINodeType() == "IDataAccessor")
                    {
                        IDataAccessor da = (IDataAccessor)n;
                        if (da.GetDirection() == Direction.OUTPUT)
                        {
                            if (da.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IArray || da.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IVariableSizedArray || da.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IFixedSizeArray)
                            {
                                resultlist.Add(Indentation(indnt) + "for(int i=0; i<" + da.GetInputTerminalIDataType(0).GetArraySize() + "; i++)");
                                resultlist.Add(AddSemicolon(indnt + 1, "printf(\"Element[" + Formatting(n) + "]=" + Formatting(n) + "\\n\", i, " + n.GetName() + "[i])"));
                            }
                            else
                                resultlist.Add(AddSemicolon(indnt, "printf(\"" + Formatting(n) + "\\n\"," + n.GetName() + ")"));
                        }
                    }
                    else if (n.GetINodeType() == "ITunnel")
                    {
                        ITunnel t = (ITunnel)n;
                        if (t.IsOutputCaseITunnel() && t.IsBorderNodeInside(iDiagram))
                        {
                            t.SetIsOccupied(false);
                        }
                    }
                    continue;
                }


                if (n.GetINodeType() == "IFeedbackInputNode")
                {
                    IFeedbackInputNode fi = (IFeedbackInputNode)n;
                    if (!fi.GetIsFeedbackDeclared())
                    {
                        resultlist.Add(AddSemicolon(indnt, "<" + Declaration(fi, true, 0, "", false) + "> " + fi.GetFeedbackName() + "()"));
                        fi.SetIsFeedbackDeclared(true);
                    }
                    resultlist.Add(AddSemicolon(indnt, IDataAccessorCode(n, iDiagram)));
                    resultlist.Add(AddSemicolon(indnt, fi.GetFeedbackName() + ".push_back(" + fi.GetName() + ")"));
                    fi.SetIsOccupied(true);
                }
                else if (n.GetINodeType() == "IFeedbackOutputNode")
                {
                    IFeedbackOutputNode fo = (IFeedbackOutputNode)n;
                    if (!fo.GetIsDeclared() && fo.GetIsInputConnected())
                    {
                        resultlist.Add(AddSemicolon(indnt, InitFeedbackCode(fo, iDiagram)));
                    }
                    IFeedbackInputNode fi = fo.GetIFeedbackInputNode();
                    string loop_index = fo.GetLoopIndexName();
                    if (loop_index.Equals(""))
                    {
                        loop_index = "loop_index";
                        resultlist.Add(AddSemicolon(indnt, "int " + loop_index + "=0"));
                    }
                    resultlist.Add(AddSemicolon(indnt, Declaration(fo, false, 0, fo.GetName(), false)));
                    resultlist.Add(AddSemicolon(indnt, "FeedbackFunction(" + fi.GetFeedbackName() + ", " + fi.GetDalay() + ", " + loop_index + ", " + fo.GetInitName() + ", " + fo.GetName() + ")"));
                    fo.SetIsOccupied(true);
                }
                else if (n.GetINodeType() == "ICompoundArithmeticNode")
                {
                    ICompoundArithmeticNode c = (ICompoundArithmeticNode)n;
                    resultlist.Add(AddSemicolon(indnt, ICompoundArithmeticNodeCode(c, iDiagram)));
                    c.SetIsOccupied(true);
                }
                else if (n.GetINodeType() == "IPrimitive")
                {
                    IPrimitive p = (IPrimitive)n;
                    if (p.GetMode() == IPrimitiveMode.ExSubtractPrimitive || p.GetMode() == IPrimitiveMode.ExDividePrimitive)
                    {
                        resultlist.Add(AddSemicolon(indnt, ArithmeticIPrimitiveCode(p, iDiagram)));
                        p.SetIsOccupied(true);
                    }
                    else
                    {
                        for (int i = 0; i < p.GetOutputTerminals().Count; i++)
                        {
                            resultlist.Add(AddSemicolon(indnt, Declaration(p, false, i, p.GetNameByOutputIndex(i), false)));
                        }
                        resultlist.Add(AddSemicolon(indnt, IPrimitiveCallCode(p, iDiagram)));
                        IPrimitiveFunctions.AddRange(GenerateIPrimitive(p));
                        p.SetIsOccupied(true);
                    }
                }
                else if (n.GetINodeType() == "IDataAccessor" || n.GetINodeType() == "IRightShiftRegister" || n.GetINodeType() == "ILoopMax" || n.GetINodeType() == "ICaseSelector")
                {
                    if (n.GetINodeType() == "IDataAccessor")
                    {
                        IDataAccessor da = (IDataAccessor)n;
                        if (da.GetDirection() == Direction.INPUT)
                        {
                            continue;
                        }
                    }
                    resultlist.Add(AddSemicolon(indnt, IDataAccessorCode(n, iDiagram)));
                    if (n.GetINodeType() == "IDataAccessor")
                    {
                        IDataAccessor da = (IDataAccessor)n;
                        if (da.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IArray || da.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IVariableSizedArray || da.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IFixedSizeArray)
                        {
                            resultlist.Add(Indentation(indnt) + "for(int i=0; i<" + da.GetInputTerminalIDataType(0).GetArraySize() + ";i++)");
                            resultlist.Add(AddSemicolon(indnt + 1, "printf(\"Element[" + Formatting(n) + "]=" + Formatting(n) + "\\n\", i, " + n.GetName() + "[i])"));
                        }
                        else
                            resultlist.Add(AddSemicolon(indnt, "printf(\"" + Formatting(n) + "\\n\"," + n.GetName() + ")"));
                    }
                    n.SetIsOccupied(true);
                }
                else if (n.GetINodeType() == "ITunnel")
                {
                    ITunnel t = (ITunnel)n;
                    t.SetIsOccupied(true);
                    if (t.IsIndexingITunnelInside(iDiagram))
                    {
                        resultlist.Add(AddSemicolon(indnt, IndexingITunnelCode(t, iDiagram)));
                    }
                    else
                    {
                        resultlist.Add(AddSemicolon(indnt, ITunnelCode(t, iDiagram)));
                    }
                    if (t.IsOutputCaseITunnel() && t.IsBorderNodeInside(iDiagram))
                    {
                        t.SetIsOccupied(false);
                    }

                }
                else if (n.GetINodeType() == "ILeftShiftRegister")
                {
                    ILeftShiftRegister ls = (ILeftShiftRegister)n;
                    for (int i = 0; i < ls.GetInputINodes().Count; i++)
                    {
                        if (!ls.GetIsOccupieds()[i])
                        {
                            resultlist.Add(AddSemicolon(indnt, ILeftShiftRegisterCode(ls, i, iDiagram)));
                        }
                    }
                    ls.SetIsOccupied(true);
                }

                //Process the IForLoop
                else if (n.GetINodeType() == "IForLoop")
                {
                    IForLoop f = (IForLoop)n;
                    if (f.GetIDiagram().getHasFeedback())
                    {
                        foreach (IFeedbackInputNode fi in f.GetIDiagram().getiFeedbackInputNodes())
                        {
                            if (!fi.GetIsFeedbackDeclared())
                            {
                                resultlist.Add(AddSemicolon(indnt, "<" + Declaration(fi, true, 0, "", false) + "> " + fi.GetFeedbackName() + "()"));
                                fi.SetIsFeedbackDeclared(true);
                            }
                        }
                    }
                    resultlist.AddRange(BorderNodeDeclaration(f, iDiagram, indnt));
                    resultlist.Add(Indentation(indnt) + "for(int " + f.GetILoopIndex().GetName() + " = 0; " + f.GetILoopIndex().GetName() + " < " + f.GetILoopMax().GetName() + "; " + f.GetILoopIndex().GetName() + "++){");
                    resultlist.AddRange(Printout(f.GetIDiagram(), false, indnt + 1, codeFileName));
                    resultlist.AddRange(ShiftRegisterCode(f.GetIRightShiftRegisters(), indnt + 2));
                    resultlist.Add(Indentation(indnt) + "}");
                    n.SetIsOccupied(true);
                }

                //Process the ICaseStructure
                else if (n.GetINodeType() == "ICaseStructure")
                {
                    ICaseStructure c = (ICaseStructure)n;
                    resultlist.AddRange(CaseBorderNodeDeclaration(c, iDiagram, indnt));
                    int caseNum = c.GetIDiagrams().Count;
                    if (caseNum == 1)
                    {
                        resultlist.Add(Indentation(indnt) + "{");
                        resultlist.AddRange(Printout(c.GetIDiagrams()[0], false, indnt + 1, codeFileName));
                        resultlist.Add(Indentation(indnt) + "}");
                    }
                    else
                    {
                        resultlist.Add(Indentation(indnt) + "if(" + c.GetNonDefaultCaseCode(0) + ")");
                        resultlist.Add(Indentation(indnt) + "{");
                        resultlist.AddRange(Printout(c.GetIDiagrams()[c.GetNonDefaultCaseIndex(0)], false, indnt + 1, codeFileName));
                        for (int i = 1; i < caseNum - 1; i++)
                        {
                            resultlist.Add(Indentation(indnt) + "}");
                            resultlist.Add(Indentation(indnt) + "else if(" + c.GetNonDefaultCaseCode(i) + ")");
                            resultlist.Add(Indentation(indnt) + "{");
                            resultlist.AddRange(Printout(c.GetIDiagrams()[c.GetNonDefaultCaseIndex(i)], false, indnt + 1, codeFileName));
                        }
                        resultlist.Add(Indentation(indnt) + "}");
                        resultlist.Add(Indentation(indnt) + "else");
                        resultlist.Add(Indentation(indnt) + "{");
                        resultlist.AddRange(Printout(c.GetIDiagrams()[c.GetICaseSelector().GetDefaultDiagramIndex()], false, indnt + 1, codeFileName));
                        resultlist.Add(Indentation(indnt) + "}");
                    }
                    n.SetIsOccupied(true);
                }
            }

        }


        //Obtain the IDataAccessor Code
        public virtual string IDataAccessorCode(INode dataaccessor, IDiagram iDiagram)
        {
            string result = dataaccessor.GetName() + '=';
            if (!dataaccessor.GetIsDeclared())
            {
                result = Declaration(dataaccessor, true, 0, dataaccessor.GetName(), false) + '=';
                dataaccessor.SetIsDeclared(true);
            }

            INode mynode = dataaccessor.GetInputINodes()[0];
            return result + RightVariables(dataaccessor, mynode, iDiagram);
        }


        //Obtain the right constant code
        public virtual string RightConstantCode(string s, IConstant iconstant, int j, INode arithmeticnode)
        {
            bool flag = false;
            string _operator = "";
            if (arithmeticnode.GetINodeType() == "ICompoundArithmeticNode")
            {
                ICompoundArithmeticNode n = (ICompoundArithmeticNode)arithmeticnode;
                
                //Get the boolean of inverted inputs in ICompoundArithmeticNode
                flag = n.GetInvertedInputs()[j];  
                if (j > 0)
                {
                    _operator = ICompoundArithmeticNodeModeToString[n.GetMode()];
                }
            }
            else if (arithmeticnode.GetINodeType() == "IPrimitive")
            {
                IPrimitive n = (IPrimitive)arithmeticnode;
                if (n.GetMode() == IPrimitiveMode.ExSubtractPrimitive || n.GetMode() == IPrimitiveMode.ExDividePrimitive)
                {
                    if (j > 0)
                    {
                        _operator = IPrimitiveModeToString[n.GetMode()];
                    }
                }
                else
                {
                    if (j > 0)
                    {
                        _operator = ",";
                    }
                }

            }
            if (iconstant.GetIDataType().GetBase() == BaseDataType.IBoolean)
            {
                bool temp = true;
                if (iconstant.GetValue()=="true")
                {
                    temp = true;
                }
                else if (iconstant.GetValue()=="false")
                {
                    temp = false;
                }
                if (flag)
                {
                    if (temp)
                    {
                        s = s + _operator + "false";
                    }
                    else
                    {
                        s = s + _operator + "true";
                    }
                }
                else
                {
                    if (temp)
                    {
                        s = s + _operator + "true";
                    }
                    else
                    {
                        s = s + _operator + "false";
                    }
                }
            }
            else if (iconstant.GetIDataType().GetBase() == BaseDataType.ISignedInt || iconstant.GetIDataType().GetBase() == BaseDataType.IUnsignedInt)
            {
                int t = Convert.ToInt32(iconstant.GetValue());
                if (flag)
                {
                    //When the ICompoundArithmeticNode has inverted inputs
                    if (t < 0)
                    {
                        s = s + _operator + Convert.ToString(-t); //Become positive value
                    }
                    else
                    {
                        s = s + _operator + '(' + Convert.ToString(-t) + ')';  //Become negtive value
                    }
                }
                else
                {
                    if (t < 0)
                    {
                        s = s + _operator + '(' + iconstant.GetValue() + ')';
                    }
                    else
                    {
                        s = s + _operator + iconstant.GetValue();
                    }
                }
            }
            else if (iconstant.GetIDataType().GetBase() == BaseDataType.ISignedFixedPoint || iconstant.GetIDataType().GetBase() == BaseDataType.IUnsignedFixedPoint || iconstant.GetIDataType().GetBase() == BaseDataType.IDouble)
            {
                double t = Convert.ToDouble(iconstant.GetValue());
                if (flag)
                {
                    if (t < 0)
                    {
                        s = s + _operator + Convert.ToString(-t);
                    }
                    else
                    {
                        s = s + _operator + '(' + Convert.ToString(-t) + ')';
                    }
                }
                else
                {
                    if (t < 0)
                    {
                        s = s + _operator + '(' + iconstant.GetValue() + ')';
                    }
                    else
                    {
                        s = s + _operator + iconstant.GetValue();
                    }
                }
            }
            else if (iconstant.GetIDataType().GetBase() == BaseDataType.IArray || iconstant.GetIDataType().GetBase() == BaseDataType.IVariableSizedArray || iconstant.GetIDataType().GetBase() == BaseDataType.IFixedSizeArray)
            {
                if (flag)
                {
                    s = s + _operator + "(-" + iconstant.GetName() + ")";
                }
                else
                {
                    s = s + _operator + iconstant.GetName();
                }
            }
            else if (iconstant.GetIDataType().GetBase() == BaseDataType.IComplex)
            {
                if (flag)
                {
                    s = s + _operator + "(-" + iconstant.GetName() + ")";
                }
                else
                {
                    s = s + _operator + iconstant.GetName();
                }
            }
            return s;
        }

        //Obtain the right variable
        public virtual string RightVariable(string s, string name, int j, INode m)
        {
            bool flag = false;
            string _operator = "";
            if (m.GetINodeType() == "ICompoundArithmeticNode")
            {
                ICompoundArithmeticNode n = (ICompoundArithmeticNode)m;
                flag = n.GetInvertedInputs()[j];
                if (j > 0)
                {
                    _operator = ICompoundArithmeticNodeModeToString[n.GetMode()];
                }
            }
            else if (m.GetINodeType() == "IPrimitive")
            {
                IPrimitive n = (IPrimitive)m;
                if (n.GetMode() == IPrimitiveMode.ExSubtractPrimitive || n.GetMode() == IPrimitiveMode.ExDividePrimitive)
                {
                    if (j > 0)
                    {
                        _operator = IPrimitiveModeToString[n.GetMode()];
                    }
                }
                else
                {
                    if (j > 0)
                    {
                        _operator = ",";
                    }
                }

            }
            if (flag)
            {
                s = s + _operator + "(-" + name + ')';
            }
            else
            {
                s = s + _operator + name;
            }
            return s;
        }

        public virtual string RightVariables(INode m, INode mynode, IDiagram iDiagram)
        {
            string result = "";
            if (mynode.GetINodeType() == "IDataAccessor" || mynode.GetINodeType() == "IRightShiftRegister" || mynode.GetINodeType() == "ILoopIndex" || mynode.GetINodeType() == "ILoopMax" || mynode.GetINodeType() == "ICaseSelector" || mynode.GetINodeType() == "IFeedbackOutputNode")
            {
                result += mynode.GetName();
            }
            else if (mynode.GetINodeType() == "IConstant")
            {
                IConstant c = (IConstant)mynode;
                if (c.GetIDataType().GetBase() == BaseDataType.IArray || c.GetIDataType().GetBase() == BaseDataType.IVariableSizedArray || c.GetIDataType().GetBase() == BaseDataType.IFixedSizeArray || c.GetIDataType().GetBase() == BaseDataType.IComplex)
                {
                    result += c.GetName();
                }
                else
                {
                    result += c.GetValue();
                }

            }

            else if (mynode.GetINodeType() == "ITunnel")
            {
                ITunnel t = (ITunnel)mynode;
                if (t.IsIndexingITunnelInside(iDiagram))
                {
                    result += t.GetIndexingITunnelName();
                }
                else
                {
                    result += t.GetName();
                }
            }
            else if (mynode.GetINodeType() == "ILeftShiftRegister")
            {
                ILeftShiftRegister ls = (ILeftShiftRegister)mynode;
                result += ls.GetName(ls.GetOutputConnectionIndex(m));
            }
            else if (mynode.GetINodeType() == "ICompoundArithmeticNode")
            {
                if (INodeToName.ContainsKey(mynode))
                {
                    result += INodeToName[mynode];
                }
                else
                {
                    Console.WriteLine(" Error: INodeToName don't contain:" + mynode.GetName());
                }
            }
            else if (mynode.GetINodeType() == "IPrimitive")
            {
                IPrimitive p = (IPrimitive)mynode;
                if (p.GetMode() == IPrimitiveMode.ExSubtractPrimitive || p.GetMode() == IPrimitiveMode.ExDividePrimitive)
                {
                    if (INodeToName.ContainsKey(mynode))
                    {
                        result += INodeToName[mynode];
                    }
                    else
                    {
                        Console.WriteLine("INodeToName do not contains error!!" + mynode.GetName());
                    }
                }
                else
                {
                    result += p.GetNameByOutputConnection(m);
                }
            }
            else
            {
                // Other Situation
            }
            return result;
        }

        //Obtain the right expression
        public virtual string RightExpression(INode mynode, int j, INode temp, IDiagram iDiagram)
        {
            string result = "";
            if (mynode.GetINodeType() == "IConstant")
            {
                IConstant c = (IConstant)mynode;
                result = RightConstantCode(result, c, j, temp);
            }
            else if (mynode.GetINodeType() == "IDataAccessor" || mynode.GetINodeType() == "IRightShiftRegister" || mynode.GetINodeType() == "ILoopIndex" || mynode.GetINodeType() == "ILoopMax" || mynode.GetINodeType() == "ICaseSelector" || mynode.GetINodeType() == "IFeedbackOutputNode")
            {
                result = RightVariable(result, mynode.GetName(), j, temp);
            }
            else if (mynode.GetINodeType() == "ITunnel")
            {
                ITunnel t = (ITunnel)mynode;
                if (t.IsIndexingITunnelInside(iDiagram))
                {
                    result = RightVariable(result, t.GetIndexingITunnelName(), j, temp);
                }
                else
                {
                    result = RightVariable(result, t.GetName(), j, temp);
                }
            }


            else if (mynode.GetINodeType() == "ILeftShiftRegister")
            {
                ILeftShiftRegister ls = (ILeftShiftRegister)mynode;
                int len = ls.GetOutputTerminals().Count;
                result = RightVariable(result, ls.GetName(ls.GetOutputConnectionIndex(temp)), j, temp);

            }
            else if (mynode.GetINodeType() == "ICompoundArithmeticNode")
            {
                result = RightVariable(result, INodeToName[mynode], j, temp);
            }
            else if (mynode.GetINodeType() == "IPrimitive")
            {
                IPrimitive p = (IPrimitive)mynode;
                if (p.GetMode() == IPrimitiveMode.ExSubtractPrimitive || p.GetMode() == IPrimitiveMode.ExDividePrimitive)
                {
                    result = RightVariable(result, INodeToName[mynode], j, temp);
                }
                else
                {
                    result = RightVariable(result, p.GetNameByOutputConnection(temp), j, temp);
                }
            }
            else
            {
                Console.WriteLine("ICompoundArithmeticNode do not recognize this this input INode" + mynode.GetNodeId() + " :" + mynode.GetINodeType());
            }
            return result;
        }

        //Obtain the left variable in an expression
        public virtual string LeftVariable(string result, INode currentnode, IDiagram iDiagram)
        {
            //Get the datatype of current node when it's declared
            if (!currentnode.GetIsDeclared())
            {
                result = currentnode.GetOutputTerminalIDataType(0).GetDeclarationDataType() + ' ' + result;
            }


            string left = currentnode.GetName();
            foreach (INode mynode in currentnode.GetOutputINodes())
            {
                if (mynode.GetINodeType() == "IDataAccessor" || mynode.GetINodeType() == "IRightShiftRegister" || mynode.GetINodeType() == "ILoopMax" || mynode.GetINodeType() == "ICaseSelector") 
                {
                    mynode.SetIsOccupied(true);
                    left = mynode.GetName();
                    if (mynode.GetIsDeclared())
                    {
                        result = mynode.GetName();

                    }
                    else
                    {
                        //Because the input terminal and output terminal have the same datatype
                        result = Declaration(mynode, true, 0, mynode.GetName(), false);
                        mynode.SetIsDeclared(true);
                    }
                    break;
                }

                else if (mynode.GetINodeType() == "ILeftShiftRegister")
                {
                    ILeftShiftRegister ls = (ILeftShiftRegister)mynode;
                    ls.SetIsOccupied(true, ls.GetInputConnectionIndex(currentnode));

                    //Obtain the name of ILeftShiftRegister
                    left = ls.GetName(ls.GetInputConnectionIndex(currentnode));
                    if (ls.GetIsDeclared(ls.GetInputConnectionIndex(currentnode)))
                    {
                        result = ls.GetName(ls.GetInputConnectionIndex(currentnode));
                    }
                    else
                    {
                        result = Declaration(mynode, true, 0, ls.GetName(ls.GetInputConnectionIndex(currentnode)), false);
                        ls.SetIsDeclared(true, ls.GetInputConnectionIndex(currentnode));
                    }

                    break;
                }
                else if (mynode.GetINodeType() == "ITunnel")
                {
                    ITunnel t = (ITunnel)mynode;
                    if (t.IsIndexingITunnelInside(iDiagram))
                    {
                        left = t.GetIndexingITunnelName();
                        if (mynode.GetIsDeclared())
                        {
                            result = t.GetIndexingITunnelName();
                        }
                        else
                        {
                            result = Declaration(mynode, true, 0, t.GetIndexingITunnelName(), false);
                            t.SetIsDeclared(true);
                        }
                    }
                    else
                    {
                        left = t.GetName();
                        if (mynode.GetIsDeclared())
                        {
                            result = t.GetName();
                        }
                        else
                        {
                            result = Declaration(mynode, true, 0, t.GetName(), false);
                            t.SetIsDeclared(true);
                        }

                    }
                    t.SetIsOccupied(true);

                    break;
                }
                //Currently it doesn't support IFeedbackOutputNode
                else if (mynode.GetINodeType() == "IFeedbackOutputNode")
                {
                    IFeedbackOutputNode fo = (IFeedbackOutputNode)mynode;
                    left = fo.GetInitName();
                    if (mynode.GetIsDeclared())
                    {
                        result = left;

                    }
                    else
                    {
                        result = Declaration(mynode, true, 0, left, false);
                        mynode.SetIsDeclared(true);
                    }
                    break;
                }
            }
            INodeToName[currentnode] = left;
            currentnode.SetIsDeclared(true);
            return result;
        }


        //Process the ICompoundArithmeticNode and get its code
        public virtual string ICompoundArithmeticNodeCode(ICompoundArithmeticNode cp, IDiagram iDiagram)
        {
            string result = "";
            bool flag = cp.GetInvertedOutput();
            string left = LeftVariable(cp.GetName(), cp, iDiagram);  //Get left variable in the expression

            string RightExpression = "";
            for (int j = 0; j < cp.GetInputINodes().Count; j++)
            {
                INode mynode = cp.GetInputINodes()[j];
                RightExpression += this.RightExpression(mynode, j, cp, iDiagram);
            }
            if (flag)
            {
                result = left + "=-(" + RightExpression + ")";
            }
            else
            {
                result = left + '=' + RightExpression;
            }
            return result;
        }

        //For IPrimitive (mode= ExSubtractIPrimitive, ExDivideIPrimitive)
        public virtual string ArithmeticIPrimitiveCode(IPrimitive ap, IDiagram iDiagram)
        {
            string left = LeftVariable(ap.GetName(), ap, iDiagram); //Get left variable in the expression
            int size = ap.GetInputINodes().Count;
            string RightExpression = "";
            for (int j = 0; j < size; j++)
            {
                //The input terminal order fo ExSubtractIPrimitive and ExDivideIPrimitive is the opposite as ICompoundArithmeticNode
                INode mynode = ap.GetInputINodes()[size - 1 - j]; 
                RightExpression += this.RightExpression(mynode, j, ap, iDiagram);
            }
            return left + '=' + RightExpression;
        }

        public virtual string IPrimitiveCallCode(IPrimitive iprimitivenode, IDiagram iDiagram)
        {
            string functionName = iprimitivenode.GetMode().ToString();
            string parameterList = "";

            int inputsize = iprimitivenode.GetInputINodes().Count;
            int outputsize = iprimitivenode.GetOutputTerminals().Count;

            INode mynode;
            for (int i = 0; i < inputsize; i++)
            {
                mynode = iprimitivenode.GetInputINodes()[i];
                parameterList += RightExpression(mynode, i, iprimitivenode, iDiagram);
            }
            for (int j = 0; j < outputsize; j++)
            {
                if (j == 0 && inputsize == 0)
                {
                    parameterList += iprimitivenode.GetName() + "_" + Convert.ToString(j);
                }
                else
                {
                    parameterList += "," + iprimitivenode.GetName() + "_" + Convert.ToString(j);
                }
            }

            return functionName + '(' + parameterList + ')';
        }

        public virtual string FunctionDeclaration(IPrimitive p)
        {
            string result = "";
            string parameterList = "";
            for (int i = 0; i < p.GetInputTerminals().Count; i++)
            {
                if (i == 0)
                {
                    parameterList += Declaration(p, true, i, "x" + Convert.ToString(i), true);
                }
                else
                {
                    parameterList += "," + Declaration(p, true, i, "x" + Convert.ToString(i), true);
                }
            }
            for (int i = 0; i < p.GetOutputTerminals().Count; i++)
            {
                if (i == 0 && p.GetInputTerminals().Count == 0)
                {
                    parameterList += Declaration(p, false, i, "y" + Convert.ToString(i), true);
                }
                else
                {
                    parameterList += "," + Declaration(p, false, i, "y" + Convert.ToString(i), true);
                }
            }
            result = "void " + p.GetMode() + "(" + parameterList + ")\n{";
            return result;
        }

        //Generate the IPrimitive function
        public virtual List<string> GenerateIPrimitive(IPrimitive iprimitive)
        {

            List<string> result = new List<string>();
            //Add declaration
            result.Add(FunctionDeclaration(iprimitive));
            //Get the inner code
            iprimitive.GetIPrimitiveCCode(result);
            result.Add("}");
            return result;
        }


        //Obtain the code for ILeftShiftRegister
        public virtual string ILeftShiftRegisterCode(ILeftShiftRegister leftshiftregister, int index, IDiagram iDiagram)
        {
            string result = leftshiftregister.GetName(index) + '=';
            if (!leftshiftregister.GetIsDeclared(index))
            {
                result = Declaration(leftshiftregister, true, index, leftshiftregister.GetName(index), false) + '=';
                leftshiftregister.SetIsDeclared(true, index);
            }
            INode mynode = leftshiftregister.GetInputINodes()[index];
            return result + RightVariables(leftshiftregister, mynode, iDiagram);
        }

        //To correspond the code for both IRightShiftRegister and ILeftShiftRegister
        public virtual List<string> ShiftRegisterCode(List<IRightShiftRegister> rslist, int indnt)
        {
            List<string> result = new List<string>();
            foreach (IRightShiftRegister rs in rslist)
            {
                ILeftShiftRegister ls = (ILeftShiftRegister)INode.GetIdToINode()[rs.GetAssociatedLeftShiftRegister().GetNodeId()];

                //Output the code for the extended ILeftShiftRegister
                int length = ls.GetInputTerminals().Count - 1;
                for (int i = length; i > 0; i--)
                {
                    result.Add(AddSemicolon(indnt, ls.GetName(i) + "=" + ls.GetName(i - 1)));
                }
                result.Add(AddSemicolon(indnt, ls.GetName(0) + "=" + rs.GetName()));
            }
            return result;
        }

        public virtual string InitFeedbackCode(IFeedbackOutputNode feedbackoutput, IDiagram iDiagram)
        {
            string result = feedbackoutput.GetInitName();
            if (!feedbackoutput.GetIsDeclared())
            {
                result = Declaration(feedbackoutput, true, 0, feedbackoutput.GetInitName(), false);
                feedbackoutput.SetIsDeclared(true);
            }

            INode mynode = feedbackoutput.GetInputINodes()[0];
            return result + '=' + RightVariables(feedbackoutput, mynode, iDiagram);
        }

        //Obtain the code for ITunnel
        public virtual string ITunnelCode(INode tunnel, IDiagram iDiagram)
        {
            string result = tunnel.GetName() + '=';
            if (!tunnel.GetIsDeclared())
            {
                result = Declaration(tunnel, true, 0, tunnel.GetName(), false) + '=';
                tunnel.SetIsDeclared(true);
            }

            INode mynode = tunnel.GetInputINodes()[iDiagram.getDiagramIndex()];
            return result + RightVariables(tunnel, mynode, iDiagram);
        }

        //Obtain the code for Indexing ITunnel
        public virtual string IndexingITunnelCode(ITunnel indexingtunnel, IDiagram iDiagram)
        {
            string result = indexingtunnel.GetIndexingITunnelName() + '=';
            if (!indexingtunnel.GetIsDeclared())
            {
                result = Declaration(indexingtunnel, true, 0, indexingtunnel.GetIndexingITunnelName(), false) + '=';
                indexingtunnel.SetIsDeclared(true);
            }
            INode mynode = indexingtunnel.GetInputINodes()[0];
            return result + RightVariables(indexingtunnel, mynode, iDiagram);
        }



        //Naming System

        //For normal node's declaration according to its isInput, index, nodename and citation
        public virtual string Declaration(INode n, bool isInput, int index, string nodename, bool citation)
        {
            string result = "";
            if (n.GetINodeType() == "IConstant")
            {
                IConstant c = (IConstant)n;
                result = SubIConstantDeclaration(c.GetIDataType(), nodename, citation);
            }
            else if (!isInput && n.GetOutputTerminalIDataType(index) != null)
            {
                result = SubDeclaration(n.GetOutputTerminalIDataType(index), nodename, citation);
            }
            else if (isInput && n.GetInputTerminalIDataType(index) != null)
            {
                result = SubDeclaration(n.GetInputTerminalIDataType(index), nodename, citation);
            }

            return result;
        }

        public virtual string SubIConstantDeclaration(IDataType dataaccessor, string name, bool citation)
        {
            string result = "";
            string _operator = "";
            if (citation)
            {
                _operator = "&";
            }
            if (dataaccessor.GetBase() == BaseDataType.IArray || dataaccessor.GetBase() == BaseDataType.IVariableSizedArray)
            {
                result = dataaccessor.GetDeclarationDataType() + _operator + ' ' + name + "[]";
            }
            else if (dataaccessor.GetBase() == BaseDataType.IFixedSizeArray)
            {
                result = dataaccessor.GetDeclarationDataType() + _operator + ' ' + name + "[" + dataaccessor.GetArraySize() + "]";
            }
            else
            {
                result = dataaccessor.GetDeclarationDataType() + _operator + ' ' + name;
            }
            return result;
        }

        public virtual string SubDeclaration(IDataType datatype, string name, bool citation)
        {
            string result = "";
            string _operator = "";
            if (citation)
            {
                _operator = "&";
            }
            if (datatype.GetBase() == BaseDataType.IArray || datatype.GetBase() == BaseDataType.IVariableSizedArray)
            {
                result = "Array<" + datatype.GetDeclarationDataType() + ">" + _operator + ' ' + name;
            }
            else if (datatype.GetBase() == BaseDataType.IFixedSizeArray)
            {

                result = datatype.GetDeclarationDataType() + " " + name + "[" + datatype.GetArraySize() + "]";
            }
            else
            {
                result = datatype.GetDeclarationDataType() + _operator + ' ' + name;
            }
            return result;
        }


        //For the border nodes ILeftShiftRegister,ITunnel,IRightShiftRegister's declaration
        public virtual List<string> BorderNodeDeclaration(IForLoop f, IDiagram iDiagram, int indnt)
        {
            List<string> resultlist = new List<string>();
            foreach (INode mynode in f.GetInputBorderNodes())
            {
                string result = "";
                if (mynode.GetINodeType() == "ILeftShiftRegister")
                {
                    ILeftShiftRegister ls = (ILeftShiftRegister)mynode;
                    for (int index = 0; index < ls.GetInputINodes().Count; index++)
                    {
                        if (!ls.GetIsDeclared(index))
                        {
                            result = Declaration(mynode, true, 0, ls.GetName(index), false);
                            ls.SetIsDeclared(true, index);
                        }
                    }
                }
                else if (mynode.GetINodeType() == "ITunnel")
                {
                    ITunnel t = (ITunnel)mynode;
                    if (mynode.GetIsDeclared())
                    {
                        continue;
                    }
                    if (t.IsIndexingITunnelInside(iDiagram))
                    {
                        result = Declaration(mynode, true, 0, t.GetIndexingITunnelName(), false);
                        t.SetIsDeclared(true);
                    }
                    else
                    {
                        result = Declaration(mynode, true, 0, t.GetName(), false);
                        t.SetIsDeclared(true);
                    }

                }
                if (!result.Equals(""))
                {
                    resultlist.Add(AddSemicolon(indnt, result));
                }
            }
            foreach (INode mynode in f.GetOutputBorderNodes())
            {
                string result = "";
                if (mynode.GetIsDeclared())
                {
                    continue;
                }
                if (mynode.GetINodeType() == "IRightShiftRegister")
                {
                    IRightShiftRegister rs = (IRightShiftRegister)mynode;
                    result = Declaration(rs, true, 0, rs.GetName(), false);
                    rs.SetIsDeclared(true);
                }
                else if (mynode.GetINodeType() == "ITunnel")
                {
                    ITunnel t = (ITunnel)mynode;
                    if (t.IsIndexingITunnelInside(iDiagram))
                    {
                        result = Declaration(mynode, false, 0, t.GetIndexingITunnelName(), false);
                        t.SetIsDeclared(true);
                    }
                    else
                    {
                        result = Declaration(mynode, false, 0, t.GetName(), false);
                        t.SetIsDeclared(true);
                    }

                }
                if (!result.Equals(""))
                {
                    resultlist.Add(AddSemicolon(indnt, result));
                }
            }
            return resultlist;
        }

        //Judge this group need declaration or not
        public virtual bool IsDeclarationGroup(INode n)
        {
            bool result = false;
            if (n.GetINodeType() == "IDataAccessor")
            {
                IDataAccessor da = (IDataAccessor)n;
                if (da.GetDirection() == Direction.INPUT)
                {
                    result = true;
                }
            }
            // For array IConstants and complex declaration
            else if (n.GetINodeType() == "IConstant")
            {
                IConstant c = (IConstant)n;
                if (c.GetIDataType().GetBase() == BaseDataType.IArray || c.GetIDataType().GetBase() == BaseDataType.IVariableSizedArray || c.GetIDataType().GetBase() == BaseDataType.IFixedSizeArray || c.GetIDataType().GetBase() == BaseDataType.IComplex)
                {
                    result = true;
                }
            }
            return result;
        }


        //The border node declaration only for ICaseStructure
        public virtual List<string> CaseBorderNodeDeclaration(ICaseStructure c, IDiagram iDiagram, int indnt)
        {
            List<string> resultlist = new List<string>();
            foreach (INode mynode in c.GetITunnels())
            {
                string result = "";
                ITunnel t = (ITunnel)mynode;
                if (mynode.GetIsDeclared())
                {
                    continue;
                }
                if (t.IsIndexingITunnelInside(iDiagram))
                {
                    result = Declaration(mynode, true, 0, t.GetIndexingITunnelName(), false);
                    t.SetIsDeclared(true);
                }
                else
                {
                    result = Declaration(mynode, true, 0, t.GetName(), false);
                    t.SetIsDeclared(true);
                }
                if (!result.Equals(""))
                {
                    resultlist.Add(AddSemicolon(indnt, result));
                }
            }
            return resultlist;
        }


       
        //Code Organization System

        //Add the semicolon for every statement
        public virtual string AddSemicolon(int indnt, string code)
        {
            return Indentation(indnt) + code + ';';
        }

        //Add the indentation for every statment to make it look more readable.
        public virtual string Indentation(int indnt)
        {
            string s = "";
            for (int i = 0; i < indnt; i++)
            {
                s += "  ";
            }
            return s;
        }


        //For the datatype formatting of scanf_s() and printf()
        public virtual string Formatting(INode n)
        {

            IDataAccessor dataaccesor = (IDataAccessor)n;
            string s = "";
            if (dataaccesor.GetDirection() == Direction.INPUT)
            {
                if ((dataaccesor.GetOutputTerminalIDataType(0).GetBase() == BaseDataType.ISignedInt) || (dataaccesor.GetOutputTerminalIDataType(0).GetBase() == BaseDataType.IUnsignedInt)|| (dataaccesor.GetOutputTerminalIDataType(0).GetBase() == BaseDataType.IBoolean)||(dataaccesor.GetOutputTerminalIDataType(0).GetBase()==BaseDataType.IFixedSizeArray))
                    s = "%d";
                else if ((dataaccesor.GetOutputTerminalIDataType(0).GetBase() == BaseDataType.ISignedFixedPoint) || (dataaccesor.GetOutputTerminalIDataType(0).GetBase() == BaseDataType.IUnsignedFixedPoint))
                    s = "%lf";
            }
            if (dataaccesor.GetDirection() == Direction.OUTPUT)
            {
                if ((dataaccesor.GetInputTerminalIDataType(0).GetBase() == BaseDataType.ISignedInt) || (dataaccesor.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IUnsignedInt)|| (dataaccesor.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IBoolean)||(dataaccesor.GetInputTerminalIDataType(0).GetBase()==BaseDataType.IFixedSizeArray))
                    s = "%d";
                else if ((dataaccesor.GetInputTerminalIDataType(0).GetBase() == BaseDataType.ISignedFixedPoint) || (dataaccesor.GetInputTerminalIDataType(0).GetBase() == BaseDataType.IUnsignedFixedPoint))
                    s = "%lf";
            }
            return s;
        }


        


        //Print out the .c file
        public virtual List<string> Printout(IDiagram iDiagram, bool isTopIDiagram, int indnt, string cfilename)
        {
            GetZeroIndegreeNodes zeroIndegreeNodes = new GetZeroIndegreeNodes();
            TopologicalSorting sortOrder = new TopologicalSorting();
            List<INode> sortednodes = new List<INode>();
            List<string> resultlist = new List<string>();

            //Get the zero indegree nodes list
            LinkedList<INode> zeroindegreenodes = zeroIndegreeNodes.ZeroIndegreeNodesList(iDiagram, isTopIDiagram);
            //Sort all the nodes by topological sorting
            sortOrder.Sorting(zeroindegreenodes, sortednodes, iDiagram);

            //Process all of the sorted nodes
            Process(sortednodes, resultlist, iDiagram, indnt + 1,cfilename);

            if (isTopIDiagram)
            {
                headCode.Add("#include<stdio.h>");
                headCode.Add("#include<stdlib.h>");
                if (IDiagram.getHasComplex())
                {
                    headCode.Add("#include<complex>");
                }
                if (IDiagram.getHasCmath())
                {
                    headCode.Add("#include<cmath>");
                }

                
                using (StreamWriter C_Code = new StreamWriter(cfilename)) //Output the C code in .c file
                {
                    Console.WriteLine();
                    foreach (string s in headCode)
                    {
                        Console.WriteLine(s);
                        C_Code.WriteLine(s);
                    }

                    foreach (string s in IPrimitiveFunctions)
                    {
                        Console.WriteLine(s);
                        C_Code.WriteLine(s);
                    }

                    Console.WriteLine("\nint main()\n{");
                    C_Code.WriteLine("\nint main()\n{");

                    foreach (string s in resultlist)
                    {
                        Console.WriteLine(s);
                        C_Code.WriteLine(s);
                    }
                    Console.WriteLine(Indentation(indnt + 1) + "return 0;");
                    Console.WriteLine("}");

                    C_Code.WriteLine("\n" + Indentation(indnt + 1) + "system(\"pause\");");
                    C_Code.WriteLine(Indentation(indnt + 1) + "return 0;\n}\n");

                }

            }

            return resultlist;

        }

    }
}
