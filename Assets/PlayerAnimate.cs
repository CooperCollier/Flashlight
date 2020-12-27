using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour {

    //--------------------------------------------------------------------------------

    private Animator animator;
    public bool walking;

    //--------------------------------------------------------------------------------

    void Start() {
    	animator = GetComponent<Animator>();
    }

    //--------------------------------------------------------------------------------

    void Update() {
    	walking = Player.isWalking();
    	animator.SetBool("walking", walking);
    }

    //--------------------------------------------------------------------------------

}

