﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class CenterNodeInformationHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {
            GameObject aContributionPanel = ourGWM.createObject("UI/ContributionPanel");
            aContributionPanel.name = "ContributionPanel";
            aContributionPanel.transform.SetParent(GameObject.Find("UICanvas").transform);
            aContributionPanel.transform.localPosition = new Vector3(0, 0, 0);
            aContributionPanel.transform.FindChild("ExitButton").GetComponent<Button>().onClick.AddListener(() => GameObject.Find("SceneScriptsObject").GetComponent<GameUI>().contributionExitButton_Clicked());
            aContributionPanel.transform.FindChild("NameLabel").GetComponent<Text>().text = anObjectIn.GetUtfString("Name");
            aContributionPanel.transform.FindChild("LevelLabel").GetComponent<Text>().text = "(Level " + anObjectIn.GetInt("Level") + ")";
            aContributionPanel.transform.FindChild("CurrentContributionLabel").GetComponent<Text>().text = anObjectIn.GetInt("Contribution").ToString();
            aContributionPanel.transform.FindChild("ContributionCapTotalLabel").GetComponent<Text>().text = anObjectIn.GetInt("ContributionCap").ToString();
        }
    }
}