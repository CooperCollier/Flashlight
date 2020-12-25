using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour {

	public int speed = 10;

	public int aggroRadius = 3;

	public bool provoked = false;

	public int provokeTime = 0;

	Transform playerTransform;

	//--------------------------------------------------------------------------------

    void Update() {

    	playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = (transform.position - playerTransform.position).magnitude;
        if (distance < aggroRadius) {
        	provoked = true;
        	provokeTime = 100;
        }

        if (provokeTime > 0) {
            provokeTime -= 1;
        } else {
            provoked = false;
        }

        if (provoked) {
        	Vector2 move = (transform.position - playerTransform.position).normalized;
        	float angle = Vector2.Angle(Vector2.up, move);
            Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        	// Debug.Log(Vector2.Angle(move, Vector2.up));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        
    }

    //--------------------------------------------------------------------------------

}
