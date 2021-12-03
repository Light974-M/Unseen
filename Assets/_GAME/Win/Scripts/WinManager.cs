using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [SerializeField]
    private GameObject YouEscaped;

    [SerializeField]
    private GameObject Title;

    [SerializeField]
    private GameObject Violin;

    [SerializeField]
    private GameObject Credits;

    [SerializeField]
    private GameObject font;

    [SerializeField]
    private float timeBeforeTitle = 5;

    [SerializeField]
    private float timeBeforeCredits = 20;

    private float timer = 0;

    private void Update()
    {
        if(timer >= timeBeforeTitle && timer <= timeBeforeTitle + 0.1f)
        {
            YouEscaped.SetActive(false);
            Title.SetActive(true);
            font.SetActive(false);
        }

        if(timer >= timeBeforeCredits && timer <= timeBeforeCredits + 0.1f)
        {
            Credits.SetActive(true);
            Violin.SetActive(true);
        }

        timer += Time.deltaTime;
    }
}
