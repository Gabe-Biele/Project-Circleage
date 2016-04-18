using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public Vector3 cameraPosition;

    public bool InCombat;

    public float deltaX;
    public float sensitivityX = 6.0f;
    public float deltaY;
    public float sensitivityY = 3.0f;

    // Use this for initialization
    void Start ()
    {
        cameraPosition = new Vector3(0, 4, -8);

        InCombat = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void LateUpdate()
    {
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            setCombatMode(InCombat);
        }
        if(cameraTarget != null)
        {
            if(InCombat)
            {
                deltaX = Input.GetAxis("Mouse X");
                cameraPosition = Quaternion.AngleAxis(deltaX * sensitivityX, Vector3.up) * cameraPosition;
                deltaY = Input.GetAxis("Mouse Y");
                cameraPosition = Quaternion.AngleAxis(deltaY * sensitivityY, Vector3.left) * cameraPosition;
                Vector3 proposedLocalPosition = cameraTarget.localPosition + cameraPosition;
                if(proposedLocalPosition.y < 0.2f)
                {
                    proposedLocalPosition = new Vector3(proposedLocalPosition.x, 0.2f, proposedLocalPosition.z);
                }
                this.transform.localPosition = proposedLocalPosition;
            }
            this.transform.LookAt(cameraTarget);
        }
    }
    public void ResetCamera()
    {
        cameraPosition = new Vector3(0, cameraPosition.y, -8);
    }
    public void setTarget(GameObject cameraPoint)
    {
        cameraTarget = cameraPoint.transform;
    }
    public void setCombatMode(bool cm)
    {
        if(cm)
        {
            InCombat = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(!cm)
        {
            InCombat = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
