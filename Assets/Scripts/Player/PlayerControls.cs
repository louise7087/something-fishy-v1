using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] public bool lockMovement;

    [Header("Input Settings")]
    [SerializeField] InputActionReference moveReference;
    [SerializeField] InputActionReference fireReference;
    [SerializeField] InputActionReference inventoryReference;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float bobberDisableDelay = 1f;

    private InventoryUI inventoryUI;
    private MarketplaceUI marketplaceUI;

    private GameManager gameManager;

    private Rigidbody2D rb;

    private Vector2 currentInput;
    private Vector2 facingDirection;

    private bool isMoving;
    private bool isFishing;

    private Inventory inventory;

    private Camera mainCamera;

    private Bobber bobber;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = GetComponent<Inventory>();
        mainCamera = Camera.main;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        inventoryUI = GameObject.FindWithTag("InventoryUI").GetComponent<InventoryUI>();
        marketplaceUI = GameObject.FindWithTag("MarketplaceUI").GetComponent<MarketplaceUI>();
        bobber = GameObject.FindWithTag("Bobber").GetComponent<Bobber>();

        inventoryUI.Init();
        marketplaceUI.Init();

        // When player spawns in, enable the camera movement script
        mainCamera.GetComponent<CameraMovement>().enabled = true;

        // For debug add basic rod
        if(!inventory.ContainsItem("rod.rustline"))
        {
            inventory.AddItem("rod.rustline");
        }

        inventory.EquipItemAtIndex(0);
    }

    private void Update()
    {
        if (fireReference.action.WasPressedThisFrame())
        {
            Fire();
        }

        if(inventoryReference.action.WasPressedThisFrame())
        {
            inventoryUI.Toggle();
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = moveReference.action.ReadValue<Vector2>();
        MovePlayer(input);
        SetAnimation();
    }

    private void Fire()
    {
        // Called when player fires

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (inventory.GetEquippedItem() is RodEntry && !isFishing)
        {
            // Player has equipped rod

            bobber.SetPosition(mousePosition);
            bobber.Enable();

            if(hit.collider != null)
            {
                // We hit something
                if (hit.collider.CompareTag("FishShadow"))
                {
                    // Player hit fish shadow
                    gameManager.StartFishCatch(hit.collider.GetComponent<FishShadow>());
                    isFishing = true;
                }
                else
                {
                    bobber.DelayedDisable(bobberDisableDelay);
                }
            }
        }

        // Debug logic for zone unlock
        if(hit.collider != null)
        {
            if(hit.collider.CompareTag("Zone"))
            {
                var zone = hit.collider.GetComponent<Zone>();
                gameManager.UnlockZone(zone.GetId());
            }
        }

        if(hit.collider != null && hit.collider.CompareTag("Marketplace"))
        {
            marketplaceUI.Toggle();
        }
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
        spriteRenderer.flipX = facingDirection.x < 0; // Flips sprite to face to the right if player is facing the right

        if (facingDirection.y > 0)
        {
            // Put whatever object in player hands behind player sprite
            inventory.SetInHandLayer(-1);
        }
        else
        {
            inventory.SetInHandLayer(1);
        }

        if(facingDirection.x < 0)
        {
            // If facing the left, put item in left hand
            inventory.SetHand(false);
        }
        else
        {
            inventory.SetHand(true);
        }

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

    public void FinishedFishing()
    {
        Debug.Log("Not fishing anymore");
        bobber.Disable();
        isFishing = false;
    }
}
