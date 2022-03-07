#pragma once
#include "TcpMgr.h"

class MessageMgr {
public:
	void ResolveMessage(char* msg, TcpMgr client);
	MessageMgr();
	~MessageMgr();
	std::vector<std::string> SplitStr(std::string str, std::string pattern);
	char* CreateMessage(char* msg, char* resType, bool success = true);
	void GetResFromSer(const std::string& path, TcpMgr client);
};