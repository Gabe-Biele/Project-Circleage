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
            Debug.Log("Hello");
            int[] itemArray = anObjectIn.GetIntArray("IDArray");
            int[] quantityArray = anObjectIn.GetIntArray("QuantityArray");
            string[] locationArray = anObjectIn.GetUtfStringArray("SubLocationArray");
            if (itemArray.Length != quantityArray.Length)
            {
                Debug.Log("Item array and quantity array are not the same size.");
            }
            int itemPosition = 0;
            foreach (int itemID in itemArray)
            {
                for (int i = 0; i < quantityArray[itemPosition]; i++)
                {
                    ourGWM.getLPC().addItem(itemID, locationArray[itemPosition]);
                }
                itemPosition++;
            }


        }
    }
}
