using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {


    private int ID;

    private float maxHealth;
    private float currentHealth;

	// Use this for initialization
	void Start () {

        //Get these healths from the server eventually
        maxHealth = 34;
        currentHealth = 34;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void takeDamage(int amount)
    {
        //This stuff needs server verification in the future
        //Actually, this shouldnt go here
        //Player sends spell command to server -> server verifies that they can cast spell -> server applies damage -> player gets new health
        //Maybe this happens on both sides to lessen appearance of lag?

        currentHealth = currentHealth - amount;

        if (currentHealth<1)
        {
            //Destroy this object
        }

    }
    public string healthString()
    {
        return currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public int getID()
    {
        return ID;
    }
}
