package Dfir_representation;

public class Connection{
	   private int terminalId;
	   private int nodeId;
	   public int getTerminalId() {
		return terminalId;
	}
	public void setTerminalId(int terminalId) {
		this.terminalId = terminalId;
	}
	public int getNodeId() {
		return nodeId;
	}
	public void setNodeId(int nodeId) {
		this.nodeId = nodeId;
	}
	public Connection(int TerminalId, int NodeId){
		   this.terminalId = TerminalId;
		   this.nodeId = NodeId;
	   }
	   public void print(){
		   
		   System.out.println("TerminalId: " + this.terminalId + ' ' + "NodeId: " + this.nodeId);
		   
	   }
}