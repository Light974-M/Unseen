using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepController : MonoBehaviour
{
    [Header("MESH DETECTOR____________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("Mesh that will decrease opacity trough time")]
    private GameObject footStepMesh;

    private PlayerController PlayerFootSteps;

    private float despawnUpdater = 255;

    void Awake()
    {
        PlayerFootSteps = Transform.FindObjectOfType<PlayerController>();

        footStepMesh.transform.localPosition = new Vector3(PlayerFootSteps.FootStepPosSwitch, 0, 0);
        despawnUpdater = 255 / (PlayerFootSteps.FootStepsTimer * 100);

        Color color = footStepMesh.GetComponent<Renderer>().material.color;
        color.a = despawnUpdater;
        footStepMesh.GetComponent<Renderer>().material.color = color;
    }

    void Update()
    {
        if(footStepMesh.GetComponent<Renderer>().material.color.a >= 0.02f)
        {
            Color color = footStepMesh.GetComponent<Renderer>().material.color;
            color.a = despawnUpdater;
            footStepMesh.GetComponent<Renderer>().material.color = color;

            despawnUpdater = despawnUpdater / 1.01f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
