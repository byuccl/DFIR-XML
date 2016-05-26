#include<stdio.h>
#include<stdlib.h>

int main()
{
  int input_0;
  scanf_s("%d", &input_0);
  int input_1;
  scanf_s("%d", &input_1);
  int inverse_input_3;
  scanf_s("%d", &inverse_input_3);
  int sum=input_0*input_1*5*inverse_input_3;
  printf("%d\n",sum);

  system("pause");
  return 0;
}

