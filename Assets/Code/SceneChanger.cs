using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	void Update ()
    {
	    if (Input.GetKeyUp(KeyCode.Escape))
        {
            Screen.showCursor = true;
            Screen.lockCursor = false;

            if (Application.loadedLevel != 0)
                Application.LoadLevel("Main Menu");
            else
                Application.Quit();
        }
	}

    public void Chunk()
    {
        Application.LoadLevel("Chunk");
    }

    public void Inventory()
    {
        Application.LoadLevel("Inventory");
    }

    public void Pathfinding()
    {
        Application.LoadLevel("PathFinding");
    }

    public void Networking()
    {
        Application.LoadLevel("Networking");
    }
}
