using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    //_______________________________SERIALIZED VARIABLES_________________________________

    [Header("KEY LINKED____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the script of the key you want to linked with")]
    private Grab key;

    [SerializeField]
    [Tooltip("the key that will be linked with")]
    private GameObject keyObject;

    [Header("ANIMATION____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the animation you want for the door's opening")]
    private RuntimeAnimatorController Door;

    [Header("INPUT RANGE____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the player values for range of door")]
    private PlayerController PlayerOpen;
    //___________________________________PRIVATE VARIABLES________________________________

    private Animator openAnim;
    private bool isDoorclosed = true;
    private bool _updateNavMeshPhase = false;    public bool UpdateNavMeshPhase => _updateNavMeshPhase;
    private float updateNavMeshTimer = 0;

    private NavigationBaker navigationBaker;

//___________________________________AWAKE AND START________________________________

    void Awake()
    {
        if (PlayerOpen == null)
        {
            PlayerOpen = Transform.FindObjectOfType<PlayerController>();
        }

        navigationBaker = FindObjectOfType<NavigationBaker>();
        openAnim = GetComponent<Animator>();
    }

//_______________________________________UPDATER____________________________________

    void Update()
    {
        if(isDoorclosed)
        {
            if (Input.GetAxis("Action1") == 1)
            {
                if (Vector3.Distance(keyObject.transform.position, transform.position) <= PlayerOpen.DoorRange)
                {
                    if (key.KeyState)
                    {
                        OpenDoor();
                    }
                }
            }
        }

        if (_updateNavMeshPhase)
        {
            if (updateNavMeshTimer >= 2)
            {
                navigationBaker.UpdateNavMesh();
                _updateNavMeshPhase = false;
            }

            updateNavMeshTimer += Time.deltaTime;
        }
        else
        {
            updateNavMeshTimer = 0;
        }
    }

//_______________________________________FUNCTIONS__________________________________

    public void OpenDoor()
    {
        isDoorclosed = false;
        openAnim.runtimeAnimatorController = null;
        openAnim.runtimeAnimatorController = Door;
        openAnim.enabled = true;
        keyObject.SetActive(false);
        _updateNavMeshPhase = true;
        PlayerOpen.DoorOpenSound();
    }
}
