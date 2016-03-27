using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Sfs2X;
using UnityEngine.SceneManagement;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    private GameObject CharacterListPanel;
    private List<GameObject> CharacterPanel;
    private GameObject SelectedCharacterPanel;

    private SmartFox SFServer;

    // Use this for initialization
    void Start()
    {
        //Identify UI Elements
        this.CharacterPanel = new List<GameObject>();
        this.CharacterListPanel = GameObject.Find("CharacterListPanel");

        if(SmartFoxConnection.Connection == null)
        {
            SceneManager.LoadScene("Login");
            return;
        }
        SFServer = SmartFoxConnection.Connection;

        // Register callback delegates
        SFServer.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        SFServer.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

        //Send AccountName to get back list of Account's Characters
        ISFSObject ObjectIn = new SFSObject();
        ObjectIn.PutUtfString("AccountName", SFServer.MySelf.Name.ToLower());
        SFServer.Send(new ExtensionRequest("CharacterList", ObjectIn));
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
    private void OnExtensionResponse(BaseEvent evt)
    {
        try
        {
            String ResponseType = (string)evt.Params["cmd"];
            Debug.Log("Received Response: " + ResponseType);
            ISFSObject ObjectIn = (SFSObject)evt.Params["params"];
            if(ResponseType == "CharacterList")
            {
                for(int i = 0; i < ObjectIn.GetUtfStringArray("NameList").Length; i++)
                {
                    float posY = 200 - (50 + (80 * i));
                    //Debug.Log(posY);
                    CharacterPanel.Add((GameObject)Instantiate(Resources.Load("UI/CharacterPanel", typeof(GameObject))));
                    CharacterPanel[i].GetComponent<RectTransform>().parent = this.CharacterListPanel.GetComponent<RectTransform>();
                    CharacterPanel[i].GetComponent<RectTransform>().localPosition = new Vector3(0, posY, 0);
                    CharacterPanel[i].GetComponentInChildren<Text>().text = ObjectIn.GetUtfStringArray("NameList")[i];

                    GameObject aCP = CharacterPanel[i];
                    CharacterPanel[i].GetComponent<Button>().onClick.AddListener(() => CharacterPanel_Clicked(aCP));
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Exception handling response: " + e.Message + " >>> " + e.StackTrace);
        }
    }
    public void PlayButton_Click()
    {
        if(this.SelectedCharacterPanel != null)
        {
            ISFSObject ObjectIn = new SFSObject();
            ObjectIn.PutUtfString("CharacterName", this.SelectedCharacterPanel.GetComponentInChildren<Text>().text);
            this.SFServer.Send(new ExtensionRequest("SelectCharacter", ObjectIn));

            SceneManager.LoadScene("GameWorld");
            this.SFServer.RemoveAllEventListeners();
        }
    }
    public void CreateButton_Click()
    {
        SceneManager.LoadScene("CharacterCreation");
        this.SFServer.RemoveAllEventListeners();
    }
    public void CharacterPanel_Clicked(GameObject aCharacterPanel)
    {
        for(int i = 0; i < this.CharacterPanel.Count; i++)
        {
            this.CharacterPanel[i].GetComponent<Image>().color = new Color(0.47f, 0.56f, 1, 1);
        }
        aCharacterPanel.GetComponent<Image>().color = new Color(1, 1, 0.47f, 0.85f);
        SelectedCharacterPanel = aCharacterPanel;
    }

    public void OnConnectionLost(BaseEvent evt)
    {
        // Reset all internal states so we kick back to login screen
        SFServer.RemoveAllEventListeners();
        SceneManager.LoadScene("Login");
    }
}
