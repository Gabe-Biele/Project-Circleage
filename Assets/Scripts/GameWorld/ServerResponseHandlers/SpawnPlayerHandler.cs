﻿using System;
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
            Debug.Log("Server spawning player.");
            float[] locationArray = anObjectIn.GetFloatArray("Location");
            float aRotation = anObjectIn.GetFloat("Rotation");
            string aCharacterName = anObjectIn.GetUtfString("CharacterName");

            if(anObjectIn.GetBool("IsLocal"))
            {
                GameObject aLocalPlayer = ourGWM.createObject("Prefabs/Player/PlayerBasic");
                aLocalPlayer.transform.position = new Vector3(locationArray[0], locationArray[1], locationArray[2]);
                aLocalPlayer.transform.rotation = Quaternion.identity;

                // Since this is the local player, lets add a controller and fix the camera
                aLocalPlayer.AddComponent<LocalPlayerController>();
                aLocalPlayer.AddComponent<InputController>();
                ourGWM.setLPC(aLocalPlayer.GetComponent<LocalPlayerController>());
                ourGWM.getLPC().SetName(aCharacterName);
                aLocalPlayer.GetComponentInChildren<TextMesh>().text = aCharacterName;


                GameObject cameraAttach = new GameObject();
                cameraAttach.transform.parent = aLocalPlayer.transform;
                cameraAttach.transform.localPosition = new Vector3(1f, 2.5f, 1.0f);
                cameraAttach.name = "Camera Target";
                Camera.main.transform.parent = cameraAttach.transform;
                Camera.main.GetComponent<CameraController>().setTarget(cameraAttach);
                Camera.main.GetComponent<CameraController>().setCursorVisible(false);
            }
            else if(!anObjectIn.GetBool("IsLocal"))
            {
                GameObject aRemotePlayer = ourGWM.createObject("Prefabs/Player/PlayerBasic");

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
