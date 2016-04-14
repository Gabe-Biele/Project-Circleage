using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float ourCameraDragSpeed = 2;
    public bool inCombat = true;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    void LateUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X") * ourCameraDragSpeed;
            float vertical = Input.GetAxis("Mouse Y");
            target.transform.Rotate(0, horizontal, 0);
            transform.Translate(0, -vertical, 0);
        }
        if(target != null)
        {
            transform.LookAt(target.transform);
        }
    }
}
