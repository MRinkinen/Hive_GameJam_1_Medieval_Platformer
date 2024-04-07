using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject[] enemies;
    public GameObject gameOverPanel;
    public GameObject gameWonPanel;
    public GameObject startPanel;
    public GameObject scorePanel;
    public GameObject kamera;
    public bool isPlayerAlive = true;
    public AudioSource backgroundAudio;
    public AudioClip startAudio;
    public AudioClip gameAudio;
    public AudioClip gameOverAudio;
    public AudioClip gameWonAudio;
    bool isGameOn = false;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        backgroundAudio.clip = startAudio;
        backgroundAudio.Play();
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            score++;
            //Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
        }
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Infected villagers " + score.ToString();

        if (score == 0 && isGameOn)
        {
            backgroundAudio.Stop();
            backgroundAudio.clip = gameWonAudio;
            backgroundAudio.Play();
            isGameOn = false;
            gameWonPanel.SetActive(true);
            kamera.GetComponent<camera>().isGameStarted = false;
            player.GetComponent<player_movement>().animator.SetTrigger("gamewon");
            player.GetComponent<player_movement>().isPlayerAlive = false;
            Vector3 lookDirection = new Vector3(kamera.transform.position.x, player.transform.position.y, kamera.transform.position.z) - player.transform.position;
            if (lookDirection != Vector3.zero)
            {
                player.GetComponent<player_movement>().playerCharacter.transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            //Time.timeScale = 0;
        }
        if (!isPlayerAlive)
        {
            gameOverPanel.SetActive(true);
            //Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            //SceneManager.LoadScene("Start");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GameScene");
        }
        // {
        //     Application.Quit();
        //     //SceneManager.LoadScene("Start");
        // }
        if (Input.GetKeyDown(KeyCode.Space) && !isGameOn)
        {
            isGameOn = true;
            backgroundAudio.Stop();
            backgroundAudio.clip = gameAudio;
            backgroundAudio.Play();
            scorePanel.SetActive(true);
            startPanel.SetActive(false);
            player.GetComponent<player_movement>().isPlayerAlive = true;
            kamera.GetComponent<camera>().isGameStarted = true;
        }
    }
    public void StopEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<enemy>().canMove = false;
            enemy.GetComponent<Animator>().SetBool("idle", true);
            Destroy(enemy.GetComponent<CapsuleCollider>());
            Destroy(enemy.GetComponent<Rigidbody>());

        }
    }
}
