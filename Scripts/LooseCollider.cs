using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseCollider : MonoBehaviour
{
    private Ball gameBall;

    private LevelManager levelManager;

    private UI_Manager targetObject;
    [SerializeField] AudioClip lifeLost;
    private Vector3 gameCameraPos;

    private AudioSource backgroundMusic;

    private void Start()
    {
        this.levelManager = GameObject.FindObjectOfType<LevelManager>();

        this.targetObject = GameObject.FindObjectOfType<UI_Manager>();
        this.gameCameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        this.backgroundMusic = GameObject.Find("Level Manager").GetComponent<AudioSource>();

        this.gameBall = GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Game Ball" && GameObject.Find("Game Ball(Clone)") == null)
        {
            this.StartCoroutine(StartAgain());
        }
        else if (collision.gameObject.name == "Game Ball" && GameObject.Find("Game Ball(Clone)") != null) //if the true game ball hits the bottom place it in a spot where the player can't see it
        {
            //this.gameBall.gameObject.SetActive(false);
            this.gameBall.transform.position = new Vector2(2.0f, -2.0f);
        }        
        else if (collision.gameObject.name == "Game Ball(Clone)" && GameObject.FindGameObjectsWithTag("ball").Length > 2) //Always destroy clones of the ball if they hit the bottom
        {
            GameObject.Destroy(collision.gameObject);
        }
        else if (collision.gameObject.name == "Game Ball(Clone)" && GameObject.FindGameObjectsWithTag("ball").Length <= 2 &&
                 this.gameBall.gameObject.transform.position.y == -2.0f) //Always destroy clones of the ball if they hit the bottom
        {
            GameObject.Destroy(collision.gameObject);            
        }
        else if (collision.gameObject.name == "Game Ball(Clone)" && GameObject.FindGameObjectsWithTag("ball").Length <= 2 &&
                 this.gameBall.gameObject.transform.position.y != -2.0f) //Always destroy clones of the ball if they hit the bottom
        {
            GameObject.Destroy(collision.gameObject);
            this.StartCoroutine(StartAgain());
        }

        if(collision.gameObject.tag != "ball") //anything other than the ball should be destroyed
            GameObject.Destroy(collision.gameObject);
    }

    public IEnumerator StartAgain()
    {
        this.backgroundMusic.Pause();
        this.levelManager.IsGamePaused = true;

        AudioSource.PlayClipAtPoint(this.lifeLost, this.gameCameraPos);
        this.gameBall.HasGameStarted = false;
        this.targetObject.SendMessage("SubtractALife");

        Time.timeScale = 0;        

        yield return new WaitForSecondsRealtime(1.0f);

        this.levelManager.IsGamePaused = false;
        this.backgroundMusic.UnPause();
        Time.timeScale = 1;
    }
}
