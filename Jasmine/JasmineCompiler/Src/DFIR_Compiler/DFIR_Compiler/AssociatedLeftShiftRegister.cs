using System;

namespace DFIR_Compiler
{
    public class AssociatedLeftShiftRegister
    {
        private int nodeId;
        private int parentId;
        
        public AssociatedLeftShiftRegister(int NodeId, int ParentId)
        {
            parentId = ParentId;
            nodeId = NodeId;
        }

        public virtual int GetNodeId()
        {
            return nodeId;
        }
        public virtual int GetParentId()
        {
            return parentId;
        }

        public virtual void Print()
        {
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
        }

    }
}
