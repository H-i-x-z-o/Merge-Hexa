using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int LevelIndex = 1;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // DontDestroyOnLoad(this);
    }

    public void LoadLevel(int levelIndex)
    {
        if(!SaveData.Instance.IsLevelUnlocked(levelIndex))    return;
        LevelIndex = levelIndex;
        Debug.Log(levelIndex);
        StartCoroutine(LoadLevelCoEnumerator());
    }

    public IEnumerator LoadLevelCoEnumerator()
    {
        StartCoroutine(UIHomeManager.Instance.Transition());
        yield return new WaitForSeconds(0.3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void HomeToLevel()
    {
        StartCoroutine(UIHomeManager.Instance.HomeToLevel());
    }

    public void LevelToHome()
    {
        StartCoroutine(UIHomeManager.Instance.LevelToHome());
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
