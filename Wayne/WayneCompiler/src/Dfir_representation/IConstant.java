package Dfir_representation;

import java.util.Vector;


public  class IConstant extends JNode{
	   private String value;
	   private IDataType iDataType;
	   public IConstant(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, String value, IDataType iDataType){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.value = value;
		   this.iDataType = iDataType;
		   JNodeType = "IConstant";
	   }
	   public String getValue(){
		   if(this.getIDataType().getBase() == BaseDataType.valueOf("IArray") || this.getIDataType().getBase() == BaseDataType.valueOf("IVariableSizedArray")  ||this.getIDataType().getBase() == BaseDataType.valueOf("IFixedSizeArray") ){
				String s = this.value;
				return s.replace('[', '{').replace(']', '}');
			}else if(this.getIDataType().getBase() == BaseDataType.valueOf("IBoolean")){
				if(this.value.equals("False")){
					return "false";
				}else if(this.value.equals("True")){
					return "true";
				}
				
			}
		   return this.value;
	   }
	   
	   public String getReal(){
			if(this.iDataType.getBase() == BaseDataType.valueOf("IComplex")){
				return this.value.split("\\+")[0].trim();
			}
			return "IComplex Real Error!!!";
		}
	   public String getImag(){
			if(this.iDataType.getBase() == BaseDataType.valueOf("IComplex")){
				return this.value.split("\\+")[1].split("i")[0].trim();
			}
			return "IComplex Real Error!!!";
		}
	   
	   public IDataType getIDataType(){
		   return this.iDataType;
	   }
	   public void print(){
		   System.out.println("IConstant begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId + "\nValue: " + this.value);
		   iDataType.print();
		   for(int i=0; i<outputTerminals.size(); i++){
			   outputTerminals.get(i).print();
		   }
		   System.out.println("IConstant end\n");
	   }
	   public void AddOutputTerminals(ITerminal iTerminal){
		   this.outputTerminals.addElement(iTerminal);
	   }
}