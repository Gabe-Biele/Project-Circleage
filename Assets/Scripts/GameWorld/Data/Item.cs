using UnityEngine;
using System.Collections;
using System;

public class Item : MonoBehaviour
{
    private int anID;
    private int anItemID;
    private int aQuantity;
    private string aName;
    private string aDescription;
    private string aLocation;

    public Item(int iD, int itemID, int quantity, string name, string description, string location)
    {
        anID = iD;
        anItemID = itemID;
        aQuantity = quantity;
        aName = name;
        aDescription = description;
        aLocation = location;
    }
    public void updateItemData(int itemID, int quantity, string name, string description, string location)
    {
        anItemID = itemID;
        aQuantity = quantity;
        aName = name;
        aDescription = description;
        aLocation = location;
    }
    public bool inInventory()
    {
        if(aLocation.Split('_')[0] == "Inventory") return true;
        else return false;
    }
    public string getPosition()
    {
        return aLocation.Split('_')[1];
    }
    public int getItemID()
    {
        return anItemID;
    }

    public int getQuantity()
    {
        return aQuantity;
    }

    public string getName()
    {
        return aName;
    }

    public string getDescription()
    {
        return aDescription;
    }
}