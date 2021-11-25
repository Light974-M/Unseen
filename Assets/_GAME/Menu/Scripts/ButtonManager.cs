using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnSettingsClick()
    {

    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnReturnClick()
    {

    }
}
