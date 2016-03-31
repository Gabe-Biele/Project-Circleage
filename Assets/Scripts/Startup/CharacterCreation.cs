using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using UnityEngine.SceneManagement;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class CharacterCreation : MonoBehaviour
{
    private SmartFox SFServer;
    private GameObject CharacterNameTB;
    private GameObject ErrorText;


    // Use this for initialization
    void Start ()
    {
        //Define UI Elements
        this.CharacterNameTB = GameObject.Find("CharacterNameTB");
        this.ErrorText = GameObject.Find("ErrorText");
        this.ErrorText.SetActive(false);

        if(SmartFoxConnection.Connection == null)
        {
            SceneManager.LoadScene("Login");
            return;
        }
        SFServer = SmartFoxConnection.Connection;

        // Register callback delegates
        SFServer.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        SFServer.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    public void CreateButton_Click()
    {
        //Define Regex
        Regex r = new Regex("^[a-zA-Z0-9]*$");
        string CharName = this.CharacterNameTB.GetComponent<InputField>().text;
        if(CharName.Length < 3 || CharName.Length > 16 || !r.IsMatch(CharName))
        {
            this.ErrorText.SetActive(true);
            this.ErrorText.GetComponent<Text>().text = "Error: Character Name must be 3 to 16 character long and only contain alphanumeric characters.";
        }
        else
        {
            ISFSObject ObjectIn = new SFSObject();
            //Debug.Log(this.CharacterNameTB.GetComponent<InputField>().text);
            ObjectIn.PutUtfString("CharacterName", this.CharacterNameTB.GetComponent<InputField>().text);
            SFServer.Send(new ExtensionRequest("CreateCharacter", ObjectIn));
        }
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        try
        {
            String ResponseType = (string)evt.Params["cmd"];
            Debug.Log("Received Response: " + ResponseType);
            ISFSObject ObjectIn = (SFSObject)evt.Params["params"];
            if(ResponseType == "CreateCharacter")
            {
                if(ObjectIn.GetBool("IsCreated"))
                {
                    SFServer.RemoveAllEventListeners();
                    SceneManager.LoadScene("CharacterSelection");
                }
                else
                {
                    this.ErrorText.SetActive(true);
                    this.ErrorText.GetComponent<Text>().text = ObjectIn.GetUtfString("ErrorMessage");
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Exception handling response: " + e.Message + " >>> " + e.StackTrace);
        }
    }

                // Update is called once per frame
                void Update()
    {
    }
    void FixedUpdate()
    {
        if(SFServer != null)
        {
            SFServer.ProcessEvents();
        }
    }
    public void OnConnectionLost(BaseEvent evt)
    {
        // Reset all internal states so we kick back to login screen
        SFServer.RemoveAllEventListeners();
        SceneManager.LoadScene("Login");
    }
}
