using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class TopologicalSorting
    {
        public virtual void Sorting(LinkedList<INode> zeroindegreelist, List<INode> sortedlist, IDiagram iDiagram)
        {
            while (zeroindegreelist.Count > 0)
            {
                //Get and remove the current node in the zeroindegreelist
                INode currentnode = zeroindegreelist.First();
                zeroindegreelist.RemoveFirst();

                //Put the output border nodes of IForLoop in the zeroindegreelist
                if (currentnode.GetINodeType() == "IForLoop")
                {
                    IForLoop forloop = (IForLoop)currentnode;
                    foreach (INode n in forloop.GetOutputBorderNodes())
                    {
                        zeroindegreelist.AddLast(n);
                    }
                }
                //Put the ITunnel of ICaseStructure in the zeroindegreelist
                else if (currentnode.GetINodeType() == "ICaseStructure")
                {
                    ICaseStructure casestructure = (ICaseStructure)currentnode;
                    foreach (INode n in casestructure.GetITunnels())
                    {
                        zeroindegreelist.AddLast(n);
                    }
                }


                sortedlist.Add(currentnode);
                //These nodes should be processed inside the loop
                if ((currentnode.GetINodeType() == "ILeftShiftRegister" || currentnode.GetINodeType() == "ILoopMax"||currentnode.GetINodeType() == "ICaseSelector" || currentnode.GetINodeType() == "ITunnel" ) && !((IBorderNode)currentnode).IsBorderNodeInside(iDiagram))
                {
                    bool isInputITunnel = true;
                    if (currentnode.GetINodeType() == "ITunnel")
                    {
                        ITunnel t = (ITunnel)currentnode;
                        if (!t.GetIsInput())
                        {
                            isInputITunnel = false; 
                        }
                    }
                    if (isInputITunnel)
                    {
                        continue;
                    }
                } 
                //These nodes should be processed outside the loop
                if ((currentnode.GetINodeType() == "ITunnel" || currentnode.GetINodeType() == "IRightShiftRegister") && ((IBorderNode)currentnode).IsBorderNodeInside(iDiagram)) 
                {
                    bool isOutputITunnel = true;
                    if (currentnode.GetINodeType() == "ITunnel")
                    {
                        ITunnel t = (ITunnel)currentnode;
                        if (t.GetIsInput())
                        {
                            isOutputITunnel = false; 
                        }
                    }
                    if (isOutputITunnel)
                    {
                        continue;
                    }
                } 

                List<INode> backINodes = currentnode.GetOutputINodes();

                if (currentnode.GetINodeType() == "ICaseSelector")
                {
                    List<INode> tempnode = new List<INode>();
                    ITerminal t = currentnode.GetOutputTerminals()[iDiagram.getDiagramIndex()];
                    for (int i = 0; i < t.GetConnections().Count; i++)
                    {
                        tempnode.Add(INode.GetIdToINode()[t.GetConnections()[i].GetNodeId()]);
                    }
                    backINodes = tempnode;
                }
                if (currentnode.GetINodeType() == "IFeedbackInputNode")
                {
                    IFeedbackInputNode fi = (IFeedbackInputNode)currentnode;
                    backINodes.Add(INode.GetIdToINode()[fi.GetOutputNode().GetNodeId()]);
                }
                foreach (INode n in backINodes)
                {
                    n.DecreaseIndegree();
                    if (n.GetIndegree() == 0)
                    {
                        zeroindegreelist.AddLast(n);

                        if ((n.GetINodeType() == "ILeftShiftRegister" || n.GetINodeType() == "ILoopMax"||n.GetINodeType() == "ICaseSelector" || n.GetINodeType() == "ITunnel"  ) && !((IBorderNode)n).IsBorderNodeInside(iDiagram))
                        {
                            bool isInputITunnel = true;
                            if (n.GetINodeType() == "ITunnel")
                            {
                                ITunnel t = (ITunnel)n;
                                if (!t.GetIsInput()) 
                                {
                                    isInputITunnel = false;
                                }
                            }
                            Console.WriteLine("ParentNode: " + n.GetParentINode().GetName() + " Indegree: " + n.GetParentINode().GetIndegree());
                            if (isInputITunnel && n.GetParentINode().GetIndegree() == 0)
                            {
                                zeroindegreelist.AddLast(n.GetParentINode());
                            }
                        } 

                    }
                    if (n.GetINodeType() == "ITunnel")
                    {
                        ITunnel t = (ITunnel)n;
                        if (t.IsOutputCaseITunnel())
                        {
                            t.IncreaseIndegree();
                        }
                    }
                }

            }
            Console.WriteLine("\n------------------------------------");
            Console.WriteLine("Topological Sorting:");
            foreach (INode i in sortedlist)
            {
                Console.WriteLine(i.GetName());
            }
        }

    }
}
