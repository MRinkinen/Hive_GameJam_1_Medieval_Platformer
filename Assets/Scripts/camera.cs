using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float cameraSpeed;
    public Transform player;
    public float cameraY;
    public float cameraZ;
    public float playerSpeed;
    public bool isGameStarted;
    // Start is called before the first frame update
    void Start()
    {
        isGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted)
        {
            playerSpeed = player.GetComponent<Rigidbody>().velocity.magnitude / 7;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.position.x, cameraY, cameraZ - playerSpeed), cameraSpeed * Time.deltaTime);
        }
        //gameObject.transform.position = new Vector3(player.position.x, player.position.y + 10, player.position.z - 10);
        //gameObject.transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
    }
}
