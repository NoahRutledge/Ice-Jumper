using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

    public string main_scene_name;

    public void gameStart()
    {
        SceneManager.LoadScene(main_scene_name);
    }

    public void gameQuit()
    {
        Debug.Log("ran game quit");
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
