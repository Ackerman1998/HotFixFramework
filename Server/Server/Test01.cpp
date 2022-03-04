#include <iostream>
#include <vector>
#include <list>
#include <map>
#include <string>
#include <set>
using namespace std;

class MyString {
public:
	MyString(const char* s="") :_str(new char[strlen(s) + 1]) {
		strcpy_s(_str, strlen(s) + 1,s);
	}
	/*MyString(const char* s = "") {
		_str = new char[strlen(s) + 1];
		strcpy_s(_str, strlen(s) + 1, s);
	}*/
	MyString operator=(const MyString& s) {
		if (this!=&s) {
			delete _str;
			_str = new char[strlen(s._str)+1];
			strcpy_s(_str, strlen(s._str) + 1,s._str);
		}
		return *this;
	}
	/*MyString operatornew(const char* s) {
		_str = new char[strlen(s) + 1];
		strcpy_s(_str, strlen(s) + 1, s);
		return *this;
	}*/
private:
	char* _str;
};

void Test(int & a) {
	cout << a << endl;
	cout << &a << endl;
}
void TestChar() {
	int a = 1;
	int& b = a;
	cout << b << endl;
	cout << &b << endl;
	cout << "11" << endl;
	Test(a);
	char aa = '1';
	char aaa = '1111';
	char* str;
	str = "abc";
	cout << str << endl;
	cout << *str << endl;
	cout << &str << endl;
	str++;
	cout << *str << endl;
	char astr[] = "cderfvbgt";
	cout << astr << endl;
	cout << *astr << endl;
	cout << &astr << endl;
	// ms = new MyString();
}

void Test() {
	char* s = "abc";
	MyString my1(s);
	MyString my2 = MyString("DD");
	MyString* my3 = new MyString(s);
	
}

int main() {
	/*list<int>::iterator bianli;
	list<int> testlist;
	for (int i = 0; i < 10;i++) {
		testlist.push_back(i);
	}
	bianli = testlist.begin();
	while (bianli!=testlist.end()) {
		cout << *bianli++ << endl;
	}*/
	/*vector<int> myVec;
	vector<int>::iterator bianli2;
	for (int i = 0; i < 10; i++) {
		myVec.push_back(i);
	}
	cout << myVec[0] << endl;
	bianli2 = myVec.begin();
	while (bianli2!=myVec.end()) {
		cout << *bianli2++ << endl;
	}*/
	//map<int,string> testMap;
	//map<int, string>::iterator testMapIterator;

	//for (int i = 1; i < 10; i++) {
	//	//testMap.insert(pair<string,int>("abc"+i,i));
	//	testMap[i] = "abc";
	//}
	//testMapIterator = testMap.begin();
	//while ( testMapIterator != testMap.end()) {
	//	cout << (testMapIterator->first) << "," << (testMapIterator->second)<<endl;
	//	testMapIterator++;
	//}
	set<int, int> testset;
	set<int, int>::iterator iteratorset;
	
}