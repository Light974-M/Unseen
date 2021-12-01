using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimManager : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void OnEnable()
    {
        anim.enabled = true;
    }

    private void OnDisable()
    {
        anim.enabled = false;
    }
}
