using System;
using System.Collections.Generic;


namespace DFIR_Compiler
{
    public class IDiagram:INode
    {
        private int diagramIndex;
        private static bool hasComplex = false;
        private static bool hasSelect = false;
        private static bool hasCmath = false;
        private bool hasFeedback = false;
        private List<ICompoundArithmeticNode> iCompoundArithmeticNodes;
        private List<IDataAccessor> iDataAccessors;
        private List<IConstant> iConstants;
        private List<IForLoop> iForLoops;
        private List<ICaseStructure> iCaseStructures;
        private List<IPrimitive> iPrimitives;
        private List<IFeedbackOutputNode> iFeedbackOutputNodes;
        private List<IFeedbackInputNode> iFeedbackInputNodes;
        private List<IArrayIndexNode> iArrayIndexNodes;



        public IDiagram(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, int diagramIndex) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            INodeType = "IDiagram";
            this.diagramIndex = diagramIndex;
            iCompoundArithmeticNodes = new List<ICompoundArithmeticNode>();
            iDataAccessors = new List<IDataAccessor>();
            iConstants = new List<IConstant>();
            iForLoops = new List<IForLoop>();
            iCaseStructures = new List<ICaseStructure>();
            iPrimitives = new List<IPrimitive>();
            iFeedbackOutputNodes = new List<IFeedbackOutputNode>();
            iFeedbackInputNodes = new List<IFeedbackInputNode>();
            iArrayIndexNodes = new List<IArrayIndexNode>();

        }



        public static bool getHasCmath()
        {
            return hasCmath;
        }

        public static void setHasCmath(bool hasCmath)
        {
            IDiagram.hasCmath = hasCmath;
        }
        public static bool getHasSelect()
        {
            return hasSelect;
        }
        public static void setHasSelect(bool hasSelect)
        {
            IDiagram.hasSelect = hasSelect;
        }
        public static bool getHasComplex()
        {
            return hasComplex;
        }

        public virtual void setHasComplex(bool hasComplex)
        {
            IDiagram.hasComplex = hasComplex;
        }

        public virtual bool getHasFeedback()
        {
            return hasFeedback;
        }

        public virtual void setHasFeedback(bool hasFeedback)
        {
            this.hasFeedback = hasFeedback;
        }

        public virtual int getDiagramIndex()
        {
            return diagramIndex;
        }

        public virtual void setDiagramIndex(int diagramIndex)
        {
            this.diagramIndex = diagramIndex;
        }


        public virtual List<ICaseStructure> getiCaseStructures()
        {
            return iCaseStructures;
        }

        public virtual void setiCaseStructures(List<ICaseStructure> iCaseStructures)
        {
            this.iCaseStructures = iCaseStructures;
        }

        public virtual List<IFeedbackOutputNode> getiFeedbackOutputNodes()
        {
            return iFeedbackOutputNodes;
        }

        public virtual void setiFeedbackOutputNodes(List<IFeedbackOutputNode> iFeedbackOutputNodes)
        {
            this.iFeedbackOutputNodes = iFeedbackOutputNodes;
        }

        public virtual List<IFeedbackInputNode> getiFeedbackInputNodes()
        {
            return iFeedbackInputNodes;
        }

        public virtual void setiFeedbackInputNodes(List<IFeedbackInputNode> iFeedbackInputNodes)
        {
            this.iFeedbackInputNodes = iFeedbackInputNodes;
        }

        public virtual void setIConstants(List<IConstant> c)
        {
            iConstants = c;
        }
        public virtual void setIDataAccessors(List<IDataAccessor> d)
        {
            iDataAccessors = d;
        }
        public virtual void setICompoundArithmeticNodes(List<ICompoundArithmeticNode> c)
        {
            iCompoundArithmeticNodes = c;
        }
        public virtual void setIForLoops(List<IForLoop> c)
        {
            iForLoops = c;
        }
        public virtual void setIPrimitives(List<IPrimitive> c)
        {
            iPrimitives = c;
        }
        public virtual List<IConstant> getIConstants()
        {
            return iConstants;
        }
        public virtual List<IDataAccessor> getIDataAccessors()
        {
            return iDataAccessors;
        }
        public virtual List<ICompoundArithmeticNode> getICompoundArithmeticNodes()
        {
            return iCompoundArithmeticNodes;
        }
        public virtual List<IForLoop> getIForLoops()
        {
            return iForLoops;
        }
        public virtual List<IPrimitive> getIPrimitives()
        {
            return iPrimitives;
        }

        public virtual List<ICompoundArithmeticNode> getiCompoundArithmeticNodes()
        {
            return iCompoundArithmeticNodes;
        }


        
        public virtual List<IArrayIndexNode> getiArrayIndexNodes()
        {
            return iArrayIndexNodes;
        }


        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IDiagram:");
            Console.WriteLine("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
            Console.WriteLine("DiagramIndex: " + this.diagramIndex);
            for (int i = 0; i < iConstants.Count; i++)
            {
                iConstants[i].Print();
            }
            for (int i = 0; i < iDataAccessors.Count; i++)
            {
                iDataAccessors[i].Print();
            }
            for (int i = 0; i < iCompoundArithmeticNodes.Count; i++)
            {
                iCompoundArithmeticNodes[i].Print();
            }
            for (int i = 0; i < iForLoops.Count; i++)
            {
                iForLoops[i].Print();
            }            
            for (int i = 0; i < iPrimitives.Count; i++)
            {
                iPrimitives[i].Print();
            }
            for (int i = 0; i < iCaseStructures.Count; i++)
            {
                iCaseStructures[i].Print();
            }
            for (int i = 0; i < iFeedbackOutputNodes.Count; i++)
            {
                iFeedbackOutputNodes[i].Print();
            }
            for (int i = 0; i < iFeedbackInputNodes.Count; i++)
            {
                iFeedbackInputNodes[i].Print();
            }
        }

    }
}
