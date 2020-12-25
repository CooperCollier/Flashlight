using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    //--------------------------------------------------------------------------------

    public float smoothSpeed = 0.125f;

    private Transform playerTransform;

    //--------------------------------------------------------------------------------

    void Start() {
    	playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //--------------------------------------------------------------------------------

    void LateUpdate() {
    	transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
    }

    //--------------------------------------------------------------------------------

}
