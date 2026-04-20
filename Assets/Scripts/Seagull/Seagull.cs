using UnityEngine;

public class Seagull : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeBetweenFlight;
    [SerializeField] private float deviation;
    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float tolerance = 0.5f;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 targetPosition;
    private float waitTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PickNewTarget();
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var diff = (Vector2)(transform.position - targetPosition);

        if(diff.sqrMagnitude < tolerance * tolerance)
        {
            animator.SetBool("isFlying", false);
            waitTimer += Time.deltaTime;

            if(waitTimer >= timeBetweenFlight)
            {
                PickNewTarget();
                waitTimer = 0;
            }

            return;
        }

        if(targetPosition.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        animator.SetBool("isFlying", true);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void PickNewTarget()
    {
        targetPosition = Random.insideUnitCircle * maxDistanceFromPlayer;
    }
}
