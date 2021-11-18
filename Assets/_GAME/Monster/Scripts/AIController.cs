using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [SerializeField, Tooltip("is the player founded by AI")]
    private bool isFounded = false;

    [SerializeField, Tooltip("is the player suspected by AI")]
    private bool isSus = false;

    [SerializeField, Tooltip("update random pos of AI")]
    private bool updateRandomPos = false;

    [SerializeField, Tooltip("player that will be used for detections and positions")]
    private Transform player;

    [SerializeField, Tooltip("zone used to calculate random Pos")]
    private Transform randomPosZone;

    [SerializeField, Tooltip("Capsule collider of Player")]
    private CapsuleCollider capsuleCollider;

    [SerializeField, Tooltip("mirror used")]
    private Transform mirror;

    [SerializeField, Tooltip("max dstance the monster can see")]
    private float maxDistance = 10f;

    private bool isUpdatingSusPos = false;

    [SerializeField, Tooltip("x fields of view")]
    private float xFOV = 30f;

    [SerializeField, Tooltip("y fields of view")]
    private float yFOV = 30f;

    [SerializeField, Tooltip("number of ray")]
    private float rayNumber = 30f;

    [SerializeField, Tooltip("min timer before update sreaching state")]
    private int minTimerSearching = 5;

    [SerializeField, Tooltip("max timer before update sreaching state")]
    private int maxTimerSearching = 14;

    private float updateTimer = 0;
    private int randomTimer;

    private PlayerController playerController;

    private bool detectedPhase = false;
    private bool visibleDetectedPhase = false;


    int i = 0;
    float yFov;


    private void Awake()
    {
        yFov = -yFOV;
        playerController = FindObjectOfType<PlayerController>();

        if (navMeshAgent == null)
            navMeshAgent = FindObjectOfType<NavMeshAgent>();
    }

    private void Update()
    {
        if (isFounded)
        {
            PlayerFounded();
        }
        else
        {
            navMeshAgent.speed = 2f;
            navMeshAgent.angularSpeed = 100f;
            navMeshAgent.acceleration = 8f;

            if (isSus)
                SuspectPlayer();
            else
                SearchPlayer();
        }

        if (updateRandomPos)
        {
            updateTimer = randomTimer;
            updateRandomPos = false;
        }
    }

    private void FixedUpdate()
    {
        DrawRay();
        UpdateSearchingState();
    }

    private void SearchPlayer()
    {
        isUpdatingSusPos = true;

        if (updateTimer == 0)
            randomTimer = Random.Range(minTimerSearching, maxTimerSearching);

        if (updateTimer >= randomTimer)
        {
            Vector3 APos = randomPosZone.Find("A").position;
            Vector3 BPos = randomPosZone.Find("B").position;

            float rX = Random.Range(APos.x, BPos.x);
            float rY = Random.Range(APos.y, BPos.y);
            float rZ = Random.Range(APos.z, BPos.z);
            Vector3 randomPos = new Vector3(rX, rY, rZ);

            navMeshAgent.SetDestination(transform.position);

            if (updateTimer >= randomTimer + 4)
            {
                navMeshAgent.SetDestination(randomPos);
                navMeshAgent.updateRotation = true;

                updateTimer = 0;
            }
        }

        updateTimer += Time.deltaTime;
    }

    private void PlayerFounded()
    {
        isUpdatingSusPos = true;
        navMeshAgent.speed = 4.6f;
        navMeshAgent.angularSpeed = 300f;
        navMeshAgent.acceleration = 40f;

        navMeshAgent.SetDestination(player.position);
        navMeshAgent.updateRotation = true;
    }

    private void SuspectPlayer()
    {
        if (isUpdatingSusPos)
        {
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.updateRotation = true;

            isUpdatingSusPos = false;
        }
    }

    private void UpdateSearchingState()
    {
        if (detectedPhase)
        {
            if (playerController.IsVisuallyDetectable)
            {
                isFounded = true;
            }
            detectedPhase = false;
        }

        if (!playerController.IsVisuallyDetectable)
        {
            isFounded = false;
        }

        if (visibleDetectedPhase)
        {
            isFounded = true;
        }
    }

    private void DrawRay()
    {
        float xFov = -xFOV;
        visibleDetectedPhase = false;

        RaycastHit hit;
        RaycastHit mirrorHit;
        RaycastHit reflectedHit;
        Vector3 playerVisionInit = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        Vector3 playerMirrorVisionInit = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Vector3 direction = (player.position - playerVisionInit).normalized;
        LayerMask layerMask = ~(1 << 8);

        bool isCollided = Physics.Raycast(playerVisionInit, direction, out hit, maxDistance, layerMask);

        for (int j = 0; j < rayNumber; j++)
        {
            Vector3 originalDir = transform.TransformDirection(new Vector3(xFov, 0, 1).normalized);

            if (Physics.Raycast(playerMirrorVisionInit, originalDir, out mirrorHit, maxDistance, 1 << 9))
            {
                float x = playerMirrorVisionInit.x + (2 * (mirrorHit.point.x - playerMirrorVisionInit.x));
                float y = playerMirrorVisionInit.y + (2 * (mirrorHit.point.y - playerMirrorVisionInit.y));

                if (Physics.Raycast(mirrorHit.point, (new Vector3(x, y, playerMirrorVisionInit.z) - mirrorHit.point).normalized, out reflectedHit))
                {
                    if (reflectedHit.collider == capsuleCollider)
                    {
                        Debug.DrawRay(mirrorHit.point, (new Vector3(x, y, playerMirrorVisionInit.z) - mirrorHit.point).normalized * reflectedHit.distance, Color.green);
                        visibleDetectedPhase = true;
                    }
                    else
                    {
                        Debug.DrawRay(mirrorHit.point, (new Vector3(x, y, playerMirrorVisionInit.z) - mirrorHit.point).normalized * reflectedHit.distance, Color.blue);
                    }

                }
                else
                    Debug.DrawRay(mirrorHit.point, (new Vector3(x, y, playerMirrorVisionInit.z) - mirrorHit.point).normalized * maxDistance, Color.red);

            }
            xFov += 2 * xFOV / rayNumber;
        }

        if (isCollided)
        {
            if (hit.collider == capsuleCollider)
            {
                Debug.DrawRay(playerVisionInit, direction * hit.distance, Color.green);
                detectedPhase = true;
            }
            else
                Debug.DrawRay(playerVisionInit, direction * hit.distance, Color.blue);
        }
        else
            Debug.DrawRay(playerVisionInit, direction * maxDistance, Color.red);
    }
}
