using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int PLAYER_ID;
    public Vector2 velocity;
    public float gravity = 1f;
    public float distanceToHit = .3f;
    public float maxVerticalSpeed;
    public float maxHorizontalSpeed;
    public CoinController coinPrefab;

    private PlayerInput playerInput;
    private bool isGrounded;
    private bool isRightBounded;
    private bool isLeftBounded;
    private bool shouldIgnoreGrounded;
    private bool shouldIgnoreRightBounded;
    private bool shouldIgnoreLeftBounded;

    private Rigidbody2D rg2d;
    private LineRenderer aimArrow;

    private CoinController leftCoin;
    private CoinController rightCoin;

    void Start() {
        playerInput = GetComponent<PlayerInput>();
        rg2d = GetComponent<Rigidbody2D>();
        aimArrow = GetComponentInChildren<LineRenderer>();
    }

    void Update() {
        HandleInput();
    }

    void HandleInput() {
        Vector3 aimVector = playerInput.aimVector * 2;
        Vector3 extrudedAimVector = aimVector + transform.position;
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        positions[1] = extrudedAimVector;
        aimArrow.SetPositions(positions);
    }

    public void reboundForce(float value, Vector2 direction) {
        velocity += direction.normalized;
        if (direction.y > 0) {
            shouldIgnoreGrounded = true;
        }
        if (direction.x > 0) {
            shouldIgnoreLeftBounded = true;
        }
        if (direction.x < 0) {
            shouldIgnoreRightBounded = true;
        }
    }

    void FixedUpdate() {
        float verticalSpeed;

        isGrounded = Utils.checkBoundedInDirection(Vector2.down, distanceToHit, rg2d.position) && !shouldIgnoreGrounded;
        if (isGrounded) {
            verticalSpeed = 0;
        } else {
            verticalSpeed = velocity.y - (shouldIgnoreGrounded ? 0 : gravity);
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
            shouldIgnoreGrounded = false;
        }

        isLeftBounded = Utils.checkBoundedInDirection(Vector2.left, distanceToHit, rg2d.position) && !shouldIgnoreLeftBounded;
        isRightBounded = Utils.checkBoundedInDirection(Vector2.right, distanceToHit, rg2d.position) && !shouldIgnoreRightBounded;
        float horizontalSpeed;
        if (isLeftBounded || isRightBounded) {
            horizontalSpeed = 0;
        } else {
            horizontalSpeed = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
            shouldIgnoreLeftBounded = false;
            shouldIgnoreRightBounded = false;
        }

        velocity = new Vector2(horizontalSpeed, verticalSpeed);
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

    void Fire1Down(float value) {
        fireCoin(ref rightCoin, value);
    }

    void Fire2Down(float value) {
        fireCoin(ref leftCoin, value);
    }

    void fireCoin(ref CoinController coin, float value) {
        if (coin == null) {
            Vector3 aimVector = playerInput.aimVector * .4f;
            Vector3 extrudedAimVector = aimVector + transform.position;
            coin = Instantiate(coinPrefab, extrudedAimVector, transform.rotation);
            coin.setConnectedPlayer(this);
        } else {
            coin.OnPlayerForce(value);
        }
    }

    void Fire1Up() {
        ////DestroyImmediate(leftCoin);
        rightCoin = null;
    }

    void Fire2Up() {
        leftCoin = null;
    }
}
