using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LocalPlayerController : MonoBehaviour
{
    private SmartFox SFServer;
    private string PlayerName = "";
    private float PlayerSpeed = 10;
    private float RotationSpeed = 40;
    private Rigidbody PlayerRB;
    GameUI theUI;

    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");

    void Start()
    {
        SFServer = SmartFoxConnection.Connection;

        this.PlayerRB = this.GetComponent<Rigidbody>();
        this.MecAnim = this.GetComponentInChildren<Animator>();
        theUI = (GameUI)FindObjectOfType(typeof(GameUI));

    }

    void Update()
    {
        if (!theUI.GetchatTBFocus())
        {
            if (Input.GetKeyDown(KeyCode.W)) this.MecAnim.SetBool(RUN_ANIMATION, true);
            if (Input.GetKeyUp(KeyCode.W)) this.MecAnim.SetBool(RUN_ANIMATION, false);

            if (Input.GetKey(KeyCode.W) && !theUI.GetchatTBFocus())
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
        }
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
}
