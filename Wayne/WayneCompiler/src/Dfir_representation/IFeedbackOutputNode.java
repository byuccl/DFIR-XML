package Dfir_representation;

import java.util.Vector;


public class IFeedbackOutputNode extends JNode{
	   
    
       private InputNode inputNode;
	   public IFeedbackOutputNode(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals,InputNode inputNode){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.inputNode = inputNode;
		   JNodeType = "IFeedbackOutputNode";
	   }
	  
	   public void print(){
		   System.out.println("IFeedbackOutputNode begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   this.inputNode.print();
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		   for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IFeedbackOutputNode end\n");
	   }
	 

    public IFeedbackInputNode getIFeedbackInputNode(){
    	return (IFeedbackInputNode)JNode.getIdToJNode().get(inputNode.getNodeId());
    }
	public InputNode getInputNode() {
		return inputNode;
	}

	public void setInputNode(InputNode inputNode) {
		this.inputNode = inputNode;
	}
	
	public String getInitName(){
		if(this.getIsInputConnected()){
			return "Init"+Integer.toString(this.nodeId);
		}
		return "0";
	}
	public String getLoopIndexLabel(){
		if(this.getParentJNode().getParentJNode() != null){
			if(this.getParentJNode().getParentJNode().getJNodeType() == "IForLoop"){
				IForLoop f = (IForLoop) this.getParentJNode().getParentJNode();
				return f.getILoopIndex().getName();
			}else{
				return "";
			}
		}else{
			return "";
		}
		
	}
}
