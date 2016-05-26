using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class INode
    {
        protected internal int nodeId;
        protected internal int parentId;
        protected internal int indegree;
        protected internal string INodeType;

        protected internal bool isDeclared;
        protected internal bool isOccupied;
        protected internal bool isInputConnected;
        protected internal bool isOutputConnected;

        protected internal List<ITerminal> inputTerminals;
        protected internal List<ITerminal> outputTerminals;        
        protected internal static IDictionary<int, INode> IdToINode = new Dictionary<int, INode>();


        public INode(int NodeId, int ParentId, List<ITerminal> InputTerminals, List<ITerminal> OutputTerminals)
        {
            nodeId = NodeId;
            parentId = ParentId;
            inputTerminals = InputTerminals;
            outputTerminals = OutputTerminals;
            INodeType = "INode";
            
            isOccupied = false;
            isDeclared = false;
            indegree = GetConnectedInputTerminals().Count;
            IdToINode[nodeId] = this;

            isInputConnected = false;
            foreach (ITerminal t in inputTerminals)
            {
                if (t.GetConnections().Count != 0)
                {
                    isInputConnected = true;
                }
            }

            isOutputConnected = false;
            foreach (ITerminal t in outputTerminals)
            {
                if (t.GetConnections().Count != 0)
                {
                    isOutputConnected = true;
                }
            }

        }

        public virtual int GetNodeId()
        {
            return nodeId;
        }
        public virtual int GetParentId()
        {
            return parentId;
        }
        public virtual int GetIndegree()
        {
            return indegree;
        }
        public virtual string GetINodeType()
        {
            return INodeType;
        }

        public virtual bool GetIsDeclared()
        {
            return isDeclared;
        }
        public virtual bool GetIsOccupied()
        {
            return isOccupied;
        }
        public virtual bool GetIsInputConnected()
        {
            return isInputConnected;
        }
        public virtual bool GetIsOutputConnected()
        {
            return isOutputConnected;
        }

        public virtual List<ITerminal> GetInputTerminals()
        {
            return inputTerminals;
        }

        public virtual List<ITerminal> GetOutputTerminals()
        {
            return outputTerminals;
        }
        public static IDictionary<int, INode> GetIdToINode()
        {
            return IdToINode;
        }



        public virtual void SetIsDeclared(bool isdeclared)
        {
            isDeclared = isdeclared;
        }
        public virtual void SetIsOccupied(bool isoccupied)
        {
            isOccupied = isoccupied;
        }


        public virtual IDataType GetInputTerminalIDataType(int index)
        {
            if (inputTerminals.Count == 0)
            {
                return null;
            }
            return inputTerminals[index].GetIDataType();

        }
        public virtual IDataType GetOutputTerminalIDataType(int index)
        {
            if (outputTerminals.Count == 0)
            {
                return null;
            }
            return outputTerminals[index].GetIDataType();

        }

        public virtual INode GetParentINode()
        {
            if (IdToINode.ContainsKey(parentId))
            {
                return IdToINode[parentId];
            }
            else
            {
                return null;
            }
        }
       

        public virtual List<ITerminal> GetConnectedInputTerminals() 
        {
            List<ITerminal> result = new List<ITerminal>();
            foreach (ITerminal temp in inputTerminals)
            {
                if (temp.GetConnections().Count != 0)
                {
                    result.Add(temp);
                }
            }
            return result;
        }
        public virtual List<ITerminal> GetConnectedOutputTerminals()
        {
            List<ITerminal> result = new List<ITerminal>();
            foreach (ITerminal temp in outputTerminals)
            {
                if (temp.GetConnections().Count != 0)
                {
                    result.Add(temp);
                }
            }
            return result;
        }
        public virtual List<INode> GetInputINodes() 
        {
            List<INode> inputINodes = new List<INode>();
            if (inputTerminals.Count != 0)
            {
                for (int i = 0; i < inputTerminals.Count; i++)
                {
                    for (int j = 0; j < inputTerminals[i].GetConnections().Count; j++)
                    {
                        inputINodes.Add(IdToINode[inputTerminals[i].GetConnections()[j].GetNodeId()]);
                    }
                }
            }
            return inputINodes;
        }

        public virtual List<INode> GetOutputINodes()
        {
            List<INode> outputINodes = new List<INode>();
            if (outputTerminals.Count != 0)
            {
                for (int i = 0; i < outputTerminals.Count; i++)
                {
                    for (int j = 0; j < outputTerminals[i].GetConnections().Count; j++)
                    {
                        outputINodes.Add(IdToINode[outputTerminals[i].GetConnections()[j].GetNodeId()]);
                    }
                }
            }
            return outputINodes;
        }


        public virtual void DecreaseIndegree()
        {
            indegree--;
        }
        public virtual void IncreaseIndegree()
        {
            indegree++;
        }



        public virtual string GetName()
        {
            return INodeType + Convert.ToString(nodeId);
        }


        public virtual int GetInputConnectionIndex(INode n)
        {
            int result = -1;
            for (int i = 0; i < GetInputINodes().Count; i++)
            {
                if (GetInputINodes()[i] == n)
                {
                    result = i;
                    break;
                }
            }
            if (result == -1)
            {
                Console.WriteLine("Cannot find this connection!");
            }
            return result;
        }
        public virtual int GetOutputConnectionIndex(INode n)
        {
            int result = -1;
            for (int i = 0; i < GetOutputTerminals().Count; i++)
            {
                ITerminal t = GetOutputTerminals()[i];
                for (int j = 0; j < t.GetConnections().Count; j++)
                {
                    Connection c = t.GetConnections()[j];
                    if (c.GetNodeId() == n.GetNodeId())
                    {
                        result = i;
                        break;
                    }
                }
            }
            if (result == -1)
            {
                Console.WriteLine("Cannot find this connection!");
            }
            return result;
        }

        public virtual void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("INode:");
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
