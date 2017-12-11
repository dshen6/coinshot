using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public float GRAVITY = 1f;
    public float DISTANCE_TO_HIT = .13f;
    public float MAX_VERTICAL_SPEED = 12f;
    public float MAX_HORIZONTAL_SPEED = 12f;
    public float MAX_TERMINAL_VELOCITY = 10f;

    public PlayerController connectedPlayer;
    public Vector2 velocity;
    private Rigidbody2D rg2d;
    private bool isGrounded;
    private bool isLeftRightBounded;

    public void OnPlayerForce(float value) {
        if (rg2d == null) {
            return;
        }
        Vector2 fromPlayer = rg2d.position - connectedPlayer.GetComponent<Rigidbody2D>().position;
        if (isLeftRightBounded) {
            connectedPlayer.reboundForce(value, -fromPlayer);
        }
        velocity += fromPlayer * value * value;
    }

	// Use this for initialization
	void Start () {
        rg2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
    }

    void FixedUpdate() {
        float verticalSpeed;
        isGrounded = Utils.checkBoundedInDirection(Vector2.down, DISTANCE_TO_HIT, rg2d.position);
        if (isGrounded) {
            verticalSpeed = 0;
        } else {
            verticalSpeed = velocity.y - GRAVITY;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -MAX_VERTICAL_SPEED, MAX_VERTICAL_SPEED);
        }

        isLeftRightBounded = Utils.checkBoundedInDirection(Vector2.left, DISTANCE_TO_HIT, rg2d.position) || Utils.checkBoundedInDirection(Vector2.right, DISTANCE_TO_HIT, rg2d.position);
        float horizontalSpeed;
        if (isLeftRightBounded) {
            horizontalSpeed = 0;
        } else {
            horizontalSpeed = Mathf.Clamp(velocity.x, -MAX_HORIZONTAL_SPEED, MAX_HORIZONTAL_SPEED);
        }

        velocity = new Vector2(horizontalSpeed, verticalSpeed);

        // add friction
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

}
