using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : singleton<DataManager>
{
    public string currentPlayerId { get; set; }
    private List<HighscoreEntry> highscores = new List<HighscoreEntry>();
    private const int MaxHighscores = 3;
    private const string SaveFileName = "highscoreList.json";

    [System.Serializable]
    public class HighscoreEntry
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    private class SaveData
    {
        public List<HighscoreEntry> highscores;
    }

    public override void Awake()
    {
        base.Awake();
        LoadHighscores();
    }

    public bool AddOrUpdateHighscore(string playerName, int score)
    {
        bool updated = false;
        HighscoreEntry existingEntry = null;
        for (int i = 0; i < highscores.Count; i++)
        {
            if (highscores[i].playerName == playerName)
            {
                existingEntry = highscores[i];
                break;
            }
        }
        
        if (existingEntry != null)
        {
            // Only update if the new score is higher
            if (score > existingEntry.score)
            {
                existingEntry.score = score;
                SortHighscores();
                updated = true;
            }
        }
        else
        {
            // Add new entry
            highscores.Add(new HighscoreEntry { playerName = playerName, score = score });
            SortHighscores();
            if (highscores.Count > MaxHighscores)
            {
                highscores.RemoveAt(highscores.Count - 1);
            }
            updated = true;
        }

        if (updated)
        {
            SaveHighscores();
        }

        return updated;
    }

    private void SortHighscores()
    {
        highscores.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private void SaveHighscores()
    {
        SaveData saveData = new SaveData { highscores = highscores };
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSaveFilePath(), json);
    }

    private void LoadHighscores()
    {
        string path = GetSaveFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            highscores = saveData.highscores;
        }
    }

    private string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

    public string GetFormattedHighscores()
    {
        string formattedHighscores = "";
        for (int i = 0; i < highscores.Count; i++)
        {
            formattedHighscores += $"{i + 1}. {highscores[i].playerName}: {highscores[i].score}\n";
        }
        return formattedHighscores.TrimEnd('\n');
    }

    public void ResetHighscores()
    {
        highscores.Clear();
        SaveHighscores();
        Debug.Log("Highscores have been reset.");
    }
}