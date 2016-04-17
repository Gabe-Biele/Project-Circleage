using UnityEngine;
using System.Collections;

public class RemotePlayerController : MonoBehaviour
{
    private float PlayerSpeed = 10;
    private Rigidbody PlayerRB;

    private bool PlayerMoving;
    private Animator MecAnim;
    private static int RUN_ANIMATION = Animator.StringToHash("IsRunning");


    // Use this for initialization
    void Start ()
    {
        this.PlayerMoving = false;
        this.PlayerRB = GetComponent<Rigidbody>();
        this.MecAnim = this.GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(this.PlayerMoving == true)
        {
            this.PlayerRB.MovePosition(transform.position + (transform.forward * Time.deltaTime * PlayerSpeed));
        }
	}
    public void SetPlayerMoving(bool Moving)
    {
        this.PlayerMoving = Moving;
        //Sets the player animation based on moving 
        this.MecAnim.SetBool(RUN_ANIMATION, Moving);
    }
    public void SetRotation(float Rot)
    {

        this.transform.localRotation = Quaternion.Euler(0, Rot, 0);
    }
}
