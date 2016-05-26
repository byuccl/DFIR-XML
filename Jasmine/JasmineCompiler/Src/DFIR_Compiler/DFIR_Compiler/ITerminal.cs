using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class ITerminal
    {
        private int terminalId;
        private int terminalIndex;
        private IDataType iDataType;
        private static IDictionary<int, ITerminal> IdToITerminal = new Dictionary<int, ITerminal>();
        private List<Connection> connections = new List<Connection>();

        public ITerminal(int TerminalId, int TerminalIndex, IDataType _iDataType)
        {
            terminalId = TerminalId;
            terminalIndex = TerminalIndex;
            iDataType = _iDataType;
            connections = new List<Connection>();
            IdToITerminal[terminalId] = this;
        }


        public virtual int GetITerminalId()
        {
            return terminalId;
        }
        public virtual int GetITerminalIndex()
        {
            return terminalIndex;
        }
        public virtual IDataType GetIDataType()
        {
            return iDataType;
        }
        public static IDictionary<int, ITerminal> GetIdToITerminal()
        {
            return IdToITerminal;
        }
        public virtual List<Connection> GetConnections()
        {
            return connections;
        }

       
        public virtual void AddConnection(Connection connection)
        {
            connections.Add(connection);
        }

        public virtual void Print()
        {
            Console.WriteLine("ITerminal:");
            Console.WriteLine("TerminalId: " + terminalId + ' ' + "TerminalIndex: " + terminalIndex);
            iDataType.Print();
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Print();
            }
        }

    }
}
