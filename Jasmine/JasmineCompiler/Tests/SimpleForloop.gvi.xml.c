#include<stdio.h>
#include<stdlib.h>
void ExIncrementPrimitive(int& x0,int& y0)
{
  y0 = x0+1;
}

int main()
{
  int input_2;
  scanf_s("%d", &input_2);
  int input_1;
  scanf_s("%d", &input_1);
  int ILoopMax5=3;
  int ILeftShiftRegister7_0=input_2;
  int ILeftShiftRegister7_1=5;
  int ILeftShiftRegister7_2=4;
  int ITunnel6=input_1;
  int IRightShiftRegister8;
  for(int ILoopIndex4 = 0; ILoopIndex4 < ILoopMax5; ILoopIndex4++){
      int ICompoundArithmeticNode9=ITunnel6+ILeftShiftRegister7_2;
      int IPrimitive12_0;
      ExIncrementPrimitive(ICompoundArithmeticNode9,IPrimitive12_0);
      IRightShiftRegister8=IPrimitive12_0*2;
      ILeftShiftRegister7_2=ILeftShiftRegister7_1;
      ILeftShiftRegister7_1=ILeftShiftRegister7_0;
      ILeftShiftRegister7_0=IRightShiftRegister8;
  }
  int output=IRightShiftRegister8;
  printf("%d\n",output);

  system("pause");
  return 0;
}

