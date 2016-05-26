using System;

namespace DFIR_Compiler
{
    public class AssociatedRightShiftRegister
    {
        private int nodeId;
        private int parentId;

        public AssociatedRightShiftRegister(int NodeId, int ParentId)
        {
            nodeId = NodeId;
            parentId = ParentId;
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
