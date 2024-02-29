using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class StatsGenerate : MonoBehaviour
{
    [SerializeField] private List<StatsSpace> _stats;


    private IEnumerator Start()
    {
        yield return StartCoroutine(DataBase.InitializeDatabase());

        LoadStats();
    }

    [ContextMenu("LoadStats")]
    public void LoadStats()
    {
        foreach (var stat in _stats)
        {
            QueryAndApplyStat(stat.Table, stat);
        }
    }

    public void RefreshStats(string rType)
    {
        foreach (var stat in _stats)
        {
            if (rType == stat.Table)
            {
                QueryAndApplyStat(rType, stat);
            }
        }
    }

    private void QueryAndApplyStat(string table, StatsSpace stat)
    {
        try
        {
            string query = $"SELECT {table} FROM Players ORDER BY RANDOM() LIMIT 2;";
            string result = DataBase.ExecuteScalarQuery(query);

            if (result == "")
                RefreshStats(table);          

            stat.TextBoxMessage(result);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ќе удалось запросить статистику дл€ таблицы. {table}: {ex.Message}");
        }
    }
}