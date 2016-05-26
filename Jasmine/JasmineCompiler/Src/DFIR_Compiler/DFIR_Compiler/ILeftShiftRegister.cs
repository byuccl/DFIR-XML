using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class ILeftShiftRegister:IBorderNode
    {
        private AssociatedRightShiftRegister associatedRightShiftRegister;
        private List<bool> isOccupieds;
        private List<bool> isDeclareds;
        public ILeftShiftRegister(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, AssociatedRightShiftRegister _associatedRightShiftRegister) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            associatedRightShiftRegister = _associatedRightShiftRegister;            
            isOccupieds = new List<bool>();
            isDeclareds = new List<bool>();
            for (int i = 0; i < this.inputTerminals.Count; i++)
            {
                isOccupieds.Add(false);
                isDeclareds.Add(false);
            }
            INodeType = "ILeftShiftRegister";
        }


        public virtual AssociatedRightShiftRegister GetAssociatedRightShiftRegister()
        {
            return associatedRightShiftRegister;
        }
        public virtual List<bool> GetIsOccupieds()
        {
            return isOccupieds;
        }
        public virtual bool GetIsDeclared(int index)
        {
            return isDeclareds[index];
        }
        public override INode GetParentINode()
        {
            return IdToINode[parentId];
        }
        public virtual string GetName(int index)
        {
            string lsname;
            lsname = GetINodeType() + Convert.ToString(GetNodeId()) + "_" + Convert.ToString(index);
            return lsname;
        }


        public virtual void SetIsOccupied(bool isoccupied, int index)
        {
            isOccupieds[index] = isoccupied;
        }
        public virtual void SetIsDeclared(bool isdeclared, int index)
        {
            isDeclareds[index] = isdeclared;
        }


        public override void DecreaseIndegree()
        {
            indegree--;
            if (indegree == 0)
            {
                IdToINode[parentId].indegree--;
            }
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ILeftShiftRegister:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            associatedRightShiftRegister.Print();
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
