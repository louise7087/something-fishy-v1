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

    [Header("Input Settings")]
    [SerializeField] InputActionReference move;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private Vector2 currentInput;
    private Vector2 facingDirection;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 input = move.action.ReadValue<Vector2>();
        MovePlayer(input);
        SetAnimation();
    }

    private void MovePlayer(Vector2 input)
    {

        if (lockMovement) input = Vector2.zero;

        rb.linearVelocity = input * speed;

        if (input == Vector2.zero)
        {
            isMoving = false;
            return;
        }

        facingDirection = input;
        isMoving = true;
    }

    private void SetAnimation()
    {
        spriteRenderer.flipX = facingDirection.x > 0; // Flips sprite to face to the right if player is facing the right

        if (isMoving)
        {
            // Walking Animations
            animator.SetBool("isMoving", true);
        }
        else
        {
            // Idle Animations
            animator.SetBool("isMoving", false);
        }

        if(facingDirection.x < 0 && facingDirection.y < 0)
        {
            // Edge case where player is moving diagonally down and to the left.
            // The math function below rounds it to 0 so we will manually set animation
            animator.SetFloat("X", -1f);
            animator.SetFloat("Y", -1f);
            return;
        }

        animator.SetFloat("X", Mathf.CeilToInt(facingDirection.x));
        animator.SetFloat("Y", Mathf.CeilToInt(facingDirection.y));
    }
}
