using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] private Transform Content;
    [SerializeField] private Level LevelPrefab;
    [SerializeField] private List<Level> Levels;

    private void OnEnable()
    {
        Object[] levels = Resources.LoadAll("Prefabs/Levels/LevelsSO", typeof(ScriptableObject));
        for(int i = 0 ; i < levels.Length; i++)
        {
            var level = levels[i] as ScriptableObject;
            var levelInstance = Instantiate(LevelPrefab, Content);     
            levelInstance.gameObject.SetActive(true);
            levelInstance.name = (i + 1).ToString();
            levelInstance.GetComponentInChildren<TextMeshProUGUI>().text = level.name;
            int levelIndex = i + 1;
            Button button = levelInstance.GetComponent<Button>();
            button.onClick.AddListener(() => {GameManager.Instance.LoadLevel(levelIndex);});
            Levels.Add(levelInstance);
            if(SaveData.Instance.IsLevelUnlocked(levelIndex)) levelInstance.Unlock();
            else levelInstance.Lock();
        }
    }
}
