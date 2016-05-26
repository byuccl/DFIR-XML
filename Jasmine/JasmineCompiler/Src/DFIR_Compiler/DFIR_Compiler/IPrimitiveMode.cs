using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFIR_Compiler
{
    public enum IPrimitiveMode
    {
        ExSubtractPrimitive,
		ExDividePrimitive,

		ExSelectPrimitive,

		//Numeric IPrimitive Mode
		ExIncrementPrimitive,
		ExDecrementPrimitive,
		ExAbsoluteValuePrimitive,
		ExAddArrayElementsPrimitive,
		ExScaleByPowerOf2Primitive,
		ExSquareRootPrimitive,
		ExSquarePrimitive,
		ExNegatePrimitive,
		ExReciprocalPrimitive,
		ExMaxAndMinPrimitive,

        //Boolean IPrimitive Mode
        ExNumberToBooleanArrayPrimitive,
        ExBooleanArrayToNumberPrimitive,
        ExBooleanTo0Or1Primitive,
        ExNotPrimitive,
        ExImpliesPrimitive,
        ExAndArrayElementsPrimitive,
        ExOrArrayElementsPrimitive,

        //Comparison IPrimitive Mode
        ExIsEqualPrimitive,
        ExIsEqualTo0Primitive,
        ExIsNotEqualPrimitive,
        ExIsNotEqualTo0Primitive,
        ExIsGreaterPrimitive,
        ExIsGreaterThan0Primitive,
        ExIsGreaterOrEqualPrimitive,
        ExIsGreaterOrEqualTo0Primitive,
        ExIsLessPrimitive,
        ExIsLessThan0Primitive,
        ExIsLessOrEqualPrimitive,
        ExIsLessOrEqualTo0Primitive,

        //Complex IPrimitive Mode
        ExComplexConjugatePrimitive,
		ExPolarToComplexPrimitive,
		ExComplexToPolarPrimitive,
		ExReOrImToComplexPrimitive,
		ExComplexToReOrImPrimitive,
		ExReOrImToPolarPrimitive,
		ExPolarToReOrImPrimitive,

        //Array IPrimitive Mode
        ExRotate1DArrayPrimitive,
		ExReverse1DArrayPrimitive,
		ExArrayMaxAndMinPrimitive,
		ExSplit1DArrayPrimitive,
		ExArraySizePrimitive,

        //Trigonometric IPrimitive Mode
        ExSinePrimitive,
		ExCosinePrimitive,
		ExInverseTangent2InputPrimitive,
		ExSineAndCosinePrimitive,

    }
}
