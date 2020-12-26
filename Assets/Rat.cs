using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour {

	public int speed = 10;

	public int aggroRadius = 3;

	public bool provoked = false;

	Transform playerTransform;

	//--------------------------------------------------------------------------------

    void Update() {

    	playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = (transform.position - playerTransform.position).magnitude;
        if (distance < aggroRadius) {
        	provoked = true;
        } else {
            provoked = false;
        }

        if (provoked) {
        	Vector2 move = (transform.position - playerTransform.position).normalized;
        	float angle = Vector2.SignedAngle(Vector2.up, move);
            Quaternion newRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    //--------------------------------------------------------------------------------

}
