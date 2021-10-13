using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

//_______________________________SERIALIZED VARIABLES_________________________________

    [SerializeField]
    [Tooltip("the script of the key you want to linked with")]
    private Grab key;

    [SerializeField]
    [Tooltip("the key that will be linked with")]
    private GameObject keyObject;

    [SerializeField]
    [Tooltip("the animation you want for the door's opening")]
    private RuntimeAnimatorController Door;

//___________________________________PRIVATE VARIABLES________________________________

    private Animator openAnim;
    private bool isDoorclosed = true;

//___________________________________AWAKE AND START________________________________

    void Awake()
    {
        openAnim = GetComponent<Animator>();
    }

//_______________________________________UPDATER____________________________________

    void Update()
    {
        if(isDoorclosed)
        {
            if (Input.GetAxis("Action1") == 1)
            {
                if (Vector3.Distance(keyObject.transform.position, transform.position) <= 2)
                {
                    if (key.KeyState)
                    {
                        OpenDoor();
                    }
                }
            }
        }
        
    }

//_______________________________________FUNCTIONS__________________________________

    public void OpenDoor()
    {
        isDoorclosed = false;
        openAnim.runtimeAnimatorController = null;
        openAnim.runtimeAnimatorController = Door;
        openAnim.enabled = true;
        /*keyObject.transform.position = new Vector3(0, 100000, 0);
        keyObject.GetComponent<MeshRenderer>.enabled = false*/
        keyObject.SetActive(false);
    }
}
