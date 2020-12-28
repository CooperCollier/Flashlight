using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy {

	//--------------------------------------------------------------------------------

    public override void StunAction() {
    	transform.position = previousFrameLocation;
        return;
    }


    //--------------------------------------------------------------------------------

}
