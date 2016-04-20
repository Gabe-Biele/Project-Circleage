using UnityEngine;
using System.Collections;
using Assets.Scripts.GameWorld.PlayerActions;

public class RayCastManager : MonoBehaviour
{
    public GameUI ourGameUI;
    public GameWorldManager ourGWM;
    public GameObject currentRayCastObject;
    public bool onNPC;

    // Use this for initialization
    void Start ()
    {
        ourGameUI = GameObject.Find("SceneScriptsObject").GetComponent<GameUI>();
        ourGWM = GameObject.Find("SceneScriptsObject").GetComponent<GameWorldManager>();
        onNPC = false;
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
            if(hit.collider.gameObject.tag == "Resource" && currentRayCastObject != hit.collider.gameObject && Vector3.Distance(hit.collider.transform.position, ourGWM.getLPC().GetComponentInParent<Transform>().position) < 2.5)
            {
                setCurrentRayCastObject(hit.collider.gameObject);
                ourGWM.getLPC().setPlayerAction(new GatherResourceAction());
            }
            else if(hit.collider.gameObject.tag == "NPC" && currentRayCastObject != hit.collider.gameObject && Vector3.Distance(hit.collider.transform.position, ourGWM.getLPC().GetComponentInParent<Transform>().position) < 40)
            {
                setCurrentRayCastObject(hit.collider.gameObject);
                onNPC = true;
                Debug.Log("Hit an npc!");
            }
            //If this is just an else, the text flashes.
            if(hit.collider.gameObject.tag != "Resource" && hit.collider.gameObject.tag != "NPC")
            {
                ourGameUI.deactivateRayCastLabel();
                currentRayCastObject = null;
                ourGWM.getLPC().setPlayerAction(null);
                onNPC = false;
            }
        }
    }
    public void setCurrentRayCastObject(GameObject aGameObject)
    {
        currentRayCastObject = aGameObject;
        ourGameUI.activateRayCastLabel(currentRayCastObject);
    }
}
