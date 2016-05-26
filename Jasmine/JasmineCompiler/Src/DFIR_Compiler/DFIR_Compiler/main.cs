//Currently my compiler can handle the IConstant, IDataAccessor, ICompoundArithmeticNode, IPrimitive, IForLoop, ICaseStructure
//And if you want to add more nodes in the compiler, just follow the topological sorting thoughts to build class and process them.

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace DFIR_Compiler
{

    class main
    {
        static void Main(string[] args)
        {
            //Load XML File
            string xmlFileName = @"C:\DFIR Project\DFIR\Tests\MyAdder.gvi.xml";
            string xmlFileName2= @"C:\DFIR Project\Compiler\Tests\MyAdder.gvi.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName);
            xmlDoc.DocumentElement.Normalize();


            //Validate XML File with XML Schema
            string schemaFileName = @"C:\DFIR Project\DFIR\Src\DFIR.xsd";
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", schemaFileName);
            SchemaValidation.Validate(xmlFileName, schemas);


            //XML DOM Parser
            Console.WriteLine("Root element :" + xmlDoc.DocumentElement.Name);
            IDiagram iDiagram = DomParser.GetIDiagram(xmlDoc.DocumentElement);
            Console.WriteLine("\nIDiagram Print :\n");
            iDiagram.Print();

            //Compiler
            File.Delete(xmlFileName2);
            File.Copy(xmlFileName, xmlFileName2);      //Copy the XML file into the Compiler directory
            CFileName cfilename = new CFileName(xmlFileName2);
            string s = cfilename.GetCCodeName(cfilename.GetCodeFileName());
            CodeConverter converter = new CodeConverter();
            converter.Printout(iDiagram, true, 0, s);

            Console.ReadKey();
        }
    }
}
