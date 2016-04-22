using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnSettlementHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            string aSettlementName = anObjectIn.GetUtfString("Name");
            int ID = anObjectIn.GetInt("ID");
            float[] location = anObjectIn.GetFloatArray("LocationArray");
            int level = anObjectIn.GetInt("CenterNodeLevel");

            GameObject aSettlement = ourGWM.createObject("Prefabs/Settlements/" + aSettlementName + "/" + level.ToString());
            aSettlement.name = "Settlement_" + aSettlementName + "_" + ID;
            aSettlement.transform.position = new Vector3(location[0], location[1], location[2]);
        }
    }
}
