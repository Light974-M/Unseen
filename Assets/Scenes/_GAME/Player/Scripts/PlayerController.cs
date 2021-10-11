using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform cam;

    [SerializeField]
    private Transform PlayerMesh;

    [SerializeField]
    private CharacterController controller;

    private float speed;

    [SerializeField]
    private float normalSpeed;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float gravity = 9.81f;
    private Vector3 velocity;

    [SerializeField]
    private Transform groundCheck;

    private float groundDistance = 0.4f;

    [SerializeField]
    private LayerMask groundMask;

    bool isGrounded;

    public static GameObject nearestKeyAvailable;

    [SerializeField]
    private GameObject key110;

    [SerializeField]
    private GameObject key120;

    void Awake()
    {

    }

    void Update()
    {
        NearestKey();

        if (Input.GetAxis("Sprint") == 1)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * speed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 && Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
        {
            Vector3 NextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (NextDir != Vector3.zero)
            {
                PlayerMesh.rotation = Quaternion.LookRotation(NextDir);
                PlayerMesh.localEulerAngles += new Vector3(0, cam.eulerAngles.y, 0);
            }
            
            
        }
    }

    public void NearestKey()
    {
        if(Vector3.Distance(transform.position, key110.transform.position) >= Vector3.Distance(transform.position, key120.transform.position))
        {
            nearestKeyAvailable = key120;
        }
        else
        {
            nearestKeyAvailable = key110;
        }
    }
}
