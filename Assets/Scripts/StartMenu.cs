using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    public Animator animator;
    public Button continueButton;

    public void gameNew()
    {
        Debug.Log("Ran game new");
        PlayerPrefs.SetInt("Level", 1);
        animator.SetTrigger("Level_Change");
    }

    public void gameContinue()
    {
        animator.SetTrigger("Level_Change");
    }

    public void gameQuit()
    {
        Debug.Log("ran game quit");
        Application.Quit();
    }

    // Update is called once per frame
    void Start () {
        if(PlayerPrefs.GetInt("Level", -1) == -1)
        {
            continueButton.enabled = false;
        }
        Screen.SetResolution(600, 800, false);
	}

    private void Update()
    {
        if (PlayerPrefs.GetInt("Level", -1) != 1 || PlayerPrefs.GetInt("Level", -1) != -1)
        {
            continueButton.enabled = true;
        }
    }
}
