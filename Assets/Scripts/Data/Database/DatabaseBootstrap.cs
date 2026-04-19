using UnityEngine;

public class DatabaseBootstrap : MonoBehaviour
{
    private void Awake()
    {
        string dbPath = DbPathProvider.GetDatabasePath();

        using var db = new GameDbContext(dbPath);
        db.Database.EnsureCreated();
    }
}