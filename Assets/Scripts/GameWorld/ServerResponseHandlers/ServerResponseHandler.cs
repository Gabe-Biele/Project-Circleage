using Sfs2X.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    interface ServerResponseHandler
    {
        void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM);
    }
}
