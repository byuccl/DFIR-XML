using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class ITunnel:IBorderNode
    {        
        private bool isInput;
        private ITerminal innerTerminal;
        private ITerminal outerTerminal;
        private IndexingMode indexingMode;

        public ITunnel(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, IndexingMode indexingMode, IsInputMode isInput, int innerTerminalId, int outerTerminalId) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            if (isInput == IsInputMode.False)
            {
                this.isInput = false;
            }
            else
            {
                this.isInput = true;
            }
            innerTerminal = ITerminal.GetIdToITerminal()[innerTerminalId];
            outerTerminal = ITerminal.GetIdToITerminal()[outerTerminalId];
            SetIndexingMode(indexingMode);
            INodeType = "ITunnel";
            indegree = 1;
        }

        public virtual bool GetIsInput()
        {
            return isInput;
        }
        public virtual ITerminal GetInnerTerminal()
        {
            return innerTerminal;
        }
        public virtual ITerminal GetOuterTerminal()
        {
            return outerTerminal;
        }
        public virtual IndexingMode GetIndexingMode()
        {
            return indexingMode;
        }
        public override INode GetParentINode()
        {
            return IdToINode[parentId];
        }


        public virtual void SetIndexingMode(IndexingMode indexingMode)
        {
            this.indexingMode = indexingMode;
        }

        public virtual string GetILoopIndexName()
        {
            IForLoop forloop = (IForLoop)GetParentINode();
            return forloop.GetILoopIndex().GetName();
        }
        public virtual string GetIndexingITunnelName()
        {
            return GetName() + '[' + GetILoopIndexName() + ']';
        }


        public virtual bool IsOutputCaseITunnel()
        {
            bool result = false;
            if (GetParentINode().GetINodeType() == "ICaseStructure")
            {
                if (!GetIsInput())
                {
                    result = true;
                }
            }
            return result;
        }

        public virtual bool IsIndexingITunnelInside(IDiagram iDiagram)
        {
            return( (IsBorderNodeInside(iDiagram)) && (indexingMode == IndexingMode.Indexing));
        }

        public override void DecreaseIndegree()
        {
            indegree--;
            if (isInput)
            {
                IdToINode[parentId].indegree--;
            }

        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ITunnel:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            Console.WriteLine("IsInput: " + GetIsInput());
            Console.WriteLine("GetInnerTerminal: " + GetInnerTerminal().GetITerminalId());
            Console.WriteLine("GetOuterTerminal: " + GetOuterTerminal().GetITerminalId());
            Console.WriteLine("IndexingMode: " + GetIndexingMode());
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
