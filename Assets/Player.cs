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

    static float MaxBattery = 35f;
	public static float battery;
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
    Quaternion previousFrameRotation;
    Quaternion thisFrameRotation;

    public GameObject AudioFootsteps;
    public AudioSource AudioDoors;
    public AudioSource AudioCoin;
    public AudioSource AudioLight;

    //--------------------------------------------------------------------------------

    void Start() {

        bleedParticlesObj = transform.GetChild(0).gameObject;
        darkScreenObj = transform.GetChild(1).gameObject;
        stunObj = transform.GetChild(3).gameObject;
        flashlightObj = transform.GetChild(4).gameObject;

        AudioFootsteps = transform.GetChild(6).gameObject;
        AudioDoors = transform.GetChild(7).gameObject.GetComponent<AudioSource>();
        AudioCoin = transform.GetChild(8).gameObject.GetComponent<AudioSource>();
        AudioLight = transform.GetChild(9).gameObject.GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        darkScreenRenderer = darkScreenObj.GetComponent<SpriteRenderer>();
        bleedParticles = bleedParticlesObj.GetComponent<ParticleSystem>();

        IsPlayerDead = false;
        IsGameWon = false;

        if (PlayerPrefs.GetInt("NextScene") == 0) {
        	setLocation(0);
        } else if (PlayerPrefs.GetInt("NextScene") == 1) {
        	setLocation(1);
        } else if (PlayerPrefs.GetInt("NextScene") == 2) {
        	setLocation(2);
        }

        money = PlayerPrefs.GetInt("Money");
        battery = PlayerPrefs.GetFloat("Battery");

        thisFrameLocation = transform.position;
        previousFrameLocation = transform.position;

        PlayerPrefs.SetInt("NextScene", 0);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetFloat("Battery", MaxBattery);

    }

    //--------------------------------------------------------------------------------

    void Update() {

        rigidbody2D.velocity = new Vector2(0, 0);

        Move();

        thisFrameLocation = transform.position;
        if ((previousFrameLocation == thisFrameLocation) || IsPlayerDead) {
            walking = false;
            AudioFootsteps.SetActive(false);
        } else {
            walking = true;
            AudioFootsteps.SetActive(true);
        }
        previousFrameLocation = thisFrameLocation;

        thisFrameRotation = transform.rotation;
        previousFrameRotation = thisFrameRotation;

        if (fadeInTime > 0) {
            float ratio = (float) ((float) (maxFadeInTime - fadeInTime) / (float) maxFadeInTime);
            darkScreenRenderer.color = new Color(0f, 0f, 0f, ratio);
            fadeInTime -= Time.deltaTime;
            if (fadeInTime <= 0 && ending) {
                IsGameWon = true;
            }
            if (fadeInTime <= 0 && goingUp) {
            	PlayerPrefs.SetInt("NextScene", 1);
                PlayerPrefs.SetInt("Money", money);
                PlayerPrefs.SetFloat("Battery", battery);
                SceneManager.LoadScene(1);
            }
            if (fadeInTime <= 0 && goingDown) {
            	PlayerPrefs.SetInt("NextScene", 2);
                PlayerPrefs.SetInt("Money", money);
                PlayerPrefs.SetFloat("Battery", battery);
                SceneManager.LoadScene(2);
            }
        }

    	if (IsPlayerDead && timeToVanish > 0) {
        	timeToVanish -= (float) Time.deltaTime;
        	sprite.color = new Color(1f, 1f, 1f, (float) (timeToVanish / maxTimeToVanish));
        } else if (IsPlayerDead && timeToVanish <= 0) {
            return;
        }

    	if (Input.GetKey(KeyCode.Space) && battery > 0 &&
         Time.timeScale != 0 && fadeInTime <= 0 && !IsPlayerDead) {
    		flashlightObj.SetActive(true);
            stunObj.SetActive(true);
            battery -= Time.deltaTime;
            AudioLight.Play();
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
        } else {
            transform.position = previousFrameLocation;
            transform.rotation = previousFrameRotation;
        }

    }

    //--------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
            money += 5;
            Destroy(other.gameObject);
            AudioCoin.Play();
        }
        if (other.tag == "Gem1") {
            money += 30;
            Destroy(other.gameObject);
            AudioCoin.Play();
        }
        if (other.tag == "Gem2") {
            money += 50;
            Destroy(other.gameObject);
            AudioCoin.Play();
        }
        if (other.tag == "Trophy") {
            money += 200;
            Destroy(other.gameObject);
            AudioCoin.Play();
        }
        if (other.tag == "Battery") {
            battery += (MaxBattery / 10);
            if (battery > MaxBattery) {
            	battery = MaxBattery;
            }
            Destroy(other.gameObject);
            AudioCoin.Play();
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Door") {
            AudioDoors.Play();
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
        if (floor == 0) {
        	transform.position = new Vector2(56, -1);
        } else if (floor == 1) {
            transform.position = new Vector2(58, 40);
        } else if (floor == 2) {
            transform.position = new Vector2(231, 585);
        }
    }

    public static int GetBatteryPercent() {
    	if (battery <= 0) {
    		return 0;
    	} else {
    		return (int) ((100f * battery) / MaxBattery);
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
