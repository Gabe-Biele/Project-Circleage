using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Assets.Scripts.GameWorld.ServerResponseHandlers
{
    class InventoryUpdateHandler : ServerResponseHandler
    {

        public void HandleResponse(ISFSObject anObjectIn, GameWorldManager ourGWM)
        {


            Debug.Log("Inventory Update Recieved");
            int[] iDArray = anObjectIn.GetIntArray("IDArray");
            int[] itemArray = anObjectIn.GetIntArray("ItemIDArray");
            int[] quantityArray = anObjectIn.GetIntArray("QuantityArray");
            string[] nameArray = anObjectIn.GetUtfStringArray("NameArray");
            string[] descriptionArray = anObjectIn.GetUtfStringArray("DescriptionArray");
            string[] locationArray = anObjectIn.GetUtfStringArray("SubLocationArray");

            bool reopenInventory = false;
            if(GameObject.Find("InventoryPanel") != null)
            {
                GameObject.Find("SceneScriptsObject").GetComponent<GameUI>().openInventory();
                reopenInventory = true;
            }

            if(itemArray.Length != quantityArray.Length)
            {
                Debug.Log("Item array and quantity array are not the same size.");
            }
            int iPos = 0;
            foreach (int ID in iDArray)
            {
                //If the inventory already has this item in it, simply update its data.
                if(ourGWM.getLPC().getInventory().ContainsKey(ID))
                {
                    ourGWM.getLPC().getInventory()[ID].updateItemData(itemArray[iPos], quantityArray[iPos], nameArray[iPos], descriptionArray[iPos], locationArray[iPos]);
                }
                else if(!ourGWM.getLPC().getInventory().ContainsKey(ID))
                {
                    ourGWM.getLPC().getInventory().Add(ID, new Item(ID, itemArray[iPos], quantityArray[iPos], nameArray[iPos], descriptionArray[iPos], locationArray[iPos]));
                }
                iPos++;
            }
            if(reopenInventory)
            {
                GameObject.Find("SceneScriptsObject").GetComponent<GameUI>().openInventory();
            }
        }
    }
}
