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
    private LineRenderer aimArrow;
    public Rigidbody2D coin;

    private Rigidbody2D leftCoin;
    private Rigidbody2D rightCoin;

    void Start() {
        playerInput = GetComponent<PlayerInput>();
        rg2d = GetComponent<Rigidbody2D>();
        aimArrow = GetComponentInChildren<LineRenderer>();
    }

    void Update() {
        Vector3 aimVector = playerInput.aimVector * 2;
        Vector3 extrudedAimVector = aimVector + transform.position;
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        positions[1] = extrudedAimVector;
        aimArrow.SetPositions(positions);
    }

    void FixedUpdate() {
        checkGrounded();
        float verticalSpeed;
        if (!isGrounded) {
            verticalSpeed = velocity.y - gravity;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
        } else {
            verticalSpeed = 0;
        }
        velocity = new Vector2(velocity.x, verticalSpeed);
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

    void checkGrounded() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(rg2d.position, Vector2.down);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag("Player")) {
                continue;
            }
            //Debug.Log(hit.distance + " " + hit.rigidbody.tag);
            if (hit.distance < .3f) {
                //Debug.Log("hit stage");
                isGrounded = true;
            }
        }
    }

    void Fire1Down(float value) {
        if (leftCoin == null) {
            Vector3 aimVector = playerInput.aimVector * .4f;
            Vector3 extrudedAimVector = aimVector + transform.position;
            leftCoin = (Rigidbody2D)Instantiate(coin, extrudedAimVector, transform.rotation);
        } else {

        }
    }

    void Fire1Up() {
        ////DestroyImmediate(leftCoin);
        leftCoin = null;
    }

    void Fire2Down(float value) {
        if (rightCoin == null) {
            Vector3 aimVector = playerInput.aimVector * .4f;
            Vector3 extrudedAimVector = aimVector + transform.position;
            rightCoin = (Rigidbody2D)Instantiate(coin, extrudedAimVector, transform.rotation);
        }
    }

    void Fire2Up() {
        rightCoin = null;
    }
}
