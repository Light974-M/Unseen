using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private PlayerController player;

    public static float camPosX = 0;
    public static float camPosZ = 0;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        Camera.main.transform.LookAt(player.transform.position);
        Camera.main.transform.eulerAngles = new Vector3(camPosX, Camera.main.transform.eulerAngles.y, camPosZ);
    }
}
