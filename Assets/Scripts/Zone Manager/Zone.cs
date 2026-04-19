using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private string id;

    public string GetId()
    {
        return id;
    }
}
