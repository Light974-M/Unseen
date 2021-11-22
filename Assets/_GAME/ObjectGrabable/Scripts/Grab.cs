using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grab : MonoBehaviour
{

    //_______________________________SERIALIZED VARIABLES_________________________________

    [Header("GRAB LINK____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the player that will be detected for grab")]
    private Transform Player;

    [SerializeField]
    [Tooltip("the door that will be linked with the key")]
    private Transform Door;

    [SerializeField]
    [Tooltip("the place where the key will be parented")]
    private Transform PlayerMesh;

    [Header("GRAB RANGE____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the player values for range of grab")]
    private PlayerController PlayerGrab;

    [SerializeField, Tooltip("text use to render key name")]
    private Text keyText;

    //___________________________________PRIVATE VARIABLES________________________________

    private bool isGrabed = false;
    private bool isInputDown = false;
    private bool _keyState = false;     public bool KeyState => _keyState;

//___________________________________AWAKE AND START________________________________

    void Awake()
    {
        if(PlayerGrab == null)
        {
            PlayerGrab = Transform.FindObjectOfType<PlayerController>();
        }
    }

//_______________________________________UPDATER____________________________________

    void Update()
    {
        GrabManager();
    }

//_______________________________________FUNCTIONS__________________________________

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
                if (Vector3.Distance(transform.position, Player.position) < PlayerGrab.GrabRange)
                {
                    if (Input.GetAxis("Action1") == 1)
                    {
                        if(gameObject == PlayerController.nearestKeyAvailable)
                        {
                            _keyState = true;
                            transform.SetParent(PlayerMesh);
                            transform.localPosition = new Vector3(0, 0, 1.3f);

                            keyText.text = gameObject.name;
                            keyText.enabled = true;

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
                    if(Vector3.Distance(transform.position, Door.position) > PlayerGrab.GrabRange)
                    {
                        _keyState = false;
                        transform.SetParent(null);

                        keyText.enabled = false;

                        isGrabed = false;
                        isInputDown = true;
                    }
                }
            }
        }
    }
}
