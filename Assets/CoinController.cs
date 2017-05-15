using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public float gravity = 1f;
    PlayerController connectedPlayer;
    public Vector2 velocity;
    public float maxVerticalSpeed;
    private Rigidbody2D rg2d;
    private bool isGrounded;

    public void OnPlayerForce(float value) {
        Vector2 fromPlayer = rg2d.position - connectedPlayer.GetComponent<Rigidbody2D>().position;
        velocity = Vector2.ClampMagnitude(velocity + fromPlayer, 10f);
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
        checkGrounded();
        float verticalSpeed;
        if (!isGrounded) {
            verticalSpeed = velocity.y - gravity;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxVerticalSpeed, maxVerticalSpeed);
        } else {
            verticalSpeed = 0;
        }
        velocity = new Vector2(velocity.x, verticalSpeed);
        // add friction
        rg2d.MovePosition(rg2d.position + velocity * Time.fixedDeltaTime);
    }

    void checkGrounded() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(rg2d.position, Vector2.down);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag("Player") || hit.collider.gameObject.Equals(this.gameObject)) {
                continue;
            }
            if (hit.distance < .13f) {
                Debug.Log("isGrounded " + isGrounded);
            }
        }
    }
}
