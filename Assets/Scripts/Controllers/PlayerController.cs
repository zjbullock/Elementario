using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Vector3 destination;

    [SerializeField]
    private bool isMoving = false;

    [SerializeField]
    private bool isTurning = false;

    [SerializeField]
    [Range(5f, 20f)]
    private float movementAnimationSpeed = 5f;

    [SerializeField]
    [Range(10f, 50f)]
    private float turnAnimationSpeed = 10f;


    [SerializeField]
    private Quaternion destinationRotation;

    private ElementarioInputs inputActions;

    void Awake() {
        inputActions = new ElementarioInputs();
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Turn.started += OnTurn;
        inputActions.Player.Strafe.started += OnStrafe;
    }

    // Start is called before the first frame update
    void Start() {

    }

    void OnEnable() {
        inputActions.Enable();
    }

    void OnDisable() {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        this.PerformTurn();
        this.PerformMove();
    }

    bool IsActing {get => this.isMoving || this.isTurning;}

    void PerformMove() {
        if (!this.IsActing) {
            return;
        }
        this.transform.position = Vector3.Lerp(this.transform.position, this.destination, Time.fixedDeltaTime * this.movementAnimationSpeed);
        if (Vector3.Distance(this.transform.position, this.destination) <= 0.1f) {
            this.transform.position = this.destination;
            this.isMoving = false;
        }
    }

    void PerformTurn() {
        if(!this.IsActing) {
            return;
        }
        
        transform.rotation = Quaternion.Lerp(transform.rotation, this.destinationRotation, Time.fixedDeltaTime * this.turnAnimationSpeed);
        if (this.RotationEquals(transform.rotation, this.destinationRotation)) {
            this.transform.rotation = this.destinationRotation;
            this.isTurning = false;
        }
    }

    bool RotationEquals(Quaternion r1, Quaternion r2) {
        float abs = Mathf.Abs(Quaternion.Dot(r1, r2));
        if (abs >= 0.999999f)
            return true;
        return false;
    }

    void OnMove(InputAction.CallbackContext context) {
        if (this.IsActing) {
            return;
        }
        Vector2 movementValue = context.ReadValue<Vector2>();
        Vector3 finalDestination = this.transform.position + transform.up * movementValue.y;
        finalDestination.x = Mathf.RoundToInt(finalDestination.x);
        finalDestination.y = Mathf.RoundToInt(finalDestination.y);
        this.destination = finalDestination;
        this.isMoving = true;
    }

    void OnStrafe(InputAction.CallbackContext context) {
        if (this.IsActing) {
            return;
        }
        Vector2 movementValue = context.ReadValue<Vector2>();
        Vector3 finalDestination = transform.position + transform.right * movementValue.x;
        finalDestination.x = Mathf.RoundToInt(finalDestination.x);
        finalDestination.y = Mathf.RoundToInt(finalDestination.y);
        this.destination = finalDestination;
        this.isMoving = true;
    }

    void OnTurn(InputAction.CallbackContext context) {
        if(this.IsActing) {
            return;
        }
        Vector2 turnValue = context.ReadValue<Vector2>();

        Vector3 newAngle = transform.rotation.eulerAngles;
        newAngle.z += Mathf.RoundToInt(-90 * turnValue.x);
        this.destinationRotation = Quaternion.Euler(newAngle);
        this.isTurning = true;
    }
}
