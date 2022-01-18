#pragma once

class INetSessionMgr {
public:
	INetSessionMgr();
	virtual ~INetSessionMgr();
	static INetSessionMgr* GetInstance() {
		return mInstance;
	}
	
private:
	static INetSessionMgr* mInstance;
};