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
  int ICompoundArithmeticNode2=input_1&input_2&true;
  int ICompoundArithmeticNode5=ICompoundArithmeticNode2|false|input_3;
  int logical_XOR=ICompoundArithmeticNode5^input_3;
  printf("%d\n",logical_XOR);

  system("pause");
  return 0;
}

