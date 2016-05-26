using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class ICaseStructure:INode
    {
        private ICaseSelector iCaseSelector;
        private List<ITunnel> iTunnels;
        private List<IDiagram> iDiagrams;

        public ICaseStructure(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, ICaseSelector iCaseSelector, List<ITunnel> iTunnels, List<IDiagram> iDiagrams) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            INodeType = "ICaseStructure";
            this.iCaseSelector = iCaseSelector;
            this.iTunnels = iTunnels;
            this.iDiagrams = iDiagrams;
            indegree = 1;
            foreach (ITunnel i in this.iTunnels)
            {
                if (i.GetIsInput())
                {
                    indegree++;
                }
            }
        }

        public virtual ICaseSelector GetICaseSelector()
        {
            return iCaseSelector;
        }
        public virtual List<ITunnel> GetITunnels()
        {
            return iTunnels;
        }
        public virtual List<IDiagram> GetIDiagrams()
        {
            return iDiagrams;
        }


        public virtual void SetICaseSelector(ICaseSelector iCaseSelector)
        {
            this.iCaseSelector = iCaseSelector;
        }
        public virtual void SetITunnels(List<ITunnel> iTunnels)
        {
            this.iTunnels = iTunnels;
        }
        public virtual void SetIDiagrams(List<IDiagram> iDiagrams)
        {
            this.iDiagrams = iDiagrams;
        }


        //Get the nondefault index for ICaseStructure
        public virtual int GetNonDefaultCaseIndex(int index)
        {
            int nondefault_index = index;
            ICaseSelector caseselector = iCaseSelector;
            if (caseselector.GetDefaultDiagramIndex() <= index)
            {
                nondefault_index++;
            }
            return nondefault_index;
        }

        //Get the judgement code for nondefault case
        public virtual string GetNonDefaultCaseCode(int index)
        {
            string result = "";
            int nondefault_index = GetNonDefaultCaseIndex(index);

            ICaseSelector caseselector = iCaseSelector;
            List<Range> rangelist = new List<Range>();
            for (int i = 0; i < caseselector.GetRanges().Count; i++)
            {
                Range current_range = caseselector.GetRanges()[i];
                if (current_range.GetDiagramIndex() == nondefault_index)
                {
                    rangelist.Add(current_range);
                }
            }
            for (int i = 0; i < rangelist.Count; i++)
            {
                string _operator = "||";
                if (i == 0)
                {
                    _operator = "";
                }
                Range temprange = rangelist[i];

                if (temprange.GetIsBoolean())
                {
                    result += _operator + iCaseSelector.GetName() + " == " + temprange.GetTrueOrFalse();
                }               
                else if (temprange.GetIsSingle())
                {
                    result += _operator + iCaseSelector.GetName() + " == " + temprange.GetSingleValue();
                }
                else
                {
                    //result += _operator + temprange.GetLowValue() + "<=" + iCaseSelector.GetName() + "<=" + temprange.GetHighValue();
                    result += _operator + iCaseSelector.GetName()+">="+ temprange.GetLowValue() + " && " + iCaseSelector.GetName()+ "<=" + temprange.GetHighValue();
                }
            }
            return result;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("ICaseStructure:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            iCaseSelector.Print();
            for (int i = 0; i < iTunnels.Count; i++)
            {
                iTunnels[i].Print();
            }
            for (int i = 0; i < iDiagrams.Count; i++)
            {
                iDiagrams[i].Print();
            }
        }

    }
}
