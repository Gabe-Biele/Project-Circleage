using UnityEngine;
using System.Collections;

public class RayCastManager : MonoBehaviour
{
    public GameUI ourGameUI;
    public GameObject currentRayCastObject;

    // Use this for initialization
    void Start ()
    {
        ourGameUI = GameObject.Find("SceneScriptsObject").GetComponent<GameUI>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 20))
        {
            if(hit.collider.gameObject.tag == "Resource" && currentRayCastObject != hit.collider.gameObject)
            {
                setCurrentRayCastObject(hit.collider.gameObject);
            }
            if(hit.collider.gameObject.tag != "Resource")
            {
                ourGameUI.deactivateRayCastLabel();
                currentRayCastObject = null;
            }
        }
    }
    public void setCurrentRayCastObject(GameObject aGameObject)
    {
        currentRayCastObject = aGameObject;
        ourGameUI.activateRayCastLabel(currentRayCastObject);
    }
}
