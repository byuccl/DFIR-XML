#include<iostream>
#include<complex>
#include"array.h"
using namespace std;
int main(){
  complex<double> IConstant35(0,0);
  double x;
  cin >> x;
  int x_2;
  cin >> x_2;
  short x_3;
  cin >> x_3;
  unsigned long x_4;
  cin >> x_4;
  bool x_5;
  cin >> x_5;
  bool x_7;
  cin >> x_7;
  bool x_6;
  cin >> x_6;
  complex<double> x_8;
  cin >> x_8;
  Array<int> x_9;
  cin >> x_9;
  Array<int> x_10;
  cin >> x_10;
  double y=x+1;
  int y_2=x_2-1;
  short y_3=2*x_3;
  unsigned long y_4=1+x_4;
  bool y_6=!(true&x_5);
  bool y_7=(!x_7)|false;
  bool y_5=false^x_6;
  complex<double> y_8=IConstant35+x_8;
  Array<int> y_9=x_9+x_10;
  cout << y << endl;
  cout << y_2 << endl;
  cout << y_3 << endl;
  cout << y_4 << endl;
  cout << y_6 << endl;
  cout << y_7 << endl;
  cout << y_5 << endl;
  cout << y_8 << endl;
  cout << y_9 << endl;
  return 0;
}
