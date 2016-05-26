package Dfir_representation;

import java.util.Vector;

import org.w3c.dom.Node;
import org.w3c.dom.NodeList;


public class ICompoundArithmeticNode extends JNode{
	   
	   private ICompoundArithmeticNodeMode mode;
	   private Vector<Boolean> invertedInputs;
	   private Boolean invertedOutput;
	   public ICompoundArithmeticNode(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, ICompoundArithmeticNodeMode mode, NodeList InvertedInputs, Node InvertedOutput){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.mode = mode;
		   this.invertedInputs = new Vector<Boolean>();
		   for(int i=0; i<InvertedInputs.getLength(); i++){
			   if(InvertedInputs.item(i).getTextContent().equals("False")){
				   this.invertedInputs.addElement(false);
			   }
			   else{
				   this.invertedInputs.addElement(true);
			   }
		   }
		   if(InvertedOutput.getTextContent().equals("False")){
			   this.invertedOutput = false;
		   }
		   else{
			   this.invertedOutput = true;
		   }
		   JNodeType = "ICompoundArithmeticNode";
	   }
	   public ICompoundArithmeticNodeMode getMode(){
		   return this.mode;
	   }
	   public Vector<Boolean> getInvertedInputs(){
		   return this.invertedInputs;
	   }
	   
	   public Boolean getInvertedOutput(){
		   return this.invertedOutput;
	   }
	   public void print(){
		   System.out.println("ICompoundArithmeticNode begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId + "\nMode: " + this.mode);
		   for(int i=0; i<invertedInputs.size(); i++){
			   System.out.println("InvertedInput: " + invertedInputs.get(i));
	       }
		   System.out.println("InvertedOutput: " + invertedOutput);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("ICompoundArithmeticNode end\n");
	   }
}