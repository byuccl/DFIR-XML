package Main;

import java.io.*;
import java.util.Vector;

import javax.xml.XMLConstants;
import javax.xml.validation.*;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.*;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamSource;
import javax.xml.validation.Schema;
import javax.xml.validation.SchemaFactory;

import org.w3c.dom.Document;

import Converter.TopologicalConverter;
import DFir_Parser.IDiagramCreator;
import Dfir_representation.IDiagram;



public class MyCompiler {

	public static void main(String[] args) {
		try {	

			if (args.length != 2) {
				System.err.println("Args required: xmlFileName xmlSchemaFileName");
				System.exit(1);
			}
			 //input XML file
	         File inputFile = new File(args[0]);
	         DocumentBuilderFactory dbFactory 
	            = DocumentBuilderFactory.newInstance();
	         DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
	         Document doc = dBuilder.parse(inputFile);
	         doc.getDocumentElement().normalize();
	         
	         //validate the XML file
	         SchemaFactory factory = SchemaFactory.newInstance(XMLConstants.W3C_XML_SCHEMA_NS_URI);
	         Source schemaFile = new StreamSource(new File(args[1]));
	         Schema schema = factory.newSchema(schemaFile);
	         Validator validator = (Validator) schema.newValidator();
	         validator.validate(new DOMSource(doc));
	         
	         //parse the validated XML into IDiagram
	         IDiagramCreator creator = new IDiagramCreator();
	         IDiagram iDiagram = creator.GetIDiagram(doc.getDocumentElement());
	         
	         //convert the IDiagram into a vector of C++ code
	         TopologicalConverter converter = new TopologicalConverter();
	         Vector<String> vec = converter.output(iDiagram,true, 0);
	         
	         //output the C++ code into a cpp file
	         String outputFileName = args[0].split("\\.")[0] + ".cpp";
	         File outputFile = new File(outputFileName);
			 FileWriter fileWriter = new FileWriter(outputFile);
			 for(String s: vec){
				 fileWriter.write(s+'\n');
			 }
			 fileWriter.flush();
			 fileWriter.close();
	         
	      } catch (Exception e) {
	         e.printStackTrace();
	      }
	}

}
