package Dfir_representation;

public class InputNode{
	   private int parentId;
	   private int nodeId;
	   public InputNode(int NodeId,int parentId){
		   this.parentId = parentId;
		   this.nodeId = NodeId;
	   }
	   public void print(){
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
	   }
	public int getParentId() {
		return parentId;
	}
	public void setParentId(int parentId) {
		this.parentId = parentId;
	}
	public int getNodeId() {
		return nodeId;
	}
	public void setNodeId(int nodeId) {
		this.nodeId = nodeId;
	}
}