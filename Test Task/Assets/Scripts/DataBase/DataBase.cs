using UnityEngine.Networking;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

static class DataBase
{
    private const string FileName = "Statistic.db.bytes";
    private static string DatabasePath;

    public static IEnumerator InitializeDatabase()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                string filePath = System.IO.Path.Combine(Application.persistentDataPath, FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    yield return CopyDatabaseToPersistentDataPath(filePath);
                }
                DatabasePath = filePath;
                break;

            default: // Assuming Editor as default
                DatabasePath = System.IO.Path.Combine(Application.streamingAssetsPath, FileName);
                break;
        }
    }

    private static IEnumerator CopyDatabaseToPersistentDataPath(string toPath)
    {
        string fromPath = System.IO.Path.Combine(Application.streamingAssetsPath, FileName);
        using (UnityWebRequest www = UnityWebRequest.Get(fromPath))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError($"Failed to load database from {fromPath}: {www.error}");
            }
            else
            {
                System.IO.File.WriteAllBytes(toPath, www.downloadHandler.data);
            }
        }
    }

    public static void ExecuteNonQuery(string query)
    {
        using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    public static string ExecuteScalarQuery(string query)
    {
        using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                var result = command.ExecuteScalar();
                connection.Close();
                return result?.ToString();
            }
        }
    }

    public static DataTable ExecuteQuery(string query)
    {
        using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
        {
            connection.Open();
            using (var adapter = new SqliteDataAdapter(query, connection))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                connection.Close();

                return dataSet.Tables[0];
            }
        }
    }
}