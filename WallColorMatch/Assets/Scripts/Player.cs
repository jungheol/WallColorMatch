using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float jumpForce = 10f;
    public float moveSpeed = 5f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid.velocity = new Vector2(moveSpeed, jumpForce);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Jump();
        }
    }

    private void Jump() {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
    }

    private void ReverseX() {
        float x = Mathf.Sign(rigid.velocity.x);
        rigid.velocity = new Vector2(-x * moveSpeed, rigid.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Wall")) {
            if (other.GetComponent<SpriteRenderer>().color != spriteRenderer.color) {
                PlayerDie();
            } else {
                ReverseX();
                GameManager.instance.CollisionWall();
            }
        } else if (other.CompareTag("DeathZone")) {
            PlayerDie();
        }
    }

    private void PlayerDie() {
        gameObject.SetActive(false);
    }
}
