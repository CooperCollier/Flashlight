using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	//--------------------------------------------------------------------------------

	public static bool paused;
    public static bool dead;
    public static bool finished;

	public GameObject pauseMenuUI;
    public GameObject retryMenuUI;
    public GameObject EndCard;

    public GameObject resumeButton;
    public GameObject quitButton;
    
    public GameObject playerObj;

    //--------------------------------------------------------------------------------
    
    void Start() {

    	playerObj = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
        paused = false;
        Time.timeScale = 1f;
        resumeButton.SetActive(false);
        quitButton.SetActive(false);
        pauseMenuUI.SetActive(false);
        retryMenuUI.SetActive(false);
        
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	if (!paused && !dead && !finished) {
    		pauseMenuUI.SetActive(false);
        	resumeButton.SetActive(false);
        	quitButton.SetActive(false);
    	}
    	if (!Player.GetLife()) {
    		dead = true;
    		StartCoroutine(ShowRetryScreen());
    	}
        if (Player.GetFinished()) {
            finished = true;
            ShowEndCard();
        }
    }

    public void PauseGame() {
        if (!dead && !paused) {
    	   pauseMenuUI.SetActive(true);
           resumeButton.SetActive(true);
           quitButton.SetActive(true);
    	   Time.timeScale = 0f;
    	   paused = true;
        }
    }

    public void ResumeGame() {
    	pauseMenuUI.SetActive(false);
        resumeButton.SetActive(false);
        quitButton.SetActive(false);
    	Time.timeScale = 1f;
    	paused = false;
    }

    public void QuitGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ShowEndCard() {
        Time.timeScale = 0f;
        EndCard.SetActive(true);
        quitButton.SetActive(true);
    }

    IEnumerator ShowRetryScreen() {
        yield return new WaitForSeconds(2);
        retryMenuUI.SetActive(true);
        quitButton.SetActive(true);
        Time.timeScale = 0f;
    }

    //--------------------------------------------------------------------------------

}
