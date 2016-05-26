using System;

namespace DFIR_Compiler
{
    public class IDataType
    {
        private BaseDataType _base;
        private int WordLength;
        private int LeftLength;
        private int RightLength;
        private int Dimensions;
        private int ArraySize;
        private IDataType ElementType;

        public IDataType(BaseDataType @base)
        {
            _base = @base;
            WordLength = -1;
            LeftLength = -1;
            RightLength = -1;
            Dimensions = -1;
            ArraySize = -1;
        }
      

        public virtual BaseDataType GetBase()
        {
            return _base;
        }
        public virtual int GetWordLength()
        {
            return WordLength;
        }
        public virtual int GetLeftLength()
        {
            return LeftLength;
        }
        public virtual int GetRightLength()
        {
            return RightLength;
        }
        public virtual int GetDimensions()
        {
            return Dimensions;
        }
        public virtual int GetArraySize()
        {
            return ArraySize;
        }
        public virtual IDataType GetElementType()
        {
            return ElementType;
        }


        public virtual void setWordLength(int wordLength)
        {
            WordLength = wordLength;
        }

        public virtual void setLeftLength(int leftLength)
        {
            LeftLength = leftLength;
        }

        public virtual void setRightLength(int rightLength)
        {
            RightLength = rightLength;
        }

        public virtual void setDimensions(int dimensions)
        {
            Dimensions = dimensions;
        }

        public virtual void setArraySize(int arraySize)
        {
            ArraySize = arraySize;
        }

        public virtual void setElementType(IDataType elementType)
        {
            ElementType = elementType;
        }


        //Print out the datatype of the variable when they are declared
        public virtual string GetDeclarationDataType()
        {
            string result = "";
            if (_base == BaseDataType.ISignedInt)
            {
                if (WordLength == -1)
                {
                    Console.WriteLine("ISignedInt Error");
                }
                if (WordLength <= 16)
                {
                    result = "short";
                }
                else if (WordLength <= 32)
                {
                    result = "int";
                }
                else
                {
                    result = "long";
                }
            }
            else if (_base == BaseDataType.IUnsignedInt)
            {
                if (WordLength != -1)
                {
                    Console.WriteLine("IUnsignedInt Error");
                }
                if (WordLength <= 16)
                {
                    result = "unsigned short";
                }
                else if (WordLength <= 32)
                {
                    result = "unsigned int";
                }
                else
                {
                    result = "unsigned long";
                }
            }
            //Because the FPGA VI in LabVIEW Communications only support fixedpoint, while double is the most similar datatype with fixedpoint in C
            else if (_base == BaseDataType.IDouble || _base == BaseDataType.ISignedFixedPoint || _base == BaseDataType.IUnsignedFixedPoint || _base == BaseDataType.ISingle)
            {
                result = "double";
            }

            //Becasue C cannot input and output Boolean variable, I use int to replace bool in the bool declaration. 
            //1 is true. 0 is false.
            else if (_base == BaseDataType.IBoolean)
            {
                result = "int";
            }
            else if (_base == BaseDataType.IComplex)
            {
                result = "complex<" + ElementType.GetDeclarationDataType() + ">";
            }
            else if (_base == BaseDataType.IArray || _base == BaseDataType.IVariableSizedArray || _base == BaseDataType.IFixedSizeArray)
            {
                result = ElementType.GetDeclarationDataType();
            }
            return result;
        }

        public virtual void Print()
        {
            Console.WriteLine("IDataType: " + _base);
            if (WordLength != -1)
            {
                Console.WriteLine("WordLength: " + WordLength);
            }
            if (LeftLength != -1)
            {
                Console.WriteLine("LeftLength: " + LeftLength);
            }
            if (RightLength != -1)
            {
                Console.WriteLine("RightLength: " + RightLength);
            }
            if (Dimensions != -1)
            {
                Console.WriteLine("Dimensions: " + Dimensions);
            }
            if (ArraySize != -1)
            {
                Console.WriteLine("ArraySize: " + ArraySize);
            }
            if (ElementType != null)
            {
                ElementType.Print();
            }
        }
    }
}
