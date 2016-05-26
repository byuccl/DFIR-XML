using System;


namespace DFIR_Compiler
{
    public class OutputNode
    {
        private int parentId;
        private int nodeId;

        public OutputNode(int NodeId, int ParentId)
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
        public virtual void SetParentId(int ParentId)
        {
            parentId = ParentId;
        }


        public virtual void Print()
        {
            Console.WriteLine("OutputNode:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
        }

    }
}
