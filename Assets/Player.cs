using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //--------------------------------------------------------------------------------

	[SerializeField]
    float startX;

    [SerializeField]
    float startY;

    [SerializeField]
    float speed;

    public Vector2 direction;

    // If startDark is true, then the beginning darkness fade in is occuring.
    bool startDark = false;
    // If endDark is true, then the ending darkness fade in is occuring.
    bool endDark = false;
    // maxFadeInTime is the number of frames it takes to fade in darkness.
    int maxFadeInTime = 2000;
    // gameStarted is true if the player has entered the house.
    bool gameStarted = false;
    // timeToVanish is the number of frames it takes for the player to disssappear on death.
    int timeToVanish = 150;

    // dead is true if the player has died.
    public static bool dead;
    // finished is true if the game has been won.
    public static bool finished;

    static int MaxBattery = 5000;
	public static int battery;
	public static int money;
    int fadeInTime;

    public GameObject CameraObj;
    public GameObject darkObj;
    public GameObject brightObj;
    public GameObject stunObj;
    public GameObject darkShadowObj;
    public GameObject brightShadowObj;
    public GameObject darkScreenObj;

    public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer sprite;
    public ParticleSystem bleedParticles;
    public SpriteRenderer darkScreenRenderer;

    //--------------------------------------------------------------------------------

    void Start() {

        darkObj = transform.GetChild(0).gameObject;
        brightObj = transform.GetChild(1).gameObject;
        stunObj = transform.GetChild(2).gameObject;
        bleedParticles = transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        darkShadowObj = transform.GetChild(4).gameObject;
        brightShadowObj = transform.GetChild(5).gameObject;
        darkScreenObj = transform.GetChild(6).gameObject;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        transform.position = new Vector2(startX, startY);
        darkScreenRenderer = darkScreenObj.GetComponent<SpriteRenderer>();

        battery = MaxBattery;
        money = 0;
        dead = false;
        finished = false;

        darkObj.transform.localScale = new Vector3(3f, 3f, 3f);

    }

    //--------------------------------------------------------------------------------

    void Update() {

        if (startDark && fadeInTime > 0) {
            darkObj.transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
            fadeInTime -= 1;
            if (fadeInTime == 0) {
                startDark = false;
            }
        }

        if (endDark && fadeInTime > 0) {
            float ratio = (float) ((float) (maxFadeInTime - fadeInTime) / (float) maxFadeInTime);
            darkScreenRenderer.color = new Color(0f, 0f, 0f, ratio);
            fadeInTime -= 2;
            if (fadeInTime <= 0) {
                finished = true;
            }
        }

    	if (dead) { 
    		if (timeToVanish > 0) {
        		timeToVanish -= 1;
        		sprite.color = new Color(1f, 1f, 1f, (float) (timeToVanish * 0.05f));
        	}
        	return;
        }

        Move();

    	if (Input.GetKey(KeyCode.Space) && battery > 0 && Time.timeScale != 0 && !endDark) {
    		darkObj.SetActive(false);
            darkShadowObj.SetActive(false);
            brightObj.SetActive(true);
            brightShadowObj.SetActive(true);
            stunObj.SetActive(true);
    		battery -= 1;
    	} else {
            darkObj.SetActive(true);
            darkShadowObj.SetActive(true);
            brightObj.SetActive(false);
            brightShadowObj.SetActive(false);
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
        if (other.tag == "GameEnd" && gameStarted && !endDark) {
            gameEnd();
        }
        if (other.tag == "BeginDarkness") {
            startDark = true;
            fadeInTime = maxFadeInTime;
            other.enabled = false;
        }
    }

    //--------------------------------------------------------------------------------

    public void gameEnd() {
    	endDark = true;
        fadeInTime = maxFadeInTime;
    }

    public void Die() {
    	if (dead) { return; }
    	dead = true;
    	bleedParticles.Play();
    	rigidbody2D.isKinematic = true;
    	boxCollider2D.enabled = false;
    }

    //--------------------------------------------------------------------------------

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
        return !dead;
    }

    public static bool GetFinished() {
        return finished;
    }

    //--------------------------------------------------------------------------------

}
