package Dfir_representation;

import java.util.Vector;


public class ICaseStructure extends IStructure{
	   
	   private Vector<ITunnel> iTunnels;
	   private Vector<IDiagram> iDiagrams;
	   private ICaseSelector iCaseSelector;
	
	   public Vector<ITunnel> getiTunnels() {
		   return iTunnels;
	    }
	    public void setiTunnels(Vector<ITunnel> iTunnels) {
			this.iTunnels = iTunnels;
		}

		public Vector<IDiagram> getiDiagrams() {
			return iDiagrams;
		}

		public void setiDiagrams(Vector<IDiagram> iDiagrams) {
			this.iDiagrams = iDiagrams;
		}

		public ICaseSelector getiCaseSelector() {
			return iCaseSelector;
		}

		public void setiCaseSelector(ICaseSelector iCaseSelector) {
			this.iCaseSelector = iCaseSelector;
		} 

	

	   public ICaseStructure(int nodeId, int parentId, Vector<ITerminal> inputTerminals,
			                 Vector<ITerminal> outputTerminals, ICaseSelector iCaseSelector,
			                 Vector<ITunnel> iTunnels,Vector<IDiagram> iDiagrams){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "ICaseStructure";
		   this.iCaseSelector = iCaseSelector;
		   this.iTunnels = iTunnels;
		   this.iDiagrams = iDiagrams;
		   this.indegree = 1;
		   for(ITunnel i : this.iTunnels){
			   if(i.getIsInput()){ 
				  this.indegree++;
			   }
		   }
		   
	   }
	   
	   public int getIndexForNonDefaultCase(int index){
		   int res_index = index;
		   ICaseSelector slt = this.iCaseSelector;
		   if(slt.getDefaultDiagramIndex() <= index){
			   res_index++;
		   }
		   return res_index;
	   }
	   
	   public String getCodeForNonDefaultCase(int index){
		   String res="";
		   int res_index = getIndexForNonDefaultCase(index);
		   
		   ICaseSelector slt = this.iCaseSelector;
		   Vector<Range> vec = new Vector<Range>();
		   for(int i=0; i<slt.getRanges().size(); i++){
			   Range cur = slt.getRanges().get(i);
			   if(cur.getDiagramIndex() == res_index){
				   vec.addElement(cur);
			   }
		   }
		   for(int i=0; i<vec.size(); i++){
			   String op = "||";
			   if(i == 0) op = "";
			   Range cur = vec.get(i);
			   if(cur.getIsSingle()){
				   res += op + this.iCaseSelector.getName() + " == " + cur.getSingleValue();
			   }else if(cur.getIsBoolean()){
				   res += op + this.iCaseSelector.getName() + " == " + cur.getTf();
			   }else{
				   res += op +  this.iCaseSelector.getName() + "<=" + cur.getHighValue() + " && " + this.iCaseSelector.getName() +  ">=" + cur.getLowValue();
			   }
		   }
		   
		   return res;
	   }
	   
	   public void print(){
		   System.out.println("ICaseStructure begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   this.iCaseSelector.print();
		   for(int i=0; i<this.iTunnels.size(); i++){
			   this.iTunnels.get(i).print();
		   }
		   for(int i=0; i<this.iDiagrams.size(); i++){
			   this.iDiagrams.get(i).print();
		   }
		   System.out.println("ICaseStructure end\n");
	   }
}
