using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Sfs2X.Core;
using Sfs2X;
using System.Collections.Generic;
using Sfs2X.Entities;
using System;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using Assets.Scripts.GameWorld.ServerResponseHandlers;

public class GameWorldManager : MonoBehaviour
{
    private SmartFox SFServer;

    private GameObject LocalPlayer;
    private LocalPlayerController ourLPC;

    private Dictionary<string, GameObject> ourPlayerDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, ServerResponseHandler> ourSRHDictionary = new Dictionary<string, ServerResponseHandler>();
    private Dictionary<int, GameObject> ourNPCDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> ourResourceDictionary = new Dictionary<int, GameObject>();

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

        // Register callback delegates
        SFServer.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        SFServer.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

        //Register ServerResponseHandlers
        ourSRHDictionary.Add("SpawnPlayer", new SpawnPlayerHandler());
        ourSRHDictionary.Add("DespawnPlayer", new DespawnPlayerHandler());
        ourSRHDictionary.Add("PositionUpdate", new PositionUpdateHandler());
        ourSRHDictionary.Add("RotationUpdate", new RotationUpdateHandler());
        ourSRHDictionary.Add("SpawnNPC", new SpawnNPCHandler());
        ourSRHDictionary.Add("SpawnResource", new SpawnResourceHandler());
        ourSRHDictionary.Add("ProcessChat", new ProcessChatHandler());
        ourSRHDictionary.Add("GatherResource", new GatherResourceHandler());

        ISFSObject ObjectIn = new SFSObject();
        ObjectIn.PutUtfString("AccountName", SFServer.MySelf.Name.ToLower());
        SFServer.Send(new ExtensionRequest("SpawnPlayer", ObjectIn));
    }


    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            ISFSObject ObjectIn = new SFSObject();
            ObjectIn.PutFloatArray("Location", ourLPC.GetLocation());
            ObjectIn.PutBool("IsMoving", true);
            SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            ISFSObject ObjectIn = new SFSObject();
            ObjectIn.PutFloatArray("Location", ourLPC.GetLocation());
            ObjectIn.PutBool("IsMoving", false);
            SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));
        }
        if(Input.GetKey(KeyCode.W) && Input.GetAxis("Mouse X") != 0)
        {
            ISFSObject ObjectIn = new SFSObject();
            ObjectIn.PutFloat("Rotation", ourLPC.GetRotation());
            SFServer.Send(new ExtensionRequest("RotationUpdate", ObjectIn));
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(ourLPC.getPlayerAction() != null)
            {
                ourLPC.getPlayerAction().PerformAction(GameObject.Find("SceneScriptsObject"));
            }
        }
        TextMesh[] textObjects = FindObjectsOfType<TextMesh>();

        //Is there a better way to do this?
        foreach (TextMesh textObject in textObjects)
        {
            textObject.transform.LookAt(Camera.main.transform);
            textObject.transform.Rotate(new Vector3(0, 180, 0));
        }
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
            //Debug.Log("Received Response: " + ResponseType);
            ISFSObject anObjectIn = (SFSObject)evt.Params["params"];

            ourSRHDictionary[ResponseType].HandleResponse(anObjectIn, this);
        }
        catch(Exception e)
        {
            Debug.Log("Exception handling response: " + e.Message + " >>> " + e.StackTrace);
        }
    }
    public void OnConnectionLost(BaseEvent evt)
    {
        // Reset all internal states so we kick back to login screen
        SFServer.RemoveAllEventListeners();
        SceneManager.LoadScene("Login");
    }
    public GameObject createObject(string objectName)
    {
        return (GameObject)Instantiate(Resources.Load(objectName, typeof(GameObject)));
    }
    public void destroyObject(string objectName)
    {
        Destroy(GameObject.Find(objectName));
    }
    public void destroyObject(GameObject o)
    {
        Destroy(o);
    }
    public LocalPlayerController getLPC()
    {
        return this.ourLPC;
    }
    public void setLPC(LocalPlayerController aLPC)
    {
        this.ourLPC = aLPC;
    }
    public Dictionary<string, GameObject> getPlayerDictionary()
    {
        return this.ourPlayerDictionary;
    }
    public Dictionary<int, GameObject> getNPCDictionary()
    {
        return this.ourNPCDictionary;
    }
    public Dictionary<int, GameObject> getResourceDictionary()
    {
        return this.ourResourceDictionary;
    }
}
