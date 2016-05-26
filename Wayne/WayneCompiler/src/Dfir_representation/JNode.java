package Dfir_representation;

import java.util.HashMap;
import java.util.Map;
import java.util.Stack;
import java.util.Vector;

public class JNode {
	protected int nodeId;
	protected int parentId;
	protected String JNodeType;
	protected Vector<ITerminal> inputTerminals;
	protected Vector<ITerminal> outputTerminals;
	protected int indegree;
	protected static Map<Integer, JNode> IdToJNode = new HashMap<Integer, JNode>();
	protected Boolean isInputConnected;
	protected Boolean isOutputConnected;
	protected Boolean isdeclared;
	protected Boolean isused;

	public JNode(int n, int p, Vector<ITerminal> inputTerminals, Vector<ITerminal> outputTerminals) {
		this.nodeId = n;
		this.parentId = p;
		this.inputTerminals = inputTerminals;
		this.outputTerminals = outputTerminals;
		this.JNodeType = "JNode";
		JNode.IdToJNode.put(this.nodeId, this);
		this.isused = false;
		this.isdeclared = false;
		indegree = this.getConnectedInputTerminals().size();
		
		this.isInputConnected = false;
		for(ITerminal t : this.inputTerminals){
			if(t.getConnections().size()!=0){
				this.isInputConnected = true;
			}
		}
		this.isOutputConnected = false;
		for(ITerminal t : this.outputTerminals){
			if(t.getConnections().size()!=0){
				this.isOutputConnected = true;
			}
		}

	}
	
	public Boolean getIsDeclared(){
		return this.isdeclared;
	}
	public void setIsDeclared(Boolean b){
		this.isdeclared = b;
	}
	
	public IDataType getOutputTerminalIDataType(int index){
		if(this.outputTerminals.size()==0) return null;
		return this.outputTerminals.get(index).getIDataType();
		
	}

	public IDataType getInputTerminalIDataType(int index){
		if(this.inputTerminals.size()==0) return null;
		return this.inputTerminals.get(index).getIDataType();
		
	}
	public Boolean getIsInputConnected(){
		return this.isInputConnected;
	}
	public Boolean getIsOutputConnected(){
		return this.isOutputConnected;
	}
	public JNode getParentJNode(){
		if(JNode.IdToJNode.containsKey(this.parentId)){
			return JNode.IdToJNode.get(this.parentId);
		}else{
			return null;
		}
	}
	public int getNodeId(){
		return this.nodeId;
	}
	
	public int getParentId(){
		return this.parentId;
	}
	
	public String getJNodeType(){
		return this.JNodeType;
	}
	public Boolean getIsused(){
		return this.isused;
	}
	public void setIsused(Boolean b){
		this.isused = b;
	}
	
	public static Map<Integer, JNode> getIdToJNode(){
		return JNode.IdToJNode;
	}
	
	public Vector<ITerminal> getInputTerminals(){
		return this.inputTerminals;
	}
	
	public Vector<ITerminal> getOutputTerminals(){
		return this.outputTerminals;
	}
	public Vector<ITerminal> getConnectedInputTerminals(){//for feedback nodes, the initTerminal can be not connected!
		Vector<ITerminal> res = new Vector<ITerminal>();
		for(ITerminal t:this.inputTerminals){
			if(t.getConnections().size() != 0){
				res.addElement(t);
			}
		}
		return res;
	}
	
	public Vector<ITerminal> getConnectedOutputTerminals(){
		Vector<ITerminal> res = new Vector<ITerminal>();
		for(ITerminal t:this.outputTerminals){
			if(t.getConnections().size() != 0){
				res.addElement(t);
			}
		}
		return res;
	}
	public Vector<JNode> getInputJNodes(){//can be used after all the node have been generated
		Vector<JNode> inputJNodes = new Vector<JNode>();
		if (this.inputTerminals.size() != 0) {
			for (int i = 0; i < this.inputTerminals.size(); i++) {
				for (int j = 0; j < this.inputTerminals.get(i).getConnections().size(); j++) {
					inputJNodes.addElement(JNode.IdToJNode.get(this.inputTerminals.get(i).getConnections().get(j).getNodeId()));
				}
			}
		}
		return inputJNodes;
	}
	
	public Vector<JNode> getOutputJNodes(){
		Vector<JNode> outputJNodes = new Vector<JNode>();
		if (this.outputTerminals.size() != 0) {
			for (int i = 0; i < this.outputTerminals.size(); i++) {
				for (int j = 0; j < this.outputTerminals.get(i).getConnections().size(); j++) {
					outputJNodes.addElement(JNode.IdToJNode.get(this.outputTerminals.get(i).getConnections().get(j).getNodeId()));
				}
			}
		}
		return outputJNodes;
	}
	public int getIndegree(){
		return this.indegree;
	}
	public void decIndegree(){
		this.indegree--;
	}
	public void incIndegree(){
		this.indegree++;
	}



	public String getName() {
		return this.JNodeType + Integer.toString(this.nodeId);
	}
	
	
	public int getInputConnectionIndex(JNode n){
		   int res = -1;
		   for(int i=0; i<this.getInputJNodes().size(); i++){
			   if(this.getInputJNodes().get(i) == n){
				   res = i;
				   break;
			   }
		   }
		   if(res == -1){
			   System.out.println("Cannot find this connection!");
		   }
		   return res;
	   }
	   public int getOutputConnectionIndex(JNode n){
		   int res = -1;
		   for(int i=0; i<this.getOutputTerminals().size(); i++){
			   ITerminal t = this.getOutputTerminals().get(i);
			   for(int j=0; j<t.getConnections().size(); j++){
				    Connection c = t.getConnections().get(j);
				    if(c.getNodeId() == n.getNodeId()){
				    	res = i;
						break;
				    }
			   }
		   }
		   if(res == -1){
			   System.out.println("Cannot find this connection!");
		   }
		   return res;
	   }
	   
	   public void print() {
			System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
			for (int i = 0; i < inputTerminals.size(); i++) {
				inputTerminals.get(i).print();
			}
			for (int i = 0; i < outputTerminals.size(); i++) {
				outputTerminals.get(i).print();
			}
	   }
	
}
