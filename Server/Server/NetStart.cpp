#include "Net/MessageMgr.h"
#include "Net/TcpMgr.h";
#define HTTPPORT 1212

template <typename T>
inline T Max(const T &a, const T &b) {
	return a > b ? a : b;
}

template <class T>
class Container {
public:
	T Pop();
	void Push(const T &a);
private:
	vector<T> sta;
};

int main() {
	//get是智能指针的函数，返回当前当前智能指针对象，即用以判断是否为空 
	unique_ptr<Container<int>> containers;
	//containers.get()->
	shared_ptr<Container<int>> shareContainer;
	//shareContainer.use_count;
	weak_ptr<Container<int>> weakContainer;
	WSADATA ws;
	WSAStartup(MAKEWORD(2, 2), &ws);
	TcpMgr tMgr;
	MessageMgr mg;
	tMgr.Bind(HTTPPORT);
	char buf[1024];
	while (true) {
		TcpMgr client = tMgr.Accept();
		if (client.sock<=0) {
			
		}
		else {
			client.Recv(buf, sizeof(buf) - 1);
			mg.ResolveMessage(buf, client);
			client.Close();
			//printf("Receive Msg:%s", buf);
		}
	}
	return 0;
}