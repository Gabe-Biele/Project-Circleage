using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Assets.Scripts.GameWorld.PlayerActions;

public class InputController : MonoBehaviour {

    public GameUI theUI;
    public GameObject ourSSO;
    public RayCastManager ourRCM;
    public bool update;

    private float PlayerSpeed = 10;
    private float RotationSpeed = 40;
    private Rigidbody PlayerRB;

    public LocalPlayerController ourLPC;
    private SmartFox SFServer;


    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");

    // Use this for initialization
    void Start () {
        this.MecAnim = this.GetComponentInChildren<Animator>();
        this.PlayerRB = this.GetComponent<Rigidbody>();
        SFServer = SmartFoxConnection.Connection;
    }

    void FixedUpdate()
    {
        if (SFServer != null)
        {
            SFServer.ProcessEvents();
        }
    }


    // Update is called once per frame
    void Update () {
        if (update)
        {
            if (!theUI.GetchatTBFocus())
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    ISFSObject ObjectIn = new SFSObject();
                    ObjectIn.PutFloatArray("Location", ourLPC.GetLocation());
                    ObjectIn.PutBool("IsMoving", true);
                    SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));

                    this.MecAnim.SetBool(RUN_ANIMATION, true);
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    ISFSObject ObjectIn = new SFSObject();
                    ObjectIn.PutFloatArray("Location", ourLPC.GetLocation());
                    ObjectIn.PutBool("IsMoving", false);
                    SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));

                    this.MecAnim.SetBool(RUN_ANIMATION, false);
                }
                if (Input.GetKey(KeyCode.W) && Input.GetAxis("Mouse X") != 0)
                {
                    ISFSObject ObjectIn = new SFSObject();
                    ObjectIn.PutFloat("Rotation", ourLPC.GetRotation());
                    SFServer.Send(new ExtensionRequest("RotationUpdate", ObjectIn));
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (ourLPC.getPlayerAction() != null)
                    {
                        ourLPC.getPlayerAction().PerformAction(GameObject.Find("SceneScriptsObject"));
                    }
                }

                if (Input.GetKey(KeyCode.W))
                {
                    CameraController cameraControllerObj = (CameraController)Camera.main.GetComponent("CameraController");
                    cameraControllerObj.setCursorVisible(false);
                    this.PlayerRB.transform.Rotate(0, Camera.main.transform.localRotation.eulerAngles.y, 0);
                    cameraControllerObj.ResetCamera();
                    this.PlayerRB.MovePosition(transform.position + (transform.forward * Time.deltaTime * PlayerSpeed));
                }

                // Left/right makes player model rotate around own axis
                float rotation = Input.GetAxis("Horizontal");
                if (rotation != 0)
                {
                    this.transform.Rotate(Vector3.up, rotation * Time.deltaTime * RotationSpeed);
                }
                //TODO fill in stuff here
                if (Input.GetMouseButtonDown(0) && ourRCM.onNPC)
                {
                    if (ourRCM.currentRayCastObject.tag == "NPC")
                    {
                        ISFSObject ObjectIn = new SFSObject();
                        ObjectIn.PutInt("SpellID", 1);
                        ObjectIn.PutInt("Target", ourRCM.currentRayCastObject.transform.parent.GetComponent<NPCController>().getID());
                        SFServer.Send(new ExtensionRequest("Attack", ObjectIn));
                        ourRCM.currentRayCastObject.transform.parent.GetComponent<NPCController>().takeDamage(1);
                        theUI.activateRayCastLabel(ourRCM.currentRayCastObject);
                    }
                }
            }
        }
    }

}
