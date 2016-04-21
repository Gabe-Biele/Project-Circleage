using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnResourceHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            string aResourceName = anObjectIn.GetUtfString("Name");
            int ID = anObjectIn.GetInt("ID");
            float[] location = anObjectIn.GetFloatArray("Location");

            GameObject aResource = ourGWM.createObject("Prefabs/Resources/" + aResourceName);
            aResource.name = "Resource_" + aResourceName + "_" + ID;
            aResource.transform.position = new Vector3(location[0], location[1], location[2]);

            //Add Newly spawned player to Dictionary
            ourGWM.getResourceDictionary().Add(ID, aResource);
        }
    }
}
