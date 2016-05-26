package Dfir_representation;

import java.util.Vector;

public class IReplaceArraySubsetNode extends JNode{
	
	   public IReplaceArraySubsetNode(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "IReplaceArraySubsetNode";
	   }
	   
	   public void print(){
		   System.out.println("IReplaceArraySubsetNode begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IReplaceArraySubsetNode end\n");
	   }
	   
}
