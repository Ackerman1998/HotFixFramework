#include "Net/TcpMgr.h";
#include <vector>
#define HTTPPORT 1212
std::vector<std::string> split(std::string str, std::string pattern)
{
	std::string::size_type pos;
	std::vector<std::string> result;
	str += pattern;//扩展字符串以方便操作
	int size = str.size();
	for (int i = 0; i < size; i++)
	{
		pos = str.find(pattern, i);
		if (pos < size)
		{
			std::string s = str.substr(i, pos - i);
			result.push_back(s);
			i = pos + pattern.size() - 1;
		}
	}
	return result;
}

char* CreateMessage(char* msg,bool success=true) {
	char buf[1024];
	if (success) {
		strcpy_s(buf, "HTTP/1.1 200 OK\r\n");
	}
	else {
		strcpy_s(buf, "HTTP/1.1 404 NotFound\r\n");
	}
	strcat_s(buf,"Content-Type: text/html\r\n");
	strcat_s(buf,"Connection: Close\r\n");
	strcat_s(buf,"\r\n");
	strcat_s(buf, msg);
	cout << "Return msg: " << buf << endl;
	return buf;
}

void ResolveData(char* msg,TcpMgr client) {
	char* fengefu = "\r\n";
	char* fengefuSpace = " ";
	std::vector<std::string> strs =split(msg, fengefu);
	if (sizeof(strs)<=0) {
		
	}
	else {
		cout << strs[0] << endl;
		std::vector<std::string> firstline = split(strs[0], fengefuSpace);
		if (firstline[0]=="GET") {
			cout <<"GET URL: "<< firstline[1] << endl;
			if (firstline[1]=="/geturl") {
				char* str = "hello,i am ackerman";
				char* sendMsg = CreateMessage(str);
				client.Send(sendMsg);
			}
			else {
				//no res ,return 404 html
			}
		}
		else if (firstline[0] == "POST") {
			//暂不处理POST
		}
	}

	//std::vector<std::string>::iterator bianli;
	//bianli = strs.begin();
	//while (bianli!=strs.end()) {
	//	cout << "get resolve data :" << *bianli<< "\n" << endl;
	//	//printf("get resolve data : %s\n",*bianli);
	//	bianli++;
	//}
}

int main() {
	WSADATA ws;
	WSAStartup(MAKEWORD(2, 2), &ws);
	TcpMgr tMgr;
	tMgr.Bind(HTTPPORT);
	char buf[1024];
	while (true) {
		TcpMgr client = tMgr.Accept();
		if (client.sock<=0) {
			
		}
		else {
			client.Recv(buf, sizeof(buf) - 1);
			ResolveData(buf, client);
			client.Close();
			//printf("Receive Msg:%s", buf);
		}
	}
	return 0;
}