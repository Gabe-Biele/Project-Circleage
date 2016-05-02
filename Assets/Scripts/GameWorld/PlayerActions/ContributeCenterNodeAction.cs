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
    class ContributeCenterNodeAction : PlayerAction
    {
        public void activateRaycastLabel(GameObject anObject, GameUI ourUI)
        {
        }

        public void performAction(GameObject ourSSO)
        {
            InputField ContributeTB = GameObject.Find("ContributionPanel").transform.FindChild("ContributeTB").GetComponent<InputField>();
            int quantity;
            bool isInt = int.TryParse(ContributeTB.GetComponentsInChildren<Text>()[1].text, out quantity);
            if(isInt)
            {
                ISFSObject aSFSObject = new SFSObject();
                aSFSObject.PutInt("Quantity", quantity);
                aSFSObject.PutUtfString("SettlementName", GameObject.Find("ContributionPanel").transform.FindChild("NameLabel").GetComponent<Text>().text);
                SmartFox SFServer = SmartFoxConnection.Connection;
                SFServer.Send(new ExtensionRequest("CenterNodeContribute", aSFSObject));

                //Close Panel
                ourSSO.GetComponent<GameWorldManager>().destroyObject("ContributionPanel");

                //Switch Cursor Mode
                Camera.main.GetComponent<CameraController>().setCursorVisible(false);
            }
        }

        public bool withinMaxDistance(float distance)
        {
            if(distance <= 3) return true;
            else return false;
        }
    }
}
