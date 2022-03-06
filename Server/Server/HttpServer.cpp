#include "iostream"
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <string.h>
#pragma comment£¨lib£¬"ws2_32.lib"£©
using namespace std;
#define SERVERPORT 1212

void get_line(int sock,char* buf,int size) {

}

void DoHttpRequest(int client_sock) {
	char buf[256];
	int len = 0;
	len = recv(client_sock,buf,sizeof(buf),0);
	cout << "Start Receive Msg!" << endl;
	if (len>0) {
		cout << "Receive Success,Msg: " <<buf<< endl;
	}
	else {
		//cout << "Receive Failed..." << endl;
	}
}

void TestHttp() {
	bool start = false;
	int sock;
	sockaddr_in server_add;
	sock = socket(AF_INET, SOCK_STREAM, 0);
	server_add.sin_family = AF_INET;
	server_add.sin_addr.S_un.S_addr = htonl(INADDR_ANY);
	server_add.sin_family = htons(SERVERPORT);
	bind(sock, (sockaddr*)&server_add, sizeof(server_add));
	listen(sock, 128);
	cout << "Start Http Server Success..." << sock << endl;
	start = true;
	while (start) {
		sockaddr_in client;
		int clientsock;
		char client_ip[64];
		char buf[256];
		socklen_t client_addr_len;
		client_addr_len = sizeof(client);
		clientsock = accept(sock, (sockaddr*)&client, &client_addr_len);
		/*	char* outIP;
			outIP = (char*)inet_ntop(AF_INET,&client.sin_addr.S_un.S_addr, client_ip,sizeof(client_ip));
			int outPort = ntohs(client.sin_port);
			cout << "Client Ip :" << outIP << " Port: "<<outPort;*/
		if (clientsock == -1) {

		}
		else {
			DoHttpRequest(clientsock);
			closesocket(clientsock);
		}
	}
}

//int main() {
//	WSADATA ws;
//	WSAStartup(MAKEWORD(2,2),&ws);
//	
//	
//	return 0;
//}