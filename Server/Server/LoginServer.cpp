#include <iostream>
#include"Tools.h"
#include "stdafx.h"
#include <malloc.h>
#include <csignal>
#include <windows.h>
#include <thread>
#include <vector>
using namespace std;
#define PI 3.14159
#define MIN(a,b) (a<b ? a : b)
#define DEBUG true
#define MKSTR( x ) #x
class Message {
public:
	Message(int num) :msg1(num) {};
	int GetMsg() {
		return msg1;
	}
	bool operator==(const Message mm) {
		if (this->msg1==mm.msg1) {
			return true;
		}
		else {
			return false;
		}
	}
	~Message() {
		cout << "Destrot Message:"<< msg1 << endl;
	}
	 static inline std::string adress="ddd";
private:
	int msg1;
};

bool LoadAllConfig();
void TestPointer();
void TestPointer2();
void TestChar(const char* ptr);
CLSNetSessionMgr* csm;


#pragma region ������
/// <summary>
/// ���Զ�̬�ڴ�
/// </summary>
void TestMalloc() {
	double* value = NULL;
	value = new double;
	*value = 2222;
	cout << "double value:" << *value << endl;
	delete value;
}

void TestMalloc2() {
	int* p;
	p = (int*)malloc(sizeof(int));
	*p = 111;
	cout << "Malloc:" << *p << endl;
	free(p);
}

//ģ�巽������
template <typename T>
void Swap(T &a, T &b) {
//void SwapInt(T a, T b) {
	T temp = a;
	a = b;
	b = temp;
}

class A
{
public:
	int _a;
};
class B : virtual public A
{
public:
	int _b;
};
class C : virtual public A
{
public:
	int _c;
};
class D : public B, public C
{
public:
	int _d;
};

class BaseMono {
public:
	virtual void Number() = 0;
	virtual void Init() = 0;
};

class MonoBehaviour :public BaseMono {
public:
	void BaseMono::Number() {
		cout << "Init Number MonoBehaviour" << endl;
	}
	void BaseMono::Init() {
		cout << "Init Mono" << endl;
	}
};

class AA1 {
public:
	virtual void foo() {
		cout << "A::foo() is called" << endl;
	}
};
class BB1 :public AA1 {
public:
	void foo() {
		cout << "B::foo() is called" << endl;
	}
	void GetString(int index) {
	
	}
	void GetString(string index) {

	}
};

template <class T1,class T2>
class Map {
public:
	T1 key;
	T2 Value;
	Map(T1 k, T2 v) :key(k), Value(v) {};
};

void signalHandler(int num) {
	exit(num);
}

#pragma endregion ������

int main() {
	WSADATA wsData;
	::WSAStartup(MAKEWORD(2, 2), &wsData);
	/*Ӧ�ó����DLLֻ����һ�γɹ���WSAStartup()����֮��
	���ܵ��ý�һ����Windows Sockets*/
	cout << "start loginserver"<<endl;
	csm = new CLSNetSessionMgr;
	
	if (LoadAllConfig()) {
		printf("Load StartUp.ini Failed...\n");
		return false;
	}
	
}

void TestPointer() {
	char str[] = "12345678", 
	* p = str;
	int i, l = 8;
	for (i = 0; i < l; ++i) {
		printf("%c", *(p++));
		cout << endl;
	}
}
void TestPointer2() {
	char str = '4001';
	const char* p;
	p = &str;
	/*p = "kkk";*/
	cout << str << endl;
	cout << p << endl;
}
void TestChar(const char *ptr) {
	ptr++;
	cout << "ptr:" << *ptr << endl;
	cout << "&ptr:" << &ptr << endl;
	ptr++;
	cout << "ptr:" << ptr << endl;
	cout << "&ptr:" << &ptr << endl;
	ptr++;
	cout << "ptr:" << ptr << endl;
	cout << "&ptr:" << &ptr << endl;
	int arr[] = {1,2,3,4};
	int* intPtr = arr;
	cout << *intPtr << endl;
	intPtr++;
	cout << *intPtr << endl;
}
/// <summary>
/// ���Է���
/// </summary>
void TestVoid() {
	cout << Message::adress << endl;
	Message m2{ 33 };
	Message m3{ 44 };
	if (m2 == m3) {
		cout << "m2=m3" << endl;
	}
	else {
		cout << "m2!=m3" << endl;
	}
	TestMalloc();
	TestMalloc2();
	
	int a = 33;
	int b = 2;
	Swap(a, b);
	cout << "a:" << a << endl;
	cout << "b:" << b << endl;
	Map m1{ "name","joelee" };
	cout << m1.key << "\t" << m1.Value << endl;

	cout << "Value of PI :" << PI << endl;

	int i, j;
	i = 100;
	j = 30;
	cout << "��С��ֵΪ��" << MIN(i, j) << endl;
	if (DEBUG) {
		cout << "DEBUG MODE" << endl;
	}
	else {
		cout << "NORMAL MODE" << endl;
	}
	cout << MKSTR(HELLO C++) << endl;

	signal(SIGINT, signalHandler);
	while (1) {
		cout << "Going to sleep...." << endl;
		Sleep(100);
	}
	vector<int> nums;

	MonoBehaviour mono;
	mono.Init();
	mono.Number();
	AA1* aaa = new BB1();
	aaa->foo();
	
}
//�����������ã���ȡstartup.ini�����ļ���
bool LoadAllConfig() {
	//GetPrivateProfileInt:��ȡini�ļ��е�ֵ��ע��Ŀ���ļ���Ҫ���ں�slnͬһ��Ŀ¼�£�
	slsconfig.client_listen_port = GetPrivateProfileInt(_T("MainGC"), _T("ListernPortForClient"),0, _T("LSConfig\\SetUp.ini"));
	slsconfig.bs_listen_port = GetPrivateProfileInt(_T("MainBS"), _T("ListernPortForBS"),0, _T("LSConfig\\SetUp.ini"));
	slsconfig.bs_base_index = GetPrivateProfileInt(_T("MainBS"),_T("BSBaseIndex"),0, _T("LSConfig\\SetUp.ini"));
	slsconfig.bs_max_count = GetPrivateProfileInt(_T("MainBS"),_T("BSMaxCount"),0, _T("LSConfig\\SetUp.ini"));
	//cout << "client listen port:" << slsconfig.client_listen_port << endl;
	//cout << "bs_max_count:" << slsconfig.bs_max_count << endl;
	//cout << "bs_base_index:" << slsconfig.bs_base_index << endl;
	//cout << "bs_listen_port:" << slsconfig.bs_listen_port << endl;

	char server_key[64];
	char server_address[64];
	char server_address_export[64];
	for (int i =1; i <= slsconfig.bs_max_count;i++) {
		//_snprintf_s �����ĸ��������ո�ʽת��Ϊ�ַ�������ֵ����һ���ַ���
		_snprintf_s(server_key,sizeof(server_key),"BS%u",i);
		//cout << "Log:" << server_key<<endl;
		GetPrivateProfileStringA("MainBS",server_key,"",server_address,64, "LSConfig\\SetUp.ini");
		//cout << "Log:" << server_address << endl;
		_snprintf_s(server_key, sizeof(server_key), "BS%uExport", i);
		GetPrivateProfileStringA("MainBS", server_key, "", server_address_export, 64, "LSConfig\\SetUp.ini");
		//cout << "Log:" << server_address_export << endl;
		sOneBsInfo& sesInfo= gAllBsInfo[slsconfig.bs_base_index+i-1];
		sesInfo.bs_Id = slsconfig.bs_base_index + i - 1;
		sesInfo.bs_isLost = true;
		sesInfo.bs_nets = 0;
		{
			char* portNum = strchr(server_address,':');// :40001
			//*portNum =0; ����??
			++portNum;
			sesInfo.bs_Ip = server_address;
			sesInfo.bs_Port = atoi(portNum);
			
		}
		{
			char* portNum = strchr(server_address_export, ':');// :40001
			//*portNum =0; ����??
			++portNum;
			sesInfo.bs_IpExport = server_address_export;
			int exPos = atoi(portNum);
			if (exPos>0) {
				csm->GetNetIp(sesInfo.bs_IpExport, exPos-1);
			}
		}
		cout << "=================================" << endl;
	}
	return true;
}