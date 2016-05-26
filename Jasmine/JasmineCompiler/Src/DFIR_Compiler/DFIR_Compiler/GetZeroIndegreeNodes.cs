using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class GetZeroIndegreeNodes
    {
        public virtual LinkedList<INode> ZeroIndegreeNodesList(IDiagram iDiagram, bool isTopIDiagram)
        {
            LinkedList<INode> zeroindegreelist = new LinkedList<INode>();

            //Put the IConstant in current IDiagram into the zeroindegreelist
            foreach (IConstant temp in iDiagram.getIConstants())
            {
                if (temp.GetParentId() == iDiagram.GetNodeId())
                {
                    zeroindegreelist.AddLast(temp);
                }
            }

            //Put the INPUT IDataAccessor in current IDiagram into the zeroindegreelist
            foreach (IDataAccessor temp in iDiagram.getIDataAccessors())
            {
                if (temp.GetParentId() == iDiagram.GetNodeId() && temp.GetDirection() == Direction.INPUT)
                {
                    zeroindegreelist.AddLast(temp);
                }
            }

            //For the situation that the current IDiagram isn't the top IDiagram
            if (!isTopIDiagram)
            {
                INode n = INode.GetIdToINode()[iDiagram.GetParentId()];

                if (n.GetINodeType() == "ICaseStructure")
                {
                    ICaseStructure c = (ICaseStructure)n;
                    //Put the ICaseSelector into the zeroindegreelist
                    if (c.GetICaseSelector().IsConnectedIDiagram(iDiagram))
                    {
                        zeroindegreelist.AddLast(c.GetICaseSelector());
                    }
                    //Put the input ITunnel into the zeroindegreelist
                    foreach (ITunnel t in c.GetITunnels())
                    {
                        if (t.GetIsInput() && t.GetParentId() == iDiagram.GetParentId())
                        {
                            zeroindegreelist.AddLast(t);

                        }
                    }
                }

                else if (n.GetINodeType() == "IForLoop")
                {
                    IForLoop forloop = (IForLoop)n;

                    //Put the output connected ILoopIndex into the zeroindegreelist
                    if (forloop.GetILoopIndex().GetIsOutputConnected())
                    {
                        zeroindegreelist.AddLast(forloop.GetILoopIndex());
                    }
                    //Put the output connceted ILoopMax into the zeroindegreelist
                    if (forloop.GetILoopMax().GetIsOutputConnected())
                    {
                        zeroindegreelist.AddLast(forloop.GetILoopMax());
                    }
                    //Put the ILeftShiftRegister into the zeroindegreelist
                    foreach (ILeftShiftRegister ls in forloop.GetILeftShiftRegisters())
                    {
                        if (ls.GetParentId() == iDiagram.GetParentId())
                        {
                            zeroindegreelist.AddLast(ls);
                        }
                    }
                    //Put the input ITunnel into the zeroindegreelist
                    foreach (ITunnel t in forloop.GetITunnels())
                    {
                        if (t.GetIsInput() && t.GetParentId() == iDiagram.GetParentId())
                        {
                            zeroindegreelist.AddLast(t);
                        }
                    }

                }

            }

            return zeroindegreelist;
        }

    }
}
