using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

//_______________________________SERIALIZED VARIABLES_________________________________

    [Header(" MOVEMENTS_________________________________________________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the camera used for reference to player movements")]
    private Transform cam;

    [SerializeField]
    [Tooltip("the character control component")]
    private CharacterController controller;

    [Header(" ROTATIONS_________________________________________________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the mesh player that will be used for player rotation")]
    private Transform PlayerMesh;

    [Header(" SPRINT_________________________________________________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("set the normal speed without sprinting")]
    private float normalSpeed;

    [SerializeField]
    [Tooltip("set the sprint speed value")]
    private float sprintSpeed;

    [Header(" GRAVITY_____________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("set gravity strength")]
    private float gravity = 9.81f;

    [SerializeField]
    [Tooltip("set the object that will detect collision with the ground")]
    private Transform groundCheck;

    [SerializeField]
    [Tooltip("set mask for the ground")]
    private LayerMask groundMask;

    [Header(" FOOTSTEPS_________________________________________________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("set footsteps visibility's duration")]
    private float footStepDuration = 10;

    [SerializeField]
    [Tooltip("object that will be instantiate for footsteps texture")]
    private GameObject FootSteps;

    [Header(" RAIN/LIGHT_________________________________________________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("material under rain")]
    private Material PlayerMatInvisible;

    [SerializeField]
    [Tooltip("material without rain")]
    private Material PlayerMaterial;

    [Header(" GRAB_________________________________________________________________________________________________________________________________________________________")]
    [Header("")]

    [SerializeField]
    [Tooltip("the range min to grab objects")]
    private float _grabRange = 4;      public float GrabRange => _grabRange;

    [SerializeField]
    [Tooltip("the range min to open door")]
    private float _doorRange = 4;      public float DoorRange => _doorRange;

    [SerializeField, Tooltip("keyList")] 
    private List<GameObject> keyList;

    //___________________________________PRIVATE VARIABLES________________________________

    private float footStepsTimer = 10000;   public float FootStepsTimer => footStepsTimer;

    private float footStepsUpdater = 0;

    private float speed;

    private float groundDistance = 0.4f;

    private Vector3 velocity;

    bool isGrounded;

    public static GameObject nearestKeyAvailable;

    private bool isVisible = false;

    private float footStepPosSwitch = 0.4f;    public float FootStepPosSwitch => footStepPosSwitch;

    private Renderer playerMat;
    private Camera camComponent;

    private bool _isVisuallyDetectable = false;   public bool IsVisuallyDetectable => _isVisuallyDetectable;

    private LevelManager levelManager;

//___________________________________AWAKE AND START________________________________
    
    void Awake()
    {
        if (playerMat == null)
            playerMat = PlayerMesh.GetComponent<Renderer>();

        if (camComponent == null)
            camComponent = cam.gameObject.GetComponent<Camera>();

        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();
    }

//_______________________________________UPDATER____________________________________

    void Update()
    {
        NearestKey();

        Sprint();

        Move();

        Jump();

        Rotation();

        FootStepManager();
        
    }

//_______________________________________FUNCTIONS__________________________________

    private void Rotation()
    {
        if (Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 && Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
        {
            Vector3 NextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (NextDir != Vector3.zero)
            {
                PlayerMesh.rotation = Quaternion.LookRotation(NextDir);
                PlayerMesh.localEulerAngles += new Vector3(0, cam.eulerAngles.y, 0);
            }
        }
    }

    private void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        Vector3.Normalize(move);
        controller.Move(move * speed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
    }

    private void Sprint()
    {
        if (Input.GetAxis("Sprint") == 1)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
    }

    private void NearestKey()
    {
        if (Input.GetAxis("Action1") == 0)
        {
            nearestKeyAvailable = keyList[0];

            for (int i = 1; i < keyList.Count; i++)
            {
                if (Vector3.Distance(transform.position, nearestKeyAvailable.transform.position) >= Vector3.Distance(transform.position, keyList[i].transform.position))
                    nearestKeyAvailable = keyList[i]; 
            }
        }
    }

    private void FootStepManager()
    {
        if(footStepsTimer <= footStepDuration)
        {
            _isVisuallyDetectable = true;
            if(footStepsUpdater >= 0.3f)
            {
                if(footStepPosSwitch == 0.3f)
                {
                    footStepPosSwitch = -0.3f;
                }
                else
                {
                    footStepPosSwitch = 0.3f;
                }

                if (Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 && Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
                {
                    Instantiate(FootSteps, new Vector3(transform.position.x, transform.position.y, transform.position.z), PlayerMesh.transform.rotation);
                }
                
                footStepsUpdater = 0;
            }
            else
            {
                footStepsUpdater += Time.deltaTime;
            }
            footStepsTimer += Time.deltaTime;
        }
        else
        {
            if(!isVisible)
                _isVisuallyDetectable = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            footStepsTimer = 0;
            footStepsUpdater = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            isVisible = true;
            _isVisuallyDetectable = true;

            playerMat.material = PlayerMatInvisible;
            camComponent.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Ground", "Water", "UI", "MessGround", "Rain", "Player", "Mirror", "Door", "MirrorBlock", "nonWalkable");
        }

        if (other.gameObject.layer == 11)
        {
            levelManager.IsWin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            isVisible = false;
            _isVisuallyDetectable = false;

            playerMat.material = PlayerMaterial;
            camComponent.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Ground", "Water", "UI", "MessGround", "Rain", "Mirror", "Door", "MirrorBlock", "nonWalkable");
        }
    }
}
