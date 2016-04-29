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
    class UseItemAction : PlayerAction
    {
        public void activateRaycastLabel(GameObject anObject, GameUI ourUI)
        {
        }

        public void performAction(GameObject ourSSO)
        {
            Item usedItem = ourSSO.GetComponent<GameUI>().getCurrentInventoryItem();

            ISFSObject aSFSObject = new SFSObject();
            aSFSObject.PutInt("ID", usedItem.getItemID());
            aSFSObject.PutUtfString("Name", usedItem.getName());
            aSFSObject.PutUtfString("Sublocation", usedItem.getLocation());

            SmartFox SFServer = SmartFoxConnection.Connection;
            SFServer.Send(new ExtensionRequest("UseItem", aSFSObject));
        }

        public bool withinMaxDistance(float distance)
        {
            if(distance <= 3) return true;
            else return false;
        }
    }
}
