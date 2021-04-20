using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_Script : MonoBehaviour
{

    public void LoadScene_PlayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadScene_EndScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadScene_StartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
