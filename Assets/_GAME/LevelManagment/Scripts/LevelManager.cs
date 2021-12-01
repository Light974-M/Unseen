using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool isPause = false;

    private double gameTimer = 0;

    [SerializeField, Tooltip("should game load win screen ?")]
    private bool _isWin = false;

    [SerializeField]
    private GameObject PauseObjects;

    private bool pauseKeyDown = false;

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

            PauseObjects.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PauseObjects.SetActive(false);
        }

        if (Input.GetAxis("Pause") == 1)
        {
            if(!pauseKeyDown)
            {
                isPause = !isPause;
                pauseKeyDown = true;
            }
        }
        else if (Input.GetAxis("Pause") == 0)
        {
            pauseKeyDown = false;
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
