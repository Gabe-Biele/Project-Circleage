using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class GameUI : MonoBehaviour
{
    private SmartFox SFServer;

    private InputField ChatTB;
   private bool ChatTBisFocused;
    private string ChatText;

    private GameObject ChatContent;
    private List<GameObject> ChatTextLabel;

    private GameObject rayCastLabel;


    // Use this for initialization
   void Start()
    {
        SFServer = SmartFoxConnection.Connection;

        ChatTB = GameObject.Find("ChatTB").GetComponent<InputField>();
        ChatContent = GameObject.Find("ChatContent");
        ChatTextLabel = new List<GameObject>();

        rayCastLabel = GameObject.Find("RayCastLabel");
        rayCastLabel.SetActive(false);
        ChatTBisFocused = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if(this.ChatTB.isFocused)
        {
            ChatTBisFocused = true;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(this.ChatTBisFocused)
            {
                this.SendMessage();
                this.ChatTBisFocused = false;
            }
            else
            {
                this.ChatTB.Select();
                this.ChatTBisFocused = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
    }

    void FixedUpdate()
    {
        if(SFServer != null)
        {
            SFServer.ProcessEvents();
        }
    }

    public void SendMessage()
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
    public void contributionExitButton_Clicked()
    {
        Destroy(GameObject.Find("ContributionPanel"));
    }

    public void processChat(String chatText)
    {
        ChatTextLabel.Add((GameObject)Instantiate(Resources.Load("UI/ChatText", typeof(GameObject))));
        ChatTextLabel[ChatTextLabel.Count - 1].name = "ChatText[" + (ChatTextLabel.Count - 1) + "]";
        ChatTextLabel[ChatTextLabel.Count - 1].GetComponent<RectTransform>().SetParent(this.ChatContent.GetComponent<RectTransform>());
        ChatTextLabel[ChatTextLabel.Count - 1].GetComponent<Text>().text = chatText;
        for(int i = 0; i < ChatTextLabel.Count; i++)
        {
            ChatTextLabel[i].GetComponent<RectTransform>().localPosition = new Vector3(205, ChatTextLabel[i].GetComponent<RectTransform>().localPosition.y + 18);
        }
    }
    public GameObject getRayCastLabel()
    {
        return rayCastLabel;
    }
    public void deactivateRayCastLabel()
    {
        rayCastLabel.SetActive(false);
    }

    public bool GetchatTBFocus()
    {
        return ChatTB.isFocused;
    }

    public void OpenInventory()
    {

    }
}
