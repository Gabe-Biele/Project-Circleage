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
    class GatherResourceAction : PlayerAction
    {
        public void PerformAction(GameObject ourSSO)
        {
            RayCastManager ourRCM = ourSSO.GetComponent<RayCastManager>();
            string astringID = ourRCM.currentRayCastObject.name.Split('_')[2];

            ISFSObject aSFSObject = new SFSObject();
            aSFSObject.PutInt("ID", Convert.ToInt32(astringID));

            SmartFox SFServer = SmartFoxConnection.Connection;
            SFServer.Send(new ExtensionRequest("GatherResource", aSFSObject));
        }
    }
}
