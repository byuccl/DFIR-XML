package Dfir_representation;

import java.util.Vector;


public class IFeedbackInputNode extends JNode{
	private int delay;
	private Boolean IsFeedbackDeclared;
	private OutputNode outputNode;
	   public IFeedbackInputNode(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, int delay, OutputNode outputNode){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.delay = delay;
		   this.outputNode = outputNode;
		   JNodeType = "IFeedbackInputNode";
		   IsFeedbackDeclared = false;
	   }
	  
	   public void print(){
		   System.out.println("IFeedbackInputNode begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   System.out.println("delay: " + this.delay);
		   this.outputNode.print();
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IFeedbackInputNode end\n");
	   }
	   
	   public IFeedbackOutputNode getIFeedbackOutputNode(){
	    	return (IFeedbackOutputNode)JNode.getIdToJNode().get(outputNode.getNodeId());
	    }
	   
    public String getFeedbackName(){
    	return "Feedback" + Integer.toString(this.getNodeId());
    }
	public void setIsFeedbackDeclared(Boolean isFeedbackDeclared) {
		IsFeedbackDeclared = isFeedbackDeclared;
	}

	public Boolean getIsFeedbackDeclared(){
		return this.IsFeedbackDeclared;
	}
	public int getdelay() {
		return delay;
	}

	public void setdelay(int delay) {
		this.delay = delay;
	}

	public OutputNode getOutputNode() {
		return outputNode;
	}

	public void setOutputNode(OutputNode outputNode) {
		this.outputNode = outputNode;
	}
}