using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_movement : MonoBehaviour
{
    //public GameManager gameManager;
    public CapsuleCollider playerCollider;
    public PlayerInput playerInput;
    private Vector3 lookDirection;
    public Animator animator;
    public Rigidbody rb;
    public float shootTimer;
    public float shootCooldown = 1;
    public float jumpTimer;
    public float jumpForce = 1000;
    public float jumpCoolDown = 1;
    public GameObject playerCharacter;
    public bool canMove = true;
    public float moveSpeed = 100f;
    public float maxForwardSpeed = 120;
    public Transform shootPoint;
    public GameObject fireball;
    public float fireballSpeed = 1000;
    bool isCasting = false;
    public bool isPlayerAlive = true;
    public AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {

        isPlayerAlive = false;
        shootTimer = 0;
        jumpTimer = 0;
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpTimer += Time.deltaTime;
        if (isPlayerAlive)
        {
            if (canMove)
                HandleJump();
            HandleShooting();
        }

    }
    private void FixedUpdate()
    {
        if (isPlayerAlive)
        {
            HandleMovement();
        }
    }
    void HandleShooting()
    {
        if (playerInput.actions["Fire"].IsPressed())
        {
            canMove = false;
            shootTimer += Time.deltaTime;

            animator.SetBool("casting", true);
            if (shootTimer > shootCooldown)
            {
                //animator.SetBool("casting", false);
                animator.SetTrigger("cast");
                GameObject fireballInstance = Instantiate(fireball, shootPoint.position, shootPoint.rotation);
                fireballInstance.GetComponent<Rigidbody>().AddForce(shootPoint.forward * fireballSpeed, ForceMode.Impulse);
                shootTimer = 0;
            }
        }
        else
        {
            animator.SetBool("casting", false);
            canMove = true;
            shootTimer = 0;

        }
    }

    void HandleMovement()
    {
        // New Input systemin liikkuminen
        float horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
        float vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;
        if (canMove)
        {
            rb.AddRelativeForce(Vector3.right * moveSpeed * horizontal);
            rb.AddRelativeForce(Vector3.forward * moveSpeed * vertical);
            if (rb.velocity.magnitude > maxForwardSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxForwardSpeed;
            }
        }
        if (playerInput.actions["Move"].IsPressed())
        {
            if (canMove)
                animator.SetBool("run", true);
            lookDirection = new Vector3(horizontal, 0, vertical);
            if (lookDirection != Vector3.zero)
            {
                playerCharacter.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        else
        {
            animator.SetBool("run", false);
        }
    }
    void HandleJump()
    {
        if (playerInput.actions["Jump"].WasPressedThisFrame() && jumpTimer > jumpCoolDown)
        {
            playerCollider.height = 0.8f;
            animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpTimer = 0;
        }
        else if (jumpTimer > jumpCoolDown / 2)
        {
            playerCollider.height = 1.8f;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("fire"))
        {
            deathSound.Play();
            isPlayerAlive = false;
            canMove = false;
            animator.SetTrigger("die");
            //animator.SetBool("run", false);
            GameManager.Instance.isPlayerAlive = false;
            GameManager.Instance.backgroundAudio.Stop();
            GameManager.Instance.backgroundAudio.loop = false;
            GameManager.Instance.backgroundAudio.clip = GameManager.Instance.gameOverAudio;
            GameManager.Instance.backgroundAudio.Play();
            GameManager.Instance.StopEnemies();
        }

    }
}
