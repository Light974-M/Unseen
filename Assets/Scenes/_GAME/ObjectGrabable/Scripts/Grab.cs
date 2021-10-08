using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

    [SerializeField]
    private Transform Player;

    private bool isGrabed = false;
    private bool isInputDown = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(!isGrabed)
        {
            if (isInputDown)
            {
                if (Input.GetAxis("Action1") == 0)
                {
                    isInputDown = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, Player.position) < 2)
                {
                    if (Input.GetAxis("Action1") == 1)
                    {
                        transform.SetParent(Player);
                        isGrabed = true;
                        isInputDown = true;
                    }
                }
            }
        }
        
        if(isGrabed)
        {
            Debug.Log(isInputDown);
            if(isInputDown)
            {
                if (Input.GetAxis("Action1") == 0)
                {
                    isInputDown = false;
                }
            }
            else
            {
                if (Input.GetAxis("Action1") == 1)
                {
                    transform.SetParent(null);
                    isGrabed = false;
                    isInputDown = true;
                }
            }
        }
    }
}
