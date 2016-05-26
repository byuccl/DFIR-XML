#include<stdio.h>
#include<stdlib.h>

int main()
{
  double input_1;
  scanf_s("%lf", &input_1);
  double input_2;
  scanf_s("%lf", &input_2);
  double IPrimitive5=input_1-input_2;
  double result=IPrimitive5/2.5;
  printf("%lf\n",result);

  system("pause");
  return 0;
}

