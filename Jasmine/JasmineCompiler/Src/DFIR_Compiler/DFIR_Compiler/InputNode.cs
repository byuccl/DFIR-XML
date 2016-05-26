using System;

namespace DFIR_Compiler
{
    public class InputNode
    {
        private int parentId;
        private int nodeId;
        public InputNode(int NodeId, int ParentId)
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


        public virtual void SetNodeId(int NodeId)
        {
            nodeId = NodeId;
        }
        public virtual void SetParentId(int parentId)
        {
            this.parentId = parentId;
        }


        public virtual void Print()
        {
            Console.WriteLine("InputNode:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
        }
    }
}
