using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    private Rocket player;

	void Update () {
        //This will see if we are in developer mode or not
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
	}

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            player = GameObject.FindObjectOfType<Rocket>();
            player.collisionsDisabled = !player.collisionsDisabled;
        }
    }
}
