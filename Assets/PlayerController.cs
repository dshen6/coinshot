using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int PLAYER_ID;
    public Vector2 velocity;
    public float GRAVITY = 1f;
    public float DISTANCE_TO_HIT = .3f;
    public float MAX_VERTICAL_SPEED = 9f;
    public float MAX_HORIZONTAL_SPEED = 8f;
    public float MAX_TERMINAL_VELOCITY = 7f;
    public CoinController coinPrefab;

    private PlayerInput playerInput;
    private bool isGrounded;
    private bool isRightBounded;
    private bool isLeftBounded;
    private bool? isExperiencingForceUp = null;
    private bool shouldIgnoreRightBounded;
    private bool shouldIgnoreLeftBounded;

    private Rigidbody2D rg2d;
    private LineRenderer leftTetherLine;
    private LineRenderer rightTetherLine;

    private CoinController leftCoin;
    private CoinController rightCoin;

    void Start() {
        playerInput = GetComponent<PlayerInput>();
        rg2d = GetComponent<Rigidbody2D>();

        leftTetherLine = GetComponentsInChildren<LineRenderer>()[0];
        rightTetherLine = GetComponentsInChildren<LineRenderer>()[1];
    }

    void Update() {
        RenderTetherLines();
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
        velocity += direction.normalized * value;
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

        isGrounded = Utils.checkBoundedInDirection(Vector2.down, DISTANCE_TO_HIT, rg2d.position) && !isExperiencingForceUp.GetValueOrDefault(false);
        if (isGrounded) {
            verticalSpeed = 0;
        } else {
            verticalSpeed = velocity.y - (isExperiencingForceUp.GetValueOrDefault(false) ? 0 : GRAVITY);
            if (isExperiencingForceUp == null) {
                verticalSpeed = Mathf.Max(verticalSpeed, -MAX_TERMINAL_VELOCITY);
            }
            verticalSpeed = Mathf.Clamp(verticalSpeed, -MAX_VERTICAL_SPEED, MAX_VERTICAL_SPEED);
            isExperiencingForceUp = null;
        }

        isLeftBounded = Utils.checkBoundedInDirection(Vector2.left, DISTANCE_TO_HIT, rg2d.position) && !shouldIgnoreLeftBounded;
        isRightBounded = Utils.checkBoundedInDirection(Vector2.right, DISTANCE_TO_HIT, rg2d.position) && !shouldIgnoreRightBounded;
        float horizontalSpeed;
        if (isLeftBounded || isRightBounded) {
            horizontalSpeed = 0;
        } else {
            horizontalSpeed = Mathf.Clamp(velocity.x, -MAX_HORIZONTAL_SPEED, MAX_HORIZONTAL_SPEED);
            shouldIgnoreLeftBounded = false;
            shouldIgnoreRightBounded = false;
        }

        velocity = new Vector2(horizontalSpeed, verticalSpeed);
        velocity *= .92f;
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

    void Fire1Down(float value) {
        fireCoin(ref rightCoin, ref playerInput.rightAimVector, value);
    }

    void Fire1(float value) {
        pushCoin(ref rightCoin, value);
    }

    void Fire2(float value) {
        pushCoin(ref leftCoin, value);
    }

    void Fire2Down(float value) {
        fireCoin(ref leftCoin, ref playerInput.leftAimVector, value);
    }

    void fireCoin(ref CoinController coin, ref Vector2 aimVector, float value) {
        if (coin == null) {
            Vector3 aimVector3 = aimVector;
            Vector3 extrudedAimVector = aimVector3 + transform.position;
            coin = Instantiate(coinPrefab, extrudedAimVector, transform.rotation);
            coin.setConnectedPlayer(this);
        }
    }

    void pushCoin(ref CoinController coin, float value) {
        coin.OnPlayerForce(value);
    }

    void Fire1Up() {
        rightCoin = null;
    }

    void Fire2Up() {
        leftCoin = null;
    }

    //void HandleInput() {
    //    Vector3 aimVector = playerInput.leftAimVector * 2;
    //    Vector3 extrudedAimVector = aimVector * .4f + transform.position;
    //    Vector3[] positions = new Vector3[2];
    //    positions[0] = transform.position;
    //    positions[1] = extrudedAimVector;
    //    aimArrow.SetPositions(positions);
    //}
}
