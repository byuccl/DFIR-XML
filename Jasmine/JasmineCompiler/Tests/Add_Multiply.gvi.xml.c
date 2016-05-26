#include<stdio.h>
#include<stdlib.h>

int main()
{
  int input_1;
  scanf_s("%d", &input_1);
  int input_2;
  scanf_s("%d", &input_2);
  int input_3;
  scanf_s("%d", &input_3);
  int ICompoundArithmeticNode2=input_1+(-1)+(-input_2)+2;
  int product=ICompoundArithmeticNode2*input_3*3;
  printf("%d\n",product);

  system("pause");
  return 0;
}

