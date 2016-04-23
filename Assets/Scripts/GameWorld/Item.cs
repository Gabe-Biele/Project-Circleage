using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    int ID;
    int bagPosition;
    string name;
    string subLocation;

    Item(int itemID, string p_subLocation, int p_bagPosition)
    {
        ID = itemID;
        subLocation = p_subLocation;
        bagPosition = p_bagPosition;
    }
	
}
