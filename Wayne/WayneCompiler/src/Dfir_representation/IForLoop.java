package Dfir_representation;

import java.util.Vector;


public class IForLoop extends IStructure{
	   
	   private ILoopIndex iLoopIndex;
	   private ILoopMax iLoopMax;
	   private Vector<ITunnel> iTunnels;
	   private Vector<ILeftShiftRegister> iLeftShiftRegisters;
	   private Vector<IRightShiftRegister> iRightShiftRegisters;
	   private Vector<JNode> inputBorderNodes;
	   private Vector<JNode> outputBorderNodes;
	   private IDiagram iDiagram;
	   
	   public IForLoop(int nodeId, int parentId, Vector<ITerminal> inputTerminals,
			           Vector<ITerminal> outputTerminals,  ILoopIndex iLoopIndex, ILoopMax iLoopMax,
			           Vector<ITunnel> iTunnels,Vector<ILeftShiftRegister> iLeftShiftRegisters,
			           Vector<IRightShiftRegister> iRightShiftRegisters,IDiagram iDiagram){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "IForLoop";
		   this.iLoopIndex = iLoopIndex;
		   this.iLoopMax = iLoopMax;
		   this.iTunnels = iTunnels;
		   this.iLeftShiftRegisters = iLeftShiftRegisters;
		   this.iRightShiftRegisters = iRightShiftRegisters;
		   this.inputBorderNodes = new Vector<JNode>();
		   this.outputBorderNodes = new Vector<JNode>();
		   this.iDiagram = iDiagram;
		   
		   for(ITunnel i : this.iTunnels){
			   if(!i.getIsInput()){ 
				   outputBorderNodes.addElement(i);
			   }else{
				   inputBorderNodes.addElement(i);
			   }
		   }
		   for(ILeftShiftRegister i : this.iLeftShiftRegisters){
			   inputBorderNodes.addElement(i);
		   }
		   for(IRightShiftRegister i : this.iRightShiftRegisters){
			   outputBorderNodes.addElement(i);
		   }
		   this.indegree = this.inputBorderNodes.size();
		   if(this.iLoopMax.getIsInputConnected()){
			   this.indegree++;
		   }
	   }
	   
	   public IDiagram getIDiagram(){
		   return this.iDiagram;
	   }
	   public ILoopIndex getILoopIndex(){
		   return this.iLoopIndex;
	   }
	   public ILoopMax getILoopMax(){
		   return this.iLoopMax;
	   }
	   public Vector<ITunnel> getITunnels(){
		   return this.iTunnels;
	   }
	   public Vector<ILeftShiftRegister> getILeftShiftRegisters(){
		   return this.iLeftShiftRegisters;
	   }
	   public Vector<IRightShiftRegister> getIRightShiftRegisters(){
		   return this.iRightShiftRegisters;
	   }
	   public void print(){
		   System.out.println("IForLoop begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   this.iLoopIndex.print();
		   this.iLoopMax.print();
		   for(int i=0; i<this.iTunnels.size(); i++){
			   this.iTunnels.get(i).print();
		   }
		   for(int i=0; i<this.iLeftShiftRegisters.size(); i++){
			   this.iLeftShiftRegisters.get(i).print();
		   }
		   for(int i=0; i<this.iRightShiftRegisters.size(); i++){
			   this.iRightShiftRegisters.get(i).print();
		   }
		   this.iDiagram.print();
		   System.out.println("IForLoop end\n");
	   }
	   public Vector<JNode> getOutputBorderNodes(){
		      return this.outputBorderNodes;
	   }
	   public Vector<JNode> getInputBorderNodes(){
			  return this.inputBorderNodes;
	   }
}
