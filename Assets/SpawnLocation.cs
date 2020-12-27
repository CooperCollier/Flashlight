using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnLocation : ScriptableObject {

	public GameObject playerObj;

	int previousFloor = 0;
	int nextFloor = 1;

	void Update() {
		playerObj = GameObject.FindGameObjectWithTag("Player");
	}

    void Awake() {

    	Debug.Log("Prev " + previousFloor);
    	Debug.Log("Next " + nextFloor);

        DontDestroyOnLoad(this);

        playerObj = GameObject.FindGameObjectWithTag("Player");

        if (previousFloor == 1 && nextFloor == 2) {
        	playerObj.SendMessage("setLocation", 2);
        	Debug.Log("sent");
        } else if (previousFloor == 2 && nextFloor == 1) {
        	playerObj.SendMessage("setLocation", 1);
        }
    }

    void GoUp() {
    	previousFloor = nextFloor;
    	nextFloor = 1;
    }

    void GoDown() {
    	Debug.Log("going down");
    	previousFloor = nextFloor;
    	nextFloor = 2;
    }

}
