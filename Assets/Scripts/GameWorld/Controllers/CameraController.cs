using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public Vector3 cameraPosition;

    public bool InCombat = true;

    public float deltaX;
    public float sensitivityX = 5.0f;
    public float deltaY;
    public float sensitivityY = 3.0f;

    // Use this for initialization
    void Start ()
    {
        cameraPosition = new Vector3(0, 4, -8);
	}
	
    void LateUpdate()
    {
        if(cameraTarget != null)
        {
            if(InCombat && !Input.GetKey(KeyCode.LeftAlt))
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
}
