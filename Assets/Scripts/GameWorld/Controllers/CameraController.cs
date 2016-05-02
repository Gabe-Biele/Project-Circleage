using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public Vector3 cameraPosition;

    public bool CursorMode;

    public float deltaX;
    public float sensitivityX = 6.0f;
    public float deltaY;
    public float sensitivityY = 3.0f;

    // Use this for initialization
    void Start ()
    {
        cameraPosition = new Vector3(0, 4, -8);

        CursorMode = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            flipCursorVisible();
        }
        if(cameraTarget != null)
        {
            if(!CursorMode)
            {
                deltaX = Input.GetAxis("Mouse X") * sensitivityX;
                deltaY = Input.GetAxis("Mouse Y") * sensitivityY;
                cameraTarget.RotateAround(cameraTarget.parent.position, Vector3.up, deltaX);
                cameraTarget.Rotate(Vector3.left, deltaY);
                if(cameraTarget.localEulerAngles.x > 60 && cameraTarget.localEulerAngles.x < 230)
                {
                    cameraTarget.localRotation = Quaternion.Euler(60, cameraTarget.localEulerAngles.y, cameraTarget.localEulerAngles.z);
                }
                if(cameraTarget.localEulerAngles.x < 345 && cameraTarget.localEulerAngles.x > 230)
                {
                    cameraTarget.localRotation = Quaternion.Euler(345, cameraTarget.localEulerAngles.y, cameraTarget.localEulerAngles.z);
                }

                if(Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    float desiredDistance = transform.localPosition.z - Input.GetAxis("Mouse ScrollWheel") * 3;
                    desiredDistance = Mathf.Clamp(desiredDistance, -22, -6);
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, desiredDistance);
                }
            }
            this.transform.LookAt(cameraTarget);
        }
    }
    public void ResetCamera()
    {
        cameraPosition = new Vector3(0, cameraPosition.y, cameraPosition.z);
    }
    public void setTarget(GameObject cameraPoint)
    {
        cameraTarget = cameraPoint.transform;
        transform.localPosition = new Vector3(0, 2, -8);
    }
    public void setCursorVisible(bool cm)
    {
        if(cm)
        {
            CursorMode = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(!cm)
        {
            CursorMode = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void flipCursorVisible()
    {
        setCursorVisible(!CursorMode);
    }
}
