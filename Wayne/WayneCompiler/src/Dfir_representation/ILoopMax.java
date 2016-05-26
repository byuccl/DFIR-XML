package Dfir_representation;

import java.util.Vector;


public class ILoopMax extends IBorderNode{
	   
	   public ILoopMax(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "ILoopMax";
	   }
	   public void decIndegree(){
		   this.indegree--;
		   JNode.IdToJNode.get(this.parentId).indegree--;
	   }
	   
	   public void print(){
		   System.out.println("ILoopMax begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("ILoopMax end\n");
	   }
}