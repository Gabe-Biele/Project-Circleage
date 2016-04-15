using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public Transform lookAt;
    public Transform camTransform;
    private Camera cam;
    public float ourCameraDragSpeed = 2;
    public bool inCombat = true;
    private float maxDistance = 13.0f;
    private float currentX = 6.0f;
    private float currentY = 12.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 4.0f;


    // Use this for initialization
    void Start ()
    {
        camTransform = transform;
        cam = Camera.main;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, 10.0f, 80.0f);
            
        }
    }

    void LateUpdate()
    {
        //if(Input.GetMouseButton(1))
        //{
        //    float horizontal = Input.GetAxis("Mouse X") * ourCameraDragSpeed;
        //    float vertical = Input.GetAxis("Mouse Y");
        //    target.transform.Rotate(0, horizontal, 0); 
        //    transform.Translate(0, -vertical, 0);
        //}
        //if(target != null)
        //{
        //    transform.LookAt(target.transform);
        //}

        Vector3 dir = new Vector3(0, 0, -maxDistance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.localPosition = rotation * dir;
        camTransform.LookAt(target.transform);
    }

    public void ResetCamera()
    {
        currentX = 6.0f;
        currentY = 12.0f;
    }
}
