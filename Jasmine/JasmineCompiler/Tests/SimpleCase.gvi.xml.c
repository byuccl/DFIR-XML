#include<stdio.h>
#include<stdlib.h>
#include<cmath>
void ExDecrementPrimitive(int& x0,int& y0)
{
  y0 = x0-1;
}
void ExAbsoluteValuePrimitive(int& x0,int& y0)
{
  y0 = abs(x0);
}
void ExIncrementPrimitive(int& x0,int& y0)
{
  y0 = x0+1;
}

int main()
{
  int input;
  scanf_s("%d", &input);
  int ICaseSelector4=input;
  int ITunnel7;
  if(ICaseSelector4 == 5||ICaseSelector4 == 6||ICaseSelector4 == 7)
  {
      int ICompoundArithmeticNode11=ICaseSelector4*2;
      int IPrimitive13_0;
      ExDecrementPrimitive(ICompoundArithmeticNode11,IPrimitive13_0);
      ITunnel7=IPrimitive13_0;
  }
  else if(ICaseSelector4>=2 && ICaseSelector4<=4)
  {
      int ICompoundArithmeticNode14=ICaseSelector4+(-4);
      int IPrimitive16_0;
      ExAbsoluteValuePrimitive(ICompoundArithmeticNode14,IPrimitive16_0);
      ITunnel7=IPrimitive16_0;
  }
  else
  {
      int IPrimitive9=ICaseSelector4-3;
      int IPrimitive10_0;
      ExIncrementPrimitive(IPrimitive9,IPrimitive10_0);
      ITunnel7=IPrimitive10_0;
  }
  int output=ITunnel7;
  printf("%d\n",output);

  system("pause");
  return 0;
}

