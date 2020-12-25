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

    static int MaxBattery = 4000;

	public static int battery;

	public static int money;

	public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;

    public GameObject CameraObj;

    public Vector2 direction;

    public GameObject darkObj;
    public GameObject brightObj;
    public GameObject stunObj;

    public ParticleSystem runDust;

    //--------------------------------------------------------------------------------

    void Start() {

    	spriteRenderer = GetComponent<SpriteRenderer>();
    	rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        transform.position = new Vector2(startX, startY);
        darkObj = transform.GetChild(0).gameObject;
        brightObj = transform.GetChild(1).gameObject;
        stunObj = transform.GetChild(2).gameObject;
        runDust = transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        battery = MaxBattery;
        money = 0;

    }

    //--------------------------------------------------------------------------------

    void Update() {

        Move();

    	if (Input.GetKey(KeyCode.Space) && battery > 0) {
    		darkObj.SetActive(false);
            stunObj.SetActive(true);
    		battery -= 1;
    	} else {
            stunObj.SetActive(false);
            //darkObj.SetActive(true); // Uncomment this line!
            //brightObj.SetActive(true); // Uncomment this line!
        }
        
    }

    //--------------------------------------------------------------------------------

    private void Move() {

        runDust.Play();
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
            runDust.Stop();
        }

    }

    //--------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
            money += 10;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Battery") {
            battery += (MaxBattery / 5);
            Destroy(other.gameObject);
        }
    }

    //--------------------------------------------------------------------------------

    public void Die() {
        Debug.Log("Me Is Die Now.");
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

    //--------------------------------------------------------------------------------

}
