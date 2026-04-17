using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Season currentSeason = Season.SPRING;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Season GetSeason()
    {
        return currentSeason;
    }

    public void SetPlayer(GameObject player)
    {
        // Player script calls this method once spawned in
        this.player = player;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}

public enum Season
{
    SPRING,
    SUMMER,
    AUTUMN,
    WINTER
}
