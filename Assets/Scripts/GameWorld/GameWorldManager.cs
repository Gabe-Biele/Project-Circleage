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
    public void spawnRemotePlayer(String aCharacterName, float[] LocationArray, float Rotation)
    {
        //Instantiate RemotePlayerObject
        GameObject aRemotePlayer = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerBasic", typeof(GameObject)));
        aRemotePlayer.name = "GameCharacter_" + aCharacterName;
        aRemotePlayer.AddComponent<RemotePlayerController>();
        aRemotePlayer.transform.position = new Vector3(LocationArray[0], LocationArray[1], LocationArray[2]);
        aRemotePlayer.GetComponent<RemotePlayerController>().SetRotation(Rotation);
        aRemotePlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;

        //Add Newly spawned player to Dictionary
        ourPlayerDictionary.Add(aCharacterName, aRemotePlayer);
    }
    public void despawnRemotePlayer(string aCharacterName)
    {
        Destroy(GameObject.Find("GameCharacter_" + aCharacterName));
        ourPlayerDictionary.Remove(aCharacterName);
    }
    public void spawnLocalPlayer(String aCharacterName, float[] Loc)
    {
        Debug.Log(Loc[0] + "      " + Loc[1] + "      " + Loc[2]);
        // Lets spawn our local player model
        LocalPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerBasic", typeof(GameObject)));
        LocalPlayer.transform.position = new Vector3(Loc[0], Loc[1], Loc[2]);
        LocalPlayer.transform.rotation = Quaternion.identity;

        // Since this is the local player, lets add a controller and fix the camera
        LocalPlayer.AddComponent<LocalPlayerController>();
        ourLPC = LocalPlayer.GetComponent<LocalPlayerController>();
        ourLPC.SetName(aCharacterName);
        LocalPlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;
        Camera.main.transform.parent = LocalPlayer.transform;
        GameObject cameraAttach = new GameObject();
        cameraAttach.transform.parent = LocalPlayer.transform;
        cameraAttach.transform.localPosition = new Vector3(1f, 2.5f, 1.0f);
        Camera.main.GetComponent<CameraController>().setTarget(cameraAttach);
        Camera.main.GetComponent<CameraController>().setCombatMode(false); //False will turn ON combat mode
    }
    public void spawnNPC(int ID, String aNPCName, float[] location)
    {
        //Instantiate RemotePlayerObject
        GameObject aNPC = (GameObject)Instantiate(Resources.Load("Prefabs/NPC/" + aNPCName, typeof(GameObject)));
        aNPC.name = "NPC_" + aNPCName + "_" + ID;
        aNPC.AddComponent<RemotePlayerController>();
        aNPC.transform.position = new Vector3(location[0], location[1], location[2]);
        aNPC.GetComponentInChildren<TextMesh>().text = aNPCName;

        //Add Newly spawned player to Dictionary
        ourNPCDictionary.Add(ID, aNPC);
    }
    public void spawnResource(int ID, String aResourceName, float[] location)
    {
        //Instantiate RemotePlayerObject
        GameObject aResource = (GameObject)Instantiate(Resources.Load("Prefabs/Resources/" + aResourceName, typeof(GameObject)));
        aResource.name = "Resource_" + aResourceName + "_" + ID;
        aResource.AddComponent<RemotePlayerController>();
        aResource.transform.position = new Vector3(location[0], location[1], location[2]);

        //Add Newly spawned player to Dictionary
        ourResourceDictionary.Add(ID, aResource);
    }
    public void despawnResource(int aResourceID)
    {
        Destroy(ourResourceDictionary[aResourceID]);
        ourResourceDictionary.Remove(aResourceID);
    }
    public LocalPlayerController getLPC()
    {
        return this.ourLPC;
    }
    public Dictionary<string, GameObject> getPlayerDictionary()
    {
        return this.ourPlayerDictionary;
    }
    public Dictionary<int, GameObject> getResourceDictionary()
    {
        return this.ourResourceDictionary;
    }
}
