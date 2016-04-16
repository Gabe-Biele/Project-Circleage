using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class DespawnPlayerHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            string aCharacterName = anObjectIn.GetUtfString("CharacterName");
            if(aCharacterName == ourGWM.getLPC().GetName())
            {
                return;
            }
            else if(ourGWM.getPlayerDictionary().ContainsKey(aCharacterName))
            {
                ourGWM.despawnRemotePlayer(aCharacterName);
            }
        }
    }
}
