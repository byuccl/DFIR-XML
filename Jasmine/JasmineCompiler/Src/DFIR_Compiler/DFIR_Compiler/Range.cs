using System;

namespace DFIR_Compiler
{
    public class Range
    {
        private int singleValue;
        private int lowValue;
        private int highValue;

        private int diagramIndex;
        private bool trueorfalse;

        private bool isSingle;
        private bool isBoolean;

        public Range(int SingleValue, int DiagramIndex)
        {
            singleValue = SingleValue;
            diagramIndex = DiagramIndex;
            isSingle = true;
            isBoolean = false;
        }

        public Range(int LowValue, int HighValue, int DiagramIndex)
        {
            lowValue = LowValue;
            highValue = HighValue;
            diagramIndex = DiagramIndex;
            isSingle = false;
            isBoolean = false;
        }

        public Range(bool booltype, int DiagramIndex)
        {
            trueorfalse = booltype;
            diagramIndex = DiagramIndex;
            isSingle = false;
            isBoolean = true;
        }

        public virtual int GetSingleValue()
        {
            return singleValue;
        }
        public virtual int GetHighValue()
        {
            return highValue;
        }
        public virtual int GetLowValue()
        {
            return lowValue;
        }
        public virtual bool GetTrueOrFalse()
        {
            return trueorfalse;
        }
        public virtual int GetDiagramIndex()
        {
            return diagramIndex;
        }
        public virtual bool GetIsSingle()
        {
            return isSingle;
        }
        public virtual bool GetIsBoolean()
        {
            return isBoolean;
        }


        public virtual void SetSingleValue(int SingleValue)
        {
            singleValue = SingleValue;
        }
        public virtual void SetLowValue(int LowValue)
        {
            lowValue = LowValue;
        }
        public virtual void SetHighValue(int HighValue)
        {
            highValue = HighValue;
        }
        public virtual void SetTrueOrFalse(bool tf)
        {
            trueorfalse = tf;
        }
        public virtual void SetDiagramIndex(int DiagramIndex)
        {
            diagramIndex = DiagramIndex;
        }
        public virtual void SetIsSingle(bool IsSingle)
        {
            isSingle = IsSingle;
        }
        public virtual void SetIsBoolean(bool IsBoolean)
        {
            isBoolean = IsBoolean;
        }


        public virtual void Print()
        {
            Console.WriteLine("Range:");
            if (singleValue != int.MinValue)
            {
                Console.WriteLine("SingleValue: " + singleValue + ' ' + "DiagramIndex: " + diagramIndex);
            }
            else
            {
                Console.WriteLine("LowValue: " + lowValue + ' ' + "HighValue: " + highValue + ' ' + "DiagramIndex: " + diagramIndex);
            }
        }

    }
}
