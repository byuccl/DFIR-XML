package Dfir_representation;

import java.util.Vector;


public class ITunnel extends IBorderNode{
	   
	   private IndexingMode indexingMode;
	   private Boolean isInput;
	   private ITerminal innerTerminal;
	   private ITerminal outerTerminal;
	   
	   public ITunnel(int nodeId, int parentId, Vector<ITerminal> inputTerminals, 
			          Vector<ITerminal> outputTerminals,  IndexingMode indexingMode, 
			          IsInputMode isInput, int innerTerminalId, int outerTerminalId){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   if(isInput == IsInputMode.valueOf("False")){
			   this.isInput = false;
		   }
		   else{
			   this.isInput = true;
		   }
		   this.innerTerminal = ITerminal.getIdToITerminal().get(innerTerminalId);
		   this.outerTerminal = ITerminal.getIdToITerminal().get(outerTerminalId);
		   this.indexingMode=indexingMode;
		   JNodeType = "ITunnel";
		   this.indegree = 1;
	   }
	   
	   
	   public Boolean isOutputITunnelForCase(){
		   Boolean res = false;
		   if(this.getParentJNode().getJNodeType() == "ICaseStructure"){
			   if(!this.getIsInput()){
				   res = true;
			   }
		   }
		   return res;
	   }
	   public Boolean isIndexingITunnelInsideLoop(IDiagram iDiagram){
			return this.isBorderNodeInsideLoop(iDiagram) && indexingMode == IndexingMode.valueOf("Indexing");
		}
	   public String getILoopIndexName(){
		   IForLoop f = (IForLoop)this.getParentJNode();
		   return f.getILoopIndex().getName();
	   }
	   public String getIndexingITunnelName(){
		   return this.getName() + '[' + this.getILoopIndexName() + ']';
	   }
	   public JNode getParentJNode(){
		   return JNode.IdToJNode.get(this.parentId);
	   }
	   public void decIndegree(){
		   this.indegree--;
		   if(this.isInput){
			   JNode.IdToJNode.get(this.parentId).indegree--;
		   }
		   
	   }
	   public Boolean getIsInput() {
		   return this.isInput;
	   }
	   public ITerminal getInnerTerminal(){
		   return this.innerTerminal;
	   }
	   public ITerminal getOuterTerminal(){
		   return this.outerTerminal;
	   }
	   public void print(){
		   System.out.println("ITunnel begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   System.out.println("IsInput: " + this.getIsInput());
		   System.out.println("GetInnerTerminal: " + this.getInnerTerminal().getITerminalId());
		   System.out.println("GetOuterTerminal: " + this.getOuterTerminal().getITerminalId());
		   System.out.println("IndexingMode: " + this.getIndexingMode());
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("ITunnel end\n");
	   }
	   public IndexingMode getIndexingMode() {
		    return indexingMode;
	   }
	   public void setIndexingMode(IndexingMode indexingMode) {
		    this.indexingMode = indexingMode;
	   }
}

