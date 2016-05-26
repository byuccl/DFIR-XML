package Dfir_representation;

import java.util.Vector;


public class SubVI extends JNode{
	   public Vector<Pair<Integer, Integer> > pairs;
	   public SubVI(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, Vector<Pair<Integer, Integer> > pairs){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "SubVI";
		   this.pairs =pairs;
	   }
	  
	   public void print(){
		   System.out.println("SubVI begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<pairs.size(); i++){
			   System.out.println("TerminalId: " + pairs.get(i).getLeft() + ' ' + "SubVIDataAccessorId: " + pairs.get(i).getRight());
	       }
		   System.out.println("SubVI end\n");
	   }
}