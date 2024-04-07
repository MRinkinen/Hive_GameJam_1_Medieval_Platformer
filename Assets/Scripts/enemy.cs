using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject player;
    public bool isAlive = true;
    public float moveSpeed = 100f;
    public bool canMove = false;
    public Animator animator;
    public AudioSource audioSource;
    public bool isScared;
    public AudioClip start;
    public AudioClip die;
    bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        canMove = false;
        player = GameObject.FindGameObjectWithTag("Player");
        //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 10 && isAlive && !playerDetected)
        {
            playerDetected = true;
            audioSource.PlayOneShot(start, 0.7F);
            if (!isScared)
            {
                canMove = true;
            }
        }
        if (canMove && isAlive)
        {
            gameObject.transform.LookAt(player.transform.position);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }

    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("fireBall"))
        {
            if (isAlive)
            {
                audioSource.PlayOneShot(die, 0.7F);
                isAlive = false;
                GameManager.Instance.score -= 1;
                animator.SetBool("die", true);
                Destroy(gameObject.GetComponent<CapsuleCollider>());
                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject, 10);
                Debug.Log("Enemy hit" + other.gameObject.name);
            }
        }

    }
}
