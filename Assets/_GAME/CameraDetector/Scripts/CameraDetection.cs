using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{

    //_______________________________SERIALIZED VARIABLES_________________________________

    [Header("CAMERA ZONE SET____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the first point that delimitate the zone where camera position is affected")]
    private Transform A;

    [SerializeField]
    [Tooltip("the second point that delimitate the zone where camera position is affected")]
    private Transform B;

    [Header("CAMERA POSITION____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the place where the camera will be placed in affected zone")]
    private Transform camPosition;

    [Header("PLAYER DETECTOR____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the player that will be detected in zone")]
    private Transform Player;

    private float initCamAngleX;
    private float initCamAngleZ;

//___________________________________AWAKE AND START________________________________

    void Start()
    {
        initCamAngleX = camPosition.eulerAngles.x;
        initCamAngleZ = camPosition.eulerAngles.z;
    }

//_______________________________________UPDATER____________________________________

    void Update()
    {
        CameraPlacement();
    }

//_______________________________________FUNCTIONS__________________________________

    private void CameraPlacement()
    {
        camPosition.LookAt(Player.position);
        camPosition.eulerAngles = new Vector3(initCamAngleX, camPosition.eulerAngles.y, initCamAngleZ);

        if (Player.position.x > A.position.x && Player.position.y > A.position.y && Player.position.z > A.position.z && Player.position.x < B.position.x && Player.position.y < B.position.y && Player.position.z < B.position.z)
        {
            Camera.main.transform.position = camPosition.position;
            //Camera.main.transform.eulerAngles = camPosition.eulerAngles;
            CamController.camPosX = initCamAngleX;
            CamController.camPosZ = initCamAngleZ;
        }

        
    }
}
