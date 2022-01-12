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

    private Renderer footStepRenderer;
    private Color color;

    void Awake()
    {
        PlayerFootSteps = Transform.FindObjectOfType<PlayerController>();
        footStepRenderer = footStepMesh.GetComponent<Renderer>();
        despawnUpdater = 255 / (PlayerFootSteps.FootStepsTimer * 50);

        Vector3 footStepPos = new Vector3(PlayerFootSteps.FootStepPosSwitch, -2.27f, 0);
        color = footStepRenderer.material.color;

        footStepMesh.transform.localPosition = footStepPos;
        color.a = despawnUpdater;
        footStepRenderer.material.color = color;
    }

    void Update()
    {
        if(footStepRenderer.material.color.a >= 0.0001f)
        {
            color.a = despawnUpdater;
            footStepRenderer.material.color = color;

            despawnUpdater = despawnUpdater / 1.04f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
