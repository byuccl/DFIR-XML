#include<stdio.h>
#include<stdlib.h>

int main()
{
  double x;
  scanf_s("%lf", &x);
  double y;
  scanf_s("%lf", &y);
  double result=x/y;
  printf("%lf\n",result);

  system("pause");
  return 0;
}

