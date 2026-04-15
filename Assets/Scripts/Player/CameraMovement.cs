using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementInterpolationSpeed = 5f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomInterpolationSpeed = 5f;
    [SerializeField] private float minOrthographicSize = 2f;
    [SerializeField] private float maxOrthographicSize = 15f; 
    [SerializeField] InputActionReference zoom;

    private GameObject player;



    private float targetOrthographicSize;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = zoom.action.ReadValue<float>();
        if(scroll != 0)
        {
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize - Mathf.Sign(scroll), minOrthographicSize, maxOrthographicSize);
        }

        if(!Mathf.Approximately(Camera.main.orthographicSize, targetOrthographicSize))
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, zoomInterpolationSpeed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        targetOrthographicSize = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), movementInterpolationSpeed * Time.deltaTime);
    }
}
