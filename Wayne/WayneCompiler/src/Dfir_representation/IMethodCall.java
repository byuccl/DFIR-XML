package Dfir_representation;

import java.util.Vector;

public class IMethodCall extends JNode{
	   
	   public SubVI subVI;
	   public String targetName;
	   public IMethodCall(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, String targetName){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.targetName = targetName;
		   JNodeType = "IMethodCall";
	   }
	  
	   public void print(){
		   System.out.println("IMethodCall begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   System.out.println("TargetName: " + this.targetName);
		   this.subVI.print();
		   for(int i=0; i<inputTerminals.size(); i++){
			   inputTerminals.get(i).print();
	       }
	       for(int i=0; i<outputTerminals.size(); i++){
			   outputTerminals.get(i).print();
	       }  
		   System.out.println("IMethodCall end\n");
	   }
}
