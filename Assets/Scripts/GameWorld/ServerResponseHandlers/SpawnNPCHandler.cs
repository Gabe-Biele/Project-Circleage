using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnNPCHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            string aNPCName = anObjectIn.GetUtfString("Name");
            int ID = anObjectIn.GetInt("ID");
            float[] location = anObjectIn.GetFloatArray("Location");

            GameObject aNPC = ourGWM.createObject("Prefabs/NPC/" + aNPCName);
            aNPC.name = "NPC_" + aNPCName + "_" + ID;
            aNPC.AddComponent<RemotePlayerController>();
            aNPC.transform.position = new Vector3(location[0], location[1], location[2]);
            aNPC.GetComponentInChildren<TextMesh>().text = aNPCName;

            //Add Newly spawned player to Dictionary
            ourGWM.getNPCDictionary().Add(ID, aNPC);
        }
    }
}
