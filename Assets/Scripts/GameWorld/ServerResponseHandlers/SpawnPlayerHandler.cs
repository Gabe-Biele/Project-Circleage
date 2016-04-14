using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnPlayerHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            if(anObjectIn.GetBool("IsLocal"))
            {
                ourGWM.spawnLocalPlayer(anObjectIn.GetUtfString("CharacterName"), anObjectIn.GetFloatArray("Location"));
            }
            else if(!anObjectIn.GetBool("IsLocal"))
            {
                float[] LocationArray = anObjectIn.GetFloatArray("Location");
                float Rotation = anObjectIn.GetFloat("Rotation");
                string aCharacterName = anObjectIn.GetUtfString("CharacterName");
                ourGWM.spawnRemotePlayer(aCharacterName, LocationArray, Rotation);
            }
        }
    }
}
