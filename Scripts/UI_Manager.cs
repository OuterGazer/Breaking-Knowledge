using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    //Configuration Parameters (things we need to know before the game)
    private int playerLives = 5;
    [SerializeField] TextMeshProUGUI displayLives;
    private int playerScore = 0;
    public int PlayerScore => this.playerScore;
    [SerializeField] TextMeshProUGUI displayScore;

    //Cached Component References (references to other game objects or components of game objects)
    [SerializeField] GameObject gameOverScreen;
    private LevelManager levelManager;

    //State variables (to keep track of the variables that govern states)
    private bool isLifeAdded = false;

    // Start is called before the first frame update
    void Start()
    {
        this.displayLives.text = "Lives: " + this.playerLives;
        this.displayScore.text = this.playerScore.ToString();
        this.levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.playerLives > 0)
            this.displayLives.text = "Lives: " + this.playerLives;
        else
            this.displayLives.text = "Lives: 0";

        if (this.playerLives < 0)
        {
            Time.timeScale = 0;
            this.levelManager.IsGamePaused = true;
            this.levelManager.GetComponent<AudioSource>().Stop();
            this.gameOverScreen.SetActive(true);
        }

        this.displayScore.text = this.playerScore.ToString();

        if ((this.playerScore % 500 == 0) && (this.playerScore != 0) && this.isLifeAdded == false)
            this.StartCoroutine(AddALife());

        
    }

    private void AddScore(int inBlockPointsValue)
    {
        this.playerScore += inBlockPointsValue;
    }

    private void SubtractALife()
    {
        this.playerLives--;
    }

    private IEnumerator AddALife()
    {
        this.playerLives++;
        this.isLifeAdded = true;

        yield return new WaitForSeconds(10.0f);

        this.isLifeAdded = false;
    }
}
