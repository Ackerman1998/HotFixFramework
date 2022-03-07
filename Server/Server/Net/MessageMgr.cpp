#include "MessageMgr.h"

std::vector<std::string> MessageMgr::SplitStr(std::string str, std::string pattern)
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

char* MessageMgr::CreateMessage(char* msg, char* resType, bool success) {
	char buf[2000];
	if (success) {
		strcpy_s(buf, "HTTP/1.1 200 OK\r\n");
	}
	else {
		strcpy_s(buf, "HTTP/1.1 404 NotFound\r\n");
	}
	char* type;
	type = "Content-Type:";
	type += *resType;
	type += *"\r\n";
	//strcat_s(buf, "Content-Type: text/html\r\n");
	strcat_s(buf, type);
	strcat_s(buf, "Connection: Close\r\n");
	strcat_s(buf, "\r\n");
	if (success) {
		strcat_s(buf, msg);
	}
	else {
		strcat_s(buf, "<html><body><h1>404 - Not Found</h1></body></html>");
	}
	cout << "Return msg: " << buf << endl;
	return buf;
}

void MessageMgr::ResolveMessage(char* msg, TcpMgr client) {
	char* fengefu = "\r\n";
	char* fengefuSpace = " ";
	std::vector<std::string> strs = SplitStr(msg, fengefu);
	if (sizeof(strs) <= 0) {

	}
	else {
		cout << strs[0] << endl;
		std::vector<std::string> firstline = SplitStr(strs[0], fengefuSpace);
		if (firstline[0] == "GET") {
			cout << "GET URL: " << firstline[1] << endl;
			std::vector<std::string> splitStr = SplitStr(firstline[1], "/");
			cout << "GET URL URL FIRST : " << splitStr[1] << endl;
			if (firstline[1] == "/geturl") {
				char* str = "hello,i am ackerman";
				char* sendMsg = CreateMessage(str,"",true);
				client.Send(sendMsg);
			}
			else if (splitStr[1] == "res") {
				//get res
				GetResFromSer(firstline[1], client);
			}
			else {
				//no res ,return 404 html
				char* sendMsg = CreateMessage("", "text/html", false);
				client.Send(sendMsg);
			}
		}
		else if (firstline[0] == "POST") {
			//暂不处理POST
		}
	}
}



MessageMgr::MessageMgr() {

}
MessageMgr::~MessageMgr() {

}

void MessageMgr::GetResFromSer(const std::string& path, TcpMgr client) {
	std::string curDir = _getcwd(NULL, 0);
	curDir += path;
	cout << "curDir：" << curDir << endl;
	//std::ifstream ifs("D:/Ackerman/HotFixFramework/Server/Server/res/test.txt");
	std::ifstream ifs(curDir, ifstream::in | ios::binary);
	bool exist = ifs.good();
	if (exist) {
		ifs.seekg(0, ifs.end);
		size_t len = ifs.tellg();
		//cout << "len：" << len << endl;
		ifs.seekg(0, 0);
		if (sizeof(path.find(".png")>0)) {
			char c;
			char* buf = new char[len];
			cout << "File Length = "<<len << endl;
			ifs.read(buf, len);
			ifs.close();
			char* sendMsg = CreateMessage(buf, "image/png", true);
			client.Send(sendMsg);
		}
		else {
			char c;
			char* buf = new char[len];
			ifs.read(buf, len);
			cout << buf << endl;
			char* sendMsg = CreateMessage(buf, "text/plain", true);
			client.Send(sendMsg);
			
		}
	}
	else {
		char* sendMsg = CreateMessage("", "text/html", false);
		client.Send(sendMsg);
	}
	//cout << curDir << " Exsit : " << exist<<endl;
}