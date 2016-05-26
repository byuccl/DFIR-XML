#include<iostream>

using namespace std;

template <class T> class Array{
	public:
	   
	   Array(int size = 0){
	   	   this->_size = size;
	   	   this->_array =new T[size];
	   }
	   Array(int size, T* array){
	   	   this->_size = size;
	   	   this->_array = array;
	   }
	   Array<T>& operator+(const Array<T>& a){
	   	  int size = min(a._size,this->_size);
	   	  Array<T> res(size);
	   	  for(int i=0; i<_size; i++){
	   	  	  res.array[i] = a._array[i] + this->_array[i];
		  }
		  return res;
	   }
	   T operator[](int index){
	   	 if(index < this->_size){
	   	 	  return this->_array[index];
		 }
		 else{
		 	return NULL;
		 }
	   }
	   
	   int size()const{
	   	  return this->_size;
	   }
	   T* array()const{
	   	  return this->_array;
	   }
	   void setSize(int size){
	   	  this->_size = size;
	   }
	   void setArray(int i, T t){
	   	  this->_array[i] = t;
	   }
	private:
		int _size;
	    T * _array;
};

template <class T> 
ostream& operator<<(ostream& os, const Array<T>& a)
{
    for(int i=0; i<a.size(); i++){
    	os << a.array()[i] << " ";
	}
    return os;
}

template <class T>
istream& operator>>(istream &in,  Array<T>& a)
{
    if(a.size() == 0){
    	int size;
        in >> size;
        a = Array<T>(size);
	}
	for(int i=0; i<a.size(); i++){
    	T t;
    	in >> t;
    	a.setArray(i, t);
	}
    return in;
}


