package DFir_Parser;

import java.util.Vector;

import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import Dfir_representation.*;

public class IDiagramCreator {
	
	 public  IDataType GetIDataType(Node n){
		   Element e = (Element) n;
		   NodeList nl = n.getChildNodes();
		   String s = "";
		   for(int i=0; i< nl.getLength(); i++){
			   if(nl.item(i).getNodeName() != "#text"){
				   s = nl.item(i).getNodeName();
			   }
		   }
		   if(s == "") System.out.println("s is a null string");
		   BaseDataType base = BaseDataType.valueOf(s);
		   IDataTypeBuilder builder = new IDataTypeBuilder(base);
		   
		   if(s == "IBit" || s == "IBoolean" || s == "IDouble" || s == "IIncorrect" || s == "ISingle" || s == "IString" || s == "IUnknown" || s == "IUnsupported" || s == "IVoid"){
			   return builder.GetResult();
		   }
		   else if(s == "ISignedInt" || s=="IUnsignedInt"){
			   builder.SetWordLength(Integer.parseInt(e.getElementsByTagName("WordLength").item(0).getTextContent()));
			   return builder.GetResult();
		   }
		   else if(s == "ISignedFixedPoint" || s== "IUnsignedFixedPoint"){
			   builder.SetLeftLength(Integer.parseInt(e.getElementsByTagName("LeftLength").item(0).getTextContent()));
			   builder.SetRightLength(Integer.parseInt(e.getElementsByTagName("RightLength").item(0).getTextContent()));
			   return builder.GetResult();
		   }
		   else if(s == "IArray" || s== "IVariableSizedArray"){
			   builder.SetDimensions(Integer.parseInt(e.getElementsByTagName("Dimensions").item(0).getTextContent()));
			   Element elementElement = (Element) e.getElementsByTagName("Element").item(0);
			   Element dataTypeElement = (Element) elementElement.getElementsByTagName("IDataType").item(0);
			   builder.SetElementType(GetIDataType(dataTypeElement));
			   return builder.GetResult();
		   }
		   else if(s == "IComplex"){
			   Element elementElement = (Element) e.getElementsByTagName("Element").item(0);
			   Element dataTypeElement = (Element) elementElement.getElementsByTagName("IDataType").item(0);
			   builder.SetElementType(GetIDataType(dataTypeElement));
			   return builder.GetResult();
		   }else if(s == "IFixedSizeArray"){
			   builder.SetDimensions(Integer.parseInt(e.getElementsByTagName("Dimensions").item(0).getTextContent()));
			   builder.SetArraySize(Integer.parseInt(e.getElementsByTagName("ArraySize").item(0).getTextContent()));
			   Element elementElement = (Element) e.getElementsByTagName("Element").item(0);
			   Element dataTypeElement = (Element) elementElement.getElementsByTagName("IDataType").item(0);
			   builder.SetElementType(GetIDataType(dataTypeElement));
			   return builder.GetResult();
		   }else{
			   System.out.println("error");
			   return builder.GetResult();
		   }
	   }
	 
	 public  ITerminal GetITerminal(Element e){
		   Element dataTypeElement = (Element) e.getElementsByTagName("IDataType").item(0);
	   	   IDataType d = GetIDataType(dataTypeElement);
	   	   ITerminal iTerminal = new ITerminal(Integer.parseInt(e.getAttribute("TerminalId")),
	   			                               Integer.parseInt(e.getAttribute("TerminalIndex")),d);
	   	   Element connecetionsElement = (Element)e.getElementsByTagName("Connections").item(0);
	   	   NodeList connectionList = connecetionsElement.getElementsByTagName("Connection");
	   	   for(int i=0; i<connectionList.getLength(); i++){
	   		   Element temp = (Element)connectionList.item(i);
	   		   Connection connection = new Connection(Integer.parseInt(temp.getAttribute("TerminalId")),
	   				                                  Integer.parseInt(temp.getAttribute("NodeId")));
	   		   iTerminal.AddConnection(connection);
	   	   }
	   	   return iTerminal;
	 }
	 
	 public void getBothTerminals(Vector<ITerminal> InputTerminals, Vector<ITerminal> OutputTerminals, Element eElement){
		 if(eElement.getElementsByTagName("InputTerminals").getLength() > 0){
	       		Element e = (Element) eElement.getElementsByTagName("InputTerminals").item(0);
	       		NodeList eList = e.getElementsByTagName("ITerminal");
	       		for(int i=0; i<eList.getLength(); i++){
	       			Element temp1 = (Element)eList.item(i);
	       			ITerminal iTerminal = GetITerminal(temp1);
	       			InputTerminals.addElement(iTerminal);
	       		}
	       	}
	       	if(eElement.getElementsByTagName("OutputTerminals").getLength() > 0){
	       		Element e = (Element) eElement.getElementsByTagName("OutputTerminals").item(0);
	       		NodeList eList = e.getElementsByTagName("ITerminal");
	       		 for(int i=0; i<eList.getLength(); i++){
	       			Element temp1 = (Element)eList.item(i);
	       			ITerminal iTerminal = GetITerminal(temp1);
	       			OutputTerminals.addElement(iTerminal);
	       		}
	       	}
	 }
	 
	 public int getNodeId(Element eElement){
		 return Integer.parseInt(eElement.getAttribute("NodeId"));
	 }
	 public int getParentId(Element eElement){
		 return Integer.parseInt(eElement.getAttribute("ParentId"));
	 }
	 
	 public  IDiagram GetIDiagram(Node n){
		Element iDiagramElement = (Element) n;
		IDiagram iDiagram = new IDiagram(Integer.parseInt(iDiagramElement.getAttribute("NodeId")),Integer.parseInt(iDiagramElement.getAttribute("ParentId")),new Vector<ITerminal>(),new Vector<ITerminal>(),Integer.parseInt(iDiagramElement.getAttribute("DiagramIndex")));
		if(iDiagramElement.getElementsByTagName("IComplex").getLength() != 0){
			iDiagram.setHasComplex(true);
		}
	    Vector<IConstant> IConstants = new Vector<IConstant>();
	    Vector<IDataAccessor> IDataAccessors = new Vector<IDataAccessor>();
	    Vector<ICompoundArithmeticNode> ICompoundArithmeticNodes = new Vector<ICompoundArithmeticNode>();
	    Vector<IPrimitive> IPrimitives = new Vector<IPrimitive>();
	    Vector<IForLoop> iForLoops = new Vector<IForLoop>();
	    Vector<ICaseStructure> iCaseStructures = new Vector<ICaseStructure>();
	    Vector<IFeedbackInputNode> iFeedbackInputNodes = new Vector<IFeedbackInputNode>();
	    Vector<IFeedbackOutputNode> iFeedbackOutputNodes = new Vector<IFeedbackOutputNode>();
	    Vector<IArrayIndexNode> iArrayIndexNodes = new Vector<IArrayIndexNode>();
	    Vector<IReplaceArraySubsetNode> iReplaceArraySubsetNodes = new Vector<IReplaceArraySubsetNode>();
	    Vector<IInsertIntoArrayNode> iInsertIntoArrayNodes = new Vector<IInsertIntoArrayNode>();
	    Vector<IDeleteFromArrayNode> iDeleteFromArrayNodes = new Vector<IDeleteFromArrayNode>();
	    Vector<IInitializeArrayNode> iInitializeArrayNodes = new Vector<IInitializeArrayNode>();
	    Vector<IBuildArrayNode> iBuildArrayNodes = new Vector<IBuildArrayNode>();
	    Vector<IArraySubsetNode> iArraySubsetNodes = new Vector<IArraySubsetNode>();
	    
	    
	    NodeList IArrayIndexNodeList = iDiagramElement.getElementsByTagName("IArrayIndexNode");
	    for (int temp = 0; temp < IArrayIndexNodeList.getLength(); temp++) {
	       Node nNode = IArrayIndexNodeList.item(temp);
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IArrayIndexNode iArrayIndexNode = new IArrayIndexNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iArrayIndexNodes.addElement(iArrayIndexNode); 
	       }
	    }
	    iDiagram.setiArrayIndexNodes(iArrayIndexNodes);
	   
	   
	    
	    NodeList IReplaceArraySubsetNodeList = iDiagramElement.getElementsByTagName("IReplaceArraySubsetNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IReplaceArraySubsetNodeList.getLength(); temp++) {
	       Node nNode = IReplaceArraySubsetNodeList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IReplaceArraySubsetNode iReplaceArraySubsetNode = new IReplaceArraySubsetNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iReplaceArraySubsetNodes.addElement(iReplaceArraySubsetNode); 
	       }
	    }
	    iDiagram.setiReplaceArraySubsetNodes(iReplaceArraySubsetNodes);
	    
	    
	    NodeList IInsertIntoArrayNodeList = iDiagramElement.getElementsByTagName("IInsertIntoArrayNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IInsertIntoArrayNodeList.getLength(); temp++) {
	       Node nNode = IInsertIntoArrayNodeList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IInsertIntoArrayNode iInsertIntoArrayNode = new IInsertIntoArrayNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iInsertIntoArrayNodes.addElement(iInsertIntoArrayNode); 
	       }
	    }
	    iDiagram.setiInsertIntoArrayNodes(iInsertIntoArrayNodes);
	    
	    NodeList IDeleteFromArrayNodeList = iDiagramElement.getElementsByTagName("IDeleteFromArrayNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IDeleteFromArrayNodeList.getLength(); temp++) {
	       Node nNode = IDeleteFromArrayNodeList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IDeleteFromArrayNode iDeleteFromArrayNode = new IDeleteFromArrayNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iDeleteFromArrayNodes.addElement(iDeleteFromArrayNode); 
	       }
	    }
	    iDiagram.setiDeleteFromArrayNodes(iDeleteFromArrayNodes);
	    
	    
	    NodeList IInitializeArrayNodeList = iDiagramElement.getElementsByTagName("IInitializeArrayNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IInitializeArrayNodeList.getLength(); temp++) {
	       Node nNode = IInitializeArrayNodeList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IInitializeArrayNode iInitializeArrayNode = new IInitializeArrayNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iInitializeArrayNodes.addElement(iInitializeArrayNode); 
	       }
	    }
	    iDiagram.setiInitializeArrayNodes(iInitializeArrayNodes);
	    
	    
	    NodeList IBuildArrayNodeList = iDiagramElement.getElementsByTagName("IBuildArrayNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IBuildArrayNodeList.getLength(); temp++) {
	       Node nNode = IBuildArrayNodeList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IBuildArrayNode iBuildArrayNode = new IBuildArrayNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iBuildArrayNodes.addElement(iBuildArrayNode); 
	       }
	    }
	    iDiagram.setiBuildArrayNodes(iBuildArrayNodes);
	    
	    NodeList IArraySubsetNodeList = iDiagramElement.getElementsByTagName("IArraySubsetNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IArraySubsetNodeList.getLength(); temp++) {
	       Node nNode = IArraySubsetNodeList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> InputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> OutputTerminals = new Vector<ITerminal>();
	       	getBothTerminals(InputTerminals, OutputTerminals, eElement);
	       	int nodeId = getNodeId(eElement);
	       	int parentId = getParentId(eElement);
	       	IArraySubsetNode iArraySubsetNode = new IArraySubsetNode(nodeId,parentId,InputTerminals,OutputTerminals);
	       	iArraySubsetNodes.addElement(iArraySubsetNode); 
	       }
	    }
	    iDiagram.setiArraySubsetNodes(iArraySubsetNodes);
	    
	    
		NodeList nList = iDiagramElement.getElementsByTagName("IConstant");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < nList.getLength(); temp++) {
	       Node nNode = nList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Node dataTypeNode = eElement.getElementsByTagName("IDataType").item(0);
	       	IDataType d = GetIDataType(dataTypeNode);
	        Vector<ITerminal> iConstantOutputTerminals = new Vector<ITerminal>();
	        Element e = (Element) eElement.getElementsByTagName("OutputTerminals").item(0);
	   		NodeList eList = e.getElementsByTagName("ITerminal");
	   		for(int i=0; i<eList.getLength(); i++){
	   			Element temp1 = (Element)eList.item(i);
	   			ITerminal iTerminal = GetITerminal(temp1);
	   			iConstantOutputTerminals.addElement(iTerminal);
	   		}
	   		IConstant iConstant = new IConstant(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),new Vector<ITerminal>(),iConstantOutputTerminals,
	   				eElement.getElementsByTagName("Value").item(0).getTextContent(),d);
	       	IConstants.addElement(iConstant); 
	       }
	    }
	    iDiagram.setIConstants(IConstants);
	    
	    NodeList IDataAccessorList = iDiagramElement.getElementsByTagName("IDataAccessor");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IDataAccessorList.getLength(); temp++) {
	       Node nNode = IDataAccessorList.item(temp);
	       
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> iDataAccessorInputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> iDataAccessorOutputTerminals = new Vector<ITerminal>();
	       	if(eElement.getElementsByTagName("InputTerminals").getLength() > 0){
	       		Element e = (Element) eElement.getElementsByTagName("InputTerminals").item(0);
	       		NodeList eList = e.getElementsByTagName("ITerminal");
	       		for(int i=0; i<eList.getLength(); i++){
	       			Element temp1 = (Element)eList.item(i);
	       			ITerminal iTerminal = GetITerminal(temp1);
	       			iDataAccessorInputTerminals.addElement(iTerminal);
	       		}
	       	}
	       	if(eElement.getElementsByTagName("OutputTerminals").getLength() > 0){
	       		Element e = (Element) eElement.getElementsByTagName("OutputTerminals").item(0);
	       		NodeList eList = e.getElementsByTagName("ITerminal");
	       		 for(int i=0; i<eList.getLength(); i++){
	       			Element temp1 = (Element)eList.item(i);
	       			ITerminal iTerminal = GetITerminal(temp1);
	       			iDataAccessorOutputTerminals.addElement(iTerminal);
	       		}
	       	}
	       	IDataAccessor iDataAccessor = new IDataAccessor(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),iDataAccessorInputTerminals,iDataAccessorOutputTerminals,
	           		eElement.getElementsByTagName("Name").item(0).getTextContent(),Direction.valueOf(eElement.getElementsByTagName("Direction").item(0).getTextContent()));
	       	IDataAccessors.addElement(iDataAccessor); 
	       }
	    }
	    iDiagram.setIDataAccessors(IDataAccessors);
	    
	    NodeList ICompoundArithmeticNodeList = iDiagramElement.getElementsByTagName("ICompoundArithmeticNode");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < ICompoundArithmeticNodeList.getLength(); temp++) {
	       Node nNode = ICompoundArithmeticNodeList.item(temp);
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	NodeList InvertedInputs = eElement.getElementsByTagName("InvertedInput");
	       	Node InvertedOutput =  eElement.getElementsByTagName("InvertedOutput").item(0);
	       	Vector<ITerminal> iCompoundArithmeticNodeInputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> iCompoundArithmeticNodeOutputTerminals = new Vector<ITerminal>();
	       	
	       	Element e = (Element) eElement.getElementsByTagName("InputTerminals").item(0);
	    		NodeList eList = e.getElementsByTagName("ITerminal");
	   		for(int i=0; i<eList.getLength(); i++){
	   			Element temp1 = (Element)eList.item(i);
	   			ITerminal iTerminal = GetITerminal(temp1);
	   			iCompoundArithmeticNodeInputTerminals.addElement(iTerminal);
	   		}
	   		Element e1 = (Element) eElement.getElementsByTagName("OutputTerminals").item(0);
	    		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	   		for(int i=0; i<eList1.getLength(); i++){
	   			Element temp1 = (Element)eList1.item(i);
	   			ITerminal iTerminal = GetITerminal(temp1);
	   			iCompoundArithmeticNodeOutputTerminals.addElement(iTerminal);
	   		}
	   		ICompoundArithmeticNode iCompoundArithmeticNode = new ICompoundArithmeticNode(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),iCompoundArithmeticNodeInputTerminals,iCompoundArithmeticNodeOutputTerminals,ICompoundArithmeticNodeMode.valueOf(eElement.getAttribute("Mode")),InvertedInputs,InvertedOutput);
	       	
	   		
	       	ICompoundArithmeticNodes.addElement(iCompoundArithmeticNode); 
	       }
	    }
	    iDiagram.setICompoundArithmeticNodes(ICompoundArithmeticNodes);
	    
	    
	    NodeList IPrimitiveList = iDiagramElement.getElementsByTagName("IPrimitive");
	    System.out.println("----------------------------");
	    for (int temp = 0; temp < IPrimitiveList.getLength(); temp++) {
	       Node nNode = IPrimitiveList.item(temp);
	       System.out.println("\nCurrent Element :" 
	          + nNode.getNodeName());
	       
	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	       	Element eElement = (Element) nNode;
	       	if(eElement.getParentNode() != n) continue;
	       	Vector<ITerminal> iPrimitiveInputTerminals = new Vector<ITerminal>();
	       	Vector<ITerminal> iPrimitiveOutputTerminals = new Vector<ITerminal>();
	       	Element e = (Element) eElement.getElementsByTagName("InputTerminals").item(0);
	    		NodeList eList = e.getElementsByTagName("ITerminal");
	   		for(int i=0; i<eList.getLength(); i++){
	   			Element temp1 = (Element)eList.item(i);
	   			ITerminal iTerminal = GetITerminal(temp1);
	   			iPrimitiveInputTerminals.addElement(iTerminal);
	   		}
	   		Element e1 = (Element) eElement.getElementsByTagName("OutputTerminals").item(0);
	    		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	   		for(int i=0; i<eList1.getLength(); i++){
	   			Element temp1 = (Element)eList1.item(i);
	   			ITerminal iTerminal = GetITerminal(temp1);
	   			iPrimitiveOutputTerminals.addElement(iTerminal);
	   		}
	   		IPrimitive iPrimitive = new IPrimitive(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),iPrimitiveInputTerminals,iPrimitiveOutputTerminals,
	       			IPrimitiveMode.valueOf(eElement.getAttribute("Mode")));
	   		IPrimitives.addElement(iPrimitive); 
	       }
	    }
	    iDiagram.setIPrimitives(IPrimitives);
	    
	    
	    NodeList IForLoopList = iDiagramElement.getElementsByTagName("IForLoop");
	    if(iDiagramElement.getElementsByTagName("IForLoop").getLength() != 0){
	    	for (int temp = 0; temp < IForLoopList.getLength(); temp++) {
	    	       Node nNode = IForLoopList.item(temp);
	    	       System.out.println("\nCurrent Element :" 
	    	          + nNode.getNodeName());
	    	       
	    	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	    	       	Element eElement = (Element) nNode;
	    	       	if(eElement.getParentNode() != n) continue;
	    	        NodeList ITunnelList = eElement.getElementsByTagName("ITunnel");
	    	   		Vector<ITunnel> iTunnels = new Vector<ITunnel>();
	    	   		NodeList ILeftShiftRegisterList = eElement.getElementsByTagName("ILeftShiftRegister");
	    	   		Vector<ILeftShiftRegister> iLeftShiftRegisters = new Vector<ILeftShiftRegister>();
	    	   		NodeList IRightShiftRegisterList = eElement.getElementsByTagName("IRightShiftRegister");
	    	   		Vector<IRightShiftRegister> iRightShiftRegisters = new Vector<IRightShiftRegister>();
	    	   		ILoopIndex finalILoopIndex = null;
	    	   		ILoopMax finalILoopMax = null;
	    	       	{//process ILoopIndex
	    	       		NodeList iLoopMaxs = eElement.getElementsByTagName("ILoopIndex");
	    	       		for(int j=0; j<iLoopMaxs.getLength(); j++){
	    	       			Element iLoopIndexElement = (Element) eElement.getElementsByTagName("ILoopIndex").item(j);
	    	       			if(iLoopIndexElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iLoopIndexInputTerminals = new Vector<ITerminal>();
		    	           	Vector<ITerminal> iLoopIndexOutputTerminals = new Vector<ITerminal>();
		    	       		Element e1 = (Element) iLoopIndexElement.getElementsByTagName("OutputTerminals").item(0);
		    	        		NodeList eList1 = e1.getElementsByTagName("ITerminal");
		    	       		for(int i=0; i<eList1.getLength(); i++){
		    	       			Element temp1 = (Element)eList1.item(i);
		    	       			ITerminal iTerminal = GetITerminal(temp1);
		    	       			iLoopIndexOutputTerminals.addElement(iTerminal);
		    	       		}
		    	       		ILoopIndex iLoopIndex = new ILoopIndex(Integer.parseInt(iLoopIndexElement.getAttribute("NodeId")),Integer.parseInt(iLoopIndexElement.getAttribute("ParentId")),iLoopIndexInputTerminals,iLoopIndexOutputTerminals);
		    	       		finalILoopIndex = iLoopIndex;
	    	       		}
	    	       	}
	    	       	{//process ILoopMax
	    	       		NodeList iLoopMaxs = eElement.getElementsByTagName("ILoopMax");
	    	       		for(int j=0; j<iLoopMaxs.getLength(); j++){
	    	       			Element iLoopMaxElement = (Element) eElement.getElementsByTagName("ILoopMax").item(j);
	    	       			if(iLoopMaxElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iLoopMaxInputTerminals = new Vector<ITerminal>();
		    	           	Vector<ITerminal> iLoopMaxOutputTerminals = new Vector<ITerminal>();
		    	           	Element e = (Element) iLoopMaxElement.getElementsByTagName("InputTerminals").item(0);
		    	        		NodeList eList = e.getElementsByTagName("ITerminal");
		    	       		for(int i=0; i<eList.getLength(); i++){
		    	       			Element temp1 = (Element)eList.item(i);
		    	       			ITerminal iTerminal = GetITerminal(temp1);
		    	       			iLoopMaxInputTerminals.addElement(iTerminal);
		    	       		}
		    	       		Element e1 = (Element) iLoopMaxElement.getElementsByTagName("OutputTerminals").item(0);
		    	        		NodeList eList1 = e1.getElementsByTagName("ITerminal");
		    	       		for(int i=0; i<eList1.getLength(); i++){
		    	       			Element temp1 = (Element)eList1.item(i);
		    	       			if(temp1.getElementsByTagName("Connections").getLength() != 0){
		    	       				ITerminal iTerminal = GetITerminal(temp1);
		    	           			iLoopMaxOutputTerminals.addElement(iTerminal);
		    	       			}
		    	       		}

		         	       	ILoopMax iLoopMax = new ILoopMax(Integer.parseInt(iLoopMaxElement.getAttribute("NodeId")),Integer.parseInt(iLoopMaxElement.getAttribute("ParentId")),iLoopMaxInputTerminals,iLoopMaxOutputTerminals);
		         	        finalILoopMax = iLoopMax;
	    	       		}
	    	           	
	    	           }
	    	       	{//process ITunnel
	    	       		for(int j=0; j<ITunnelList.getLength(); j++){
	    	       			Element iTunnelElement = (Element) ITunnelList.item(j);
	    	       			if(iTunnelElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iTunnelInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iTunnelOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iTunnelElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iTunnelInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iTunnelElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iTunnelOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element GetInnerTerminalElement = (Element)iTunnelElement.getElementsByTagName("GetInnerTerminal").item(0);
	    	           		Element GetOuterTerminalElement = (Element)iTunnelElement.getElementsByTagName("GetOuterTerminal").item(0);
	    	           		ITunnel iTunnel = new ITunnel(Integer.parseInt(iTunnelElement.getAttribute("NodeId")),Integer.parseInt(iTunnelElement.getAttribute("ParentId")),iTunnelInputTerminals,iTunnelOutputTerminals,IndexingMode.valueOf(iTunnelElement.getElementsByTagName("IndexingMode").item(0).getTextContent()),IsInputMode.valueOf(iTunnelElement.getElementsByTagName("IsInput").item(0).getTextContent()),Integer.parseInt(GetInnerTerminalElement.getAttribute("TerminalId")),Integer.parseInt(GetOuterTerminalElement.getAttribute("TerminalId")));
	    	       			
	    	           		iTunnels.addElement(iTunnel);
	    	       		}
	    	           }
	    	       	{//process ILeftShiftRegister
	    	       		for(int j=0; j<ILeftShiftRegisterList.getLength(); j++){
	    	       			Element iLeftShiftRegisterElement = (Element) ILeftShiftRegisterList.item(j);
	    	       			if(iLeftShiftRegisterElement.getParentNode() != nNode) continue;
	    	       			Element	associated = (Element)iLeftShiftRegisterElement.getElementsByTagName("AssociatedRightShiftRegister").item(0);
	    	       			AssociatedRightShiftRegister associatedRightShiftRegister = new AssociatedRightShiftRegister(Integer.parseInt(associated.getAttribute("NodeId")),Integer.parseInt(associated.getAttribute("ParentId")));
	    	       			Vector<ITerminal> iLeftShiftRegisterInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iLeftShiftRegisterOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iLeftShiftRegisterElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iLeftShiftRegisterInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iLeftShiftRegisterElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iLeftShiftRegisterOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		ILeftShiftRegister iLeftShiftRegister = new ILeftShiftRegister(Integer.parseInt(iLeftShiftRegisterElement.getAttribute("NodeId")),Integer.parseInt(iLeftShiftRegisterElement.getAttribute("ParentId")),iLeftShiftRegisterInputTerminals,iLeftShiftRegisterOutputTerminals,associatedRightShiftRegister);
	    	       			
	    	           		iLeftShiftRegisters.addElement(iLeftShiftRegister);
	    	       		}
	    	           }
	    	       	{//process IRightShiftRegister
	    	       		for(int j=0; j<IRightShiftRegisterList.getLength(); j++){
	    	       			Element iRightShiftRegisterElement = (Element) IRightShiftRegisterList.item(j);
	    	       			if(iRightShiftRegisterElement.getParentNode() != nNode) continue;
	    	       			Element	associated = (Element)iRightShiftRegisterElement.getElementsByTagName("AssociatedLeftShiftRegister").item(0);
	    	       			AssociatedLeftShiftRegister associatedLeftShiftRegister = new AssociatedLeftShiftRegister(Integer.parseInt(associated.getAttribute("NodeId")),Integer.parseInt(associated.getAttribute("ParentId")));
	    	       			Vector<ITerminal> iRightShiftRegisterInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iRightShiftRegisterOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iRightShiftRegisterElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iRightShiftRegisterInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iRightShiftRegisterElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iRightShiftRegisterOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		IRightShiftRegister iRightShiftRegister = new IRightShiftRegister(Integer.parseInt(iRightShiftRegisterElement.getAttribute("NodeId")),Integer.parseInt(iRightShiftRegisterElement.getAttribute("ParentId")),iRightShiftRegisterInputTerminals,iRightShiftRegisterOutputTerminals,associatedLeftShiftRegister);
	    	       			iRightShiftRegisters.addElement(iRightShiftRegister);
	    	       		}
	    	           }
	    	      
	    	       	IDiagram iForLoopDiagram = GetIDiagram(eElement.getElementsByTagName("IDiagram").item(0));
	    	       	IForLoop iForLoop = new IForLoop(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),new Vector<ITerminal>(),new Vector<ITerminal>(),finalILoopIndex, finalILoopMax, iTunnels, iLeftShiftRegisters, iRightShiftRegisters,iForLoopDiagram);
	    	       	iForLoops.addElement(iForLoop);
	    	       }
	    	    }
	    }
	    iDiagram.setIForLoops(iForLoops);
	    
	    /*NodeList IWhileLoopList = iDiagramElement.getElementsByTagName("IWhileLoop");
	    if(iDiagramElement.getElementsByTagName("IWhileLoop").getLength() != 0){
	    	for (int temp = 0; temp < IWhileLoopList.getLength(); temp++) {
	    	       Node nNode = IWhileLoopList.item(temp);
	    	       System.out.println("\nCurrent Element :" 
	    	          + nNode.getNodeName());
	    	       
	    	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	    	       	Element eElement = (Element) nNode;
	    	       	if(eElement.getParentNode() != n) continue;
	    	        NodeList ITunnelList = eElement.getElementsByTagName("ITunnel");
	    	   		Vector<ITunnel> iTunnels = new Vector<ITunnel>();
	    	   		NodeList ILeftShiftRegisterList = eElement.getElementsByTagName("ILeftShiftRegister");
	    	   		Vector<ILeftShiftRegister> iLeftShiftRegisters = new Vector<ILeftShiftRegister>();
	    	   		NodeList IRightShiftRegisterList = eElement.getElementsByTagName("IRightShiftRegister");
	    	   		Vector<IRightShiftRegister> iRightShiftRegisters = new Vector<IRightShiftRegister>();
	    	   		ILoopIndex finalILoopIndex = null;
	    	   		ILoopCondition finalILoopCondition = null;
	    	       	{//process ILoopIndex
	    	       		NodeList iLoopMaxs = eElement.getElementsByTagName("ILoopIndex");
	    	       		for(int j=0; j<iLoopMaxs.getLength(); j++){
	    	       			Element iLoopIndexElement = (Element) eElement.getElementsByTagName("ILoopIndex").item(j);
	    	       			if(iLoopIndexElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iLoopIndexInputTerminals = new Vector<ITerminal>();
		    	           	Vector<ITerminal> iLoopIndexOutputTerminals = new Vector<ITerminal>();
		    	       		Element e1 = (Element) iLoopIndexElement.getElementsByTagName("OutputTerminals").item(0);
		    	        		NodeList eList1 = e1.getElementsByTagName("ITerminal");
		    	       		for(int i=0; i<eList1.getLength(); i++){
		    	       			Element temp1 = (Element)eList1.item(i);
		    	       			ITerminal iTerminal = GetITerminal(temp1);
		    	       			iLoopIndexOutputTerminals.addElement(iTerminal);
		    	       		}
		    	       		ILoopIndex iLoopIndex = new ILoopIndex(Integer.parseInt(iLoopIndexElement.getAttribute("NodeId")),Integer.parseInt(iLoopIndexElement.getAttribute("ParentId")),iLoopIndexInputTerminals,iLoopIndexOutputTerminals);
		    	       		finalILoopIndex = iLoopIndex;
	    	       		}
	    	       	}
	    	       	{//process ILoopMax
	    	       		NodeList iLoopConditions = eElement.getElementsByTagName("ILoopCondition");
	    	       		for(int j=0; j<iLoopConditions.getLength(); j++){
	    	       			Element iLoopConditionElement = (Element) eElement.getElementsByTagName("ILoopCondition").item(j);
	    	       			if(iLoopConditionElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iLoopConditionInputTerminals = new Vector<ITerminal>();
		    	           	Vector<ITerminal> iLoopConditionOutputTerminals = new Vector<ITerminal>();
		    	           	Element e = (Element) iLoopConditionElement.getElementsByTagName("InputTerminals").item(0);
		    	        		NodeList eList = e.getElementsByTagName("ITerminal");
		    	       		for(int i=0; i<eList.getLength(); i++){
		    	       			Element temp1 = (Element)eList.item(i);
		    	       			ITerminal iTerminal = GetITerminal(temp1);
		    	       			iLoopConditionInputTerminals.addElement(iTerminal);
		    	       		}
		    	       		

		    	       		ILoopCondition iLoopCondition = new ILoopCondition(Integer.parseInt(iLoopConditionElement.getAttribute("NodeId")),Integer.parseInt(iLoopConditionElement.getAttribute("ParentId")),iLoopConditionInputTerminals,iLoopConditionOutputTerminals);
		         	        finalILoopCondition = iLoopCondition;
	    	       		}
	    	           	
	    	           }
	    	       	{//process ITunnel
	    	       		for(int j=0; j<ITunnelList.getLength(); j++){
	    	       			Element iTunnelElement = (Element) ITunnelList.item(j);
	    	       			if(iTunnelElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iTunnelInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iTunnelOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iTunnelElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iTunnelInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iTunnelElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iTunnelOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element GetInnerTerminalElement = (Element)iTunnelElement.getElementsByTagName("GetInnerTerminal").item(0);
	    	           		Element GetOuterTerminalElement = (Element)iTunnelElement.getElementsByTagName("GetOuterTerminal").item(0);
	    	           		ITunnel iTunnel = new ITunnel(Integer.parseInt(iTunnelElement.getAttribute("NodeId")),Integer.parseInt(iTunnelElement.getAttribute("ParentId")),iTunnelInputTerminals,iTunnelOutputTerminals,IndexingMode.valueOf(iTunnelElement.getElementsByTagName("IndexingMode").item(0).getTextContent()),IsInputMode.valueOf(iTunnelElement.getElementsByTagName("IsInput").item(0).getTextContent()),Integer.parseInt(GetInnerTerminalElement.getAttribute("TerminalId")),Integer.parseInt(GetOuterTerminalElement.getAttribute("TerminalId")));
	    	       			
	    	           		iTunnels.addElement(iTunnel);
	    	       		}
	    	           }
	    	       	{//process ILeftShiftRegister
	    	       		for(int j=0; j<ILeftShiftRegisterList.getLength(); j++){
	    	       			Element iLeftShiftRegisterElement = (Element) ILeftShiftRegisterList.item(j);
	    	       			if(iLeftShiftRegisterElement.getParentNode() != nNode) continue;
	    	       			Element	associated = (Element)iLeftShiftRegisterElement.getElementsByTagName("AssociatedRightShiftRegister").item(0);
	    	       			AssociatedRightShiftRegister associatedRightShiftRegister = new AssociatedRightShiftRegister(Integer.parseInt(associated.getAttribute("NodeId")),Integer.parseInt(associated.getAttribute("ParentId")));
	    	       			Vector<ITerminal> iLeftShiftRegisterInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iLeftShiftRegisterOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iLeftShiftRegisterElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iLeftShiftRegisterInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iLeftShiftRegisterElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iLeftShiftRegisterOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		ILeftShiftRegister iLeftShiftRegister = new ILeftShiftRegister(Integer.parseInt(iLeftShiftRegisterElement.getAttribute("NodeId")),Integer.parseInt(iLeftShiftRegisterElement.getAttribute("ParentId")),iLeftShiftRegisterInputTerminals,iLeftShiftRegisterOutputTerminals,associatedRightShiftRegister);
	    	       			
	    	           		iLeftShiftRegisters.addElement(iLeftShiftRegister);
	    	       		}
	    	        }
	    	       	{//process IRightShiftRegister
	    	       		for(int j=0; j<IRightShiftRegisterList.getLength(); j++){
	    	       			Element iRightShiftRegisterElement = (Element) IRightShiftRegisterList.item(j);
	    	       			if(iRightShiftRegisterElement.getParentNode() != nNode) continue;
	    	       			Element	associated = (Element)iRightShiftRegisterElement.getElementsByTagName("AssociatedLeftShiftRegister").item(0);
	    	       			AssociatedLeftShiftRegister associatedLeftShiftRegister = new AssociatedLeftShiftRegister(Integer.parseInt(associated.getAttribute("NodeId")),Integer.parseInt(associated.getAttribute("ParentId")));
	    	       			Vector<ITerminal> iRightShiftRegisterInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iRightShiftRegisterOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iRightShiftRegisterElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iRightShiftRegisterInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iRightShiftRegisterElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iRightShiftRegisterOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		IRightShiftRegister iRightShiftRegister = new IRightShiftRegister(Integer.parseInt(iRightShiftRegisterElement.getAttribute("NodeId")),Integer.parseInt(iRightShiftRegisterElement.getAttribute("ParentId")),iRightShiftRegisterInputTerminals,iRightShiftRegisterOutputTerminals,associatedLeftShiftRegister);
	    	       			iRightShiftRegisters.addElement(iRightShiftRegister);
	    	       		}
	    	           }
	    	      
	    	       	IDiagram iForLoopDiagram = GetIDiagram(eElement.getElementsByTagName("IDiagram").item(0));
	    	       	IWhileLoop iWhileLoop = new IWhileLoop(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),new Vector<ITerminal>(),new Vector<ITerminal>(),finalILoopIndex, finalILoopCondition, iTunnels, iLeftShiftRegisters, iRightShiftRegisters,iForLoopDiagram);
	    	       	iWhileLoops.addElement(iWhileLoop);
	    	       }
	    	    }
       }
	    iDiagram.setiWhileLoops(iWhileLoops);*/
	    
	    NodeList ICaseStructureList = iDiagramElement.getElementsByTagName("ICaseStructure");
	    if(iDiagramElement.getElementsByTagName("ICaseStructure").getLength() != 0){
	    	for (int temp = 0; temp < ICaseStructureList.getLength(); temp++) {
	    	       Node nNode = ICaseStructureList.item(temp);
	    	       System.out.println("\nCurrent Element :" 
	    	          + nNode.getNodeName());
	    	       
	    	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	    	       	Element eElement = (Element) nNode;
	    	       	if(eElement.getParentNode() != n) continue;
	    	        NodeList ITunnelList = eElement.getElementsByTagName("ITunnel");
	    	   		Vector<ITunnel> iTunnels = new Vector<ITunnel>();
	    	   		NodeList IDiagramList = eElement.getElementsByTagName("IDiagram");
	    	   		Vector<IDiagram> iDiagrams = new Vector<IDiagram>();
	    	   		ICaseSelector iCaseSelector = null;
	    	   		
	    	       	
	    	       	{//process ITunnel
	    	       		for(int j=0; j<ITunnelList.getLength(); j++){
	    	       			Element iTunnelElement = (Element) ITunnelList.item(j);
	    	       			if(iTunnelElement.getParentNode() != nNode) continue;
	    	       			Vector<ITerminal> iTunnelInputTerminals = new Vector<ITerminal>();
	    	               	Vector<ITerminal> iTunnelOutputTerminals = new Vector<ITerminal>();
	    	               	Element e = (Element) iTunnelElement.getElementsByTagName("InputTerminals").item(0);
	    	            		NodeList eList = e.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList.getLength(); i++){
	    	           			Element temp1 = (Element)eList.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iTunnelInputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element e1 = (Element) iTunnelElement.getElementsByTagName("OutputTerminals").item(0);
	    	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	    	           		for(int i=0; i<eList1.getLength(); i++){
	    	           			Element temp1 = (Element)eList1.item(i);
	    	           			ITerminal iTerminal = GetITerminal(temp1);
	    	           			iTunnelOutputTerminals.addElement(iTerminal);
	    	           		}
	    	           		Element GetInnerTerminalElement = (Element)iTunnelElement.getElementsByTagName("GetInnerTerminal").item(0);
	    	           		Element GetOuterTerminalElement = (Element)iTunnelElement.getElementsByTagName("GetOuterTerminal").item(0);
	    	           		ITunnel iTunnel = new ITunnel(Integer.parseInt(iTunnelElement.getAttribute("NodeId")),Integer.parseInt(iTunnelElement.getAttribute("ParentId")),iTunnelInputTerminals,iTunnelOutputTerminals,IndexingMode.valueOf(iTunnelElement.getElementsByTagName("IndexingMode").item(0).getTextContent()),IsInputMode.valueOf(iTunnelElement.getElementsByTagName("IsInput").item(0).getTextContent()),Integer.parseInt(GetInnerTerminalElement.getAttribute("TerminalId")),Integer.parseInt(GetOuterTerminalElement.getAttribute("TerminalId")));
	    	       			
	    	           		iTunnels.addElement(iTunnel);
	    	       		}
	    	        }
	    	       	{//process IDiagrams
	    	       		for(int j=0; j<IDiagramList.getLength(); j++){
	    	       			Element iCaseDiagramElement = (Element) IDiagramList.item(j);
	    	       			if(iCaseDiagramElement.getParentNode() != nNode) continue;
	    	       			IDiagram iCaseDiagram = GetIDiagram(iCaseDiagramElement);
	    	       			iDiagrams.addElement(iCaseDiagram);
	    	       		}
	    	       	}	
	    	       	{//process ICaseSelector
	    	       		Element iCaseSelectorElement = (Element)eElement.getElementsByTagName("ICaseSelector").item(0);
	    	       		if(iCaseSelectorElement.getParentNode() != nNode) continue;
	    	       		Vector<ITerminal> iCaseSelectorInputTerminals = new Vector<ITerminal>();
    	               	Vector<ITerminal> iCaseSelectorOutputTerminals = new Vector<ITerminal>();
    	       			Vector<Range> Ranges = new Vector<Range>();
    	               	Element e0 = (Element) iCaseSelectorElement.getElementsByTagName("Ranges").item(0);
    	               	NodeList eList0 = e0.getElementsByTagName("Range");
	            		for(int i=0; i<eList0.getLength(); i++){
    	           			Element temp1 = (Element)eList0.item(i);
    	           			if(temp1.getElementsByTagName("SingleValue").getLength() == 0){
    	           				Ranges.addElement(new Range(Integer.parseInt(temp1.getElementsByTagName("LowValue").item(0).getTextContent()),Integer.parseInt(temp1.getElementsByTagName("HighValue").item(0).getTextContent()),Integer.parseInt(temp1.getAttribute("DiagramIndex"))));
    	           			}else{
    	           				if(temp1.getElementsByTagName("SingleValue").item(0).getTextContent().equals("False")){
    	           					Ranges.addElement(new Range(false,Integer.parseInt(temp1.getAttribute("DiagramIndex"))));    	    	           			
    	           				}else if(temp1.getElementsByTagName("SingleValue").item(0).getTextContent().equals("True")){
    	           					Ranges.addElement(new Range(true,Integer.parseInt(temp1.getAttribute("DiagramIndex"))));    
    	           				}else{
    	           					Ranges.addElement(new Range(Integer.parseInt(temp1.getElementsByTagName("SingleValue").item(0).getTextContent()),Integer.parseInt(temp1.getAttribute("DiagramIndex"))));   	    	           			
    	           				}
    	           			}
    	           		}
	            		Element e = (Element) iCaseSelectorElement.getElementsByTagName("InputTerminals").item(0);
	            		NodeList eList = e.getElementsByTagName("ITerminal");
	            		for(int i=0; i<eList.getLength(); i++){
		           			Element temp1 = (Element)eList.item(i);
		           			ITerminal iTerminal = GetITerminal(temp1);
		           			iCaseSelectorInputTerminals.addElement(iTerminal);
		           		}
		           		Element e1 = (Element) iCaseSelectorElement.getElementsByTagName("OutputTerminals").item(0);
		            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
		           		for(int i=0; i<eList1.getLength(); i++){
		           			Element temp1 = (Element)eList1.item(i);
		           			ITerminal iTerminal = GetITerminal(temp1);
		           			iCaseSelectorOutputTerminals.addElement(iTerminal);
		           		}
		           		Element defaultDiagramIndexElement = (Element)iCaseSelectorElement.getElementsByTagName("DefaultDiagramIndex").item(0);
	            		iCaseSelector = new ICaseSelector(Integer.parseInt(iCaseSelectorElement.getAttribute("NodeId")),Integer.parseInt(iCaseSelectorElement.getAttribute("ParentId")),iCaseSelectorInputTerminals,iCaseSelectorOutputTerminals,Integer.parseInt(defaultDiagramIndexElement.getAttribute("DiagramIndex")),Ranges);
	    	        }
	    	      
	    	       
	    	       	ICaseStructure iCaseStructure = new ICaseStructure(Integer.parseInt(eElement.getAttribute("NodeId")),Integer.parseInt(eElement.getAttribute("ParentId")),new Vector<ITerminal>(),new Vector<ITerminal>(),iCaseSelector, iTunnels,iDiagrams);
	    	       	iCaseStructures.addElement(iCaseStructure);
	    	       }
	    	    }
        }
	    iDiagram.setiCaseStructures(iCaseStructures);
	    
	    NodeList IFeedbackInputNodeList = iDiagramElement.getElementsByTagName("IFeedbackInputNode");
	    if(iDiagramElement.getElementsByTagName("IFeedbackInputNode").getLength() != 0){
	    	for (int temp = 0; temp < IFeedbackInputNodeList.getLength(); temp++) {
	    	       Node nNode = IFeedbackInputNodeList.item(temp);
	    	       System.out.println("\nCurrent Element :" 
	    	          + nNode.getNodeName());
	    	       
	    	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	    	          	Element iFeedbackInputNodeElement = (Element) nNode;
	    	          	if(iFeedbackInputNodeElement.getParentNode() != n) continue;
	    	          	Vector<ITerminal> iFeedbackInputNodeInputTerminals = new Vector<ITerminal>();
	    	          	Element outputNodeElement = (Element) iFeedbackInputNodeElement.getElementsByTagName("OutputNode").item(0);
	    	          	OutputNode outputNode = new OutputNode(Integer.parseInt(outputNodeElement.getAttribute("NodeId")),Integer.parseInt(outputNodeElement.getAttribute("ParentId")));
	    	          	Element e = (Element) iFeedbackInputNodeElement.getElementsByTagName("InputTerminals").item(0);
	            		NodeList eList = e.getElementsByTagName("ITerminal");
	            		for(int i=0; i<eList.getLength(); i++){
	 	           			Element temp1 = (Element)eList.item(i);
	 	           			ITerminal iTerminal = GetITerminal(temp1);
	 	           		    iFeedbackInputNodeInputTerminals.addElement(iTerminal);
	 	           		}
	            		IFeedbackInputNode iFeedbackInputNode = new IFeedbackInputNode(Integer.parseInt(iFeedbackInputNodeElement.getAttribute("NodeId")),Integer.parseInt(iFeedbackInputNodeElement.getAttribute("ParentId")),iFeedbackInputNodeInputTerminals,new Vector<ITerminal>(),Integer.parseInt(iFeedbackInputNodeElement.getElementsByTagName("Delay").item(0).getTextContent()),outputNode);
	            		iFeedbackInputNodes.addElement(iFeedbackInputNode);
	    	       }
	    	       
	    	}
	    }
	    iDiagram.setiFeedbackInputNodes(iFeedbackInputNodes);
	    
	    NodeList IFeedbackOutputNodeList = iDiagramElement.getElementsByTagName("IFeedbackOutputNode");
	    if(iDiagramElement.getElementsByTagName("IFeedbackOutputNode").getLength() != 0){
	    	for (int temp = 0; temp < IFeedbackOutputNodeList.getLength(); temp++) {
	    	       Node nNode = IFeedbackOutputNodeList.item(temp);
	    	       System.out.println("\nCurrent Element :" 
	    	          + nNode.getNodeName());
	    	       
	    	       if (nNode.getNodeType() == Node.ELEMENT_NODE) {
	    	          	Element iFeedbackOutputNodeElement = (Element) nNode;
	    	          	if(iFeedbackOutputNodeElement.getParentNode() != n) continue;
	    	          	Vector<ITerminal> iFeedbackOutputNodeInputTerminals = new Vector<ITerminal>();
	    	          	Vector<ITerminal> iFeedbackOutputNodeOutputTerminals = new Vector<ITerminal>();
	    	          	Element inputNodeNodeElement = (Element) iFeedbackOutputNodeElement.getElementsByTagName("InputNode").item(0);
	    	          	InputNode inputNode = new InputNode(Integer.parseInt(inputNodeNodeElement.getAttribute("NodeId")),Integer.parseInt(inputNodeNodeElement.getAttribute("ParentId")));
	    	          	Element e = (Element) iFeedbackOutputNodeElement.getElementsByTagName("InputTerminals").item(0);
	            		NodeList eList = e.getElementsByTagName("ITerminal");
	            		for(int i=0; i<eList.getLength(); i++){
	 	           			Element temp1 = (Element)eList.item(i);
	 	           			ITerminal iTerminal = GetITerminal(temp1);
	 	           		    iFeedbackOutputNodeInputTerminals.addElement(iTerminal);
	 	           		}
	            		Element e1 = (Element) iFeedbackOutputNodeElement.getElementsByTagName("OutputTerminals").item(0);
	            		NodeList eList1 = e1.getElementsByTagName("ITerminal");
	           		    for(int i=0; i<eList1.getLength(); i++){
	           			    Element temp1 = (Element)eList1.item(i);
	           			    ITerminal iTerminal = GetITerminal(temp1);
	           			    iFeedbackOutputNodeOutputTerminals.addElement(iTerminal);
	           		    }
	           		    IFeedbackOutputNode iFeedbackOutputNode = new IFeedbackOutputNode(Integer.parseInt(iFeedbackOutputNodeElement.getAttribute("NodeId")),Integer.parseInt(iFeedbackOutputNodeElement.getAttribute("ParentId")),iFeedbackOutputNodeInputTerminals,iFeedbackOutputNodeOutputTerminals,inputNode);
	           		    iFeedbackOutputNodes.addElement(iFeedbackOutputNode);
	    	       }
	    	       
	    	}
	    }
	    iDiagram.setiFeedbackOutputNodes(iFeedbackOutputNodes);
	    if(iFeedbackOutputNodes.size() != 0){
	    	iDiagram.setHasFeedback(true);
	    	IDiagram.setHasFeedbackInVI(true);
		    IDiagram.setHasVector(true);
	    }
	    
	    if(iDiagramElement.getElementsByTagName("IArray").getLength()!=0 || iDiagramElement.getElementsByTagName("IVariableSizedArray").getLength()!=0 || iDiagramElement.getElementsByTagName("IFixedSizeArray").getLength()!=0){
	    	IDiagram.setHasArrayOperation(true);
	    }
	    
	    return iDiagram;
	}
	  
}
