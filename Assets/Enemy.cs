using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Enemy : MonoBehaviour {

	//--------------------------------------------------------------------------------

	[SerializeField]
    public int speed;

    [SerializeField]
    public int aggroRadius;

    [SerializeField]
    float MaxDashTime;

    [SerializeField]
    float MaxStunTime;

	bool provoked = false;

    bool done = false;

    bool dashing = false;
    float dashTime = 0;
    float stunTime = 0;
    bool stunned = false;

	Transform playerTransform;
    public Rigidbody2D rigidbody2D;
    public ParticleSystem stunParticles;
    public BoxCollider2D boxCollider2D;
    public Seeker seeker;
    public AIPath aiPath;

    [SerializeField]
    public LayerMask wallLayer;

    public Vector2 previousFrameLocation;
    public Vector2 thisFrameLocation;

    public AudioSource AudioFootsteps;
    public AudioSource AudioAttack;

    bool playerDead = false;

	//--------------------------------------------------------------------------------

    void Start() {
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        stunParticles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        stunParticles.Stop();

        AudioFootsteps = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        AudioAttack = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
    }

    //--------------------------------------------------------------------------------

    void Update() {

    	if (!Player.GetLife()) {
    		playerDead = true;
    	}

        if (done) { return; }

        thisFrameLocation = transform.position;
        previousFrameLocation = thisFrameLocation;

        if (stunTime > 0) {
            stunTime -= Time.deltaTime;
        } else {
            stunned = false;
            stunParticles.Stop();
        }

        if (dashTime > 0) {
            dashTime -= Time.deltaTime;
        } else if (dashing) {
            dashing = false;
            done = true;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = (transform.position - playerTransform.position).magnitude;
        if (distance < aggroRadius && !playerDead) {
        	provoked = true;
        } else {
        	provoked = false;
        }

        if (dashing) {
            transform.Translate(Vector2.up * speed * 5 * Time.deltaTime);
            return;
        }

        if (!provoked) {
            seeker.enabled = false;
            aiPath.enabled = false;
            AudioFootsteps.Stop();
        } else if (provoked && stunned) {
            seeker.enabled = false;
            aiPath.enabled = false;
            AudioFootsteps.Stop();
            StunAction();
        } else if (provoked && !stunned) {
            seeker.enabled = true;
            aiPath.enabled = true;
            if (!AudioFootsteps.isPlaying) {
                AudioFootsteps.Play();
            }
        }

    }

    //--------------------------------------------------------------------------------

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && !stunned) {
            AudioFootsteps.Stop();
            collision.gameObject.SendMessage("Die");
            dashing = true;
            dashTime = MaxDashTime;
            AudioAttack.Play();
            playerDead = true;
        } else if (collision.gameObject.tag == "Stunner" && !stunned) {
            stunned = true;
            stunTime = MaxStunTime;
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && !stunned) {
            AudioFootsteps.Stop();
            collision.gameObject.SendMessage("Die");
            dashing = true;
            dashTime = MaxDashTime;
            AudioAttack.Play();
            playerDead = true;
        } else if (collision.gameObject.tag == "Stunner" && !stunned) {
            stunned = true;
            stunTime = MaxStunTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Stunner") {
            stunned = true;
            stunTime = MaxStunTime;
            stunParticles.Play();
        }
    }

    public abstract void StunAction();

    //--------------------------------------------------------------------------------

}
