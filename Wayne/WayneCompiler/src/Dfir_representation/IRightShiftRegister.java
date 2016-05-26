package Dfir_representation;

import java.util.Vector;


public class IRightShiftRegister extends IBorderNode{
	   
	   private AssociatedLeftShiftRegister associatedLeftShiftRegister;
	   public IRightShiftRegister(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, AssociatedLeftShiftRegister associatedLeftShiftRegister){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.associatedLeftShiftRegister = associatedLeftShiftRegister;
		   JNodeType = "IRightShiftRegister";
	   }
	   public AssociatedLeftShiftRegister getAssociatedLeftShiftRegister(){
		   return this.associatedLeftShiftRegister;
	   }
	   
	   public void print(){
		   System.out.println("IRightShiftRegister begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   this.associatedLeftShiftRegister.print();
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IRightShiftRegister end\n");
	   }
}