using UnityEngine;
using UnityEngine.InputSystem;

public class FishCatch : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionReference fireReference;

    [Header("UI Elements")]
    [SerializeField] private GameObject fishCatchUI;
    [SerializeField] private RectTransform backgroundUI;
    [SerializeField] private RectTransform targetUI;
    [SerializeField] private RectTransform indicatorUI;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float minTargetsize = 10f;
    [SerializeField] private float maxTargetSize = 25f;
    [SerializeField] private int maxChances = 5;

    private GameManager gameManager;

    private float currentIndicatorPosition;
    private int currentDirection = 1;

    private float targetZoneCentre;
    private float targetZoneHalfWidth;

    private float leftBackgroundBound;
    private float rightBackgroundBound;

    private bool isPlaying;
    private bool gameEnded;

    private int chances;

    private bool outcome;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (!isPlaying || gameEnded) return;

        MoveIndicator();

        if (fireReference.action.WasPerformedThisFrame())
        {
            CheckSuccess();
        }

        if(chances >= maxChances) EndGame();
    }

    public void StartGame()
    {
        chances = 0;

        fishCatchUI.SetActive(true);

        leftBackgroundBound = -backgroundUI.rect.width / 2f;
        rightBackgroundBound = backgroundUI.rect.width / 2f;

        currentIndicatorPosition = leftBackgroundBound;
        UpdateIndicatorPosition();

        isPlaying = true;
        gameEnded = false;

        SetupIndicator();
        SetupTargetZone();
    }

    public void StartGame(float moveSpeed, float minTargetsize, float maxTargetsize, int maxChances)
    {
        Debug.Log($"FishCatch game started with {moveSpeed} movespeed, {minTargetsize} minTargetSize, {maxTargetSize} maxTargetSize, {maxChances} chances");

        this.moveSpeed = moveSpeed;
        this.minTargetsize = minTargetsize;
        this.maxTargetSize = maxTargetsize;
        this.maxChances = maxChances;

        StartGame();
    }
    
    public void EndGame()
    {
        gameEnded = true;
        isPlaying = false;

        gameManager.SetFishCatchOutcome(outcome);

        fishCatchUI.SetActive(false);
    }

    private void SetupIndicator()
    {
        indicatorUI.anchoredPosition = new Vector2(currentIndicatorPosition, backgroundUI.anchoredPosition.y);
    }

    private void SetupTargetZone()
    {
        float randomTargetZoneWidth = Random.Range(minTargetsize, maxTargetSize);

        targetZoneHalfWidth = randomTargetZoneWidth / 2f;

        float minCentre = leftBackgroundBound + targetZoneHalfWidth;
        float maxCentre = rightBackgroundBound - targetZoneHalfWidth;

        targetZoneCentre = Random.Range(minCentre, maxCentre);

        targetUI.sizeDelta = new Vector2(randomTargetZoneWidth, backgroundUI.sizeDelta.y);
        targetUI.anchoredPosition = new Vector2(targetZoneCentre, backgroundUI.anchoredPosition.y);
    }

    private void UpdateIndicatorPosition()
    {
        indicatorUI.anchoredPosition = new Vector2(currentIndicatorPosition, backgroundUI.anchoredPosition.y);
    }

    private void MoveIndicator()
    {
        currentIndicatorPosition += moveSpeed * currentDirection * Time.deltaTime;

        if(currentIndicatorPosition >= rightBackgroundBound)
        {
            currentIndicatorPosition = rightBackgroundBound;
            currentDirection = -1;
            chances++;
        }
        else if(currentIndicatorPosition < leftBackgroundBound)
        {
            currentIndicatorPosition = leftBackgroundBound;
            currentDirection = 1;
            chances++;
        }

        UpdateIndicatorPosition();
    }

    private void CheckSuccess()
    {
        if(currentIndicatorPosition >= (targetZoneCentre - targetZoneHalfWidth) && currentIndicatorPosition <= (targetZoneCentre + targetZoneHalfWidth))
        {
            // Success
            Win();
        }
        else
        {
            // Lost
            Lose();
        }

        EndGame();
    }

    private void Win()
    {
        outcome = true;
    }

    private void Lose()
    {

    }
}
