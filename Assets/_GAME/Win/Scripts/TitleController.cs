using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    Color color;

    private void Awake()
    {
        color = GetComponent<SpriteRenderer>().color;
    }
    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime;

        if(transform.position.z > 20)
        {
            GetComponent<SpriteRenderer>().color = color;
            color.a -= 0.01f;
        }
    }
}
