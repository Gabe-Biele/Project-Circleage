using UnityEngine;
using System.Collections;
using Assets.Scripts.GameWorld.PlayerActions;
using System.Collections.Generic;

public class RayCastManager : MonoBehaviour
{
    public GameUI ourGameUI;
    public GameWorldManager ourGWM;
    public GameObject currentRayCastObject;
    public Dictionary<string, PlayerAction> ourPlayerActionDictionary = new Dictionary<string, PlayerAction>();

    // Use this for initialization
    void Start ()
    {
        ourGameUI = GameObject.Find("SceneScriptsObject").GetComponent<GameUI>();
        ourGWM = GameObject.Find("SceneScriptsObject").GetComponent<GameWorldManager>();

        ourPlayerActionDictionary.Add("Resource", new GatherResourceAction());
        ourPlayerActionDictionary.Add("Center Node", new CenterNodeAction());
        ourPlayerActionDictionary.Add("Use Item", new UseItemAction());
        ourPlayerActionDictionary.Add("ContributeCenterNode", new ContributeCenterNodeAction());
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
            if(ourPlayerActionDictionary.ContainsKey(hit.collider.gameObject.tag) && hit.collider.gameObject != currentRayCastObject)
            {
                PlayerAction anAction = ourPlayerActionDictionary[hit.collider.gameObject.tag];
                if(anAction.withinMaxDistance(Vector3.Distance(hit.collider.transform.position, ourGWM.getLPC().GetComponentInParent<Transform>().position)))
                {
                    setCurrentRayCastObject(hit.collider.gameObject);
                    anAction.activateRaycastLabel(hit.collider.gameObject, ourGameUI);
                    ourGWM.getLPC().setPlayerAction(anAction);
                }
            }
            if(!ourPlayerActionDictionary.ContainsKey(hit.collider.gameObject.tag))
            {
                ourGameUI.deactivateRayCastLabel();
                currentRayCastObject = null;
                ourGWM.getLPC().setPlayerAction(null);
            }
        }
    }
    public void setCurrentRayCastObject(GameObject aGameObject)
    {
        currentRayCastObject = aGameObject;
    }
}
