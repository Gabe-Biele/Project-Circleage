using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class ProcessChatHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {

            GameUI ourGameUI = GameObject.Find("SceneScriptsObject").GetComponent<GameUI>();
            ourGameUI.processChat(anObjectIn.GetUtfString("ChatText"));
        }
    }
}
