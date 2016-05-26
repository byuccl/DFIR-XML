package Dfir_representation;

import java.util.Vector;

public class IInsertIntoArrayNode extends JNode{
	
	   public IInsertIntoArrayNode(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "IInsertIntoArrayNode";
	   }
	   
	   public void print(){
		   System.out.println("IInsertIntoArrayNode begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IInsertIntoArrayNode end\n");
	   }
	   
}
