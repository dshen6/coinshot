using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int PLAYER_ID;
    public Vector2 velocity;
    public float gravity = 9.8f;
    public float maxVerticalSpeed;
    private PlayerInput playerInput;
    private bool isGrounded;
    private Rigidbody2D rg2d;

    void Start () {
        playerInput = GetComponent<PlayerInput>();
        rg2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() { 
        RaycastHit2D[] hits = Physics2D.RaycastAll(rg2d.position, Vector2.down);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag("Player")) {
                continue;
            }
            Debug.Log(hit.distance + " " + hit.rigidbody.tag);
            if (hit.distance < .3f) {
                Debug.Log("hit stage");
                isGrounded = true;
            }
        }
        

        float verticalSpeed;
        if (!isGrounded) { 
            verticalSpeed = velocity.y - gravity;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
        }
        else {
            verticalSpeed = 0;
        }
        velocity = new Vector2(velocity.x, verticalSpeed);
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
    }

    void Fire1Down() {

    }
}
