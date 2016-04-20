using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnPlayerHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            float[] locationArray = anObjectIn.GetFloatArray("Location");
            float aRotation = anObjectIn.GetFloat("Rotation");
            string aCharacterName = anObjectIn.GetUtfString("CharacterName");

            if(anObjectIn.GetBool("IsLocal"))
            {
                GameObject aLocalPlayer = ourGWM.createObject("Prefabs/PlayerBasic");
                aLocalPlayer.transform.position = new Vector3(locationArray[0], locationArray[1], locationArray[2]);
                aLocalPlayer.transform.rotation = Quaternion.identity;

                // Since this is the local player, lets add a controller and fix the camera
                aLocalPlayer.AddComponent<LocalPlayerController>();
                ourGWM.setLPC(aLocalPlayer.GetComponent<LocalPlayerController>());
                ourGWM.getLPC().SetName(aCharacterName);
                aLocalPlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;
                Camera.main.transform.parent = aLocalPlayer.transform;
                GameObject cameraAttach = new GameObject();
                cameraAttach.transform.parent = aLocalPlayer.transform;
                cameraAttach.transform.localPosition = new Vector3(1f, 2.5f, 1.0f);
                Camera.main.GetComponent<CameraController>().setTarget(cameraAttach);
                Camera.main.GetComponent<CameraController>().setCursorVisible(false);
            }
            else if(!anObjectIn.GetBool("IsLocal"))
            {
                GameObject aRemotePlayer = ourGWM.createObject("Prefabs/PlayerBasic");

                aRemotePlayer.name = "GameCharacter_" + aCharacterName;
                aRemotePlayer.AddComponent<RemotePlayerController>();
                aRemotePlayer.transform.position = new Vector3(locationArray[0], locationArray[1], locationArray[2]);
                aRemotePlayer.GetComponent<RemotePlayerController>().SetRotation(aRotation);
                aRemotePlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;

                //Add Newly spawned player to Dictionary
                ourGWM.getPlayerDictionary().Add(aCharacterName, aRemotePlayer);
            }
        }
    }
}
