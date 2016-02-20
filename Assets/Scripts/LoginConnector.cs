﻿using UnityEngine;
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

    //UI Elements
    private GameObject LoginPanel;
    private GameObject RegisterPanel;

	// Use this for initialization
	void Start ()
    {
        LoginPanel = GameObject.Find("LoginBox");
        RegisterPanel = GameObject.Find("RegisterBox");
        RegisterPanel.SetActive(false);

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
        SFServer.AddEventListener(SFSEvent.EXTENSION_RESPONSE, )

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
    public void Button_Clicked(String buttonName)
    {
        if(buttonName == "LoginButton")
        {

        }
        if(buttonName == "RegisterButton")
        {
            this.RegisterPanel.SetActive(true);
            this.LoginPanel.SetActive(false);
        }
        if(buttonName == "ExitButton")
        {
            SFServer.Disconnect();
            SFServer.RemoveAllEventListeners();
            Application.Quit();
        }
        if(buttonName == "CreateButton")
        {
            SFSObject NewAccountObject = new SFSObject();
            InputField UsernameTB = GameObject.Find("UsernameTB").GetComponent<InputField>();
            InputField PasswordTB = GameObject.Find("PasswordTB").GetComponent<InputField>();
            InputField ConfirmPasswordTB = GameObject.Find("ConfirmPasswordTB").GetComponent<InputField>();
            InputField EmailTB = GameObject.Find("EmailTB").GetComponent<InputField>();
            InputField RegistrationKeyTB = GameObject.Find("RegistrationKeyTB").GetComponent<InputField>();
            Debug.Log("Username: " + UsernameTB.text);
            Debug.Log("Password: " + PasswordTB.text);
            Debug.Log("ConfirmPW: " + ConfirmPasswordTB.text);
            Debug.Log("Email: " + EmailTB.text);
            Debug.Log("RegistrationKey: " + RegistrationKeyTB.text);

            if(PasswordTB.text == ConfirmPasswordTB.text)
            {
                NewAccountObject.PutUtfString("Username", UsernameTB.text);
                NewAccountObject.PutUtfString("Password", PasswordTB.text);
                NewAccountObject.PutUtfString("Email", EmailTB.text);
                NewAccountObject.PutUtfString("RegistrationKey", RegistrationKeyTB.text);

                this.SFServer.Send(new ExtensionRequest("$SignUp.Submit", NewAccountObject));
            }
            else
            {
                Debug.Log("Error Creating Account!");
            }
        }
        if(buttonName == "BackButton")
        {
            this.LoginPanel.SetActive(true);
            this.RegisterPanel.SetActive(false);
        }
    }
    public void LoginButton_Clicked()
    {
        this.RegisterPanel.SetActive(true);
        this.LoginPanel.SetActive(false);
        /*InputField UsernameTB = GameObject.Find("UsernameTB").GetComponent<InputField>();
        InputField PasswordTB = GameObject.Find("PasswordTB").GetComponent<InputField>();
        Debug.Log(UsernameTB.text);
        Debug.Log(PasswordTB.text);*/

        
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        String ExtensionResponseCode = (String)evt.Params["cmd"];

        if(ExtensionResponseCode == "$SignUp.Submit")
        {
            if((bool)evt.Params["success"])
            {
                Debug.Log("Success, thanks for registering");
            }
            else
            {
                Debug.Log("SignUp Error:" + (bool)evt.Params["errorMessage"]);
            }
        }
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
