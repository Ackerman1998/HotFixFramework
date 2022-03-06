#pragma once
#include <WinSock2.h>
#include <iostream>
#include <WS2tcpip.h>
using namespace std;
class TcpMgr {
public:
	int CreateSocket();
	void Bind(unsigned int port);
	TcpMgr Accept();
	void Close();
	int Recv(char* buf,int bufLen);
	int Send(const char* buf);
	TcpMgr();
	virtual ~TcpMgr();
	int sock = 0;
	unsigned short port = 0;
	char ip[16];
};