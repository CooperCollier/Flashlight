using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy {

    //--------------------------------------------------------------------------------

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Door") {
            transform.Translate(Vector2.down * speed * 5f * Time.deltaTime);
        }
    }

    //--------------------------------------------------------------------------------

    public override void StunAction() {
        transform.Translate(Vector2.down * speed * 0.5f * Time.deltaTime);
    }

    //--------------------------------------------------------------------------------

}
