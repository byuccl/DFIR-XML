package Dfir_representation;

import java.util.Vector;

public class IStructure extends JNode{
	   
	   
	   public IStructure(int nodeId, int parentId, Vector<ITerminal> inputTerminals, Vector<ITerminal> outputTerminals){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   
		   JNodeType = "IStructure";
	   }
	  
	   public void print(){
		   System.out.println("IStructure begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IStructure end\n");
	   }
	   
}


