using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [SerializeField, Tooltip("credits speed to defil")]
    private float textDefilSpeed = 1;

    private Color color = Color.white;

    private void Update()
    {
        if(transform.position .y < 1850)
            transform.position += Vector3.up * Time.deltaTime * textDefilSpeed;
        else
        {
            GetComponent<Text>().color = color;
            color.a -= 0.01f;

            if(color.a <= 0)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
