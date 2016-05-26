package Dfir_representation;

public enum IPrimitiveMode {
	
	//numeric
	ExSubtractPrimitive, 
	ExDividePrimitive,
	ExSelectPrimitive,
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
	
	//complex
	ExComplexConjugatePrimitive,
	ExPolarToComplexPrimitive,
	ExComplexToPolarPrimitive,
	ExReOrImToComplexPrimitive,
	ExComplexToReOrImPrimitive,
	ExReOrImToPolarPrimitive,
	ExPolarToReOrImPrimitive,
	
	//comparison
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
	
	//boolean
	ExNumberToBooleanArrayPrimitive,
	ExBooleanArrayToNumberPrimitive,
	ExBooleanTo0Or1Primitive,
	ExNotPrimitive,
	ExImpliesPrimitive,
	ExAndArrayElementsPrimitive,
	ExOrArrayElementsPrimitive,
	
	//array
	ExRotate1DArrayPrimitive,
	ExReverse1DArrayPrimitive,
	ExArrayMaxAndMinPrimitive,
	ExSplit1DArrayPrimitive,
	ExArraySizePrimitive,
	
	//trigonometric
	ExSinePrimitive,
	ExCosinePrimitive,
	ExInverseTangent2InputPrimitive,
	ExSineAndCosinePrimitive,
	
	//data manipulation
	ExSplitNumberPrimitive,
	ExJoinNumbersPrimitive,
	ExSwapBytesPrimitive,
	ExSwapWordsPrimitive,
	ExRotateLeftWithCarryPrimitive,
	ExRotateRightWithCarryPrimitive,
	ExLogicalShiftPrimitive,
	ExRotatePrimitive,
	ExReinterpretNumberPrimitive,
	
	//conversion
	ExToSigned8BitIntegerPrimitive,
	ExToSigned16BitIntegerPrimitive,
	ExToSigned32BitIntegerPrimitive,
	ExToSigned64BitIntegerPrimitive,
	ExToUnsigned8BitIntegerPrimitive,
	ExToUnsigned16BitIntegerPrimitive,
	ExToUnsigned32BitIntegerPrimitive,
	ExToUnsigned64BitIntegerPrimitive,
	ExToFixedPointPrimitive,
	ExToComplexFixedPointPrimitive
	
}
