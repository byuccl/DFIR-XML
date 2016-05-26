using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class IFeedbackInputNode:INode
    {
        private int delay;
        private bool isFeedbackDeclared;
        private OutputNode outputNode;

        public IFeedbackInputNode(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, int Delay, OutputNode _outputNode) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            delay = Delay;
            outputNode = _outputNode;
            isFeedbackDeclared = false;
            INodeType = "IFeedbackInputNode";
        }


        public virtual int GetDalay()
        {
            return delay;
        }
        public virtual bool GetIsFeedbackDeclared()
        {
            return isFeedbackDeclared;
        }
        public virtual OutputNode GetOutputNode()
        {
            return outputNode;
        }

        public virtual IFeedbackOutputNode GetIFeedbackOutputNode()
        {
            return (IFeedbackOutputNode)GetIdToINode()[outputNode.GetNodeId()];
        }

        public virtual string GetFeedbackName()
        {
            return "Feedback" + Convert.ToString(GetNodeId());
        }


        public virtual void SetDalay(int Delay)
        {
            delay = Delay;
        }
        public virtual void SetIsFeedbackDeclared(bool isFeedbackDeclared)
        {
            this.isFeedbackDeclared = isFeedbackDeclared;
        }
        public virtual void SetOutputNode(OutputNode outputNode)
        {
            this.outputNode = outputNode;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IFeedbackInputNode:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            Console.WriteLine("Dalay: " + delay);
            outputNode.Print();
            for (int i = 0; i < inputTerminals.Count(); i++)
            {
                inputTerminals[i].Print();
            }
            for (int i = 0; i < outputTerminals.Count(); i++)
            {
                outputTerminals[i].Print();
            }
        }
    }
}
