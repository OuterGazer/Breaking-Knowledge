using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject creditsWindow;
    [SerializeField] GameObject[] instructionsWindow;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

   public void ReloadGame()
    {        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCreditsWindow()
    {
       this.creditsWindow.SetActive(true);
    }

    public void CloseCreditsWindow()
    {
        this.creditsWindow.SetActive(false);
    }

    public void LoadInstructionsWindow()
    {
        this.instructionsWindow[0].SetActive(true);
    }

    public void NextInstructionsWindow()
    {
        if (this.instructionsWindow[0].activeSelf)
        {
            this.instructionsWindow[0].SetActive(false);
            this.instructionsWindow[1].SetActive(true);
        }
        else if (this.instructionsWindow[1].activeSelf)
        {
            this.instructionsWindow[1].SetActive(false);
            this.instructionsWindow[2].SetActive(true);
        }
        else if (this.instructionsWindow[2].activeSelf)
        {
            this.instructionsWindow[2].SetActive(false);
        }
    }

    public void PreviousInstructionsWindow()
    {
        if (this.instructionsWindow[0].activeSelf)
        {
            this.instructionsWindow[0].SetActive(false);
        }
        else if (this.instructionsWindow[1].activeSelf)
        {
            this.instructionsWindow[1].SetActive(false);
            this.instructionsWindow[0].SetActive(true);
        }
        if (this.instructionsWindow[2].activeSelf)
        {
            this.instructionsWindow[1].SetActive(true);
            this.instructionsWindow[2].SetActive(false);
        }
    }
}
