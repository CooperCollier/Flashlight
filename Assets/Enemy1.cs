using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

	//--------------------------------------------------------------------------------

	bool provoked = false;

    bool dashing = false;
    int MaxDashTime = 100;
    int dashTime = 0;

	[SerializeField]
	int aggroRadius;

	[SerializeField]
	int speed;

    int MaxStunTime = 250;
    int stunTime = 0;
    bool stunned = false;

    int damageFrames = 0;
    int totalFrames = 0;

	Transform playerTransform;
    public Rigidbody2D rigidbody2D;

	//--------------------------------------------------------------------------------

    void Start() {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (damageFrames >= 5) {
                collision.gameObject.SendMessage("Die");
                dashing = true;
            } else {
                damageFrames += 1;
            }
        } else if (collision.gameObject.tag == "Stunner" && !stunned) {
            stunned = true;
            stunTime = MaxStunTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Stunner") {
            stunned = true;
            stunTime = MaxStunTime;
        }
    }

    //--------------------------------------------------------------------------------

    void Update() {

        totalFrames += 1;
        if (totalFrames > 1000) {
            totalFrames = 0;
        } else if (totalFrames % 50 == 0) {
            damageFrames = 0;
        }

        if (stunTime > 0) {
            stunTime -= 1;
        } else {
            stunned = false;
        }

        if (dashTime > 0) {
            dashTime -= 1;
        } else {
            dashing = false;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = (transform.position - playerTransform.position).magnitude;
        if (distance < aggroRadius) {
        	provoked = true;
        } else {
        	provoked = false;
        }

        if (dashing) {
            rigidbody2D.velocity = new Vector2(0, speed * 4);
            return;
        }

        if (provoked) {

        	Vector2 move = (playerTransform.position - transform.position).normalized;
        	float angle = Vector2.Angle(Vector2.up, move);
            Quaternion newRotation = Quaternion.Euler(0, 0, angle);
        	// Debug.Log(Vector2.Angle(move, Vector2.up));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);

            if (!stunned) {
        	   transform.Translate(Vector2.up * speed * Time.deltaTime);
            } else {
                transform.Translate(Vector2.down * speed * 2 * Time.deltaTime);
            }

        }

    }

    //--------------------------------------------------------------------------------

}
