package Dfir_representation;

public class AssociatedLeftShiftRegister{
	   
	   private int parentId;
	   private int nodeId;
	   public AssociatedLeftShiftRegister(int NodeId,int parentId){
		   this.parentId = parentId;
		   this.nodeId = NodeId;
	   }
	   public int getParentId(){
		   return this.parentId;
	   }
	   public int getNodeId(){
		   return this.nodeId;
	   }
	   public void print(){
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
	   }
}