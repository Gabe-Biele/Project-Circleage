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
using UnityEngine.SceneManagement;

public class LoginConnector : MonoBehaviour
{
    //SmartFoxConnectionInfo
    private ConfigData OurConfigData = new ConfigData();
    private SmartFox SFServer;
    private SmartFoxConnection OurSmartFoxConnection;

    //UI Elements
    private GameObject LoginPanel;
    private GameObject RegisterPanel;
    private GameObject MessageText;
    private InputField UsernameTB;
    private InputField PasswordTB;

    //Static Elements
    private static String GAME_ZONE = "ProjectCircleage";

	// Use this for initialization
	void Start ()
    {
        LoginPanel = GameObject.Find("LoginBox");
        RegisterPanel = GameObject.Find("RegisterBox");
        RegisterPanel.SetActive(false);
        MessageText = GameObject.Find("MessageText");
        MessageText.SetActive(false);

        UsernameTB = GameObject.Find("UsernameTB").GetComponent<InputField>();
        PasswordTB = GameObject.Find("PasswordTB").GetComponent<InputField>();
        UsernameTB.Select();

        SFServer = new SmartFox();

        //Set our basic default connection parameters
        this.OurConfigData.Host = "biele.us";
        this.OurConfigData.Port = 9933;
        this.OurConfigData.Zone = GAME_ZONE;

        // Set ThreadSafeMode explicitly, or Windows Store builds will get a wrong default value (false)
        SFServer.ThreadSafeMode = true;

        SFServer.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        SFServer.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        SFServer.AddEventListener(SFSEvent.LOGIN, OnLogin);
        SFServer.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        SFServer.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

        SFServer.Connect(this.OurConfigData);
    }

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(this.UsernameTB.isFocused)
            {
                this.PasswordTB.Select();
            }
            else
            {
                this.UsernameTB.Select();
            }
        }
        if(SFServer != null)
        {
            SFServer.ProcessEvents();
        }
	}
    public void Button_Clicked(String buttonName)
    {
        if(buttonName == "LoginButton")
        {
            Debug.Log("Username: " + UsernameTB.text);
            Debug.Log("Password: " + PasswordTB.text);
            String EncryptedPW = PasswordUtil.MD5Password(PasswordTB.text);
            SFServer.Send(new LoginRequest(UsernameTB.text, EncryptedPW, GAME_ZONE));
            SFServer.Send(new JoinRoomRequest("Game"));
        }
        if(buttonName == "RegisterButton")
        {
            this.RegisterPanel.SetActive(true);
            this.LoginPanel.SetActive(false);
            SFServer.Send(new LoginRequest("", "", "Registration"));
        }
        if(buttonName == "ExitButton")
        {
            SFServer.Disconnect();
            SFServer.RemoveAllEventListeners();
            Application.Quit();
        }
        if(buttonName == "CreateButton")
        {
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
                SFSObject NewAccountObject = new SFSObject();
                NewAccountObject.PutUtfString("Username", UsernameTB.text);
                NewAccountObject.PutUtfString("PasswordHash", PasswordTB.text);
                NewAccountObject.PutUtfString("Email", EmailTB.text);
                NewAccountObject.PutUtfString("RegistrationKey", RegistrationKeyTB.text);

                this.SFServer.Send(new ExtensionRequest("$SignUp.Submit", NewAccountObject));
            }
            else
            {
                if(!MessageText.activeInHierarchy)
                {
                    MessageText.SetActive(true);
                }
                MessageText.GetComponent<Text>().text = "Passwords do not match! Please check and try again.";
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
        String ResponseType = (string)evt.Params["cmd"];
        Debug.Log("Received Response: " + ResponseType);
        ISFSObject ObjectIn = (SFSObject)evt.Params["params"];
        if(ResponseType == "$SignUp.Submit")
        {

            if(!MessageText.activeInHierarchy)
            {
                MessageText.SetActive(true);
            }
            if(ObjectIn.GetBool("success"))
            {
                Debug.Log("Success, thanks for registering");
                MessageText.GetComponent<Text>().text = "Success, thanks for registering";
                SceneManager.LoadScene("CharacterSelection");
                SFServer.RemoveAllEventListeners();
            }
            else
            {
                Debug.Log("SignUp Error:" + ObjectIn.GetUtfString("errorMessage"));
                MessageText.GetComponent<Text>().text = "SignUp Error: " + ObjectIn.GetUtfString("errorMessage");
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
            SmartFoxConnection.Connection = SFServer;
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
        Debug.Log("Login Success! You are now in " + SFServer.CurrentZone);
        if(SFServer.CurrentZone == GAME_ZONE)
        {
            SceneManager.LoadScene("CharacterSelection");
            SFServer.RemoveAllEventListeners();
        }
    }
    private void OnLoginError(BaseEvent evt)
    {
        Debug.Log("Login Error:" + evt.Params["errorMessage"]);
    }
}
