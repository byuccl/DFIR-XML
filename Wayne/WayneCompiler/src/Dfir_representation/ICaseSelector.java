package Dfir_representation;

import java.util.Vector;

public class ICaseSelector extends IBorderNode{
	   private int defaultDiagramIndex;
	   private Vector<Range> ranges;
	   public ICaseSelector(int nodeId, int parentId, Vector<ITerminal> inputTerminals,Vector<ITerminal> outputTerminals, 
			                int defaultDiagramIndex,Vector<Range> ranges){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   this.JNodeType = "ICaseSelector";
		   this.defaultDiagramIndex = defaultDiagramIndex;
		   this.ranges = ranges;
	   }
	    public int getDefaultDiagramIndex() {
			return defaultDiagramIndex;
		}

		public void setDefaultDiagramIndex(int defaultDiagramIndex) {
			this.defaultDiagramIndex = defaultDiagramIndex;
		}

		public Vector<Range> getRanges() {
			return ranges;
		}

		public void setRanges(Vector<Range> ranges) {
			this.ranges = ranges;
		}
		
		public void decIndegree(){
			   this.indegree--;
			   JNode.IdToJNode.get(this.parentId).indegree--;			   
		   }
	  
	   public void print(){
		   System.out.println("ICaseSelector begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   System.out.println("DefaultDiagramIndex: " + this.defaultDiagramIndex);
		   for(int i=0; i<this.ranges.size(); i++){
			   this.ranges.get(i).print();
		   }
		   System.out.println("ICaseSelector end\n");
	   }
	   public Boolean getIsConnectedForIDiagram(IDiagram iDiagram){
		   Boolean res = false;
		   if(this.getOutputTerminals().get(iDiagram.getDiagramIndex()).getConnections().size()!=0){
			   res = true;
		   }
		   return res;
	   }
}
