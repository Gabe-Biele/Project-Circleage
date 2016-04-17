﻿using UnityEngine;
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

    private GameObject rayCastLabel;


    // Use this for initialization
    void Start ()
    {
        SFServer = SmartFoxConnection.Connection;

        ChatTB = GameObject.Find("ChatTB").GetComponent<InputField>();
        ChatContent = GameObject.Find("ChatContent");
        ChatTextLabel = new List<GameObject>();

        rayCastLabel = GameObject.Find("RayCastLabel");
        rayCastLabel.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(this.ChatTB.isFocused);
            if(this.ChatTBisFocused)
            {
                this.EnterButton_Clicked();
            }
            else
            {
                this.ChatTB.Select();
                this.ChatTBisFocused = true;
            }
        }
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
    public void activateRayCastLabel(GameObject aGameObject)
    {
        if(aGameObject.tag == "Resource")
        {
            rayCastLabel.SetActive(true);
            rayCastLabel.GetComponent<Text>().text = aGameObject.name.Split('_')[1] + "\nPress F to Gather";
        }
    }
    public void deactivateRayCastLabel()
    {
        rayCastLabel.SetActive(false);
    }
}
