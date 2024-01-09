using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float jumpForce = 10f;

    private Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Jump();
        }
    }

    private void Jump() {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
    }
}
