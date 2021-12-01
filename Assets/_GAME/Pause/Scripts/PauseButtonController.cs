using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtonController : MonoBehaviour
{
    public void OnResumeClick()
    {
        LevelManager.isPause = false;
    }

    public void OnQuitMenuClick()
    {
        LevelManager.isPause = false;
        SceneManager.LoadScene("Menu");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
