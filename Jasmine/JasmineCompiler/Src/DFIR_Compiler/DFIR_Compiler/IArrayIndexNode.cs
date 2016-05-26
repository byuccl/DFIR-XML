using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class IArrayIndexNode:INode
    {
        public IArrayIndexNode(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            INodeType = "IArrayIndexNode";
        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IArrayIndexNode:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
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
