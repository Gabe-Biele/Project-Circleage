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

    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");

    /*private float[] Destination;
    private Vector3 DestinationPosition;
    private Quaternion DestinationRotation;
    private bool IsMoving = false;*/

    void Start()
    {
        SFServer = SmartFoxConnection.Connection;

        this.PlayerRB = this.GetComponent<Rigidbody>();
        this.MecAnim = this.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            if(this.UpdateDestination())
            {
                ISFSObject ObjectIn = new SFSObject();
                ObjectIn.PutFloatArray("Destination", Destination);
                SFServer.Send(new ExtensionRequest("MovementUpdate", ObjectIn));
            }
        }
        if(IsMoving)
        {
            this.MecAnim.SetBool(RUN_ANIMATION, true);
            transform.rotation = Quaternion.Slerp(transform.rotation, DestinationRotation, Time.deltaTime * 10);
            this.PlayerRB.velocity = (DestinationPosition - transform.position).normalized * PlayerSpeed;
            if(Vector3.Distance(this.transform.position, DestinationPosition) < 1)
            {
                this.MecAnim.SetBool(RUN_ANIMATION, false);
                IsMoving = false;
            }
        }*/

        if(Input.GetKeyDown(KeyCode.W)) this.MecAnim.SetBool(RUN_ANIMATION, true);
        if(Input.GetKeyUp(KeyCode.W)) this.MecAnim.SetBool(RUN_ANIMATION, false);

        if(Input.GetKey(KeyCode.W))
        {
            CameraController cameraControllerObj= (CameraController) Camera.main.GetComponent("CameraController");
            this.PlayerRB.transform.Rotate(0, Camera.main.transform.localRotation.eulerAngles.y-5, 0);
            cameraControllerObj.ResetCamera();
            this.PlayerRB.MovePosition(transform.position + (transform.forward * Time.deltaTime * PlayerSpeed));
            
        }

        // Left/right makes player model rotate around own axis
        float rotation = Input.GetAxis("Horizontal");
        if(rotation != 0)
        {
            this.transform.Rotate(Vector3.up, rotation * Time.deltaTime * RotationSpeed);
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
    /*private bool UpdateDestination()
    {
        RaycastHit ourRaycastTarget;
        Ray ourRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ourRay, out ourRaycastTarget, 1000))
        {
            float[] DesiredLocation = { ourRaycastTarget.point.x, ourRaycastTarget.point.y, ourRaycastTarget.point.z };
            Destination = DesiredLocation;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetDestination(float[] Destin)
    {
        this.Destination = Destin;
    }
    public void MoveToDestination()
    {
        DestinationPosition = new Vector3(Destination[0], Destination[1], Destination[2]);
        DestinationRotation = Quaternion.LookRotation(DestinationPosition - transform.position, Vector3.forward);

        DestinationRotation.x = 0f;
        DestinationRotation.z = 0f;

        IsMoving = true;
    }*/
}
