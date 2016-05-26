using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public class IDataTypeBuilder
    {
        private IDataType _IDataType;
        public IDataTypeBuilder(BaseDataType _base)
        {
            _IDataType = new IDataType(_base);
        }
        public virtual void SetWordLength(int WordLength)
        {
            _IDataType.setWordLength(WordLength);
        }
        public virtual void SetLeftLength(int LeftLength)
        {
            _IDataType.setLeftLength(LeftLength);
        }
        public virtual void SetRightLength(int RightLength)
        {
            _IDataType.setRightLength(RightLength);
        }
        public virtual void SetDimensions(int Dimensions)
        {
            _IDataType.setDimensions(Dimensions);
        }
        public virtual void SetArraySize(int ArraySize)
        {
            _IDataType.setArraySize(ArraySize);
        }
        public virtual void SetElementType(IDataType ElementType)
        {
            _IDataType.setElementType(ElementType);
        }
        public virtual IDataType GetResult()
        {
            return _IDataType;
        }

    }
}
