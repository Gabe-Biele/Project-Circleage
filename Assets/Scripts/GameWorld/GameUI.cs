﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    private SmartFox SFServer;

    private InputField ChatTB;
   private bool ChatTBisFocused;
    private string ChatText;

    private GameObject ChatContent;
    private List<GameObject> ChatTextLabel;
    private GameWorldManager ourGWM;
    private RayCastManager ourRCM;
    bool inventoryOpen;

    GameObject aInventoryPanel;
    private Item currentInventoryItem;
    private LocalPlayerController ourLPC;

    private GameObject rayCastLabel;


    // Use this for initialization
   void Start()
    {
        SFServer = SmartFoxConnection.Connection;
        ourGWM = GameObject.Find("SceneScriptsObject").GetComponent<GameWorldManager>();
        ourRCM = GameObject.Find("SceneScriptsObject").GetComponent<RayCastManager>();
        ChatTB = GameObject.Find("ChatTB").GetComponent<InputField>();
        ChatContent = GameObject.Find("ChatContent");
        ChatTextLabel = new List<GameObject>();

        rayCastLabel = GameObject.Find("RayCastLabel");
        rayCastLabel.SetActive(false);
        ChatTBisFocused = false;
        inventoryOpen = false;
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
        if(Input.GetMouseButtonDown(1) && currentInventoryItem != null)
        {
            ourRCM.ourPlayerActionDictionary["Use Item"].performAction(GameObject.Find("SceneScriptsObject"));
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            openInventory();
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
        //Switch Cursor Mode
        Camera.main.GetComponent<CameraController>().setCursorVisible(false);
    }
    public void contributionButton_Clicked()
    {
        ourRCM.ourPlayerActionDictionary["ContributeCenterNode"].performAction(GameObject.Find("SceneScriptsObject"));
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

    public void openInventory()
    {
        if (!inventoryOpen)
        {
            //Switch Cursor Mode
            Camera.main.GetComponent<CameraController>().setCursorVisible(true);

            ourLPC = GameObject.FindObjectOfType<LocalPlayerController>();
            aInventoryPanel = ourGWM.createObject("UI/InventoryWindow");
            aInventoryPanel.name = "InventoryPanel";
            aInventoryPanel.transform.SetParent(GameObject.Find("UICanvas").transform);
            aInventoryPanel.transform.localPosition = new Vector3(300, -160, 0);

            for(int x = 0; x < 6; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    GameObject anItemSlot = (GameObject)Instantiate(Resources.Load("UI/ItemSlot", typeof(GameObject)));
                    anItemSlot.transform.parent = aInventoryPanel.transform.FindChild("Items").transform;
                    anItemSlot.name = "Item " + (x * 10 + y).ToString();
                    anItemSlot.transform.localPosition = new Vector3(-220 + (y * 45), 120 - (x * 45));
                }
            }

            foreach (KeyValuePair<int, Item> entry in ourLPC.getInventory())
            {
                if(entry.Value.inInventory())
                {
                    string path = "ItemImages/" + entry.Value.getItemID().ToString();
                    Sprite itemImageSprite = Resources.Load(path, typeof(Sprite)) as Sprite;

                    GameObject itemImageObject = aInventoryPanel.transform.FindChild("Items").FindChild("Item " + entry.Value.getPosition().ToString()).gameObject;
                    ItemImageHandler itemHandler = itemImageObject.AddComponent<ItemImageHandler>();
                    itemHandler.thisItem = entry.Value;
                    itemHandler.ourUI = this;
                    Image itemImage = itemImageObject.GetComponent<Image>();
                    itemImage.sprite = itemImageSprite;
                    Debug.Log("Set sprite.");

                    GameObject quantityText = itemImage.transform.FindChild("QuantityText").gameObject;
                    if(entry.Value.getQuantity() > 1)
                    {
                        quantityText.GetComponent<Text>().text = entry.Value.getQuantity().ToString();
                        quantityText.SetActive(true);
                    }
                    else quantityText.SetActive(false);
                }
            }
            inventoryOpen = true;
        }
        else if (inventoryOpen)
        {
            //Switch Cursor Mode
            Camera.main.GetComponent<CameraController>().setCursorVisible(false);

            if(GameObject.Find("HoverWindow") != null)
            {
                Destroy(GameObject.Find("HoverWindow"));
            }
            Destroy(aInventoryPanel);
            inventoryOpen = false;
        }
    }
    public void setCurrentInventoryItem(Item i)
    {
        currentInventoryItem = i;
    }
    public Item getCurrentInventoryItem()
    {
        return currentInventoryItem;
    }

    public GameObject drawHoverBox(Item theItem)
    {
        GameObject aHoverBox = ourGWM.createObject("UI/HoverWindow");
        aHoverBox.name = "HoverWindow";
        aHoverBox.transform.SetParent(GameObject.Find("UICanvas").transform);
        aHoverBox.transform.position = Input.mousePosition + new Vector3(-95, 51);
        aHoverBox.transform.FindChild("Name").gameObject.GetComponent<Text>().text = theItem.getName();
        aHoverBox.transform.FindChild("Description").gameObject.GetComponent<Text>().text = theItem.getDescription();
        return aHoverBox;
    }
}
