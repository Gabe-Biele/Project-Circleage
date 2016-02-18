using System;
using UnityEngine;
using Sfs2X;


class SmartFoxConnection : MonoBehaviour
{
    private SmartFox SFServer;

    //Singleton Setup
    private static SmartFoxConnection OurInstance;
    private SmartFoxConnection()
    {
        OurInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
    }
    public static SmartFoxConnection GetInstance()
    {
        if(OurInstance == null)
        {
            OurInstance = new SmartFoxConnection();
        }
        return OurInstance;
    }

    public SmartFox GetSmartFoxServer()
    {
        return this.SFServer;
    }
    public void SetSmartFoxServer(SmartFox SFS)
    {
        this.SFServer = SFS;
    }

    // Handle disconnection
    // ** Important for Windows users - can cause crashes otherwise
    void OnApplicationQuit()
    {
        if(this.SFServer.IsConnected)
        {
            this.SFServer.Disconnect();
        }
    }
}

