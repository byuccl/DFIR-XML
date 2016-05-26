package Dfir_representation;

public class IDataType{
	
	
	   private BaseDataType base;
	   private int WordLength;
	   private int LeftLength;
	   private int RightLength;
	   private int Dimensions;
	   private int ArraySize;
	   private IDataType ElementType;
	   
	   public IDataType(BaseDataType base){
		   this.WordLength = -1;
		   this.LeftLength = -1;
		   this.RightLength = -1;
		   this.Dimensions = -1;
		   this.ArraySize = -1;
		   this.base = base;
	   }
	   public void print(){
		   System.out.println("IDataType: " + this.base);
		   if(this.WordLength != -1){
			   System.out.println("WordLength: " + this.WordLength);
		   }
		   if(this.LeftLength != -1){
			   System.out.println("LeftLength: " + this.LeftLength);
		   }
		   if(this.RightLength != -1){
			   System.out.println("RightLength: " + this.RightLength);
		   }
		   if(this.Dimensions != -1){
			   System.out.println("Dimensions: " + this.Dimensions);
		   }
		   if(this.ArraySize != -1){
			   System.out.println("ArraySize: " + this.ArraySize);
		   }
		   if(this.ElementType != null){
			   ElementType.print();
		   }
	   }
	public String getCType(){
		String res = "";
		
		if(this.base == BaseDataType.valueOf("ISignedInt")){
			if(this.WordLength == -1) System.out.println("ISignedInt Error");
			if( this.WordLength <= 16){
				res = "short";
			}else if(this.WordLength <= 32){
				res = "int";
			}else{
				res = "long";
			}
		}else if(this.base == BaseDataType.valueOf("IUnsignedInt")){
			if(this.WordLength == -1) System.out.println("IUnsignedInt Error");
			if( this.WordLength <= 16){
				res = "unsigned short";
			}else if(this.WordLength <= 32){
				res = "unsigned int";
			}else{
				res = "unsigned long";
			}
		}
		
		else if(this.base == BaseDataType.valueOf("IDouble") || this.base == BaseDataType.valueOf("ISignedFixedPoint")|| this.base == BaseDataType.valueOf("IUnsignedFixedPoint")|| this.base == BaseDataType.valueOf("ISingle")){
			res = "double";//currently because this FPGA VI only support fixedpoint!!
		}else if(this.base == BaseDataType.valueOf("IBoolean")){
			res = "bool";
		}else if(this.base == BaseDataType.valueOf("IComplex")){
			res = "complex<" + this.ElementType.getCType() + ">";
		}else if(this.base == BaseDataType.valueOf("IArray") || this.base == BaseDataType.valueOf("IVariableSizedArray")||this.base == BaseDataType.valueOf("IFixedSizeArray")){
			res = this.ElementType.getCType();
		}
		return res;
	}
	
	public BaseDataType getBase() {
			return base;
    }   
	public int getWordLength() {
		return WordLength;
	}
	public void setWordLength(int wordLength) {
		WordLength = wordLength;
	}
	public int getLeftLength() {
		return LeftLength;
	}
	public void setLeftLength(int leftLength) {
		LeftLength = leftLength;
	}
	public int getRightLength() {
		return RightLength;
	}
	public void setRightLength(int rightLength) {
		RightLength = rightLength;
	}
	public int getDimensions() {
		return Dimensions;
	}
	public void setDimensions(int dimensions) {
		Dimensions = dimensions;
	}
	public int getArraySize() {
		return ArraySize;
	}
	public void setArraySize(int arraySize) {
		ArraySize = arraySize;
	}
	public IDataType getElementType() {
		return ElementType;
	}
	public void setElementType(IDataType elementType) {
		ElementType = elementType;
	}  
}