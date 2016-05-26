package Dfir_representation;

import java.util.Vector;


public class IPrimitive extends JNode{
	   
	   private IPrimitiveMode mode;
	   public IPrimitive(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, IPrimitiveMode mode){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.mode = mode;
		   JNodeType = "IPrimitive";
	   }
	   public IPrimitiveMode getMode(){
		   return this.mode;
	   }
	   public void getCCode(Vector <String> res){
		   if(mode == IPrimitiveMode.valueOf("ExSelectPrimitive")){
				res.addAll(ExSelectPrimitive());
			}else if(mode == IPrimitiveMode.valueOf("ExIncrementPrimitive")){
				res.addAll(ExIncrementPrimitive());
			}else if(mode == IPrimitiveMode.valueOf("ExDecrementPrimitive")){
				res.addAll(ExDecrementPrimitive());
			}else if(mode == IPrimitiveMode.valueOf("ExAbsoluteValuePrimitive")){
				res.addAll(ExAbsoluteValuePrimitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExAddArrayElementsPrimitive")){
				res.addAll(ExAddArrayElementsPrimitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExScaleByPowerOf2Primitive")){
				res.addAll(ExScaleByPowerOf2Primitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExSquareRootPrimitive")){
				res.addAll(ExSquareRootPrimitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExSquarePrimitive")){
				res.addAll(ExSquarePrimitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExNegatePrimitive")){
				res.addAll(ExNegatePrimitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExReciprocalPrimitive")){
				res.addAll(ExReciprocalPrimitive());
			}
			else if(mode == IPrimitiveMode.valueOf("ExMaxAndMinPrimitive")){
				res.addAll(ExMaxAndMinPrimitive());
			}
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
	   public Vector<String> ExSelectPrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "if(p1 == true) q0 = p2;");
			res.addElement(indent(1) + "else q0 = p0;");
			return res;
		}
		public Vector<String> ExIncrementPrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "q0 = p0+1;");
			return res;
		}
		public Vector<String> ExDecrementPrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "q0 = p0-1;");
			return res;
		}
		public Vector<String> ExAbsoluteValuePrimitive() { 
			Vector<String> res = new Vector<String>();
			IDiagram.setHasCmath(true);
			res.addElement(indent(1) + "q0 = abs(p0);");
			return res;
		}public Vector<String> ExAddArrayElementsPrimitive() { // problem with the array size
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "q0 = p0-1;");
			return res;
		}public Vector<String> ExScaleByPowerOf2Primitive() { 
			Vector<String> res = new Vector<String>();
			IDiagram.setHasCmath(true);
			res.addElement(indent(1) + "q0 = p0 * pow(2.0, p1);");
			return res;
		}public Vector<String> ExSquareRootPrimitive() { 
			Vector<String> res = new Vector<String>();
			IDiagram.setHasCmath(true);
			res.addElement(indent(1) + "q0 = sqrt(p0);");
			return res;
		}public Vector<String> ExSquarePrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "q0 = p0*p0;");
			return res;
		}public Vector<String> ExNegatePrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "q0 = -p0;");
			return res;
		}public Vector<String> ExReciprocalPrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "q0 = 1/p0;");
			return res;
		}public Vector<String> ExMaxAndMinPrimitive() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "if(p0 >= p1){");
			res.addElement(indent(2) + "q0 = p1;");
			res.addElement(indent(2) + "q1 = p0;");
			res.addElement(indent(1) + "}else{");
			res.addElement(indent(2) + "q0 = p0;");
			res.addElement(indent(2) + "q1 = p1;");
			res.addElement(indent(1) + "}");
			return res;
		}public Vector<String> ExDecrementPrimitive1() { 
			Vector<String> res = new Vector<String>();
			res.addElement(indent(1) + "return p0-1;");
			return res;
		}
		
		public String indent(int indnt){
			String s = "";
			for(int i=0; i<indnt; i++){
				s += "  ";
			}
			return s;
		}
		
	   public void print(){
		   System.out.println("IPrimitive begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId + "\nMode: " + this.mode);
		   for(int i=0; i<inputTerminals.size(); i++){
				   inputTerminals.get(i).print();
		   }
		  for(int i=0; i<outputTerminals.size(); i++){
				   outputTerminals.get(i).print();
		   }   
		  System.out.println("IPrimitive end\n");
	   }
}
