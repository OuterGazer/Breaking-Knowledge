using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    private UI_Manager toGetScore;

    // Start is called before the first frame update
    void Start()
    {
        this.toGetScore = GameObject.FindObjectOfType<UI_Manager>();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.scoreText.text = this.toGetScore.PlayerScore.ToString();
    }
}
