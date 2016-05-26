package Dfir_representation;

public class Range{
	   private int lowValue;
	   private int highValue;
	   private int singleValue;
	   private int diagramIndex;
	   private Boolean tf;
	   

	private Boolean isSingle;
	   private Boolean isBoolean;
	   
	public Range(int lowValue, int highValue, int diagramIndex){
		  this.lowValue = lowValue;
		  this.highValue = highValue;
		  this.diagramIndex = diagramIndex;
		  this.isSingle = false;
		  this.isBoolean = false;
	   }
	   public Range(int singleValue, int diagramIndex){
		      this.singleValue = singleValue;
			  this.diagramIndex = diagramIndex;    
			  this.isSingle = true;
			  this.isBoolean = false;
	   }
	   public Range(Boolean b, int diagramIndex){
			  this.tf = b;
			  this.diagramIndex = diagramIndex;    
			  this.isBoolean = true;
			  this.isSingle = false;
	   }
	   
	    public Boolean getIsBoolean() {
		    return isBoolean;
	    }
	    public void setIsBoolean(Boolean isBoolean) {
		    this.isBoolean = isBoolean;
	    }
	    public Boolean getTf() {
			return tf;
		}
		public void setTf(Boolean tf) {
			this.tf = tf;
		}
		public Boolean getIsSingle() {
			return isSingle;
		}
		public void setIsSingle(Boolean isSingle) {
			this.isSingle = isSingle;
		}
	    public int getLowValue() {
			return lowValue;
		}
		public void setLowValue(int lowValue) {
			this.lowValue = lowValue;
		}
		public int getHighValue() {
			return highValue;
		}
		public void setHighValue(int highValue) {
			this.highValue = highValue;
		}
		public int getSingleValue() {
			return singleValue;
		}
		public void setSingleValue(int singleValue) {
			this.singleValue = singleValue;
		}
		public int getDiagramIndex() {
			return diagramIndex;
		}
		public void setDiagramIndex(int diagramIndex) {
			this.diagramIndex = diagramIndex;
		}
	  
	   public void print(){
		   System.out.println("Range begin");
		   if(this.singleValue != Integer.MIN_VALUE){
			   System.out.println("SingleValue: " + this.singleValue+ ' ' + "DiagramIndex: " + this.diagramIndex);
		   } else {
			   System.out.println("LowValue: " + this.lowValue + ' ' + "HighValue: " + this.highValue + ' ' + "DiagramIndex: " + this.diagramIndex);
		   }
		   System.out.println("Range end\n");
	   }
}
