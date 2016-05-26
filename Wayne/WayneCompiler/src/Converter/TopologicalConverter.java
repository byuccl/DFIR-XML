package Converter;

import java.util.*;
import Dfir_representation.*;

public class TopologicalConverter {
	//map the ICompoundArithmeticNodeMode and two IPrimitiveMode to String
	private static final Map<IPrimitiveMode, String> IPrimitiveModeToString = new HashMap<IPrimitiveMode, String>();
	private static final Map<ICompoundArithmeticNodeMode, String> ICompoundArithmeticNodeModeToString = new HashMap<ICompoundArithmeticNodeMode, String>();
    public TopologicalConverter(){
		   ICompoundArithmeticNodeModeToString.put(ICompoundArithmeticNodeMode.valueOf("Add"),"+");
		   ICompoundArithmeticNodeModeToString.put(ICompoundArithmeticNodeMode.valueOf("Multiply"),"*");
		   ICompoundArithmeticNodeModeToString.put(ICompoundArithmeticNodeMode.valueOf("Or"),"|");
		   ICompoundArithmeticNodeModeToString.put(ICompoundArithmeticNodeMode.valueOf("And"),"&");
		   ICompoundArithmeticNodeModeToString.put(ICompoundArithmeticNodeMode.valueOf("Xor"),"^");
		   IPrimitiveModeToString.put(IPrimitiveMode.valueOf("ExSubtractPrimitive"),"-");
		   IPrimitiveModeToString.put(IPrimitiveMode.valueOf("ExDividePrimitive"),"/");
	}	
    private static Map<JNode, String> JNodeToName = new HashMap<JNode, String>();// a map to store the mapping information from itself to its name used in C++ code 
	
	// GetRightConstant will be invoked when a middle node like ICompoundArithmeticNode compute its right constant
    //s is the computed expression, c is the IConstant that is being computed, j is the index of this constant in the right expression
    //cp is the middle node which invokes this subroutine
	public String GetRightConstant(String s, IConstant c, int j, JNode cp){
		Boolean flag = false;
		Boolean isMultiply = false;
		String op = "";
		if(cp.getJNodeType() == "ICompoundArithmeticNode"){
			 ICompoundArithmeticNode n = (ICompoundArithmeticNode) cp;
			 flag = n.getInvertedInputs().get(j);
			 if(n.getMode() == ICompoundArithmeticNodeMode.valueOf("Multiply")){
				 isMultiply = true;
			 }
			 if(j > 0) op = ICompoundArithmeticNodeModeToString.get(n.getMode());
		}else if(cp.getJNodeType() == "IPrimitive"){
			 IPrimitive n = (IPrimitive) cp;
			 if(n.getMode() == IPrimitiveMode.valueOf("ExSubtractPrimitive") || n.getMode() == IPrimitiveMode.valueOf("ExDividePrimitive")){
				  if(j > 0) op = IPrimitiveModeToString.get(n.getMode());
			 }else{
				 if(j > 0) op = ",";
			 }
			
		}
		if(c.getIDataType().getBase() == BaseDataType.valueOf("IBoolean")){
			Boolean bv = true;
			if(c.getValue().equals("true") ){
				bv = true;
			}else if(c.getValue().equals("false")){
				bv = false;
			}
			if(flag){
				 if(bv){
					 s = s + op + "false";
				 }else{
					 s = s + op + "true";
				 }
			}else{
				if(bv){
				    s = s + op + "true";
			    }else{
				    s = s + op + "false";
			    }
			}
		}else if(c.getIDataType().getBase() == BaseDataType.valueOf("ISignedInt") || c.getIDataType().getBase() == BaseDataType.valueOf("IUnsignedInt")){
			int t = Integer.parseInt(c.getValue());
			if(flag){
				 if(isMultiply){
					 if(t < 0){
						 s = s + op + "1/(" + Integer.toString(t) + ")";
					 }else{
						 s = s + op + "1/" + Integer.toString(t);
					 }
				 }else{
					 if(t < 0){
						 s = s + op + Integer.toString(-t);
					 }else{
						 s = s + op + '(' + Integer.toString(-t) + ')';
					 }
				 }
			}else{
				if(t < 0){
				    s = s + op + '(' + c.getValue() + ')';
			    }else{
				    s = s + op + c.getValue();
			    }
			}
		}else if(c.getIDataType().getBase() == BaseDataType.valueOf("ISignedFixedPoint") || c.getIDataType().getBase() == BaseDataType.valueOf("IUnsignedFixedPoint") || c.getIDataType().getBase() == BaseDataType.valueOf("IDouble")){
			double t = Double.parseDouble(c.getValue());
			if(flag){
				 if(isMultiply){
					 if(t < 0){
						 s = s + op + "1/(" + Double.toString(t) + ")";
					 }else{
						 s = s + op + "1/" + Double.toString(t);
					 }
				 }else{
					 if(t < 0){
						 s = s + op + Double.toString(-t);
					 }else{
						 s = s + op + '(' + Double.toString(-t) + ')';
					 }
				 }				 
			}else{
				if(t < 0){
				    s = s + op + '(' + c.getValue() + ')';
			    }else{
				    s = s + op + c.getValue();
			    }
			}
		}else if(c.getIDataType().getBase() == BaseDataType.valueOf("IArray") || c.getIDataType().getBase() == BaseDataType.valueOf("IVariableSizedArray")  ||c.getIDataType().getBase() == BaseDataType.valueOf("IFixedSizeArray") ){
			if(flag){
				 if(isMultiply){
					 s = s + op + "(1/" + c.getName() + ")";
				 }else{
					 s = s + op + "(-" + c.getName() + ")";
				 }
			}else{
				s = s + op + c.getName();
			}
		}else if(c.getIDataType().getBase() == BaseDataType.valueOf("IComplex") ){
			if(flag){
				if(isMultiply){
					 s = s + op + "(1/" + c.getName() + ")";
				 }else{
					 s = s + op + "(-" + c.getName() + ")";
				 }	 
		    }else{
			     s = s + op + c.getName();
		    }
		}
		return s;
	}
	
	// getOneOfRightVariable will be invoked when a middle node like ICompoundArithmeticNode compute its right variable
    // s is the computed expression, name is the variable name that is used in the generated expression, j is the index of this constant in the right expression
    // cp is the middle node which invokes this subroutine
	public String getOneOfRightVariable(String s, String name, int j, JNode cp){
		Boolean flag = false;
		Boolean isMultiply = false;
		Boolean isBoolean = false;
		String op = "";
		if(cp.getJNodeType() == "ICompoundArithmeticNode"){
			 ICompoundArithmeticNode n = (ICompoundArithmeticNode) cp;
			 flag = n.getInvertedInputs().get(j);
			 if(n.getMode() == ICompoundArithmeticNodeMode.valueOf("Multiply")){
				 isMultiply = true;
			 }
			 if(n.getMode() == ICompoundArithmeticNodeMode.valueOf("Or")||n.getMode() == ICompoundArithmeticNodeMode.valueOf("Xor")
					                                                  ||n.getMode() == ICompoundArithmeticNodeMode.valueOf("And")){
				 isBoolean = true;
			 }
			 if(j > 0) op = ICompoundArithmeticNodeModeToString.get(n.getMode());
		}else if(cp.getJNodeType() == "IPrimitive"){
			 IPrimitive n = (IPrimitive) cp;
			 if(n.getMode() == IPrimitiveMode.valueOf("ExSubtractPrimitive") || n.getMode() == IPrimitiveMode.valueOf("ExDividePrimitive")){
				  if(j > 0) op = IPrimitiveModeToString.get(n.getMode());
			 }else{
				 if(j > 0) op = ",";
			 }
			
		}
		if(flag){
			if(isMultiply){
				s = s + op + "(1/" + name + ')';
			}else if(isBoolean){
				s = s + op + "(!" + name + ')';
			}
			else{
				s = s + op + "(-" + name + ')';
			}
		}else{
			s = s + op + name;
		}
		return s;
	}
	

	// getLeftVariable will be invoked when a middle node like ICompoundArithmeticNode compute its left variable
    // s is the name for left variable, cp is the middle node which invokes this subroutine, iDiagram is the IDiagram which this middle node inside
	public String getLeftVariable(String s, JNode cp, IDiagram iDiagram){
		if(!cp.getIsDeclared()) s = cp.getOutputTerminalIDataType(0).getCType() + ' ' + s;
		String leftLabel = cp.getName();
		for(JNode jth : cp.getOutputJNodes()){
			if(jth.getJNodeType() == "IDataAccessor"|| jth.getJNodeType() == "IRightShiftRegister" || jth.getJNodeType() == "ILoopMax"|| jth.getJNodeType() == "ICaseSelector"){//|| jth.getJNodeType() == "IFeedbackInputNode"
				jth.setIsused(true);
				leftLabel = jth.getName();
				if(jth.getIsDeclared()){
					s = jth.getName();
					
				}else{
					s = getDeclaration(jth, true, 0,jth.getName(),false);//considering the same datatype for input terminal and output terminal
					jth.setIsDeclared(true);
				}
				break;
			}
			else if(jth.getJNodeType() == "IFeedbackOutputNode"){
				//jth.setIsused(true); //for  init situation
				IFeedbackOutputNode fo = (IFeedbackOutputNode)jth;
				leftLabel = fo.getInitName();
				if(jth.getIsDeclared()){
					s = leftLabel;
					
				}else{
					s = getDeclaration(jth, true, 0,leftLabel,false);//considering the same datatype for input terminal and output terminal
					jth.setIsDeclared(true);
				}
				break;
			}
			else if(jth.getJNodeType() == "ILeftShiftRegister"){
				ILeftShiftRegister ls = (ILeftShiftRegister) jth;
				ls.setIsused(true,ls.getInputConnectionIndex(cp));
				leftLabel = ls.getName(ls.getInputConnectionIndex(cp));
				if(ls.getIsDeclared(ls.getInputConnectionIndex(cp))){
					s = ls.getName(ls.getInputConnectionIndex(cp));
				}else{
					s = getDeclaration(jth, true, 0,ls.getName(ls.getInputConnectionIndex(cp)),false);
					//s = jth.getFirstInputTerminalIDataType().getCType() + ' ' + ls.getName(ls.getInputConnectionIndex(cp));//considering the same datatype for input terminal and output terminal
					ls.setIsDeclared(true,ls.getInputConnectionIndex(cp));
				}
				
				break;
			}
			else if(jth.getJNodeType() == "ITunnel"){
				ITunnel t = (ITunnel) jth;
				if(t.isIndexingITunnelInsideLoop(iDiagram)){
					leftLabel = t.getIndexingITunnelName();
					if(jth.getIsDeclared()){
						s = t.getIndexingITunnelName();
					}else{
						s = getDeclaration(jth,true, 0, t.getIndexingITunnelName(),false);
						t.setIsDeclared(true);
					}
				}else{
					leftLabel = t.getName();
					if(jth.getIsDeclared()){
						s = t.getName();
					}else{
						s = getDeclaration(jth,true, 0, t.getName(),false);
						t.setIsDeclared(true);
					}
					
				}
				t.setIsused(true);
				
				break;
			}
		}
		JNodeToName.put(cp, leftLabel);
		cp.setIsDeclared(true);
		return s;
	}
	
	//getRightExpression will be invoked when a middle node like ICompoundArithmeticNode compute its right expression
	// it will use GetRightConstant or getOneOfRightVariable to convert the constants or variables
	// jth is the node which will be as its right constant or variable,j is the index of this constant or variable in this right expression
	// cp is the middle node which invokes this subroutine. iDiagram is the IDiagram which this middle node inside
	public String getRightExpression(JNode jth,int j, JNode cp,IDiagram iDiagram){
		String res="";
		if(jth.getJNodeType() == "IConstant"){
			 IConstant c = (IConstant)jth;
			 res = GetRightConstant(res,c,j,cp);
		 }
		 else if(jth.getJNodeType() == "IDataAccessor" || jth.getJNodeType() == "IRightShiftRegister" ||jth.getJNodeType() == "ILoopIndex"|| jth.getJNodeType() == "ILoopMax"|| jth.getJNodeType() == "ICaseSelector"|| jth.getJNodeType() == "IFeedbackOutputNode"){
			res = getOneOfRightVariable(res,jth.getName(),j,cp);
		 }
		 else if(jth.getJNodeType() == "ITunnel"){
			    ITunnel t = (ITunnel)jth;
			    if(t.isIndexingITunnelInsideLoop(iDiagram)){
			    	res = getOneOfRightVariable(res,t.getIndexingITunnelName(),j,cp);
			    }else{
			    	res = getOneOfRightVariable(res,t.getName(),j,cp);
			    }
		 }else if(jth.getJNodeType() == "ILeftShiftRegister"){
			 ILeftShiftRegister ls = (ILeftShiftRegister)jth;
			 res = getOneOfRightVariable(res,ls.getName(ls.getOutputConnectionIndex(cp)),j,cp);
		 }else if(jth.getJNodeType() == "ICompoundArithmeticNode"){
			 res = getOneOfRightVariable(res,JNodeToName.get(jth),j,cp);
		 }else if(jth.getJNodeType() == "IPrimitive"){
			 IPrimitive p = (IPrimitive)jth;
			 if(p.getMode() == IPrimitiveMode.valueOf("ExSubtractPrimitive") || p.getMode() == IPrimitiveMode.valueOf("ExDividePrimitive")){
				 res = getOneOfRightVariable(res,JNodeToName.get(jth),j,cp);
			 }else{
				 res = getOneOfRightVariable(res,p.getNameByOutputConnection(cp),j,cp);
			 }
		 }else{
			 System.out.println("ICompoundArithmeticNode do not recognize this this input JNode" + jth.getNodeId()+ " :" +jth.getJNodeType());
		 }
		return res;
	}
	
	//getICompoundArithmeticNodeCode is invoked to generate the C++ code for a ICompoundArithmeticNode
	public String getICompoundArithmeticNodeCode(ICompoundArithmeticNode cp, IDiagram iDiagram){
		String res = "";
		Boolean flag = cp.getInvertedOutput();
		Boolean isMultiply = false;
		Boolean isBoolean = false;
		if(cp.getMode() == ICompoundArithmeticNodeMode.valueOf("Multiply")){
			 isMultiply = true;
		 }
		 if(cp.getMode() == ICompoundArithmeticNodeMode.valueOf("Or")||cp.getMode() == ICompoundArithmeticNodeMode.valueOf("Xor")
                 ||cp.getMode() == ICompoundArithmeticNodeMode.valueOf("And")){
			 isBoolean = true; 
		 }

		String leftVariable = getLeftVariable(cp.getName(),cp,iDiagram);
		
		String RightExpression = "";
		for(int j=0; j<cp.getInputJNodes().size(); j++){
			 JNode jth = cp.getInputJNodes().get(j);
			 RightExpression += getRightExpression(jth,j,cp,iDiagram);
		 }
		if(flag){
			if(isMultiply){
				res = leftVariable + "=1/(" + RightExpression + ")"; 
			}else if(isBoolean){
				res = leftVariable + "=!(" + RightExpression + ")"; 
			}
			else{
				res = leftVariable + "=-(" + RightExpression + ")"; 
			}
		}else{
			res = leftVariable + '=' + RightExpression;
		}
		return res;
	}
	
	//getArithmeticIPrimitiveCode is invoked to generate the C++ code for a IPrimitive which has the mode of ExSubtractPrimitive or ExDividePrimitive
	public String getArithmeticIPrimitiveCode(IPrimitive cp,IDiagram iDiagram){
		String leftVariable = getLeftVariable(cp.getName(),cp,iDiagram);
		int size = cp.getInputJNodes().size();
		String RightExpression ="";
		for(int j=0; j<size; j++){
			 JNode jth = cp.getInputJNodes().get(size-1-j);//because the input order of  IPrimitive subtract and divide is the opposite as ICompoundArithmeticNode Node
			 RightExpression += getRightExpression(jth,j,cp,iDiagram);
		 }
		return leftVariable + '=' + RightExpression;
	}
	
	//getIPrimitiveCallCode is used to generate the C++ code for other IPrimitive nodes
	public String getIPrimitiveCallCode(IPrimitive cp,IDiagram iDiagram){
		int inputsize = cp.getInputJNodes().size();
		int outputsize = cp.getOutputTerminals().size();
		String functionName = cp.getMode().toString();
		String parameterList = "";
		JNode jth;
		for(int i=0; i<inputsize; i++){
			 jth = cp.getInputJNodes().get(i);
			 parameterList += getRightExpression(jth,i,cp,iDiagram);
		}
		for(int j=0; j<outputsize; j++){
			if(j==0 && inputsize==0){
				 parameterList += cp.getName() + "_" + Integer.toString(j);
			}else{
				 parameterList += "," + cp.getName() + "_" + Integer.toString(j);
			}
		}
		
		return functionName + '(' + parameterList + ')';
	}
	
	//getWholeRightVariable is invoked when a left value node cp to compute its one and only one Right Variable converted from jth
	public String getWholeRightVariable(JNode cp, JNode jth,  IDiagram iDiagram){
		String res = "";
		if(jth.getJNodeType() == "IConstant"){
			 IConstant c = (IConstant)jth;
			 if(c.getIDataType().getBase() == BaseDataType.valueOf("IArray") || c.getIDataType().getBase() == BaseDataType.valueOf("IVariableSizedArray")  ||c.getIDataType().getBase() == BaseDataType.valueOf("IFixedSizeArray") ||c.getIDataType().getBase() == BaseDataType.valueOf("IComplex")){
				 res += c.getName();
			 }else{
				  res += c.getValue();
			 }
			
		}
		else if(jth.getJNodeType() == "IDataAccessor" || jth.getJNodeType() == "IRightShiftRegister" ||jth.getJNodeType() == "ILoopIndex" || jth.getJNodeType() == "ILoopMax"|| jth.getJNodeType() == "ICaseSelector" || jth.getJNodeType() == "IFeedbackOutputNode"){
			res  += jth.getName();
		}else if(jth.getJNodeType() == "ITunnel"){
			ITunnel t = (ITunnel)jth;
			if(t.isIndexingITunnelInsideLoop(iDiagram)){
				res  += t.getIndexingITunnelName();
			}
			else{
				res  += t.getName();
			}
		}else if(jth.getJNodeType() == "ILeftShiftRegister"){
			ILeftShiftRegister ls = (ILeftShiftRegister)jth;
			res += ls.getName(ls.getOutputConnectionIndex(cp));
		}else if(jth.getJNodeType() == "ICompoundArithmeticNode" ){
			if(JNodeToName.containsKey(jth)){
				res += JNodeToName.get(jth);
			}else{
				System.out.println("JNodeToName do not contains error!!" + jth.getName());
			}
		}else if(jth.getJNodeType() == "IPrimitive"){
			IPrimitive p = (IPrimitive)jth;
			if(p.getMode() == IPrimitiveMode.valueOf("ExSubtractPrimitive") || p.getMode() == IPrimitiveMode.valueOf("ExDividePrimitive")){
				if(JNodeToName.containsKey(jth)){
					res += JNodeToName.get(jth);
				}else{
					System.out.println("JNodeToName do not contains error!!" + jth.getName());
				}
		    }else{
		    	res += p.getNameByOutputConnection(cp);
		    }
		}else{
			// currently neglect other scenario where other node also has no inputTerminals
		}
		return res;
	}
	
	//getOutputIDataAccessorCode is invoked to generate the C++ code for a left value node like Output IDataAccessor cp
	public String getOutputIDataAccessorCode(JNode cp, IDiagram iDiagram){
		String res = cp.getName()+ '=';
		if(!cp.getIsDeclared()){
			res = getDeclaration(cp, true, 0, cp.getName(),false) + '=';
			cp.setIsDeclared(true);
		}
		
		JNode jth = cp.getInputJNodes().get(0);
		return res + getWholeRightVariable(cp,jth,iDiagram);
	}
	
	//getOutputInitFeedbackCode is invoked to generate the C++ code for the init terminal in IFeedbackOutputNode
	public String getOutputInitFeedbackCode(IFeedbackOutputNode cp, IDiagram iDiagram){
		String res = cp.getInitName();
		if(!cp.getIsDeclared()){
			res = getDeclaration(cp, true, 0, cp.getInitName(),false);
			cp.setIsDeclared(true);
		}
		
		JNode jth = cp.getInputJNodes().get(0);
		return res + '=' + getWholeRightVariable(cp,jth,iDiagram);
	}
	
	//getOutputITunnelCode is invoked to generate the C++ code for the output ITunnel in the case structure
	public String getOutputITunnelCode(JNode cp, IDiagram iDiagram){
		String res = cp.getName()+ '=';
		if(!cp.getIsDeclared()){
			res = getDeclaration(cp, true, 0, cp.getName(),false) + '=';
			cp.setIsDeclared(true);
		}
		
		JNode jth = cp.getInputJNodes().get(iDiagram.getDiagramIndex());
		return res + getWholeRightVariable(cp,jth,iDiagram);
	}
	
	//getOutputIndexingITunnelCode is invoked to generate the C++ code for the indexing ITunnel in the for loop
	public String getOutputIndexingITunnelCode(ITunnel cp, IDiagram iDiagram){
		String res = cp.getIndexingITunnelName() + '=';
		if(!cp.getIsDeclared()){
			res = getDeclaration(cp, true, 0, cp.getIndexingITunnelName(),false) + '=';
			cp.setIsDeclared(true);
		}
		JNode jth = cp.getInputJNodes().get(0);
		return res + getWholeRightVariable(cp,jth,iDiagram);
	}
	
	//getOutputILeftShiftRegisterCode is invoked to generate the C++ code for the ILeftShiftRegister in the for loop
	public String getOutputILeftShiftRegisterCode(ILeftShiftRegister cp, int index,IDiagram iDiagram){
		String res = cp.getName(index)+ '=';
		if(!cp.getIsDeclared(index)){
			res = getDeclaration(cp, true, index, cp.getName(index),false) + '=';
			cp.setIsDeclared(true,index);
		}
		JNode jth = cp.getInputJNodes().get(index);
		return res + getWholeRightVariable(cp,jth,iDiagram);
	}
	
	//outputShiftRegister is invoked to output the shift register codes inside the for loop
	public Vector<String> outputShiftRegister(Vector<IRightShiftRegister> v, int indnt){
		Vector<String> res = new Vector<String>();
		for(IRightShiftRegister rs : v){
			ILeftShiftRegister ls = (ILeftShiftRegister)JNode.getIdToJNode().get(rs.getAssociatedLeftShiftRegister().getNodeId());
			int len = ls.getInputTerminals().size()-1;
			for(int i=len; i>0; i--){
				res.addElement(polish(indnt,ls.getName(len) + "=" + ls.getName(len-1)));
			}
			res.addElement(polish(indnt,ls.getName(0) + "=" + rs.getName()));
		}
		return res;
	}	
	
	//get the auxiliary declaration for the constant
	public String getSubIConstantDeclaration(IDataType dt, String label, Boolean isReference){
		String res = "";
		String op = "";
		if(isReference) op = "&";
		if(dt.getBase() == BaseDataType.valueOf("IArray") || dt.getBase() == BaseDataType.valueOf("IVariableSizedArray")){
			res = dt.getCType() + op + ' ' +  label + "[]";
		}else if(dt.getBase() == BaseDataType.valueOf("IFixedSizeArray")){
			res = dt.getCType() + op + ' ' + label + "[" + dt.getArraySize() + "]";
		}else{
			res = dt.getCType() + op + ' ' + label;
		}
		return res;
	}
	
	//get the auxiliary declaration for variable
	public String getSubDeclaration(IDataType dt, String label, Boolean isReference){
		String res = "";
		String op = "";
		if(isReference) op = "&";
		if(dt.getBase() == BaseDataType.valueOf("IArray") || dt.getBase() == BaseDataType.valueOf("IVariableSizedArray")){
			res = "Array<"+ dt.getCType() +  ">" + op + ' ' +  label;
		}else if(dt.getBase() == BaseDataType.valueOf("IFixedSizeArray")){
			res = "Array<"+ dt.getCType() +  ">" + op + ' ' +  label + "(" + dt.getArraySize() + ")";
		}else{
			res = dt.getCType() + op + ' ' + label;
		}
		return res;
	}
    
	//get the declaration for constant or variable n, use the data type of n's the index terminal which is input when isInput is true 
	//and is output when it is false, isReference is used to decide whether to declare it in reference mode
	public String getDeclaration(JNode n, Boolean isInput, int index, String label, Boolean isReference){
		String res = "";
		if(n.getJNodeType() == "IConstant"){
			IConstant c = (IConstant)n;
			res = getSubIConstantDeclaration(c.getIDataType(), label, isReference);
		}else if(!isInput && n.getOutputTerminalIDataType(index) != null){
			res = getSubDeclaration(n.getOutputTerminalIDataType(index), label,isReference);
		}else if(isInput && n.getInputTerminalIDataType(index) != null){
			res = getSubDeclaration(n.getInputTerminalIDataType(index), label,isReference);
		}
		
		return res;
	}
	
	
	//isNeedDeclarationGroup is used to decide which node shoule be declared first in the main function of the converted C++ code
	public Boolean isNeedDeclarationGroup(JNode n){
		Boolean res = false;
		if(n.getJNodeType() == "IDataAccessor"){
			IDataAccessor da = (IDataAccessor) n;
			if(da.getDirection() == Direction.valueOf("INPUT")){
				res = true;
			}
		}else if(n.getJNodeType().equals("IConstant")){// for array IConstants and complex declaration
			IConstant c = (IConstant) n;
			if(c.getIDataType().getBase() == BaseDataType.valueOf("IArray") || c.getIDataType().getBase() == BaseDataType.valueOf("IVariableSizedArray") ||c.getIDataType().getBase() == BaseDataType.valueOf("IFixedSizeArray") || c.getIDataType().getBase() == BaseDataType.valueOf("IComplex") ){
				res = true;
			}
		}
		return res;
	}
	
	//getBorderNodeDeclaration is used to declare the border node in the for loop before entering the for loop
	public Vector<String> getBorderNodeDeclaration (IForLoop f, IDiagram iDiagram, int indnt){
		Vector<String> res_vec = new Vector<String>();
		for(JNode jth : f.getOutputBorderNodes()){
			String res = "";
			if(jth.getIsDeclared()) continue;
			if(jth.getJNodeType() == "IRightShiftRegister"){
				IRightShiftRegister rs = (IRightShiftRegister) jth;
				res = getDeclaration(rs,true, 0, rs.getName(),false);
				rs.setIsDeclared(true);
			}
			else if(jth.getJNodeType() == "ITunnel"){
				ITunnel t = (ITunnel) jth;
				if(t.isIndexingITunnelInsideLoop(iDiagram)){
					res = getDeclaration(jth,false, 0, t.getIndexingITunnelName(),false);
					t.setIsDeclared(true);
				}else{
					res = getDeclaration(jth,false, 0, t.getName(),false);
					t.setIsDeclared(true);
				}
				
			}
			if(!res.equals("")) res_vec.addElement(polish(indnt,res));
		}
		return res_vec;
	}
	
	//getCaseBorderNodeDeclaration is used to declare the border node in the case structure  before entering it
	public Vector<String> getCaseBorderNodeDeclaration (ICaseStructure c, IDiagram iDiagram, int indnt){
		Vector<String> res_vec = new Vector<String>();
		for(JNode jth : c.getiTunnels()){
			String res = "";
			ITunnel t = (ITunnel) jth;
			if(jth.getIsDeclared()) continue;
			if(t.isIndexingITunnelInsideLoop(iDiagram)){
				res = getDeclaration(jth,true, 0, t.getIndexingITunnelName(),false);
				t.setIsDeclared(true);
			}else{
				res = getDeclaration(jth,true, 0, t.getName(),false);
				t.setIsDeclared(true);
			}
			if(!res.equals("")) res_vec.addElement(polish(indnt,res));
		}
		return res_vec;
	}
	
	//add ";" and indention for the generated code
	public String polish(int indnt, String code){
		return indent(indnt) + code + ';';
	}
	
	//generate the FunctionDeclaration
	/*public String getFunctionDeclaration(IPrimitive p){
		String res = "";
		String parameterList = "";
		for(int i=0; i<p.getInputTerminals().size(); i++){
			if(i==0){
				parameterList += getDeclaration(p,true,i, "p"+Integer.toString(i),true);
			}else{
				parameterList += "," + getDeclaration(p,true,i,"p"+Integer.toString(i),true);
			}
		}
		for(int i=0; i<p.getOutputTerminals().size(); i++){
			if(i==0 && p.getInputTerminals().size()==0){
				parameterList += getDeclaration(p,false,i, "q"+Integer.toString(i),true);
			}else{
				parameterList += "," + getDeclaration(p,false,i,"q"+Integer.toString(i),true);
			}
		}
		res = "void " + p.getMode() + "(" + parameterList + "){"; 
		return res;
	}
	
	
	public Vector<String> generateIPrimitive(IPrimitive p) { 
			
		Vector<String> res = new Vector<String>();
		res.addElement(getFunctionDeclaration(p));
		p.getCCode(res);
		res.addElement("}");
		return res;
	}*/
	
    //used for indention
	public String indent(int indnt){
		String s = "";
		for(int i=0; i<indnt; i++){
			s += "  ";
		}
		return s;
	}
	
	
	//the core subroutine to process the nodes which have been sorted in topological order
	public void processAllJNodes(Vector<JNode> vec,Vector<String> res, IDiagram iDiagram, int indnt){
		
		 for(JNode n : vec){
			if(!n.getIsused() && !n.getIsDeclared() && isNeedDeclarationGroup(n)){
				if(n.getJNodeType().equals("IConstant")){// for array IConstants and complex declaration
					IConstant c = (IConstant) n;
					if(c.getIDataType().getBase() == BaseDataType.valueOf("IArray") || c.getIDataType().getBase() == BaseDataType.valueOf("IVariableSizedArray")  ||c.getIDataType().getBase() == BaseDataType.valueOf("IFixedSizeArray") ){
						res.addElement(polish(indnt,getDeclaration(c,false,0, "_"+c.getName(),false) + "=" + c.getValue()));
						res.addElement(polish(indnt,"Array<"+ c.getIDataType().getCType() +  ">" + ' ' +  c.getName() + "(" + "_"+c.getName()+")"));
					}else if(c.getIDataType().getBase() == BaseDataType.valueOf("IComplex")){
						res.addElement(polish(indnt,getDeclaration(c,false,0, c.getName(),false) + '(' + c.getReal() + ',' +c.getImag() + ')'));
					}
					
				}else if(n.getJNodeType().equals("IDataAccessor")){
					res.addElement(polish(indnt,getDeclaration(n,false, 0, n.getName(),false)));
					res.addElement(polish(indnt,"cin >> " + n.getName()));
				}
				//n.setIsused(true);
				n.setIsDeclared(true);
				
			}
		}// declare special IConstants and INPUT IDataAccessor!
		
		 
		 
		for(JNode n : vec){
			
			if(n.getJNodeType() == "ITunnel"){
				ITunnel t = (ITunnel)n;
				if(t.isOutputITunnelForCase() && !t.isBorderNodeInsideLoop(iDiagram)){
					t.setIsused(true);
				}
			}// to label OutputITunnelForCase as isused outside the case structure!
			
			if(n.getIsused()) {
				if(n.getJNodeType() == "IDataAccessor"){
					IDataAccessor da = (IDataAccessor) n;
					if(da.getDirection() == Direction.valueOf("OUTPUT")){
						res.addElement(polish(indnt,"cout << " + n.getName() + " << endl"));
					}
				}else if(n.getJNodeType() == "ITunnel"){
					ITunnel t = (ITunnel)n;
					if(t.isOutputITunnelForCase() && t.isBorderNodeInsideLoop(iDiagram)){
						t.setIsused(false);
					}
				}
				continue;
			}// to output the isused output IDataAccessor and handle the special case of output ITunnel for ICaseStructure
			
			
			if(n.getJNodeType() == "IFeedbackInputNode"){
				IFeedbackInputNode fi = (IFeedbackInputNode)n;
				if(!fi.getIsFeedbackDeclared()){
					res.addElement(polish(indnt,"vector<" + getDeclaration(fi,true,0,"",false) + "> "+ fi.getFeedbackName()));
					fi.setIsFeedbackDeclared(true);
				}
				res.addElement(polish(indnt,getOutputIDataAccessorCode(n,iDiagram)));
				res.addElement(polish(indnt, fi.getFeedbackName()+".push_back("+ fi.getName() + ")"));
				fi.setIsused(true);
			}else if(n.getJNodeType() == "IFeedbackOutputNode"){
				IFeedbackOutputNode fo = (IFeedbackOutputNode) n;
				IFeedbackInputNode fi = fo.getIFeedbackInputNode();
				if(!fi.getIsFeedbackDeclared()){
					res.addElement(polish(indnt,"vector<" + getDeclaration(fi,true,0,"",false) + "> "+ fi.getFeedbackName()));
					fi.setIsFeedbackDeclared(true);
				}
				if(!fo.getIsDeclared() && fo.getIsInputConnected()){
					res.addElement(polish(indnt, getOutputInitFeedbackCode(fo,iDiagram)));
				}
				
				String loop_index = fo.getLoopIndexLabel();
				if(loop_index.equals("")){
					loop_index = "0";
				}
				res.addElement(polish(indnt, getDeclaration(fo,false,0,fo.getName(),false)));
				res.addElement(polish(indnt, "getValueFromFeedback("+ fi.getFeedbackName() + ", " + fi.getdelay() + ", " + loop_index + ", " + fo.getInitName() + ", " + fo.getName() + ")"));
				fo.setIsused(true);
			}else if(n.getJNodeType() == "ICompoundArithmeticNode"){
				ICompoundArithmeticNode c = (ICompoundArithmeticNode) n;
				res.addElement(polish(indnt,getICompoundArithmeticNodeCode(c,iDiagram)));
				c.setIsused(true);
			}else if(n.getJNodeType() == "IPrimitive"){
				IPrimitive p = (IPrimitive) n;
				if(p.getMode() == IPrimitiveMode.valueOf("ExSubtractPrimitive") || p.getMode() == IPrimitiveMode.valueOf("ExDividePrimitive")){
					res.addElement(polish(indnt,getArithmeticIPrimitiveCode(p,iDiagram)));
				    p.setIsused(true);
				}else{
					for(int i=0; i<p.getOutputTerminals().size(); i++){
						 res.addElement(polish(indnt,getDeclaration(p, false, i, p.getNameByOutputIndex(i),false)));
					}
					res.addElement(polish(indnt,getIPrimitiveCallCode(p,iDiagram)));
					IDiagram.setHasPrimitive(true);
				    p.setIsused(true);
				}
			}else if(n.getJNodeType() == "IDataAccessor" || n.getJNodeType() == "IRightShiftRegister" ||n.getJNodeType() == "ILoopMax" || n.getJNodeType() == "ICaseSelector"){
				if(n.getJNodeType() == "IDataAccessor"){
					IDataAccessor da = (IDataAccessor)n;
					if(da.getDirection() == Direction.valueOf("INPUT")){
						continue;
					}
				}
				res.addElement(polish(indnt,getOutputIDataAccessorCode(n,iDiagram)));
				if(n.getJNodeType() == "IDataAccessor"){
					res.addElement(polish(indnt,"cout << " + n.getName() + " << endl"));
				}
				n.setIsused(true);
			}else if(n.getJNodeType() == "ITunnel"){
				ITunnel t = (ITunnel)n;
				t.setIsused(true);
				if(t.isIndexingITunnelInsideLoop(iDiagram)){
					res.addElement(polish(indnt,getOutputIndexingITunnelCode(t,iDiagram)));
				}
				else{
					res.addElement(polish(indnt,getOutputITunnelCode(t,iDiagram)));
				}
				if(t.isOutputITunnelForCase() && t.isBorderNodeInsideLoop(iDiagram)){
					t.setIsused(false);
				}
				
			}else if(n.getJNodeType() == "ILeftShiftRegister"){
				ILeftShiftRegister ls = (ILeftShiftRegister)n;
				for(int i=0; i<ls.getInputJNodes().size(); i++){
					   if(!ls.getIsuseds().get(i)){
						   res.addElement(polish(indnt,getOutputILeftShiftRegisterCode(ls,i,iDiagram)));
					   }
				}
				ls.setIsused(true);
			}else if(n.getJNodeType() == "IForLoop"){
				IForLoop f = (IForLoop) n;
				if(f.getIDiagram().getHasFeedback()){
					for(IFeedbackInputNode fi : f.getIDiagram().getiFeedbackInputNodes()){
						if(!fi.getIsFeedbackDeclared()){
							res.addElement(polish(indnt,"vector<" + getDeclaration(fi,true,0,"",false) + "> "+ fi.getFeedbackName()));
							fi.setIsFeedbackDeclared(true);
						}
					}
				}
				res.addAll(getBorderNodeDeclaration(f,iDiagram,indnt));
				res.addElement(indent(indnt) + "for(int " + f.getILoopIndex().getName() + " = 0; " +  f.getILoopIndex().getName() + " < " + f.getILoopMax().getName() + "; " + f.getILoopIndex().getName() + "++){");
				res.addAll(output(f.getIDiagram(),false,indnt+1));
				res.addAll(outputShiftRegister(f.getIRightShiftRegisters(),indnt+2));
				res.addElement(indent(indnt) + "}");
				n.setIsused(true);
			}
			else if(n.getJNodeType() == "ICaseStructure"){
				ICaseStructure c = (ICaseStructure) n;
				res.addAll(getCaseBorderNodeDeclaration(c,iDiagram,indnt));
				int caseNum = c.getiDiagrams().size();
				if(caseNum == 1){
					res.addElement(indent(indnt) + "{");
					res.addAll(output(c.getiDiagrams().get(0),false,indnt+1));
					res.addElement(indent(indnt) + "}");
				}else{
					res.addElement(indent(indnt) + "if(" + c.getCodeForNonDefaultCase(0) + "){");
					res.addAll(output(c.getiDiagrams().get(c.getIndexForNonDefaultCase(0)),false,indnt+1));
					for(int i=1; i<caseNum-1; i++){
						res.addElement(indent(indnt) + "}else if(" + c.getCodeForNonDefaultCase(i) + "){");
						res.addAll(output(c.getiDiagrams().get(c.getIndexForNonDefaultCase(i)),false,indnt+1));
					}
					res.addElement(indent(indnt) + "}else{");
					res.addAll(output(c.getiDiagrams().get(c.getiCaseSelector().getDefaultDiagramIndex()),false,indnt+1));
					res.addElement(indent(indnt) + "}");
				}
				n.setIsused(true);
			}
		}		
		
	}
	
	public Vector<String> output(IDiagram iDiagram, Boolean isTopIDiagram, int indnt){
		GetZeroIndegreeNodes zeroIndegreeNodes = new GetZeroIndegreeNodes();
		SortOrder sortOrder = new SortOrder();
		Vector<JNode> vec = new Vector<JNode>();
		Vector<String> res = new Vector<String>();
		
		//get the zeroIndegreeNodes
		Queue<JNode> que = zeroIndegreeNodes.work(iDiagram,isTopIDiagram);
		//sort the nodes in topological order
		sortOrder.work(que,vec,iDiagram);
		//process all the node and convert them into C++ code in vec
		processAllJNodes(vec,res,iDiagram,indnt+1);
		
		
		if(isTopIDiagram){
			Vector<String> final_res = new Vector<String>();
			
			final_res.addElement("#include<iostream>");
			if(IDiagram.getHasComplex()){
				final_res.addElement("#include<complex>");
			}
			if(IDiagram.getHasCmath()){
				final_res.addElement("#include<cmath>");
			}
			if(IDiagram.getHasVector()){
				final_res.addElement("#include<vector>");
			}
			if(IDiagram.getHasFeedbackInVI()){
				final_res.addElement("#include\"getValueFromFeedback.h\"");
			}
			if(IDiagram.getHasPrimitive()){
				final_res.addElement("#include\"primitive.h\"");
			}
			if(IDiagram.getHasArrayOperation()){
				final_res.addElement("#include\"array.h\"");
			}
			final_res.addElement("using namespace std;");
			
			final_res.addElement("int main(){");			
			for(String s : res){
				final_res.addElement(s);
		    }
			final_res.addElement(indent(indnt+1) + "return 0;");
			final_res.addElement("}");
			
			return final_res;
		}
		
		return res;
		
	}
}
