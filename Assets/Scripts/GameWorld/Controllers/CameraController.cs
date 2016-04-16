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
    private float distance = 13.0f;
    private float currentX = 5.0f;
    private float currentY = 12.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 4.0f;


    // Use this for initialization
    void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            currentY += Input.GetAxis("Mouse Y");
            currentX += Input.GetAxis("Mouse X");
            currentY = Mathf.Clamp(currentY, 10.0f, 80.0f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distance--;
            distance = Mathf.Clamp(distance, 4.0f, 13.0f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            distance++;
            distance = Mathf.Clamp(distance, 4.0f, 13.0f);
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

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.localPosition = rotation * dir;
        if (target != null)
        {
            camTransform.LookAt(target.transform);
        }
    }

    public void ResetCamera()
    {
        if (!Input.GetMouseButton(1))
        {
            currentX = 5.0f;
        }
    }
}