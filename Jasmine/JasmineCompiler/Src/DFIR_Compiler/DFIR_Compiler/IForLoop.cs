using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class IForLoop:INode
    {
        private ILoopIndex iLoopIndex;
        private ILoopMax iLoopMax;
        private List<ITunnel> iTunnels;
        private List<INode> inputBorderNodes;
        private List<INode> outputBorderNodes;
        private List<ILeftShiftRegister> iLeftShiftRegisters;
        private List<IRightShiftRegister> iRightShiftRegisters;
        private IDiagram iDiagram;

        public IForLoop(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, ILoopIndex _iLoopIndex, ILoopMax _iLoopMax, List<ITunnel> _iTunnels, List<ILeftShiftRegister> _iLeftShiftRegisters, List<IRightShiftRegister> _iRightShiftRegisters, IDiagram _iDiagram) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            iLoopIndex = _iLoopIndex;
            iLoopMax = _iLoopMax;
            iTunnels = _iTunnels;
            inputBorderNodes = new List<INode>();
            outputBorderNodes = new List<INode>();
            iLeftShiftRegisters = _iLeftShiftRegisters;
            iRightShiftRegisters = _iRightShiftRegisters;
            iDiagram = _iDiagram;
            INodeType = "IForLoop";

            foreach (ITunnel i in iTunnels)
            {
                if (!i.GetIsInput())
                {
                    outputBorderNodes.Add(i);
                }
                else
                {
                    inputBorderNodes.Add(i);
                }
            }
            foreach (ILeftShiftRegister i in iLeftShiftRegisters)
            {
                inputBorderNodes.Add(i);
            }
            foreach (IRightShiftRegister i in iRightShiftRegisters)
            {
                outputBorderNodes.Add(i);
            }
            //The number of the input border nodes is the indegree of IForLoop
            indegree = inputBorderNodes.Count; 
            if (iLoopMax.GetIsInputConnected())
            {
                indegree++;
            }
        }

        public virtual IDiagram GetIDiagram()
        {
            return iDiagram;
        }
        public virtual ILoopIndex GetILoopIndex()
        {
            return iLoopIndex;
        }
        public virtual ILoopMax GetILoopMax()
        {
            return iLoopMax;
        }
        public virtual List<ITunnel> GetITunnels()
        {
            return iTunnels;
        }
        public virtual List<INode> GetOutputBorderNodes()
        {
            return outputBorderNodes;
        }
        public virtual List<INode> GetInputBorderNodes()
        {
            return inputBorderNodes;
        }
        public virtual List<ILeftShiftRegister> GetILeftShiftRegisters()
        {
            return iLeftShiftRegisters;
        }
        public virtual List<IRightShiftRegister> GetIRightShiftRegisters()
        {
            return iRightShiftRegisters;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IForLoop:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId);
            iLoopIndex.Print();
            iLoopMax.Print();
            for (int i = 0; i < iTunnels.Count; i++)
            {
                iTunnels[i].Print();
            }
            for (int i = 0; i < iLeftShiftRegisters.Count; i++)
            {
                iLeftShiftRegisters[i].Print();
            }
            for (int i = 0; i < iRightShiftRegisters.Count; i++)
            {
                iRightShiftRegisters[i].Print();
            }
            iDiagram.Print();
        }


    }
}
