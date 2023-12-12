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
    [Range(1f, 10f)]
    private float movementAnimationSpeed = 2f;

    [SerializeField]
    private float turn;

    private ElementarioInputs inputActions;

    void Awake() {
        inputActions = new ElementarioInputs();
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Turn.started += OnTurn;
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

    void PerformMove() {
        if (!this.isMoving) {
            return;
        }
        this.transform.position = Vector3.Lerp(this.transform.position, this.destination, Time.fixedDeltaTime * this.movementAnimationSpeed);
        if (Vector3.Distance(this.transform.position, this.destination) <= 0.1f) {
            this.transform.position = this.destination;
            this.isMoving = false;
        }
    }

    void PerformTurn() {
        if(this.turn == 0f) {
            return;
        }
        Debug.Log("turn");
        this.transform.Rotate(0, 0, -90 * this.turn);
        this.turn = 0f;
    }

    void OnMove(InputAction.CallbackContext context) {
        if (this.isMoving) {
            return;
        }
        Vector2 movementValue = context.ReadValue<Vector2>();
        Debug.Log(movementValue.y);
        this.destination = this.transform.position + transform.up * movementValue.y;
        this.destination.x = Mathf.RoundToInt(this.destination.x);
        this.destination.y = Mathf.RoundToInt(this.destination.y);
        this.isMoving = true;
    }

    void OnTurn(InputAction.CallbackContext context) {
        if(this.isMoving) {
            return;
        }
        Vector2 turnValue = context.ReadValue<Vector2>();
        this.turn = turnValue.x;
    }
}
