using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameWorld.PlayerActions
{
    public interface PlayerAction
    {
        void performAction(GameObject ourSSO);
        void activateRaycastLabel(GameObject anObject, GameUI ourUI);
        bool withinMaxDistance(float distance);
    }
}
