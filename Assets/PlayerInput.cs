using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public bool isFacingRight;
    public Vector2 leftAimVector;
    public Vector2 rightAimVector;
    PlayerController controller;

    float leftHorizontal;
    float leftVertical;
    float rightHorizontal;
    float rightVertical;
    private bool leftTriggerDown;
    private bool rightTriggerDown;

    private string A_INPUT = "A_Keyboard_";
    private string B_INPUT = "B_Keyboard_";
    private string LEFT_TRIGGER_GAMEPAD = "Left_Trigger_";
    private string RIGHT_TRIGGER_GAMEPAD = "Right_Trigger_";
    private string A_GAMEPAD = "A_";
    private string B_GAMEPAD = "B_";
    private string X_GAMEPAD = "X_";
    private string HORIZONTAL_INPUT = "L_XAxis_Keyboard_";
    private string VERTICAL_INPUT = "L_YAxis_Keyboard_";
    private string RIGHT_HORIZONTAL_GAMEPAD = "R_XAxis_";
    private string RIGHT_VERTICAL_GAMEPAD = "R_YAxis_";
    private string LEFT_HORIZONTAL_GAMEPAD = "L_XAxis_";
    private string LEFT_VERTICAL_GAMEPAD = "L_YAxis_";
    private string START_INPUT = "Start_";

    void Start() {
        controller = GetComponent<PlayerController>();

        int playerId = controller.PLAYER_ID;
        A_INPUT += playerId;
        B_INPUT += playerId;
        LEFT_TRIGGER_GAMEPAD += playerId;
        RIGHT_TRIGGER_GAMEPAD += playerId;
        A_GAMEPAD += playerId;
        B_GAMEPAD += playerId;
        X_GAMEPAD += playerId;
        HORIZONTAL_INPUT += playerId;
        VERTICAL_INPUT += playerId;
        LEFT_HORIZONTAL_GAMEPAD += playerId;
        LEFT_VERTICAL_GAMEPAD += playerId;
        RIGHT_HORIZONTAL_GAMEPAD += playerId;
        RIGHT_VERTICAL_GAMEPAD += playerId;
        START_INPUT += playerId;

        leftAimVector = Vector2.zero;
        rightAimVector = Vector2.zero;
    }

    void Update() {
        leftHorizontal = Input.GetAxis(LEFT_HORIZONTAL_GAMEPAD);
        leftVertical = Input.GetAxis(LEFT_VERTICAL_GAMEPAD);
        leftAimVector.x = leftHorizontal;
        leftAimVector.y = leftVertical;

        rightHorizontal = Input.GetAxis(RIGHT_HORIZONTAL_GAMEPAD);
        rightVertical = Input.GetAxis(RIGHT_VERTICAL_GAMEPAD);
        rightAimVector.x = rightHorizontal;
        rightAimVector.y = rightVertical;

        float triggerInput = Input.GetAxis(RIGHT_TRIGGER_GAMEPAD);
        if (triggerInput > .01f) {
            if (isRightAiming()) {
                rightTriggerDown = true;
                BroadcastMessage("Fire1Down", triggerInput);
            }
            if (rightTriggerDown) {
                BroadcastMessage("Fire1", triggerInput);
            }
        } else if (rightTriggerDown) {
            rightTriggerDown = false;
            BroadcastMessage("Fire1Up");
        }

        triggerInput = Input.GetAxis(LEFT_TRIGGER_GAMEPAD);
        if (triggerInput > .01f) {
            if (isLeftAiming()) {
                leftTriggerDown = true;
                BroadcastMessage("Fire2Down", triggerInput);
            }
            if (leftTriggerDown) {
                BroadcastMessage("Fire2", triggerInput);
            }
        } else if (leftTriggerDown) {
            leftTriggerDown = false;
            BroadcastMessage("Fire2Up");
        }
    }

    private bool isLeftAiming() {
        return leftAimVector.sqrMagnitude > .1f;
    }

    private bool isRightAiming() {
        return rightAimVector.sqrMagnitude > .1f;
    }

}