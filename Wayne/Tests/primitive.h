#include<cmath>
using namespace std;

template <typename T>
void ExIncrementPrimitive(T& p0,T& q0){
  q0 = p0+1;
}
template <typename T>
void ExDecrementPrimitive(T& p0,T& q0){
  q0 = p0-1;
}
template <typename T>
void ExAbsoluteValuePrimitive(T& p0,T& q0){
  q0 = abs(p0);
}
template <typename T>
void ExSquareRootPrimitive(T& p0,T& q0){
  q0 = sqrt(p0);
}
template <typename T>
void ExNegatePrimitive(T& p0,T& q0){
  q0 = -p0;
}
template <typename T>
void ExReciprocalPrimitive(T& p0,T& q0){
  q0 = 1/p0;
}
template <typename T>
void ExSquarePrimitive(T& p0,T& q0){
  q0 = p0*p0;
}
template <typename T>
void ExScaleByPowerOf2Primitive(T& p0,T& p1,
& q0){
  q0 = p0 * pow(2, p1);
}
template <typename T>
void ExMaxAndMinPrimitive(T& p0,T& p1,T& q0,T& q1){
  if(p0 >= p1){
    q0 = p1;
    q1 = p0;
  }else{
    q0 = p0;
    q1 = p1;
  }
}
template <typename T>
void ExSelectPrimitive(T& p0,bool& p1,T& p2,T& q0){
  if(p1 == true) q0 = p2;
  else q0 = p0;
}


