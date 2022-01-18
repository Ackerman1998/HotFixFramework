#pragma once
#pragma comment(lib , "wsock32.lib")
#include "Net/LSNetSessionMgr.h"
#include "tchar.h"
#include <map>

struct sLSConfig {
	INT32 bs_listen_port;
	INT32 bs_base_index;
	INT32 bs_max_count;
	INT32 client_listen_port;
};

struct sOneBsInfo
{
	bool bs_isLost;
	UINT32 bs_nets;
	std::string bs_IpExport;	
	std::string bs_Ip;		
	UINT32 bs_Port;
	UINT32 bs_Id;
};
extern std::map<UINT32, sOneBsInfo> gAllBsInfo;
extern sLSConfig slsconfig;