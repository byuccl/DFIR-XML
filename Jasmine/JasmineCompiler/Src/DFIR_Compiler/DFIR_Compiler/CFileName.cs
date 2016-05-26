using System;

namespace DFIR_Compiler
{
    public class CFileName
    {
        private string codefilename;
       
        public CFileName(string codeFileName)
        {
            codefilename = codeFileName;
        }

        public virtual string GetCodeFileName()
        {
            return codefilename;
        }

        //Add the suffix ".c" to the XML file name
        public virtual string GetCCodeName(string s)
        {
            return s + ".c";
        }

        public virtual void Print()
        {
            Console.WriteLine(codefilename);
        }
    }
}
