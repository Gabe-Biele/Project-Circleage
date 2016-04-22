using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Assets.Scripts.GameWorld.PlayerActions;

public class InputController : MonoBehaviour
{
    private SmartFox SFServer;
    private string PlayerName = "";
    private float PlayerSpeed = 10;
    private float RotationSpeed = 40;
    private Rigidbody PlayerRB;
    public LocalPlayerController ourLPC;
    GameUI theUI;

    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");

    private PlayerAction currentPlayerAction;

    void Start()
    {
        SFServer = SmartFoxConnection.Connection;

        this.PlayerRB = this.GetComponent<Rigidbody>();
        this.MecAnim = this.GetComponentInChildren<Animator>();
        this.ourLPC = this.GetComponentInParent<LocalPlayerController>();
        theUI = (GameUI)FindObjectOfType(typeof(GameUI));

    }

    void Update()
    {
        if (!theUI.GetchatTBFocus())
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                this.MecAnim.SetBool(RUN_ANIMATION, true);
                ISFSObject ObjectIn = new SFSObject();
                ObjectIn.PutFloatArray("Location", ourLPC.GetLocation());
                ObjectIn.PutBool("IsMoving", true);
                SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                this.MecAnim.SetBool(RUN_ANIMATION, false);
                ISFSObject ObjectIn = new SFSObject();
                ObjectIn.PutFloatArray("Location", ourLPC.GetLocation());
                ObjectIn.PutBool("IsMoving", false);
                SFServer.Send(new ExtensionRequest("PositionUpdate", ObjectIn));
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
                    ourLPC.getPlayerAction().performAction(GameObject.Find("SceneScriptsObject"));
                }
            }
        }
    }
}
