using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public GameUI theUI;
    public GameObject ourSSO;
    public RayCastManager ourRCM;
    public bool update;

    private float PlayerSpeed = 10;
    private float RotationSpeed = 40;
    private Rigidbody PlayerRB;


    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");

    // Use this for initialization
    void Start () {
        this.MecAnim = this.GetComponentInChildren<Animator>();
        this.PlayerRB = this.GetComponent<Rigidbody>();
    }
	

	// Update is called once per frame
	void Update () {
        if (update)
        {
            if (!theUI.GetchatTBFocus())
            {
                if (Input.GetKeyDown(KeyCode.W)) this.MecAnim.SetBool(RUN_ANIMATION, true);
                if (Input.GetKeyUp(KeyCode.W)) this.MecAnim.SetBool(RUN_ANIMATION, false);

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
                ourSSO = GameObject.Find("SceneScriptsObject");
                RayCastManager ourRCM = ourSSO.GetComponent<RayCastManager>();
                if (Input.GetMouseButtonDown(0) && ourRCM.onNPC)
                {
                    if (ourRCM.currentRayCastObject.tag == "NPC")
                    {
                        ourRCM.currentRayCastObject.transform.parent.GetComponent<NPCController>().takeDamage(1);
                        theUI.activateRayCastLabel(ourRCM.currentRayCastObject);
                    }
                }
            }
        }
    }

}
