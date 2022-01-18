#pragma once
#include<Winsock2.h>
#include <iostream>
/// <summary>
/// ��½���������
/// </summary>
class CLSNetSessionMgr
{
public:
	CLSNetSessionMgr();
	~CLSNetSessionMgr();
	bool GetNetIp(std::string& ipaddress, int port) {
		char hostName[255]="";
		//���ر��������ı�׼������
		int returnValue = gethostname(hostName, sizeof(hostName));
		std::cout <<"LocalHostName��"<< hostName << std::endl;
		//std::cout <<"returnValue��"<< returnValue << std::endl;
		if (returnValue==SOCKET_ERROR) {
			return false;
		}
		//�ýṹ��¼��������Ϣ����������������������ַ���͡���ַ���Ⱥ͵�ַ�б�
		struct hostent* phe=gethostbyname(hostName);
		//std::cout << "hostent.h_addr_list��" << phe->h_name << std::endl;
		if (phe==NULL) {
			return false;
		}
		else {
			for (int i = 0;phe->h_addr_list[i]!=0;++i) {
				//in_addr ������ʾһ��32λ��IPv4��ַ��
				struct in_addr addr;
				memcpy(&addr, phe->h_addr_list[i],sizeof(in_addr));
				//��һ�������ֽ����IP��ַ��Ҳ���ǽṹ��in_addr���ͱ�����ת��Ϊ���ַ�������IP��ַ
				ipaddress = inet_ntoa(addr);
				std::cout << "Get IP Address:" << ipaddress<< std::endl;
				return true;
			}
		}
		return false;
	}
private:

};
