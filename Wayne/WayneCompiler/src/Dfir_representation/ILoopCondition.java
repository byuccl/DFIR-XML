package Dfir_representation;

import java.util.Vector;



public class ILoopCondition extends IBorderNode{
	   
	   public ILoopCondition(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "ILoopCondition";
	   }
	  
	   public void print(){
		   System.out.println("ILoopCondition begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("ILoopCondition end\n");
	   }
}
