using UnityEngine;
using System.Collections;
using Sfs2X;
using UnityEngine.SceneManagement;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class GameUI : MonoBehaviour
{
    private SmartFox SFServer;

    // Use this for initialization
    void Start ()
    {
        if(SmartFoxConnection.Connection == null)
        {
            SceneManager.LoadScene("Login");
            return;
        }
        SFServer = SmartFoxConnection.Connection;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void QuitButton_Clicked()
    {
        Application.Quit();
    }
    public void OnApplicationQuit()
    {
        ISFSObject ObjectIn = new SFSObject();
        ObjectIn.PutUtfString("junk", "junkval");
        SFServer.Send(new ExtensionRequest("DespawnPlayer", ObjectIn));
        SFServer.Disconnect();
        SFServer.RemoveAllEventListeners();
        Debug.Log("Quitting...");
    }
}
