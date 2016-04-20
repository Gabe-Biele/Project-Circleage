using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameWorld.PlayerActions
{
    class TalkToNPCAction : PlayerAction
    {
        public void PerformAction(GameObject ourSSO)
        {
            RayCastManager ourRCM = ourSSO.GetComponent<RayCastManager>();
            
            /* This is copy pasted from the GatherResourceAction
            string astringID = ourRCM.currentRayCastObject.name.Split('_')[2];

            ISFSObject aSFSObject = new SFSObject();
            aSFSObject.PutInt("ID", Convert.ToInt32(astringID));

            SmartFox SFServer = SmartFoxConnection.Connection;
            SFServer.Send(new ExtensionRequest("GatherResource", aSFSObject));
            */
            Debug.Log("This worked.");

            GameUI ourUI = ourSSO.GetComponent<GameUI>();
            ourUI.activateNPCSpeech("Get this from server later", ourRCM.currentRayCastObject);
        }
    }
}
