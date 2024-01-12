using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float jumpForce = 10f;
    public float moveSpeed = 5f;
    public GameObject dieParticle;

    private Rigidbody2D rigid;
    private CircleCollider2D coll;
    private SpriteRenderer spriteRenderer;
    private bool isClick = false;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;
        coll = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Start() {
        float originY = transform.position.y;
        float delayY = 0.5f;
        float moveSpeedY = 5;
    
        while (!isClick) {
            float y = originY + delayY * Mathf.Sin(Time.time * moveSpeedY);
            transform.position = new Vector2(transform.position.x, y);
            
            yield return null;
        }
    }

    public void GameStart() {
        rigid.isKinematic = false;
        rigid.velocity = new Vector2(moveSpeed, jumpForce);
        isClick = true;

        StartCoroutine(UpdateInput());
    }

    IEnumerator UpdateInput() {
        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                Jump();
            }
            
            yield return null;
        }
    }

    private void Jump() {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        Debug.Log("jump");
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
                StartCoroutine(CollisionDelay());
                GameManager.instance.CollisionWall();
            }
        } else if (other.CompareTag("DeathZone")) {
            PlayerDie();
        }
    }

    IEnumerator CollisionDelay() {
        coll.enabled = false;

        yield return new WaitForSeconds(0.1f);

        coll.enabled = true;
    }

    private void PlayerDie() {
        gameObject.SetActive(false);
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        GameManager.instance.GameOver();
    }
}
