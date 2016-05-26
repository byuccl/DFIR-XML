using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class ICompoundArithmeticNode:INode
    {
        private ICompoundArithmeticNodeMode mode;
        private List<bool> invertedInputs;
        private bool invertedOutput;
        public ICompoundArithmeticNode(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, ICompoundArithmeticNodeMode Mode, XmlNodeList InvertedInputs, XmlNode InvertedOutput) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            mode = Mode;
            invertedInputs = new List<bool>();
            for (int i = 0; i < InvertedInputs.Count; i++)
            {
                //Set the inverted conditions for input terminals
                if (InvertedInputs[i].InnerText.Equals("False"))
                {
                    invertedInputs.Add(false);
                }
                else
                {
                    invertedInputs.Add(true);
                }
            }
            //Set the inverted condition for output terminal
            if (InvertedOutput.InnerText.Equals("False"))
            {
                invertedOutput = false;
            }
            else
            {
                invertedOutput = true;
            }
            INodeType = "ICompoundArithmeticNode";
        }


        public virtual ICompoundArithmeticNodeMode GetMode()
        {
            return mode;
        }
        public virtual List<bool> GetInvertedInputs()
        {
            return invertedInputs;
        }
        public virtual bool GetInvertedOutput()
        {
            return invertedOutput;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ICompoundArithmeticNode:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId + "\nMode: " + mode);
            for (int i = 0; i < invertedInputs.Count; i++)
            {
                Console.WriteLine("InvertedInput: " + invertedInputs[i]);
            }
            Console.WriteLine("InvertedOutput: " + invertedOutput);
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
