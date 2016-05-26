#include<iostream>
using namespace std;
int main(){
  long selector;
  cin >> selector;
  long ICaseSelector4=selector*1;
  long ITunnel7;
  if(ICaseSelector4 == 11||ICaseSelector4 == 12){
      ITunnel7=ICaseSelector4*2;
  }else if(ICaseSelector4<=10 && ICaseSelector4>=6){
      ITunnel7=ICaseSelector4+1;
  }else{
      ITunnel7=ICaseSelector4-1;
  }
  long y=ITunnel7;
  cout << y << endl;
  return 0;
}
