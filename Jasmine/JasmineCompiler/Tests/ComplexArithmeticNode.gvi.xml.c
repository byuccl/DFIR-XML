#include<stdio.h>
#include<stdlib.h>
void ExIncrementPrimitive(int& x0,int& y0)
{
  y0 = x0+1;
}

int main()
{
  int input_0;
  scanf_s("%d", &input_0);
  int input_1;
  scanf_s("%d", &input_1);
  int input_2;
  scanf_s("%d", &input_2);
  int ICompoundArithmeticNode2=input_0+(-input_1)+3+4;
  int ICompoundArithmeticNode4=input_1*4*input_2*8;
  int IPrimitive3=ICompoundArithmeticNode2-ICompoundArithmeticNode4;
  int IPrimitive12_0;
  ExIncrementPrimitive(IPrimitive3,IPrimitive12_0);
  int result=IPrimitive12_0;
  printf("%d\n",result);

  system("pause");
  return 0;
}

