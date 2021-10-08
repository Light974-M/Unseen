using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{
    [SerializeField]
    private Transform A;

    [SerializeField]
    private Transform B;

    [SerializeField]
    private Transform camPosition;

    [SerializeField]
    private Transform Player;

    void Start()
    {
        
    }

    void Update()
    {
        if(Player.position.x > A.position.x && Player.position.y > A.position.y && Player.position.z > A.position.z && Player.position.x < B.position.x && Player.position.y < B.position.y && Player.position.z < B.position.z)
        {
            Camera.main.transform.position = camPosition.position;
            Camera.main.transform.eulerAngles = camPosition.eulerAngles;
        }
    }
}
