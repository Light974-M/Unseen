using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private GameObject _settings;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnSettingsClick()
    {
        _settings.SetActive(true);
        _menu.SetActive(false);
    }

    public void OnReturnClick()
    {
        _menu.SetActive(true);
        _settings.SetActive(false);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
