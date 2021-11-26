using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YouEscapedController : MonoBehaviour
{
    [SerializeField, Tooltip("timer before start"), Range(0, 1)]
    private float startTime = 5f;

    private Color color;

    private float opacityUpdater = 0;

    private Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        color = Color.white;
    }
    void Update()
    {
        if(startTime > 0)
        {
            startTime -= Time.deltaTime;
        }
        else
        {
            color.a = opacityUpdater;
            text.color = color;

            opacityUpdater += Time.deltaTime / 50;
        }
    }
}
