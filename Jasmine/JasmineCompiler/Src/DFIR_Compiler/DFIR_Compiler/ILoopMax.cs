using System;
using System.Collections.Generic;

namespace DFIR_Compiler
{
    public class ILoopMax:IBorderNode
    {

        public ILoopMax(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            INodeType = "ILoopMax";
        }


        public override void DecreaseIndegree()
        {
            indegree--;
            IdToINode[parentId].indegree--;
        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ILoopMax:");
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
