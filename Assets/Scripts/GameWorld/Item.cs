using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{

    public int ID;
    int bagPosition;
    public int quantity;
    string itemName;
    string itemDescription;
    string subLocation;
    public string picture;

    public Item(int itemID, string p_subLocation, int p_bagPosition, int p_quantity)
    {
        ID = itemID;
        subLocation = p_subLocation;
        bagPosition = p_bagPosition;
        quantity = p_quantity;
    }
    
    public void setName(string itName)
    {
        itemName = itName;
    }
    public void setDescription(string itDesc)
    {
        itemDescription = itDesc;
    }
}