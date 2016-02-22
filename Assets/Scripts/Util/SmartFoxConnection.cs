using System;
using UnityEngine;
using Sfs2X;


class SmartFoxConnection : MonoBehaviour
{
    private static SmartFox SFServer;
    private static SmartFoxConnection OurInstance;

    //Singleton Setup
    public static SmartFox Connection
    {
        get
        {
            if(OurInstance == null)
            {
                OurInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
            }
            return SFServer;
        }
        set
        {
            if(OurInstance == null)
            {
                OurInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
            }
            SFServer = value;
        }
    }

    // Handle disconnection
    // ** Important for Windows users - can cause crashes otherwise
    void OnApplicationQuit()
    {
        if(SFServer.IsConnected)
        {
            SFServer.Disconnect();
        }
    }
}

