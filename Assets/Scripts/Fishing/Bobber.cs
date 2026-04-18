using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float timer;
    private float delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            if (timer > delay) Disable();

            timer += Time.deltaTime;
        }
    }

    public void Enable()
    {
        spriteRenderer.enabled = true;
    }

    public void Disable()
    {
        spriteRenderer.enabled = false;
        delay = 0;
        timer = 0;
    }

    public void DelayedDisable(float delay)
    {
        this.delay = delay;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
}
