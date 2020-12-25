using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPercent : MonoBehaviour {

	//--------------------------------------------------------------------------------

	Text text;

	public static int batteryPercent;

	//--------------------------------------------------------------------------------

    void Start() {
        text = GetComponent<Text>();
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	batteryPercent = Player.GetBatteryPercent();
        text.text = batteryPercent.ToString() + "%";
    }

    //--------------------------------------------------------------------------------
}
