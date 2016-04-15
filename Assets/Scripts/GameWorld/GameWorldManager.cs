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

public class GameWorldManager : MonoBehaviour
{
    private SmartFox SFServer;

    private GameObject LocalPlayer;
    private LocalPlayerController OurLPC;

    private Dictionary<string, GameObject> PlayerDictionary = new Dictionary<string, GameObject>();

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
            ObjectIn.PutFloatArray("Location", OurLPC.GetLocation());
            ObjectIn.PutBool("IsMoving", true);
            SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            ISFSObject ObjectIn = new SFSObject();
            ObjectIn.PutFloatArray("Location", OurLPC.GetLocation());
            ObjectIn.PutBool("IsMoving", false);
            SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));
        }
        if(Input.GetMouseButton(1) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            ISFSObject ObjectIn = new SFSObject();
            ObjectIn.PutFloat("Rotation", OurLPC.GetRotation());
            SFServer.Send(new ExtensionRequest("RotationUpdate", ObjectIn));
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
            ISFSObject ObjectIn = (SFSObject)evt.Params["params"];
            if(ResponseType == "SpawnPlayer")
            {
                Debug.Log(ObjectIn.GetBool("IsLocal"));
                if(ObjectIn.GetBool("IsLocal")) 
                {
                    SpawnLocalPlayer(ObjectIn.GetUtfString("CharacterName"), ObjectIn.GetFloatArray("Location"));
                }
                else if(!ObjectIn.GetBool("IsLocal"))
                {
                    float[] LocationArray = ObjectIn.GetFloatArray("Location");
                    float Rotation = ObjectIn.GetFloat("Rotation");
                    string aCharacterName = ObjectIn.GetUtfString("CharacterName");
                    SpawnRemotePlayer(aCharacterName, LocationArray, Rotation);
                }
            }
            if(ResponseType == "DespawnPlayer")
            {
                string aCharacterName = ObjectIn.GetUtfString("CharacterName");
                if(aCharacterName == this.OurLPC.GetName())
                {
                    return;
                }
                else if(PlayerDictionary.ContainsKey(aCharacterName))
                {
                    Destroy(GameObject.Find("GameCharacter_" + aCharacterName));
                    PlayerDictionary.Remove(aCharacterName);
                }
            }
            if(ResponseType == "PositionUpdate")
            {
                string aCharacterName = ObjectIn.GetUtfString("CharacterName");
                if(aCharacterName == this.OurLPC.GetName())
                {
                    return;
                }
                else if(PlayerDictionary.ContainsKey(aCharacterName))
                {
                    float[] LocationArray = ObjectIn.GetFloatArray("Location");
                    bool IsMoving = ObjectIn.GetBool("IsMoving");
                    //Debug.Log("X: " + LocationArray[0] + "      Y: " + LocationArray[1] + "      Z: " + LocationArray[2]);
                    PlayerDictionary[aCharacterName].GetComponent<RemotePlayerController>().SetPlayerMoving(IsMoving);
                    PlayerDictionary[aCharacterName].transform.position = new Vector3(LocationArray[0], LocationArray[1], LocationArray[2]);
                }
            }
            if(ResponseType == "RotationUpdate")
            {
                string aCharacterName = ObjectIn.GetUtfString("CharacterName");
                if(aCharacterName == this.OurLPC.GetName())
                {
                    return;
                }
                else if(PlayerDictionary.ContainsKey(aCharacterName))
                {
                    float Rotation = ObjectIn.GetFloat("Rotation");
                    Debug.Log(Rotation);
                    PlayerDictionary[aCharacterName].GetComponent<RemotePlayerController>().SetRotation(Rotation);
                }
            }
            /*if(ResponseType == "MovementUpdate")
            {
                string aCharacterName = ObjectIn.GetUtfString("CharacterName");
                if(aCharacterName == this.OurLPC.GetName())
                {
                    Debug.Log("Moving...");
                    float[] LocationArray = ObjectIn.GetFloatArray("Location");
                    Debug.Log("X: " + LocationArray[0] + "      Y: " + LocationArray[1] + "      Z: " + LocationArray[2]);
                    this.OurLPC.MoveToDestination();
                }
                else if(PlayerDictionary.ContainsKey(aCharacterName))
                {
                    //
                }
            }*/
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
    private void SpawnRemotePlayer(String aCharacterName, float[] LocationArray, float Rotation)
    {
        //Instantiate RemotePlayerObject
        GameObject aRemotePlayer = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerBasic", typeof(GameObject)));
        aRemotePlayer.name = "GameCharacter_" + aCharacterName;
        aRemotePlayer.AddComponent<RemotePlayerController>();
        aRemotePlayer.transform.position = new Vector3(LocationArray[0], LocationArray[1], LocationArray[2]);
        aRemotePlayer.GetComponent<RemotePlayerController>().SetRotation(Rotation);
        aRemotePlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;

        //Add Newly spawned player to Dictionary
        PlayerDictionary.Add(aCharacterName, aRemotePlayer);
    }
    private void SpawnLocalPlayer(String aCharacterName, float[] Loc)
    {
        //Debug.Log(Loc[0] + "      " + Loc[1] + "      " + Loc[2]);
        // Lets spawn our local player model
        LocalPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/PlayerBasic", typeof(GameObject)));
        LocalPlayer.transform.position = new Vector3(Loc[0], Loc[1], Loc[2]);
        LocalPlayer.transform.rotation = Quaternion.identity;

        // Since this is the local player, lets add a controller and fix the camera
        LocalPlayer.AddComponent<LocalPlayerController>();
        OurLPC = LocalPlayer.GetComponent<LocalPlayerController>();
        OurLPC.SetName(aCharacterName);
        LocalPlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;
        Camera.main.transform.parent = LocalPlayer.transform;
        Camera.main.GetComponent<CameraController>().target = LocalPlayer;
        Camera.main.transform.localPosition = new Vector3(0, 5, -12);
    }

}
