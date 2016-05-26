#include<iostream>
#include<vector>
#include"getValueFromFeedback.h"
using namespace std;
int main(){
  int loop_count;
  cin >> loop_count;
  int ILeftShiftRegister8_0=0;
  int ILoopMax5=loop_count;
  vector<int > Feedback41;
  int ITunnel6;
  int ITunnel7;
  int IRightShiftRegister9;
  for(int ILoopIndex4 = 0; ILoopIndex4 < ILoopMax5; ILoopIndex4++){
      int IFeedbackOutputNode42;
      getValueFromFeedback(Feedback41, 1, ILoopIndex4, 0, IFeedbackOutputNode42);
      int ILeftShiftRegister16_0=0;
      int ILoopMax15=ILoopMax5;
      int IRightShiftRegister17;
      for(int ILoopIndex14 = 0; ILoopIndex14 < ILoopMax15; ILoopIndex14++){
          IRightShiftRegister17=ILeftShiftRegister16_0+ILoopIndex14;
          ILeftShiftRegister16_0=IRightShiftRegister17;
      }
      ITunnel7=IRightShiftRegister17;
      ITunnel6=IRightShiftRegister17+IFeedbackOutputNode42;
      IRightShiftRegister9=IRightShiftRegister17+ILeftShiftRegister8_0;
      int IFeedbackInputNode41=ITunnel6;
      Feedback41.push_back(IFeedbackInputNode41);
      ILeftShiftRegister8_0=IRightShiftRegister9;
  }
  int y_2=ITunnel6;
  cout << y_2 << endl;
  int y=ITunnel7;
  cout << y << endl;
  int y_3=IRightShiftRegister9;
  cout << y_3 << endl;
  return 0;
}
