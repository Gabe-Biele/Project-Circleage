using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Assets.Scripts.GameWorld.PlayerActions;

public class LocalPlayerController : MonoBehaviour
{
    private SmartFox SFServer;
    private string PlayerName = "";
    private float PlayerSpeed = 10;
    private float RotationSpeed = 40;
    private Rigidbody PlayerRB;
    private List<Item> itemList;
    GameUI theUI;

    private GameWorldManager ourGWM;

    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");

    private PlayerAction currentPlayerAction;

    void Start()
    {
        SFServer = SmartFoxConnection.Connection;
        this.PlayerRB = this.GetComponent<Rigidbody>();
        this.MecAnim = this.GetComponentInChildren<Animator>();
        theUI = (GameUI)FindObjectOfType(typeof(GameUI));
    }

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

    public float[] GetLocation()
    {
        float[] OurLocation = { this.transform.position.x, this.transform.position.y, this.transform.position.z };
        return OurLocation;
    }
    public float GetRotation()
    {
        return this.transform.localEulerAngles.y;
    }
    public string GetName()
    {
        return this.PlayerName;
    }
    public void SetName(string aPN)
    {
        this.PlayerName = aPN;
    }
    public void setPlayerAction(PlayerAction aPA)
    {
        currentPlayerAction = aPA;
    }
    public PlayerAction getPlayerAction()
    {
        return currentPlayerAction;
    }

    public void addItem(int itemID, string subLocation, int baglocation, int quantity)
    {
        Debug.Log("Sup");
        if(ourGWM == null)
        {
            ourGWM = GameObject.Find("SceneScriptsObject").GetComponent<GameWorldManager>();
        }
        if (itemList == null)
        {
            itemList = new List<Item>();
        }
        Debug.Log("Adding item.");
        Item itemAdded = new Item(itemID, subLocation, baglocation, quantity);
        string itemName;
        if (ourGWM.getItemNameDictionary().TryGetValue(itemID, out itemName))
        {
            Debug.Log(itemName);
            itemAdded.setName(itemName);
        }
        string itemDesc;
        if (ourGWM.getItemDescriptionDictionary().TryGetValue(itemID, out itemDesc))
        {
            Debug.Log(itemDesc);
            itemAdded.setDescription(itemDesc);
        }
        itemList.Add(itemAdded);
    }
    public List<Item> getItems()
    {
        Debug.Log(itemList.Count);
        return itemList;
    }
}
