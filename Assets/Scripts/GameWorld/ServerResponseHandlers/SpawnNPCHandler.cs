using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class SpawnNPCHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            ourGWM.spawnNPC(anObjectIn.GetUtfString("Name"), anObjectIn.GetFloatArray("Location"));
        }
    }
}
