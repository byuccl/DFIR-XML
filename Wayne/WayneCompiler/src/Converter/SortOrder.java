package Converter;

import java.util.Queue;
import java.util.Vector;
import Dfir_representation.*;

public class SortOrder {
	public void work(Queue<JNode> que, Vector<JNode> vec,IDiagram iDiagram){
		while(!que.isEmpty()){
			JNode cur = que.remove();
			if(cur.getJNodeType() == "IForLoop"){
				IForLoop f = (IForLoop) cur;
				for(JNode n : f.getOutputBorderNodes()){
					que.add(n);
				}
			}//add the output border nodes of for loop after it.
			/*else if(cur.getJNodeType() == "IWhileLoop"){
				IWhileLoop f = (IWhileLoop) cur;
				for(JNode n : f.getOutputBorderNodes()){
					que.add(n);
				}
			}*/
			else if(cur.getJNodeType() == "ICaseStructure"){
				ICaseStructure c = (ICaseStructure) cur;
				for(JNode n : c.getiTunnels()){
					que.add(n);
				}
			}//add the output border nodes of case structure after it.
			vec.addElement(cur);
			
			if((cur.getJNodeType() == "ICaseSelector" || cur.getJNodeType() == "ITunnel" || cur.getJNodeType() == "ILeftShiftRegister" || cur.getJNodeType() == "ILoopMax") && !((IBorderNode) cur).isBorderNodeInsideLoop(iDiagram)){
				Boolean isInputITunnel = true;
				if(cur.getJNodeType() == "ITunnel"){
					ITunnel t = (ITunnel)cur;
					if(!t.getIsInput()){
						isInputITunnel = false;// to allow output itunnel to add its output Jnodes
					}
				}
				if(isInputITunnel)continue;
			}// to prevent add inputBorderNode's output Node into vector, since they should be processed inside the loop
			if((cur.getJNodeType() == "ITunnel" || cur.getJNodeType() == "IRightShiftRegister") && ((IBorderNode) cur).isBorderNodeInsideLoop(iDiagram)){//cur.getParentId() == iDiagram.getParentId()
				Boolean isOutputITunnel = true;
				if(cur.getJNodeType() == "ITunnel"){
					ITunnel t = (ITunnel)cur;
					if(t.getIsInput()){
						isOutputITunnel = false;// to allow input itunnel to add its output Jnodes
					}
				}
				if(isOutputITunnel)continue;
			}//to prevent add outputBorderNode's output Nodes into vector, since they should be processed outside the loop
			
			Vector<JNode> backJNodes = cur.getOutputJNodes();
			
			if(cur.getJNodeType() == "ICaseSelector"){
				Vector<JNode> tmp = new Vector<JNode>(); 
				ITerminal t = cur.getOutputTerminals().get(iDiagram.getDiagramIndex());
				for(int i=0; i<t.getConnections().size(); i++){
					tmp.addElement(JNode.getIdToJNode().get(t.getConnections().get(i).getNodeId()));
				}
				backJNodes = tmp;
			}//handling for special case of ICaseSelector, which can have a connection in different diagram inside the case structure.
			
			for(JNode n: backJNodes){
				n.decIndegree();
				if(n.getIndegree() == 0){
					que.add(n);
					
					if((n.getJNodeType() == "ICaseSelector" || n.getJNodeType() == "ITunnel" || n.getJNodeType() == "ILeftShiftRegister" || n.getJNodeType() == "ILoopMax") && !((IBorderNode) n).isBorderNodeInsideLoop(iDiagram)){
						Boolean isInputITunnel = true;
						if(n.getJNodeType() == "ITunnel"){
							ITunnel t = (ITunnel)n;
							if(!t.getIsInput()) isInputITunnel = false;// to exclude output ITunnel
						}
						System.out.println("ParentNode: " + n.getParentJNode().getName() +  " Indegree: " + n.getParentJNode().getIndegree());
						if(isInputITunnel && n.getParentJNode().getIndegree() == 0){
							que.add(n.getParentJNode());
						}
					}// to add forloop or case structure
					
				}
				
				if(n.getJNodeType() == "ITunnel"){
					ITunnel t = (ITunnel)n;
					if(t.isOutputITunnelForCase()){
						t.incIndegree();
					}
				}
			}//handling the special case for output tunnel in the case structure.
			
		}
		System.out.println("\n\nOutput node topological order-----------");
		for(JNode i : vec){
			System.out.println(i.getName());
		}
		System.out.println("\nEnd of Output node topological order-----------");	
	}
}
