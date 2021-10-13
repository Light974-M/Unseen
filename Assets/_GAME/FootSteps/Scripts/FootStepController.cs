using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Mesh that will decrease opacity trough time")]
    private GameObject footStepMesh;

    private float despawnUpdater = 255;

    void Awake()
    {
        Color color = footStepMesh.GetComponent<Renderer>().material.color;
        color.a = 255;
        footStepMesh.GetComponent<Renderer>().material.color = color;
    }

    void Update()
    {
        if(despawnUpdater > 0.05f)
        {
            Color color = footStepMesh.GetComponent<Renderer>().material.color;
            color.a = despawnUpdater;
            footStepMesh.GetComponent<Renderer>().material.color = color;

            despawnUpdater = despawnUpdater / 1.03f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
