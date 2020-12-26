using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	//--------------------------------------------------------------------------------

	public GameObject Tutorial;
	public GameObject PlayButton;
    public GameObject TutorialOpenButton;
    public GameObject TutorialCloseButton;

	//--------------------------------------------------------------------------------
    
    public void Play() {
    	SceneManager.LoadScene(1);
    }

    public void TutorialOpen() {
    	PlayButton.SetActive(false);
    	TutorialOpenButton.SetActive(false);
    	Tutorial.SetActive(true);
    	TutorialCloseButton.SetActive(true);
    }

    public void TutorialClose() {
    	PlayButton.SetActive(true);
    	TutorialOpenButton.SetActive(true);
    	Tutorial.SetActive(false);
    	TutorialCloseButton.SetActive(false);
    }

    //--------------------------------------------------------------------------------

}