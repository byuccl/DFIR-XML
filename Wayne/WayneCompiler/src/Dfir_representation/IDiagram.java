package Dfir_representation;

import java.util.Vector;

public  class IDiagram extends JNode{
	   private int diagramIndex;
	   private Vector<ICompoundArithmeticNode> iCompoundArithmeticNodes;
	   private Vector<IDataAccessor> iDataAccessors;
	   private Vector<IConstant> iConstants;
	   private Vector<IForLoop> iForLoops;
	   private Vector<ICaseStructure> iCaseStructures;
	   private Vector<IMethodCall> iMethodCalls;
	   private Vector<IPrimitive> iPrimitives;
	   private Vector<IFeedbackOutputNode> iFeedbackOutputNodes;
	   private Vector<IFeedbackInputNode> iFeedbackInputNodes;
	   private Vector<IArrayIndexNode> iArrayIndexNodes;
	   private Vector<IReplaceArraySubsetNode> iReplaceArraySubsetNodes;
	   private Vector<IInsertIntoArrayNode> iInsertIntoArrayNodes;
	   private Vector<IDeleteFromArrayNode> iDeleteFromArrayNodes;
	   private Vector<IInitializeArrayNode> iInitializeArrayNodes;
	   private Vector<IBuildArrayNode> iBuildArrayNodes;
	   private Vector<IArraySubsetNode> iArraySubsetNodes;
	   private static Boolean hasComplex = false; 
	   private static Boolean hasSelect = false; 
	   private static Boolean hasCmath = false; 
	   private static Boolean hasVector = false;
	   private static Boolean hasFeedbackInVI = false;
	   private static Boolean hasPrimitive = false;
	   private static Boolean hasArrayOperation = false;
	   private Boolean hasFeedback = false;
	   

	public IDiagram(int nodeId, int parentId, Vector<ITerminal> inputTerminals,
			          Vector<ITerminal> outputTerminals, int diagramIndex){
		   super(nodeId,parentId,inputTerminals,outputTerminals);
		   JNodeType = "IDiagram";
		   this.diagramIndex = diagramIndex;
		   iCompoundArithmeticNodes = new Vector<ICompoundArithmeticNode>();
		   iDataAccessors = new Vector<IDataAccessor>();
		   iConstants = new Vector<IConstant>();
		   iForLoops = new Vector<IForLoop>();
		   iCaseStructures = new Vector<ICaseStructure>();
		   iMethodCalls = new Vector<IMethodCall>();
		   iPrimitives = new Vector<IPrimitive>();
		   iFeedbackOutputNodes = new Vector<IFeedbackOutputNode>();
		   iArrayIndexNodes = new Vector<IArrayIndexNode>();
		   iReplaceArraySubsetNodes = new Vector<IReplaceArraySubsetNode>();
		   iInsertIntoArrayNodes = new Vector<IInsertIntoArrayNode>();
		   iDeleteFromArrayNodes = new Vector<IDeleteFromArrayNode>();
		   iInitializeArrayNodes = new Vector<IInitializeArrayNode>();
		   iBuildArrayNodes = new Vector<IBuildArrayNode>();
		   iArraySubsetNodes = new Vector<IArraySubsetNode>();
	   }
	    
	 
	    
	    public static Boolean getHasFeedbackInVI() {
		return hasFeedbackInVI;
	}



	public static void setHasFeedbackInVI(Boolean hasFeedbackInVI) {
		IDiagram.hasFeedbackInVI = hasFeedbackInVI;
	}



		public static Boolean getHasArrayOperation() {
		return hasArrayOperation;
	}



	public static void setHasArrayOperation(Boolean hasArrayOperation) {
		IDiagram.hasArrayOperation = hasArrayOperation;
	}



		public static Boolean getHasPrimitive() {
		return hasPrimitive;
	}



	public static void setHasPrimitive(Boolean hasPrimitive) {
		IDiagram.hasPrimitive = hasPrimitive;
	}



		public static Boolean getHasCmath() {
		    return hasCmath;
	    }
    
	    public static void setHasCmath(Boolean hasCmath) {
		   IDiagram.hasCmath = hasCmath;
	    }
	    public static  Boolean getHasSelect() {
			return hasSelect;
		}
		public static void setHasSelect(Boolean hasSelect) {
			IDiagram.hasSelect = hasSelect;
		}
		public static Boolean getHasComplex() {
			return hasComplex;
		}

		public void setHasComplex(Boolean hasComplex) {
			IDiagram.hasComplex = hasComplex;
		}
		public static Boolean getHasVector() {
			return hasVector;
		}

		public static void setHasVector(Boolean hasVector) {
			IDiagram.hasVector = hasVector;
		}

		public Boolean getHasFeedback() {
			return hasFeedback;
		}

		public void setHasFeedback(Boolean hasFeedback) {
			this.hasFeedback = hasFeedback;
		}

		public int getDiagramIndex() {
			return diagramIndex;
		}

		public void setDiagramIndex(int diagramIndex) {
			this.diagramIndex = diagramIndex;
		}

	

		public Vector<ICaseStructure> getiCaseStructures() {
			return iCaseStructures;
		}

		public void setiCaseStructures(Vector<ICaseStructure> iCaseStructures) {
			this.iCaseStructures = iCaseStructures;
		}

		public Vector<IMethodCall> getiMethodCalls() {
			return iMethodCalls;
		}

		public void setiMethodCalls(Vector<IMethodCall> iMethodCalls) {
			this.iMethodCalls = iMethodCalls;
		}

		public Vector<IFeedbackOutputNode> getiFeedbackOutputNodes() {
			return iFeedbackOutputNodes;
		}

		public void setiFeedbackOutputNodes(Vector<IFeedbackOutputNode> iFeedbackOutputNodes) {
			this.iFeedbackOutputNodes = iFeedbackOutputNodes;
		}

		public Vector<IFeedbackInputNode> getiFeedbackInputNodes() {
			return iFeedbackInputNodes;
		}

		public void setiFeedbackInputNodes(Vector<IFeedbackInputNode> iFeedbackInputNodes) {
			this.iFeedbackInputNodes = iFeedbackInputNodes;
		}

	   public void setIConstants(Vector<IConstant> c){
		   this.iConstants = c;
	   }
	   public void setIDataAccessors(Vector<IDataAccessor> d){
		   this.iDataAccessors = d;
	   }
	   public void setICompoundArithmeticNodes(Vector<ICompoundArithmeticNode> c){
		   this.iCompoundArithmeticNodes = c;
	   }
	   public void setIForLoops(Vector<IForLoop> c){
		   this.iForLoops = c;
	   }
	   public void setIPrimitives(Vector<IPrimitive> c){
		   this.iPrimitives = c;
	   }
	   public Vector<IConstant> getIConstants(){
		   return this.iConstants;
	   }
	   public Vector<IDataAccessor> getIDataAccessors(){
		   return this.iDataAccessors;
	   }
	   public Vector<ICompoundArithmeticNode> getICompoundArithmeticNodes(){
		   return this.iCompoundArithmeticNodes;
	   }
	   public Vector<IForLoop> getIForLoops(){
		   return this.iForLoops;
	   }
	   public Vector<IPrimitive> getIPrimitives(){
		   return this.iPrimitives;
	   }
	   
	   public Vector<ICompoundArithmeticNode> getiCompoundArithmeticNodes() {
		return iCompoundArithmeticNodes;
	}

	

	public Vector<IArrayIndexNode> getiArrayIndexNodes() {
		return iArrayIndexNodes;
	}



	public void setiArrayIndexNodes(Vector<IArrayIndexNode> iArrayIndexNodes) {
		this.iArrayIndexNodes = iArrayIndexNodes;
	}



	public Vector<IReplaceArraySubsetNode> getiReplaceArraySubsetNodes() {
		return iReplaceArraySubsetNodes;
	}



	public void setiReplaceArraySubsetNodes(Vector<IReplaceArraySubsetNode> iReplaceArraySubsetNodes) {
		this.iReplaceArraySubsetNodes = iReplaceArraySubsetNodes;
	}



	public Vector<IInsertIntoArrayNode> getiInsertIntoArrayNodes() {
		return iInsertIntoArrayNodes;
	}



	public void setiInsertIntoArrayNodes(Vector<IInsertIntoArrayNode> iInsertIntoArrayNodes) {
		this.iInsertIntoArrayNodes = iInsertIntoArrayNodes;
	}



	public Vector<IDeleteFromArrayNode> getiDeleteFromArrayNodes() {
		return iDeleteFromArrayNodes;
	}



	public void setiDeleteFromArrayNodes(Vector<IDeleteFromArrayNode> iDeleteFromArrayNodes) {
		this.iDeleteFromArrayNodes = iDeleteFromArrayNodes;
	}



	public Vector<IInitializeArrayNode> getiInitializeArrayNodes() {
		return iInitializeArrayNodes;
	}



	public void setiInitializeArrayNodes(Vector<IInitializeArrayNode> iInitializeArrayNodes) {
		this.iInitializeArrayNodes = iInitializeArrayNodes;
	}



	public Vector<IBuildArrayNode> getiBuildArrayNodes() {
		return iBuildArrayNodes;
	}



	public void setiBuildArrayNodes(Vector<IBuildArrayNode> iBuildArrayNodes) {
		this.iBuildArrayNodes = iBuildArrayNodes;
	}



	public Vector<IArraySubsetNode> getiArraySubsetNodes() {
		return iArraySubsetNodes;
	}



	public void setiArraySubsetNodes(Vector<IArraySubsetNode> iArraySubsetNodes) {
		this.iArraySubsetNodes = iArraySubsetNodes;
	}



	public void print(){
		   System.out.println("IDiagram begin");
		   System.out.println("NodeId: " + this.nodeId + ' ' + "ParentId: " + this.parentId);
		   System.out.println("DiagramIndex: " + this.diagramIndex);
		   for(int i=0; i<iConstants.size(); i++){
			   iConstants.get(i).print();
		   }
		   for(int i=0; i<iDataAccessors.size(); i++){
			   iDataAccessors.get(i).print();
		   }
		   for(int i=0; i<iCompoundArithmeticNodes.size(); i++){
			   iCompoundArithmeticNodes.get(i).print();
		   }
		   for(int i=0; i<iForLoops.size(); i++){
			   iForLoops.get(i).print();
		   }
		 
		   for(int i=0; i<iPrimitives.size(); i++){
			   iPrimitives.get(i).print();
		   }
		   for(int i=0; i<iCaseStructures.size(); i++){
			   iCaseStructures.get(i).print();
		   }
		   for(int i=0; i<iMethodCalls.size(); i++){
			   iMethodCalls.get(i).print();
		   }
		   for(int i=0; i<iFeedbackOutputNodes.size(); i++){
			   iFeedbackOutputNodes.get(i).print();
		   }
		   for(int i=0; i<iFeedbackInputNodes.size(); i++){
			   iFeedbackInputNodes.get(i).print();
		   }
		   System.out.println("IDiagram end\n");
	   }
	   
}   