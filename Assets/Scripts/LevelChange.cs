using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange: MonoBehaviour{

    public string main_scene_name;

    public void animationComplete()
    {
        SceneManager.LoadScene(main_scene_name);
    }
}
