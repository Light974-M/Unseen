using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LooseButtonController : MonoBehaviour
{
    [SerializeField, Tooltip("timer before start"), Range(0, 10)]
    private float startTime = 5f;

    [SerializeField, Tooltip("timer before start"), Range(0, 10)]
    private float startTimeYouDied = 1f;

    private Color color1;
    private Color color2;
    private Color color3;

    private float opacityUpdater = 0;
    private float opacityUpdater2 = 0;

    [SerializeField]
    private Image image1;

    [SerializeField]
    private Image image2;

    [SerializeField]
    private Text text1;

    [SerializeField]
    private Text text2;

    [SerializeField]
    private Text youDied;

    private void Awake()
    {
        color1 = Color.black;
        color2 = Color.white;
        color3 = Color.white;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;
        }
        else
        {
            color1.a = opacityUpdater;
            color2.a = opacityUpdater;

            image1.color = color1;
            image2.color = color1;
            text1.color = color2;
            text2.color = color2;

            opacityUpdater += Time.deltaTime / 50;
        }

        if (startTimeYouDied > 0)
        {
            startTimeYouDied -= Time.deltaTime;
        }
        else
        {
            color3.a = opacityUpdater2;

            youDied.color = color3;

            opacityUpdater2 += Time.deltaTime / 50;
        }
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnQuitClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
