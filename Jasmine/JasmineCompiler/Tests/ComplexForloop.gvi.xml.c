#include<stdio.h>
#include<stdlib.h>
void ExIncrementPrimitive(int& x0,int& y0)
{
  y0 = x0+1;
}
void ExNegatePrimitive(int& x0,int& y0)
{
  y0 = -x0;
}
void ExSquarePrimitive(int& x0,int& y0)
{
  y0 = x0*x0;
}

int main()
{
  int loop_count;
  scanf_s("%d", &loop_count);
  int x;
  scanf_s("%d", &x);
  int ILoopMax5=loop_count;
  int ITunnel7=x;
  int ITunnel6;
  for(int ILoopIndex4 = 0; ILoopIndex4 < ILoopMax5; ILoopIndex4++){
      int ILeftShiftRegister12_0=1;
      int IPrimitive19_0;
      ExIncrementPrimitive(ILoopIndex4,IPrimitive19_0);
      int ILoopMax11=ILoopMax5;
      int IRightShiftRegister13;
      for(int ILoopIndex10 = 0; ILoopIndex10 < ILoopMax11; ILoopIndex10++){
          IRightShiftRegister13=ILeftShiftRegister12_0+ILoopIndex10;
          ILeftShiftRegister12_0=IRightShiftRegister13;
      }
      int IPrimitive18=ITunnel7-IPrimitive19_0;
      int IPrimitive16_0;
      ExNegatePrimitive(IRightShiftRegister13,IPrimitive16_0);
      ITunnel6=IPrimitive16_0*IPrimitive18;
  }
  int IPrimitive21_0;
  ExSquarePrimitive(ITunnel6,IPrimitive21_0);
  int result=IPrimitive21_0;
  printf("%d\n",result);

  system("pause");
  return 0;
}

