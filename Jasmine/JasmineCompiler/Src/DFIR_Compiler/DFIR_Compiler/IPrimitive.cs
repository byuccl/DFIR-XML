using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class IPrimitive:INode
    {
        private IPrimitiveMode mode;
        public IPrimitive(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, IPrimitiveMode mode) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            this.mode = mode;
            INodeType = "IPrimitive";
        }

        public virtual IPrimitiveMode GetMode()
        {
            return mode;
        }

        public virtual string GetNameByOutputIndex(int index)
        {
            string res = "";
            res = GetName() + '_' + Convert.ToString(index);
            return res;
        }
        public virtual string GetNameByOutputConnection(INode n)
        {
            string res = "";
            res = GetNameByOutputIndex(GetOutputConnectionIndex(n));
            return res;
        }

        //The IPrimitive Function
        public virtual List<string> ExSelectPrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "if(x1 == true)");
            res.Add(Indentation(2) + "y0 = x2;");
            res.Add(Indentation(1) + "else");
            res.Add(Indentation(2) + " y0 = x0;");
            return res;
        }
        public virtual List<string> ExIncrementPrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "y0 = x0+1;");
            return res;
        }
        public virtual List<string> ExDecrementPrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "y0 = x0-1;");
            return res;
        }
        public virtual List<string> ExAbsoluteValuePrimitive()
        {
            List<string> res = new List<string>();
            IDiagram.setHasCmath(true);
            res.Add(Indentation(1) + "y0 = abs(x0);");
            return res;
        } 
        public virtual List<string> ExAddArrayElementsPrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "y0 = x0-1;");
            return res;
        }
        public virtual List<string> ExScaleByPowerOf2Primitive()
        {
            List<string> res = new List<string>();
            IDiagram.setHasCmath(true);
            res.Add(Indentation(1) + "y0 = x0 * pow(2.0, x1);");
            return res;
        }
        public virtual List<string> ExSquareRootPrimitive()
        {
            List<string> res = new List<string>();
            IDiagram.setHasCmath(true);
            res.Add(Indentation(1) + "y0 = sqrt(x0);");
            return res;
        }
        public virtual List<string> ExSquarePrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "y0 = x0*x0;");
            return res;
        }
        public virtual List<string> ExNegatePrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "y0 = -x0;");
            return res;
        }
        public virtual List<string> ExReciprocalPrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "y0 = 1/x0;");
            return res;
        }
        public virtual List<string> ExMaxAndMinPrimitive()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "if(x0 >= x1){");
            res.Add(Indentation(2) + "y0 = x1;");
            res.Add(Indentation(2) + "y1 = x0;");
            res.Add(Indentation(1) + "}else{");
            res.Add(Indentation(2) + "y0 = x0;");
            res.Add(Indentation(2) + "y1 = x1;");
            res.Add(Indentation(1) + "}");
            return res;
        }
        public virtual List<string> ExDecrementPrimitive1()
        {
            List<string> res = new List<string>();
            res.Add(Indentation(1) + "return p0-1;");
            return res;
        }

        public virtual string Indentation(int indnt)
        {
            string s = "";
            for (int i = 0; i < indnt; i++)
            {
                s += "  ";
            }
            return s;
        }

        public virtual void GetIPrimitiveCCode(List<string> res)
        {
            if (mode == IPrimitiveMode.ExSelectPrimitive)
            {
                res.AddRange(ExSelectPrimitive());
            }
            else if (mode == IPrimitiveMode.ExIncrementPrimitive)
            {
                res.AddRange(ExIncrementPrimitive());
            }
            else if (mode == IPrimitiveMode.ExDecrementPrimitive)
            {
                res.AddRange(ExDecrementPrimitive());
            }
            else if (mode == IPrimitiveMode.ExAbsoluteValuePrimitive)
            {
                res.AddRange(ExAbsoluteValuePrimitive());
            }
            else if (mode == IPrimitiveMode.ExAddArrayElementsPrimitive)
            {
                res.AddRange(ExAddArrayElementsPrimitive());
            }
            else if (mode == IPrimitiveMode.ExScaleByPowerOf2Primitive)
            {
                res.AddRange(ExScaleByPowerOf2Primitive());
            }
            else if (mode == IPrimitiveMode.ExSquareRootPrimitive)
            {
                res.AddRange(ExSquareRootPrimitive());
            }
            else if (mode == IPrimitiveMode.ExSquarePrimitive)
            {
                res.AddRange(ExSquarePrimitive());
            }
            else if (mode == IPrimitiveMode.ExNegatePrimitive)
            {
                res.AddRange(ExNegatePrimitive());
            }
            else if (mode == IPrimitiveMode.ExReciprocalPrimitive)
            {
                res.AddRange(ExReciprocalPrimitive());
            }
            else if (mode == IPrimitiveMode.ExMaxAndMinPrimitive)
            {
                res.AddRange(ExMaxAndMinPrimitive());
            }
        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IPrimitive:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId + "\nMode: " + mode);
            for (int i = 0; i < inputTerminals.Count; i++)
            {
                inputTerminals[i].Print();
            }
            for (int i = 0; i < outputTerminals.Count; i++)
            {
                outputTerminals[i].Print();
            }
        }

    }
}
