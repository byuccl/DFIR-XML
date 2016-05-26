using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class ICaseSelector:IBorderNode
    {
        private int defaultDiagramIndex;
        private List<Range> ranges;

        public ICaseSelector(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, int DefaultDiagramIndex, List<Range> Ranges) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            INodeType = "ICaseSelector";
            defaultDiagramIndex = DefaultDiagramIndex;
            ranges = Ranges;
        }


        public virtual int GetDefaultDiagramIndex()
        {
            return defaultDiagramIndex;
        }
        public virtual List<Range> GetRanges()
        {
            return ranges;
        }


        public virtual void SetDefaultDiagramIndex(int DefaultDiagramIndex)
        {
            defaultDiagramIndex = DefaultDiagramIndex;
        }
        public virtual void SetRanges(List<Range> Ranges)
        {
            ranges = Ranges;
        }


        public virtual bool IsConnectedIDiagram(IDiagram iDiagram)
        {
            bool flag = false;
            if (GetOutputTerminals()[iDiagram.getDiagramIndex()].GetConnections().Count != 0)
            {
                flag = true;
            }
            return flag;
        }


        public override void DecreaseIndegree()
        {
            indegree--;
            IdToINode[parentId].indegree--;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ICaseSelector:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            Console.WriteLine("DefaultDiagramIndex: " + defaultDiagramIndex);
            for (int i = 0; i < ranges.Count; i++)
            {
                ranges[i].Print();
            }
        }


    }
}
