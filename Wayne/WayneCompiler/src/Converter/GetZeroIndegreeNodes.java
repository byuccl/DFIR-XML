package Converter;

import java.util.LinkedList;
import java.util.Queue;

import Dfir_representation.*;

//get the zero indegree nodes for the topological sort
public class GetZeroIndegreeNodes {
	public Queue<JNode> work(IDiagram iDiagram,Boolean isTopIDiagram){
		Queue<JNode> que = new LinkedList<JNode>();
		for(IConstant c : iDiagram.getIConstants()){
			if(c.getParentId() == iDiagram.getNodeId()){
				que.add(c);
			}
		}
		for(IDataAccessor c : iDiagram.getIDataAccessors()){
			if(c.getParentId() == iDiagram.getNodeId() && c.getDirection() == Direction.valueOf("INPUT")){
				que.add(c);
			}
		}
		for(IFeedbackOutputNode fo : iDiagram.getiFeedbackOutputNodes()){
			if(fo.getParentId() == iDiagram.getNodeId()){
				que.add(fo);
			}
		}
		//the list of zero indegree nodes should include iconstant, input IDataAccessor and IFeedbackOutputNode.
		
		//if it is not a top diagram, the list should include some input border nodes which are connected inside.
		if(!isTopIDiagram){
			JNode n = JNode.getIdToJNode().get(iDiagram.getParentId());
			if(n.getJNodeType() == "IForLoop"){
				IForLoop f = (IForLoop)n;
				if(f.getILoopIndex().getIsOutputConnected()) que.add(f.getILoopIndex());
				if(f.getILoopMax().getIsOutputConnected()) que.add(f.getILoopMax());
				for(ITunnel t : f.getITunnels()){
					if(t.getIsInput() && t.getParentId() == iDiagram.getParentId()){
						que.add(t);
					}
				}
				for(ILeftShiftRegister ls : f.getILeftShiftRegisters()){
					if(ls.getParentId() == iDiagram.getParentId()){
						que.add(ls);
					}
				}
			}
			/*else if(n.getJNodeType() == "IWhileLoop"){
				IWhileLoop f = (IWhileLoop)n;
				if(f.getILoopIndex().getIsOutputConnected()) que.add(f.getILoopIndex());
				for(ITunnel t : f.getITunnels()){
					if(t.getIsInput() && t.getParentId() == iDiagram.getParentId()){
						que.add(t);
					}
				}
				for(ILeftShiftRegister ls : f.getILeftShiftRegisters()){
					if(ls.getParentId() == iDiagram.getParentId()){
						que.add(ls);
					}
				}
			}*/
			else if(n.getJNodeType() == "ICaseStructure"){
				ICaseStructure c = (ICaseStructure) n;
				if(c.getiCaseSelector().getIsConnectedForIDiagram(iDiagram)){
					que.add(c.getiCaseSelector());
				}
				for(ITunnel t : c.getiTunnels()){
					if(t.getIsInput() && t.getParentId() == iDiagram.getParentId()){
						que.add(t);
						
					}
				}
			}
		}
		return que;
	}
}
