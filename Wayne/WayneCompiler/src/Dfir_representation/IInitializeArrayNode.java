package Dfir_representation;

import java.util.Vector;

public class IInitializeArrayNode extends JNode{
	
	   public IInitializeArrayNode(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "IInitializeArrayNode";
	   }
	   
	   public void print(){
		   System.out.println("IInitializeArrayNode begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IInitializeArrayNode end\n");
	   }
	   public String getNameByOutputIndex(int index) {
			String res = "";
			res = this.getName()+'_'+Integer.toString(index);
			return res;
		}
	   public String getNameByOutputConnection(JNode n) {
			String res = "";
			res = this.getNameByOutputIndex(this.getOutputConnectionIndex(n));
			return res;
		}
}
