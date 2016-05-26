#include<vector>
using namespace std;

template <typename T>
void getValueFromFeedback(vector<T> vec, int delay, int loop_index, T init, T& fo){
	if(loop_index < delay){
		fo = init;
	}else{
		fo = vec[loop_index - delay];
	}
}

