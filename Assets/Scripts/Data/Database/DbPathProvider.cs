using System.IO;
using UnityEngine;

public static class DbPathProvider
{
    public static string GetDatabasePath()
    {
        return Path.Combine(Application.persistentDataPath, "somethingfishy.db");
    }
}