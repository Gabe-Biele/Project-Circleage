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
    bool inventoryOpen;
    GameObject aInventoryPanel;
    private LocalPlayerController ourLPC;

    private GameObject rayCastLabel;


    // Use this for initialization
   void Start()
    {
        SFServer = SmartFoxConnection.Connection;
        ourGWM = GameObject.Find("SceneScriptsObject").GetComponent<GameWorldManager>();
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
        if (!inventoryOpen)
        {
            //Switch Cursor Mode
            Camera.main.GetComponent<CameraController>().setCursorVisible(true);

            ourLPC = GameObject.FindObjectOfType<LocalPlayerController>();
            aInventoryPanel = ourGWM.createObject("UI/InventoryWindow");
            aInventoryPanel.name = "InventoryPanel";
            aInventoryPanel.transform.SetParent(GameObject.Find("UICanvas").transform);
            aInventoryPanel.transform.localPosition = new Vector3(456, -160, 0);

            foreach (KeyValuePair<int, Item> entry in ourLPC.getInventory())
            {
                if(entry.Value.inInventory())
                {
                    Debug.Log(entry.Key);
                    string path = "ItemImages/" + entry.Value.getItemID().ToString();
                    Sprite itemImageSprite = Resources.Load(path, typeof(Sprite)) as Sprite;

                    GameObject itemImageObject = aInventoryPanel.transform.FindChild("Items").FindChild("Item " + entry.Value.getPosition().ToString()).gameObject;
                    ItemImageHandler itemHandler = itemImageObject.AddComponent<ItemImageHandler>();
                    itemHandler.thisItem = entry.Value;
                    itemHandler.ourUI = this;
                    Image itemImage = itemImageObject.GetComponent<Image>();
                    itemImage.sprite = itemImageSprite;
                    Debug.Log("Set sprite.");
                    if(entry.Value.getQuantity() > 1)
                    {
                        GameObject quantityText = itemImage.transform.FindChild("QuantityText").gameObject;
                        quantityText.GetComponent<Text>().text = entry.Value.getQuantity().ToString();
                        quantityText.SetActive(true);
                    }
                }
            }
            inventoryOpen = true;
        }
        else if (inventoryOpen)
        {
            //Switch Cursor Mode
            Camera.main.GetComponent<CameraController>().setCursorVisible(false);

            Destroy(aInventoryPanel);
            inventoryOpen = false;
        }
    }

    public GameObject drawHoverBox(Item theItem)
    {
        GameObject aHoverBox = ourGWM.createObject("UI/HoverWindow");
        aHoverBox.name = "HoverWindow";
        aHoverBox.transform.SetParent(GameObject.Find("UICanvas").transform);
        aHoverBox.transform.position = Input.mousePosition + new Vector3(50, -25);
        aHoverBox.transform.FindChild("Name").gameObject.GetComponent<Text>().text = theItem.getName();
        aHoverBox.transform.FindChild("Description").gameObject.GetComponent<Text>().text = theItem.getDescription();
        return aHoverBox;
    }



    public void attachImagetoPointer(Item movedItem)
    {
        GameObject itemImageObject = aInventoryPanel.transform.FindChild("Items").FindChild("Item " + movedItem.getPosition().ToString()).gameObject;
        itemImageObject.transform.position = Input.mousePosition;

    }

}
