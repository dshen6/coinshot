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
    public float maxTerminalVelocity;
    public CoinController coinPrefab;

    private PlayerInput playerInput;
    private bool isGrounded;
    private bool isRightBounded;
    private bool isLeftBounded;
    private bool? isExperiencingForceUp = null;
    private bool shouldIgnoreRightBounded;
    private bool shouldIgnoreLeftBounded;

    private Rigidbody2D rg2d;
    private LineRenderer aimArrow;
    private LineRenderer leftTetherLine;
    private LineRenderer rightTetherLine;

    private CoinController leftCoin;
    private CoinController rightCoin;

    void Start() {
        playerInput = GetComponent<PlayerInput>();
        rg2d = GetComponent<Rigidbody2D>();
        aimArrow = GetComponentsInChildren<LineRenderer>()[0];

        leftTetherLine = GetComponentsInChildren<LineRenderer>()[1];
        rightTetherLine = GetComponentsInChildren<LineRenderer>()[2];
    }

    void Update() {
        HandleInput();
        RenderTetherLines();
    }

    void HandleInput() {
        Vector3 aimVector = playerInput.aimVector * 2;
        Vector3 extrudedAimVector = aimVector * .4f + transform.position;
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        positions[1] = extrudedAimVector;
        aimArrow.SetPositions(positions);
    }

    void RenderTetherLines() {
        
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position;
        if (leftCoin == null) {
            positions[1] = transform.position;
        } else {
            positions[1] = leftCoin.transform.position;
        }
        leftTetherLine.SetPositions(positions);

        positions[0] = transform.position;
        if (rightCoin == null) {
            positions[1] = transform.position;
        } else {
            positions[1] = rightCoin.transform.position;
        }
        rightTetherLine.SetPositions(positions);
    }

    public void reboundForce(float value, Vector2 direction) {
        velocity += direction.normalized;
        isExperiencingForceUp = direction.y > 0;
        if (direction.x > 0) {
            shouldIgnoreLeftBounded = true;
        }
        if (direction.x < 0) {
            shouldIgnoreRightBounded = true;
        }
    }

    void FixedUpdate() {
        float verticalSpeed;

        isGrounded = Utils.checkBoundedInDirection(Vector2.down, distanceToHit, rg2d.position) && !isExperiencingForceUp.GetValueOrDefault(false);
        if (isGrounded) {
            verticalSpeed = 0;
        } else {
            verticalSpeed = velocity.y - (isExperiencingForceUp.GetValueOrDefault(false) ? 0 : gravity);
            if (isExperiencingForceUp == null) {
                verticalSpeed = Mathf.Max(verticalSpeed, -maxTerminalVelocity);
            }
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
            isExperiencingForceUp = null;
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
        velocity *= .92f;
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
            Vector3 aimVector = playerInput.aimVector;
            Vector3 extrudedAimVector = aimVector + transform.position;
            coin = Instantiate(coinPrefab, extrudedAimVector, transform.rotation);
            coin.setConnectedPlayer(this);
        } else {
            coin.OnPlayerForce(value * 6);
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
