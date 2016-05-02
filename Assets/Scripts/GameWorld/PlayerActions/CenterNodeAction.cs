using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameWorld.PlayerActions
{
    class CenterNodeAction : PlayerAction
    {
        public void activateRaycastLabel(GameObject anObject, GameUI ourUI)
        {
            ourUI.getRayCastLabel().SetActive(true);


            ourUI.getRayCastLabel().GetComponent<Text>().text = anObject.transform.parent.name.Split('_')[1] + "\nPress F to Contribute";
        }

        public void performAction(GameObject ourSSO)
        {
            RayCastManager ourRCM = ourSSO.GetComponent<RayCastManager>();
            string aSettlementID = ourRCM.currentRayCastObject.transform.parent.name.Split('_')[2];

            ISFSObject aSFSObject = new SFSObject();
            aSFSObject.PutInt("ID", Convert.ToInt32(aSettlementID));

            SmartFox SFServer = SmartFoxConnection.Connection;
            SFServer.Send(new ExtensionRequest("CenterNodeInformation", aSFSObject));
        }

        public bool withinMaxDistance(float distance)
        {
            if(distance <= 8) return true;
            else return false;
        }
    }
}
