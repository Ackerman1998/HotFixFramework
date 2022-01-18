#pragma once
#include<Winsock2.h>
#include <iostream>
/// <summary>
/// 登陆服务管理器
/// </summary>
class CLSNetSessionMgr
{
public:
	CLSNetSessionMgr();
	~CLSNetSessionMgr();
	bool GetNetIp(std::string& ipaddress, int port) {
		char hostName[255]="";
		//返回本地主机的标准主机名
		int returnValue = gethostname(hostName, sizeof(hostName));
		std::cout <<"LocalHostName："<< hostName << std::endl;
		//std::cout <<"returnValue："<< returnValue << std::endl;
		if (returnValue==SOCKET_ERROR) {
			return false;
		}
		//该结构记录主机的信息，包括主机名、别名、地址类型、地址长度和地址列表
		struct hostent* phe=gethostbyname(hostName);
		//std::cout << "hostent.h_addr_list：" << phe->h_name << std::endl;
		if (phe==NULL) {
			return false;
		}
		else {
			for (int i = 0;phe->h_addr_list[i]!=0;++i) {
				//in_addr 用来表示一个32位的IPv4地址。
				struct in_addr addr;
				memcpy(&addr, phe->h_addr_list[i],sizeof(in_addr));
				//将一个网络字节序的IP地址（也就是结构体in_addr类型变量）转化为（字符串）的IP地址
				ipaddress = inet_ntoa(addr);
				std::cout << "Get IP Address:" << ipaddress<< std::endl;
				return true;
			}
		}
		return false;
	}
private:

};
