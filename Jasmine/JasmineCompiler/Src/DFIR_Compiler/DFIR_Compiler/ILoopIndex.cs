using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class ILoopIndex:IBorderNode
    {
        public ILoopIndex(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            INodeType = "ILoopIndex";
        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ILoopIndex:");
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
