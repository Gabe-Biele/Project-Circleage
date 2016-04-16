using UnityEngine;
using System.Collections;
using Sfs2X;
using UnityEngine.SceneManagement;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine.UI;
using Sfs2X.Core;
using System;
using System.Collections.Generic;

public class GameUI : MonoBehaviour
{
    private SmartFox SFServer;

    private InputField ChatTB;
    private Boolean ChatTBisFocused;
    private string ChatText;

    private GameObject ChatContent;
    private List<GameObject> ChatTextLabel;

    // Use this for initialization
    void Start ()
    {
        if(SmartFoxConnection.Connection == null)
        {
            SceneManager.LoadScene("Login");
            return;
        }
        SFServer = SmartFoxConnection.Connection;
        SmartFoxConnection.NeedsDespawn = true;

        ChatTB = GameObject.Find("ChatTB").GetComponent<InputField>();
        ChatContent = GameObject.Find("ChatContent");
        ChatTextLabel = new List<GameObject>();
        ChatTBisFocused = false;

        SFServer.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        try
        {
            String ResponseType = (string)evt.Params["cmd"];
            Debug.Log("Received Response: " + ResponseType);
            ISFSObject ObjectIn = (SFSObject)evt.Params["params"];
            if(ResponseType == "ProcessChat")
            {
                //Debug.Log(ObjectIn.GetUtfString("ChatText"));
                ChatTextLabel.Add((GameObject)Instantiate(Resources.Load("UI/ChatText", typeof(GameObject))));
                ChatTextLabel[ChatTextLabel.Count - 1].name = "ChatText[" + (ChatTextLabel.Count - 1) + "]";
                ChatTextLabel[ChatTextLabel.Count-1].GetComponent<RectTransform>().SetParent(this.ChatContent.GetComponent<RectTransform>());
                ChatTextLabel[ChatTextLabel.Count-1].GetComponent<Text>().text = ObjectIn.GetUtfString("ChatText");
                for(int i = 0; i < ChatTextLabel.Count; i++)
                {
                    ChatTextLabel[i].GetComponent<RectTransform>().localPosition = new Vector3(205, ChatTextLabel[i].GetComponent<RectTransform>().localPosition.y + 18);
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
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (this.ChatTBisFocused)
            {
                this.EnterButton_Clicked();
                this.ChatTBisFocused = false;
            }
            else
            {
                this.ChatTB.Select();
                this.ChatTBisFocused = true;
            }
        }
        if(this.ChatTB.isFocused)
        {
            this.ChatTBisFocused = true;
        }
        else
        {
            this.ChatTBisFocused = false;
        }
       // Debug.Log(this.ChatTB.isFocused);

    }
    void FixedUpdate()
    {
        if(SFServer != null)
        {
            SFServer.ProcessEvents();
        }
    }

    public void EnterButton_Clicked()
    {
        this.ChatText = this.ChatTB.text;
        this.ChatTB.text = "";

        ISFSObject ObjectIn = new SFSObject();
        ObjectIn.PutUtfString("ChatText", this.ChatText);
        SFServer.Send(new ExtensionRequest("ProcessChat", ObjectIn));
    }
    public void QuitButton_Clicked()
    {
        Application.Quit();
    }

    public InputField GetChatTB()
    {
        return ChatTB;
    }
}
