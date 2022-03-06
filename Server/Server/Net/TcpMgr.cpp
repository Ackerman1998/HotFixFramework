#include "TcpMgr.h"


void TcpMgr::Bind(unsigned int port) {
	if (sock<=0) {
		CreateSocket();
	}
	sockaddr_in addr;
	addr.sin_family = AF_INET;
	addr.sin_port = htons(port);
	addr.sin_addr.S_un.S_addr = htons(0);
	if (bind(sock, (sockaddr*)&addr, sizeof(addr))!=0) {
		cout << "Bind port Failed" << endl;
		return;
	}
	cout << "Bind port Success" << endl;
	listen(sock,100);
	
}
int TcpMgr::CreateSocket() {
	sock = socket(AF_INET,SOCK_STREAM,0);
	if (sock==-1) {
		cout << "Create Socket Failed:" << endl;
	}
	return sock;
}

TcpMgr TcpMgr::Accept() {
	TcpMgr tMgr;
	sockaddr_in addr;
	socklen_t len = sizeof(addr);
	int client = ::accept(sock,(sockaddr*)&addr,&len);
	if (client<=0) {
		return tMgr;
	}
	printf("Accept Client %d\n",client);
	char* ip = inet_ntoa(addr.sin_addr);
	strcpy_s(tMgr.ip, ip);
	tMgr.sock = client;
	tMgr.port = ntohs(addr.sin_port);
	return tMgr;
}

void TcpMgr::Close() {
	if (sock <= 0)return;
	closesocket(sock);
}

int TcpMgr::Recv(char* buf, int bufLen) {
	return recv(sock,buf,bufLen,0);
}
TcpMgr::TcpMgr() {

}
TcpMgr::~TcpMgr() {

}
int TcpMgr::Send(const char* buf) {
	send(sock,buf, (strlen(buf)),0);
	return 0;
}