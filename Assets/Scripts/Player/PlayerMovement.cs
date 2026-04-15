using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] public bool lockMovement;

    [SerializeField] InputActionReference move;

    private Rigidbody2D rb;


    private Vector2 currentInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 input = move.action.ReadValue<Vector2>();
        Debug.Log(input);
        MovePlayer(input);
    }

    private void MovePlayer(Vector2 input)
    {
        if (lockMovement) return;
        rb.linearVelocity = input * speed;
    }
}
