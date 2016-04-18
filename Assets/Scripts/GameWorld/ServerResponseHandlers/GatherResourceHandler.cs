﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class GatherResourceHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            if(anObjectIn.GetBool("Gathered"))
            {
                ourGWM.despawnResource(anObjectIn.GetInt("ID"));
            }
        }
    }
}