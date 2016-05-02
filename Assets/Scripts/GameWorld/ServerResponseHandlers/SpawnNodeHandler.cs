using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnNodeHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            string[] types = anObjectIn.GetUtfStringArray("Type");
            int[] levels = anObjectIn.GetIntArray("Level");
            string[] location = anObjectIn.GetUtfStringArray("Location");

            for(int i = 0; i < levels.Length; i++)
            {
                GameObject aNode;
                if(levels[i] == 0)
                {
                    aNode = ourGWM.createObject("Prefabs/Settlements/WoodSign");
                }
                else
                {
                    aNode = ourGWM.createObject("Prefabs/Settlements/" + types[i] + "/" + levels[i].ToString());
                }
                aNode.name = "Node_" + types[i] + "_" + levels[i];
                aNode.transform.position = new Vector3(Convert.ToSingle(location[i].Split(',')[0]), Convert.ToSingle(location[i].Split(',')[1]), Convert.ToSingle(location[i].Split(',')[2]));
                aNode.transform.rotation = new Quaternion(0, Convert.ToSingle(location[i].Split(',')[3]), 0, 0);
            }
        }
    }
}
