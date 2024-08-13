using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;
    public List<int> UnlockedLevels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }

    public void Init()
    {
        // PlayerPrefs.DeleteAll();
        // Debug.Log(PlayerPrefs.HasKey("Level 1"));
        if(PlayerPrefs.HasKey("Level 1"))  return;
        Save(1);

    }

    public void Save(int level)
    {
        if(IsLevelUnlocked(level)) return;
        PlayerPrefs.SetInt("Level " + level, 1);
    }

    public bool IsLevelUnlocked(int level)
    {
        // Debug.Log(level);
        return PlayerPrefs.HasKey("Level " + level);
    }
}
