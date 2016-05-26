package Dfir_representation;

public  class IDataTypeBuilder{
    
    private IDataType _IDataType;
 	public IDataTypeBuilder(BaseDataType base){
	 	     this._IDataType = new IDataType(base);
	    }
	public void SetWordLength(int WordLength){
		     this._IDataType.setWordLength(WordLength);
	    }
  	public void SetLeftLength(int LeftLength){
		     this._IDataType.setLeftLength(LeftLength);
	    }
	public void SetRightLength(int RightLength){
		     this._IDataType.setRightLength(RightLength);
	    }
	public void SetDimensions(int Dimensions){
		     this._IDataType.setDimensions(Dimensions);
	    }
	public void SetArraySize(int ArraySize){
		     this._IDataType.setArraySize(ArraySize);
	    }
	public void SetElementType(IDataType ElementType){
		     this._IDataType.setElementType(ElementType);
	    }
	public IDataType GetResult(){
	    	return this._IDataType;
	    }
}