using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class PositionUpdateHandler : ServerResponseHandler
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
                float[] LocationArray = anObjectIn.GetFloatArray("Location");
                bool IsMoving = anObjectIn.GetBool("IsMoving");
                ourGWM.getPlayerDictionary()[aCharacterName].GetComponent<RemotePlayerController>().SetPlayerMoving(IsMoving);
                ourGWM.getPlayerDictionary()[aCharacterName].transform.position = new Vector3(LocationArray[0], LocationArray[1], LocationArray[2]);
            }
        }
    }
}
