using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayerController : MonoBehaviour
{
    private float PlayerSpeed = 10;
    private float RotationSpeed = 40;
    private Rigidbody PlayerRB;


    void Start()
    {
        this.PlayerRB = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.PlayerRB.MovePosition(transform.position + (transform.forward * Time.deltaTime * PlayerSpeed));
        }

        // Left/right makes player model rotate around own axis
        float rotation = Input.GetAxis("Horizontal");
        if(rotation != 0)
        {
            this.transform.Rotate(Vector3.up, rotation * Time.deltaTime * RotationSpeed);
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
}
