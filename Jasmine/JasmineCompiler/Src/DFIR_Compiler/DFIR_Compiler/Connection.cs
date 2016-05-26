using System;

namespace DFIR_Compiler
{
    public class Connection
    {
        private int nodeId;
        private int terminalId;
        public Connection(int TerminalId, int NodeId)
        {
            terminalId = TerminalId;
            nodeId = NodeId;
        }

        public virtual int GetNodeId()
        {
            return nodeId;
        }
        public virtual int GetTerminalId()
        {
            return terminalId;
        }


        public virtual void SetNodeId(int NodeId)
        {
            nodeId = NodeId;
        }
        public virtual void SetTerminalId(int TerminalId)
        {
            terminalId = TerminalId;
        }


        public virtual void Print()
        {
            Console.WriteLine("TerminalId: " + terminalId + ' ' + "NodeId: " + nodeId);
        }

    }
}
