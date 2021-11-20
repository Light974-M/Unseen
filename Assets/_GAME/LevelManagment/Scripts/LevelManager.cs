using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool isPause = false;
    private double gameTimer = 0;

    [SerializeField, Tooltip("should game load win screen ?")]
    private bool _isWin = false;

    public bool IsWin
    {
        get { return _isWin; }

        set { _isWin = value; }
    }

    void Update()
    {
        PauseManager();

        WinManager();
    }

    void PauseManager()
    {
        if (isPause)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void WinManager()
    {
        if (_isWin)
            SceneManager.LoadScene("WinScene");
        else
            gameTimer += Time.deltaTime;
    }
}
