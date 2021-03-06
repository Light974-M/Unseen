using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [Header("MANUAL UPDATE")]

    [SerializeField, Tooltip("is the player founded by AI")]
    private bool isFounded = false;

    [SerializeField, Tooltip("is the player suspected by AI")]
    private bool isSus = false;

    [SerializeField, Tooltip("update random pos of AI")]
    private bool updateRandomPos = false;

    
    [Header("GETTERS")]

    [SerializeField, Tooltip("player that will be used for detections and positions")]
    private Transform player;

    [SerializeField, Tooltip("zone used to calculate random Pos")]
    private Transform randomPosZone;

    [SerializeField, Tooltip("Capsule collider of Player")]
    private CapsuleCollider capsuleCollider;

    [SerializeField, Tooltip("mirror used")]
    private Transform mirror;

    [SerializeField, Tooltip("animator")]
    private Animator walkAnim;

    [Header("VISION")]

    [SerializeField, Tooltip("max dstance the monster can see")]
    private float maxDistance = 10f;

    [SerializeField, Tooltip("x fields of view")]
    private float mirrorXFOV = 30f;

    [SerializeField, Tooltip("y fields of view")]
    private float mirrorYFOV = 30f;

    [SerializeField, Tooltip("number of ray")]
    private float mirrorRayNumber = 30f;


    [Header("SEARCHING")]

    [SerializeField, Tooltip("min timer before update sreaching state")]
    private int minTimerSearching = 5;

    [SerializeField, Tooltip("max timer before update sreaching state")]
    private int maxTimerSearching = 14;

    [SerializeField, Tooltip("sphere were monster can easily ear player movements")]
    private float nearSphereRadius = 16f;

    [SerializeField, Tooltip("sphere were monster can fully detect player with sounds")]
    private float deadSphereRadius = 8f;

    [SerializeField, Tooltip("speed of searching and suspecting walk animation")]
    private float walkAnimSpeed = 0.5f;

    [SerializeField, Tooltip("audioRandomizer with search")]
    private GameObject searchSong;

    [SerializeField, Tooltip("audioRandomizer with found")]
    private GameObject foundSong;

    [SerializeField, Tooltip("main audioSource")]
    private AudioSource mainSource;

    [SerializeField, Tooltip("audioRandomizer with sus")]
    private AudioClip susSong;



    //PRIVATE VARIABLES_________________________________________________________________

    private float updateTimer = 0;
    private int randomTimer;

    private PlayerController playerController;

    private bool detectedPhase = false;
    private bool visibleDetectedPhase = false;
    private bool setPlayerSusPos = true;
    private Vector3 playerSusPos = Vector3.zero;
    private int susTimer = 0;
    private int searchTimer = 0;
    private bool rangeSus = false;
    private float susSprintTimer = 0;
    private bool isAudiblyDetectable = false;

    private DoorOpen[] doorOpenList;

    float yFov;


    private void Awake()
    {
        yFov = -mirrorYFOV;
        playerController = FindObjectOfType<PlayerController>();

        if (navMeshAgent == null)
            navMeshAgent = FindObjectOfType<NavMeshAgent>();

        doorOpenList = FindObjectsOfType<DoorOpen>();
    }

    private void Update()
    {
        if (!LevelManager.isPause)
        {
            navMeshAgent.isStopped = false;

            if (isFounded)
            {
                PlayerFound();
            }
            else
            {
                navMeshAgent.speed = 1f;
                navMeshAgent.angularSpeed = 100f;
                navMeshAgent.acceleration = 8f;

                if (isSus)
                    SuspectPlayer();
                else
                {
                    setPlayerSusPos = true;
                    susTimer = 0;
                    SearchPlayer();
                }
            }

            if (updateRandomPos)
            {
                updateTimer = randomTimer + 4;
                updateRandomPos = false;
            } 
        }
        else
        {
            navMeshAgent.isStopped = true;
            walkAnim.speed = 0;
        }
    }

    private void FixedUpdate()
    {
        DrawRay();
        UpdateSearchingState();
    }

    private void SearchPlayer()
    {
        searchSong.SetActive(true);
        foundSong.SetActive(false);

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

        if (!navMeshAgent.hasPath)
            searchTimer++;
        else
            searchTimer = 0;

        if (searchTimer >= 3)
        {
            walkAnim.enabled = true;
            walkAnim.speed = walkAnimSpeed;
            walkAnim.SetBool("isStand", true);
        }
        else
        {
            walkAnim.enabled = true;
            walkAnim.speed = walkAnimSpeed;
            walkAnim.SetBool("isStand", false);
            walkAnim.SetBool("isRun", false);
        }

        updateTimer += Time.deltaTime;
    }

    private void PlayerFound()
    {
        searchSong.SetActive(false);
        foundSong.SetActive(true);

        navMeshAgent.speed = 3.6f;
        navMeshAgent.angularSpeed = 300f;
        navMeshAgent.acceleration = 40f;

        walkAnim.enabled = true;
        walkAnim.speed = 2;
        walkAnim.SetBool("isStand", false);
        walkAnim.SetBool("isRun", true);

        navMeshAgent.SetDestination(player.position);
        navMeshAgent.updateRotation = true;

    }

    private void SuspectPlayer()
    {
        if (setPlayerSusPos)
        {
            mainSource.PlayOneShot(susSong);
            playerSusPos = player.position;
            setPlayerSusPos = false;
        }

        navMeshAgent.SetDestination(playerSusPos);
        navMeshAgent.updateRotation = true;

        if (!navMeshAgent.hasPath)
            susTimer++;

        if (susTimer >= 3)
        {
            isSus = false;

            walkAnim.enabled = true;
            walkAnim.speed = walkAnimSpeed;
            walkAnim.SetBool("isStand", true);
        }
        else
        {
            walkAnim.enabled = true;
            walkAnim.speed = walkAnimSpeed;
            walkAnim.SetBool("isStand", false);
            walkAnim.SetBool("isRun", false);
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

        if (!playerController.IsVisuallyDetectable && !isAudiblyDetectable)
        {
            isFounded = false;
        }

        if (visibleDetectedPhase)
        {
            isFounded = true;
        }

        foreach (DoorOpen door in doorOpenList)
        {
            if (door.UpdateNavMeshPhase)
            {
                isSus = true;
            }
        }

        if (playerController.FootStepsTimer == 0)
            isSus = true;

        if (rangeSus && isSus)
        {
            rangeSus = false;
            setPlayerSusPos = true;
        }

        if (Vector3.Distance(player.position, transform.position) <= nearSphereRadius)
        {
            if (Input.GetAxis("Sprint") == 1 || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.5f || Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.5f)
            {
                isSus = true;
                rangeSus = true;
            }
        }
        else
        {
            rangeSus = false;
        }

        if (Vector3.Distance(player.position, transform.position) <= deadSphereRadius)
        {
            if (Input.GetAxis("Sprint") == 1 || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.8f || Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.8f)
            {
                isAudiblyDetectable = true;
                isFounded = true;
            }
        }

        if(!(Input.GetAxis("Sprint") == 1 || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.8f || Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.8f))
        {
            isAudiblyDetectable = false;
        }

        if (Input.GetAxis("Sprint") == 1)
        {
            susSprintTimer += Time.deltaTime;

            if(susSprintTimer >= 4)
            {
                isSus = true;
                rangeSus = true;
            }
        }
        else
        {
            susSprintTimer = 0;
        }
    }

    private void DrawRay()
    {
        float xFov = -mirrorXFOV;
        visibleDetectedPhase = false;

        RaycastHit hit;
        RaycastHit mirrorHit;
        RaycastHit reflectedHit;
        Vector3 playerVisionInit = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        Vector3 playerMirrorVisionInit = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Vector3 direction = (player.position - playerVisionInit).normalized;
        LayerMask layerMask = ~(1 << 8);

        bool isCollided = Physics.Raycast(playerVisionInit, direction, out hit, maxDistance, layerMask);

        for (int j = 0; j < mirrorRayNumber; j++)
        {
            Vector3 originalDir = transform.TransformDirection(new Vector3(xFov, 0, 1).normalized);
            LayerMask mirrorMask = (1 << 9) | (1 << 13);

            if (Physics.Raycast(playerMirrorVisionInit, originalDir, out mirrorHit, maxDistance, mirrorMask))
            {
                float x = playerMirrorVisionInit.x + (2 * (mirrorHit.point.x - playerMirrorVisionInit.x));
                float y = playerMirrorVisionInit.y + (2 * (mirrorHit.point.y - playerMirrorVisionInit.y));

                if (mirrorHit.transform.gameObject.layer != 13)
                {
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

            }
            xFov += 2 * mirrorXFOV / mirrorRayNumber;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene("LooseScene");
        }
    }
}
