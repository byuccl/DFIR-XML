#include<stdio.h>
#include<stdlib.h>
#include<cmath>
void ExDecrementPrimitive(int& x0,int& y0)
{
  y0 = x0-1;
}
void ExIncrementPrimitive(int& x0,int& y0)
{
  y0 = x0+1;
}
void ExSelectPrimitive(int& x0,int& x1,int& x2,int& y0)
{
  if(x1 == true)
    y0 = x2;
  else
     y0 = x0;
}
void ExSquarePrimitive(int& x0,int& y0)
{
  y0 = x0*x0;
}
void ExAbsoluteValuePrimitive(int& x0,int& y0)
{
  y0 = abs(x0);
}
void ExNegatePrimitive(int& x0,int& y0)
{
  y0 = -x0;
}
void ExMaxAndMinPrimitive(int& x0,int& x1,int& y0,int& y1)
{
  if(x0 >= x1){
    y0 = x1;
    y1 = x0;
  }else{
    y0 = x0;
    y1 = x1;
  }
}

int main()
{
  int input_1;
  scanf_s("%d", &input_1);
  int input_2;
  scanf_s("%d", &input_2);
  int Boolean;
  scanf_s("%d", &Boolean);
  int IPrimitive7_0;
  ExDecrementPrimitive(input_1,IPrimitive7_0);
  int IPrimitive6_0;
  ExIncrementPrimitive(input_2,IPrimitive6_0);
  int IPrimitive2_0;
  ExSelectPrimitive(IPrimitive6_0,Boolean,IPrimitive7_0,IPrimitive2_0);
  int IPrimitive13_0;
  ExSquarePrimitive(IPrimitive2_0,IPrimitive13_0);
  int IPrimitive9_0;
  ExAbsoluteValuePrimitive(IPrimitive2_0,IPrimitive9_0);
  int IPrimitive8_0;
  ExNegatePrimitive(IPrimitive13_0,IPrimitive8_0);
  int IPrimitive10_0;
  int IPrimitive10_1;
  ExMaxAndMinPrimitive(IPrimitive9_0,IPrimitive8_0,IPrimitive10_0,IPrimitive10_1);
  int min=IPrimitive10_0;
  printf("%d\n",min);
  int max=IPrimitive10_1;
  printf("%d\n",max);

  system("pause");
  return 0;
}

