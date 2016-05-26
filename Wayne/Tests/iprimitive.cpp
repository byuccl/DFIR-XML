#include<iostream>
#include"primitive.h"
using namespace std;
int main(){
  int x;
  cin >> x;
  int IPrimitive2_0;
  ExIncrementPrimitive(x,IPrimitive2_0);
  int IPrimitive4_0;
  ExDecrementPrimitive(IPrimitive2_0,IPrimitive4_0);
  int IPrimitive5_0;
  ExAbsoluteValuePrimitive(IPrimitive4_0,IPrimitive5_0);
  int IPrimitive7_0;
  ExSquarePrimitive(IPrimitive5_0,IPrimitive7_0);
  int y=IPrimitive7_0;
  cout << y << endl;
  return 0;
}
