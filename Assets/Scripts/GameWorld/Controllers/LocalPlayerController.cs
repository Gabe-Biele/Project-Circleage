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

    private Dictionary<int, Item> playerInventory = new Dictionary<int, Item>();
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
    public Dictionary<int, Item> getInventory()
    {
        return playerInventory;
    }
}
