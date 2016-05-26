using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class IFeedbackOutputNode:INode
    {
        private InputNode inputNode;

        public IFeedbackOutputNode(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, InputNode _inputNode) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            inputNode = _inputNode;
            INodeType = "IFeedbackOutputNode";
            indegree++; //Because the IFeedbackOutputNode has the vitual connection with IFeedbackInputNode
        }

        public virtual InputNode GetInputNode()
        {
            return inputNode;
        }
        public virtual IFeedbackInputNode GetIFeedbackInputNode()
        {
            return (IFeedbackInputNode)GetIdToINode()[inputNode.GetNodeId()];
        }
        public virtual string GetInitName()
        {
            if (GetIsInputConnected())
            {
                return "Init" + Convert.ToString(nodeId);
            }
            return "0";
        }

        public virtual void SetInputNode(InputNode _inputNode)
        {
            inputNode = _inputNode;
        }


        public virtual string GetLoopIndexName()
        {
            if (GetParentINode().GetParentINode() != null)
            {
                if (GetParentINode().GetParentINode().GetINodeType() == "IForLoop")
                {
                    IForLoop f = (IForLoop)GetParentINode().GetParentINode();
                    return f.GetILoopIndex().GetName();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IFeedbackOutputNode:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            inputNode.Print();
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
