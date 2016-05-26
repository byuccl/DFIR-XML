package Dfir_representation;

import java.util.Vector;


public class ILeftShiftRegister extends IBorderNode{
	   
	   private AssociatedRightShiftRegister associatedRightShiftRegister;
	   private Vector<Boolean> isuseds;
	   private Vector<Boolean> isDeclareds;
	   public ILeftShiftRegister(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, AssociatedRightShiftRegister associatedRightShiftRegister){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.associatedRightShiftRegister = associatedRightShiftRegister;
		   JNodeType = "ILeftShiftRegister";
		   isuseds = new Vector<Boolean>();
		   isDeclareds = new Vector<Boolean>();
		   for(int i=0; i<this.inputTerminals.size(); i++){
			   isuseds.addElement(false);
			   isDeclareds.addElement(false);
		   }
	   }
	   public Boolean getIsDeclared(int index){
			return this.isDeclareds.get(index);
		}
	   public void setIsDeclared(Boolean b,int index){
			this.isDeclareds.setElementAt(b, index);
		}
	   public AssociatedRightShiftRegister getAssociatedRightShiftRegister(){
		   return this.associatedRightShiftRegister;
	   }
	   public void decIndegree(){
		   this.indegree--;
		   if(this.indegree == 0) JNode.IdToJNode.get(this.parentId).indegree--;
	   }
	   public JNode getParentJNode(){
		   return JNode.IdToJNode.get(this.parentId);
	   }
	   public void setIsused(Boolean isused, int index){
		   this.isuseds.set(index, isused);
	   }
	   public String getName(int index){
		   String res;
		   res = this.getJNodeType() + Integer.toString(this.getNodeId()) + "_" + Integer.toString(index);
		   return res;
	   }
	   public Vector<Boolean> getIsuseds(){
		   return this.isuseds;
	   }
	   
	   public void print(){
		   System.out.println("ILeftShiftRegister begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   associatedRightShiftRegister.print();
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("ILeftShiftRegister end\n");
	   }
}