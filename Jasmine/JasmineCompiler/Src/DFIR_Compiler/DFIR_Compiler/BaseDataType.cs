using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public enum BaseDataType
    {
        ISignedInt,
        IUnsignedInt,
        ISignedFixedPoint,
        IUnsignedFixedPoint,
        IArray,
        IVariableSizedArray,
        IComplex,
        IFixedSizeArray,
        IBit,
        IBoolean,
        IDouble,
        IIncorrect,
        ISingle,
        IString,
        IUnknown,
        IUnsupported,
        IVoid
    }
}
