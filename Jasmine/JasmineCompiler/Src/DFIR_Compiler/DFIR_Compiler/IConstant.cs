using System;
using System.Collections.Generic;

namespace DFIR_Compiler
{
    public class IConstant : INode
    {
        private string value;
        private IDataType iconstantIDataType;
        public IConstant(int nodeId, int parentId, List<ITerminal> inputTerminals, List<ITerminal> outputTerminals, string Value, IDataType iDataType) : base(nodeId, parentId, inputTerminals, outputTerminals)
        {
            value = Value;
            iconstantIDataType = iDataType;
            INodeType = "IConstant";
        }


        public virtual string GetValue()
        {
            if (GetIDataType().GetBase() == BaseDataType.IBoolean)
            {
                if (value == "False")
                {
                    return "false";
                }
                else if (value == "True")
                {
                    return "true";
                }
            }
            else if (GetIDataType().GetBase() == BaseDataType.IArray || GetIDataType().GetBase() == BaseDataType.IVariableSizedArray || GetIDataType().GetBase() == BaseDataType.IFixedSizeArray)
            {
                string s = value;
                return s.Replace('[', '{').Replace(']', '}');
            }
            return value;
        }

        //Get the real part of the IComplex
        public virtual string GetReal()
        {
            if (iconstantIDataType.GetBase() == BaseDataType.IComplex)
            {
                return value.Split('+')[0].Trim();
            }
            return " Error: IComplex Real!";
        }
        //Get the imaginary part of the IComplex
        public virtual string GetImag()
        {
            if (iconstantIDataType.GetBase() == BaseDataType.IComplex)
            {
                return value.Split('+')[1].Split('i')[0].Trim();
            }
            return " Error: IComplex Imag!";
        }

        public virtual IDataType GetIDataType()
        {
            return iconstantIDataType;
        }

        public override void Print()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("IConstant:");
            Console.WriteLine("NodeId: " + nodeId + ' ' + "ParentId: " + parentId + "\nValue: " + value);
            iconstantIDataType.Print();
            for (int i = 0; i < outputTerminals.Count; i++)
            {
                outputTerminals[i].Print();
            }
        }



    }
}
