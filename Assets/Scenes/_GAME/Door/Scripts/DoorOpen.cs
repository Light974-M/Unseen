using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField]
    private Grab key;

    [SerializeField]
    private GameObject keyObject;

    [SerializeField]
    private RuntimeAnimatorController Door;

    private Animator openAnim;
    private bool isDoorclosed = true;

    void Awake()
    {
        openAnim = GetComponent<Animator>();
    }

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
