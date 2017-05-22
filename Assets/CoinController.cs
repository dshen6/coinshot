using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public float gravity = 1f;
    public float distanceToHit = .13f;
    PlayerController connectedPlayer;
    public Vector2 velocity;
    public float maxVerticalSpeed;
    public float maxHorizontalSpeed;
    private Rigidbody2D rg2d;
    private bool isGrounded;
    private bool isLeftRightBounded;

    public void OnPlayerForce(float value) {
        Vector2 fromPlayer = rg2d.position - connectedPlayer.GetComponent<Rigidbody2D>().position;
        if (isLeftRightBounded) {
            connectedPlayer.reboundForce(value, -fromPlayer);
        }
        velocity += fromPlayer;
    }

    public void setConnectedPlayer(PlayerController player) {
        connectedPlayer = player;
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
        isGrounded = Utils.checkBoundedInDirection(Vector2.down, distanceToHit, rg2d.position);
        if (isGrounded) {
            verticalSpeed = 0;
        } else {
            verticalSpeed = velocity.y - gravity;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
        }

        isLeftRightBounded = Utils.checkBoundedInDirection(Vector2.left, distanceToHit, rg2d.position) || Utils.checkBoundedInDirection(Vector2.right, distanceToHit, rg2d.position);
        float horizontalSpeed;
        if (isLeftRightBounded) {
            horizontalSpeed = 0;
        } else {
            horizontalSpeed = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        }

        velocity = new Vector2(horizontalSpeed, verticalSpeed);
        // add friction
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

}
