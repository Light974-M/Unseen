using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private Transform Door;

    [SerializeField]
    private Transform PlayerMesh;

    private bool isGrabed = false;
    private bool isInputDown = false;

    private bool _keyState = false;
    public bool KeyState => _keyState;

    void Start()
    {

    }

    void Update()
    {
        GrabManager();
    }

    public void GrabManager()
    {
        if (!isGrabed)
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
                        if(gameObject == PlayerController.nearestKeyAvailable)
                        {
                            _keyState = true;
                            transform.SetParent(PlayerMesh);
                            transform.localPosition = new Vector3(0, 0, 1.3f);
                            isGrabed = true;
                            isInputDown = true;
                        }
                    }
                }
            }
        }

        if (isGrabed)
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
                if (Input.GetAxis("Action1") == 1)
                {
                    if(Vector3.Distance(transform.position, Door.position) > 2)
                    {
                        _keyState = false;
                        transform.SetParent(null);
                        isGrabed = false;
                        isInputDown = true;
                    }
                }
            }
        }
    }
}
