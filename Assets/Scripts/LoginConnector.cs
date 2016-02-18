using UnityEngine;
using UnityEngine.UI;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System.Collections;
using System;

public class LoginConnector : MonoBehaviour
{
    //SmartFoxConnectionInfo
    private ConfigData OurConfigData = new ConfigData();

    private SmartFox SFServer;

	// Use this for initialization
	void Start ()
    {
        SFServer = new SmartFox();

        //Set our basic default connection parameters
        this.OurConfigData.Host = "biele.us";
        this.OurConfigData.Port = 9933;
        this.OurConfigData.Zone = "ProjectCircleage";

        // Set ThreadSafeMode explicitly, or Windows Store builds will get a wrong default value (false)
        SFServer.ThreadSafeMode = true;

        SFServer.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        SFServer.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        SFServer.AddEventListener(SFSEvent.LOGIN, OnLogin);
        SFServer.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        SFServer.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        SFServer.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);

        SFServer.Connect(this.OurConfigData);
    }

    // Update is called once per frame
    void Update ()
    {
        if(SFServer != null)
        {
            SFServer.ProcessEvents();
        }
	}

    public void LoginButton_Clicked()
    {
        InputField UsernameTB = GameObject.Find("UsernameTB").GetComponent<InputField>();
        InputField PasswordTB = GameObject.Find("PasswordTB").GetComponent<InputField>();
        Debug.Log(UsernameTB.text);
        Debug.Log(PasswordTB.text);

        
    }

    private void OnConnection(BaseEvent evt)
    {
        //Default SFS return for a succussful connection is "sucess"
        if((bool)evt.Params["success"])
        {
            Debug.Log("Connected to Game Server!");
            // Save reference to the SmartFox instance in a static field, to share it among different scenes
            SmartFoxConnection.GetInstance().SetSmartFoxServer(SFServer);
        }
        else
        {
            // Show error message
            Debug.Log("Connection failed");
        }
    }
    private void OnConnectionLost(BaseEvent evt)
    {
        Debug.Log("Connection Lost! =(");
        // Remove SFS2X listeners
        SFServer.RemoveAllEventListeners();
    }
    private void OnLogin(BaseEvent evt)
    {
        throw new NotImplementedException();
    }
    private void OnLoginError(BaseEvent evt)
    {
        throw new NotImplementedException();
    }
    private void OnRoomJoin(BaseEvent evt)
    {
        throw new NotImplementedException();
    }
    private void OnRoomJoinError(BaseEvent evt)
    {
        throw new NotImplementedException();
    }

}
