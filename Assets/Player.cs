using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    //--------------------------------------------------------------------------------

    [SerializeField]
    float speed;

    public Vector2 direction;

    bool gameStarted = false;

    bool goingUp = false;
    bool goingDown = false;
    bool ending = false;

    float maxTimeToVanish = 1f;
    float timeToVanish = 0f;

    float maxFadeInTime = 2f;
    float fadeInTime = 0f;

    public static bool IsPlayerDead;

    public static bool IsGameWon;

    static int MaxBattery = 5000;
	public static int battery;
	public static int money;

    public GameObject CameraObj;
    public GameObject flashlightObj;
    public GameObject stunObj;
    public GameObject bleedParticlesObj;
    public GameObject darkScreenObj;

    public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer sprite;
    public ParticleSystem bleedParticles;
    public SpriteRenderer darkScreenRenderer;

    public static bool walking = false;
    Vector2 previousFrameLocation;
    Vector2 thisFrameLocation;

    public GameObject SpawnLocationObj;

    //--------------------------------------------------------------------------------

    void Start() {

        bleedParticlesObj = transform.GetChild(0).gameObject;
        darkScreenObj = transform.GetChild(1).gameObject;
        stunObj = transform.GetChild(3).gameObject;
        flashlightObj = transform.GetChild(4).gameObject;

        SpawnLocationObj = GameObject.FindGameObjectWithTag("SpawnLocator");

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        darkScreenRenderer = darkScreenObj.GetComponent<SpriteRenderer>();
        bleedParticles = bleedParticlesObj.GetComponent<ParticleSystem>();

        battery = MaxBattery;
        money = 0;
        IsPlayerDead = false;
        IsGameWon = false;

        thisFrameLocation = transform.position;
        previousFrameLocation = transform.position;

    }

    //--------------------------------------------------------------------------------

    void Update() {

        thisFrameLocation = transform.position;
        if (previousFrameLocation == thisFrameLocation) {
            walking = false;
        } else {
            walking = true;
        }
        previousFrameLocation = thisFrameLocation;

        if (fadeInTime > 0) {
            float ratio = (float) ((float) (maxFadeInTime - fadeInTime) / (float) maxFadeInTime);
            darkScreenRenderer.color = new Color(0f, 0f, 0f, ratio);
            fadeInTime -= Time.deltaTime;
            if (fadeInTime <= 0 && ending) {
                IsGameWon = true;
            }
            if (fadeInTime <= 0 && goingUp) {
                SceneManager.LoadScene(1);
            }
            if (fadeInTime <= 0 && goingDown) {
                SceneManager.LoadScene(2);
            }
        }

    	if (IsPlayerDead && timeToVanish > 0) {
        	timeToVanish -= (float) Time.deltaTime;
        	sprite.color = new Color(1f, 1f, 1f, (float) (timeToVanish / maxTimeToVanish));
        } else if (IsPlayerDead && timeToVanish <= 0) {
            return;
        }

        Move();

    	if (Input.GetKey(KeyCode.Space) && battery > 0 &&
         Time.timeScale != 0 && fadeInTime <= 0 && !IsPlayerDead) {
    		flashlightObj.SetActive(true);
            stunObj.SetActive(true);
            battery -= 1;
    	} else {
            flashlightObj.SetActive(false);
            stunObj.SetActive(false);
        }
        
    }

    //--------------------------------------------------------------------------------

    private void Move() {

        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            direction = Vector2.up;
            Quaternion newRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        } else if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            direction = Vector2.left;
            Quaternion newRotation = Quaternion.Euler(0, 0, 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        } else if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            direction = Vector2.down;
            Quaternion newRotation = Quaternion.Euler(0, 0, 180);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            direction = Vector2.right;
            Quaternion newRotation = Quaternion.Euler(0, 0, 270);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }

    }

    //--------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
            money += 10;
            Destroy(other.gameObject);
        }
        if (other.tag == "Gem1") {
            money += 30;
            Destroy(other.gameObject);
        }
        if (other.tag == "Gem2") {
            money += 50;
            Destroy(other.gameObject);
        }
        if (other.tag == "Battery") {
            battery += (MaxBattery / 10);
            if (battery > MaxBattery) {
            	battery = MaxBattery;
            }
            Destroy(other.gameObject);
        }
        if (other.tag == "GameStart") {
            gameStarted = true;
        }
        if (other.tag == "GameEnd" && gameStarted) {
            fadeInTime = maxFadeInTime;
            ending = true;
        } if (other.tag == "StairsUp") {
            fadeInTime = maxFadeInTime;
            goingUp = true;
        }  if (other.tag == "StairsDown") {
            fadeInTime = maxFadeInTime;
            goingDown = true;
        }
    }

    //--------------------------------------------------------------------------------

    public void Die() {
    	if (IsPlayerDead) { return; }
    	IsPlayerDead = true;
    	bleedParticles.Play();
    	rigidbody2D.isKinematic = true;
    	boxCollider2D.enabled = false;
        flashlightObj.SetActive(false);
        stunObj.SetActive(false);
    	timeToVanish = maxTimeToVanish;
    }

    //--------------------------------------------------------------------------------

    public void setLocation(int floor) {
        Debug.Log("recieved");
        if (floor == 1) {
            transform.position = new Vector2(60, 40);
        } else if (floor == 2) {
            transform.position = new Vector2(230, 585);
        }
    }

    public static int GetBatteryPercent() {
    	if (battery <= 0) {
    		return 0;
    	} else {
    		return (100 * battery) / MaxBattery;
    	}
    }

    public static int GetMoney() {
        return money;
    }

    public static bool GetLife() {
        return !IsPlayerDead;
    }

    public static bool GetFinished() {
        return IsGameWon;
    }

    public static bool isWalking() {
        return walking;
    }

    //--------------------------------------------------------------------------------

}
