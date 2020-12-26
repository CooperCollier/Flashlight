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

    static int MaxBattery = 5000;

    int timeToVanish = 150;

	public static int battery;
	public static int money;
	public static bool dead;

	public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;

    public GameObject CameraObj;

    public Vector2 direction;

    public GameObject darkObj;
    public GameObject brightObj;
    public GameObject stunObj;
    public ParticleSystem bleedParticles;
    public GameObject darkShadowObj;
    public GameObject brightShadowObj;
    public SpriteRenderer sprite;

    //--------------------------------------------------------------------------------

    void Start() {

    	spriteRenderer = GetComponent<SpriteRenderer>();
    	rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        transform.position = new Vector2(startX, startY);
        darkObj = transform.GetChild(0).gameObject;
        brightObj = transform.GetChild(1).gameObject;
        stunObj = transform.GetChild(2).gameObject;
        bleedParticles = transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        darkShadowObj = transform.GetChild(4).gameObject;
        brightShadowObj = transform.GetChild(5).gameObject;
        battery = MaxBattery;
        money = 0;
        dead = false;

    }

    //--------------------------------------------------------------------------------

    void Update() {

    	if (dead) { 
    		if (timeToVanish > 0) {
        		timeToVanish -= 1;
        		sprite.color = new Color(1f, 1f, 1f, (float) (timeToVanish * 0.005f));
        	}
        	return;
        }

        Move();

    	if (Input.GetKey(KeyCode.Space) && battery > 0 && Time.timeScale != 0) {
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
    }

    //--------------------------------------------------------------------------------

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

    //--------------------------------------------------------------------------------

}
