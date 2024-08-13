using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = Scripts.Grid;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("Manager")]
    public UIManager UIManager;
    public CountDown countDown;

    [Header("Level Manager properties")]
    public List<LevelSO> LevelPrefabs = new List<LevelSO>();
    public int currentLevel = 0;
    public Transform LevelPool;

    [Header("Manager this level")]
    public Grid grid;
    public State state = State.None;
    public TextMeshProUGUI freezeCountText;
    public int freezeCount = 0;
    [HideInInspector] public List<Transform> PrefabsHexagon = new List<Transform>(); 
    [HideInInspector] public List<DataMove> dataMoves = new List<DataMove>();
     public List<NumberHexagon> numberHexagons = new List<NumberHexagon>();

    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        // DontDestroyOnLoad(this);
        if(grid == null) grid = FindObjectOfType<Grid>();
        
    }

    private void Start()
    {
        currentLevel = GameManager.Instance.LevelIndex - 1;
        LoadLevel();
    }

    public void Update()
    {
        grid.TouchScreen();
    }

    public void LoadLevel()
    {
        if(LevelPool != null) Destroy(LevelPool.gameObject);
        state = State.Playing;
        Transform pool = LevelPrefabs[currentLevel].Prefab;
        LevelPool = Instantiate(pool, Vector3.zero, Quaternion.identity);
        LevelPool.SetParent(grid.transform);
        grid.pools = LevelPool;  
        grid.LoadPool();
        UIManager.LoadLevel(currentLevel);
        foreach (Transform hexa in LevelPool)
        {
            if(hexa.TryGetComponent<NumberHexagon>(out NumberHexagon numberHexagon))
            {
                numberHexagons.Add(numberHexagon);
            }
        }
        countDown.LoadTime(LevelPrefabs[currentLevel].Time);
        freezeCount = LevelPrefabs[currentLevel].FreezeCount;
        UIManager.Instance.FreezeCountText.text = freezeCount.ToString();
        UIManager.HidePanel();
    }

    public void RestartLevel()
    {
        dataMoves.Clear();
        numberHexagons.Clear();
        LoadLevel();
    }

    public void NextLevel()
    {
        currentLevel++;
        if(currentLevel >= LevelPrefabs.Count) currentLevel = 0;
        dataMoves.Clear();
        numberHexagons.Clear();
        LoadLevel();

    }

    public void Undo()
    {
        // Debug.Log("Undo");
        if(dataMoves.Count == 0) return;
        DataMove dataMove = dataMoves[dataMoves.Count - 1];
        if (dataMove.firstHex == null || dataMove.firstHex.Hexa == null)
        {
            Debug.LogError("firstHex or Hexa is null");
            return;
        }
        if(dataMove.secondHex != null && dataMove.secondHex.gameObject != null) dataMove.secondHex.gameObject.SetActive(true);
        dataMove.firstHex.Hexa.number = dataMove.firstHex.PreNum;
        dataMove.firstHex.Hexa.transform.position = dataMove.firstHex.PrePos;
        dataMove.firstHex.Hexa.UpdateSprite();
        grid.UpdateNeighbors();
        dataMoves.RemoveAt(dataMoves.Count - 1);
    }

    public void FreezeGame(int time)
    {
        if(freezeCount <= 0) return;
        freezeCount--;
        UIManager.Instance.FreezeCountText.text = freezeCount.ToString();
        StartCoroutine(countDown.FreezeTime(time));
    }

    public void CheckWinCondition()
    {
        
         int count = 0;
         foreach (var num in numberHexagons)
         {
             if(num.gameObject.activeSelf) count++;
         }
         
         if (count == 1)
         {
             state = State.GameOver;
             UIManager.Instance.ShowWinPanel();
             SoundManager.PlaySound(SoundType.Win);
             SaveData.Instance.Save(currentLevel + 2);
         }
         else if(countDown.timeLeft <= 0)
         {
             state = State.GameOver;
             UIManager.Instance.ShowLosePanel();
             SoundManager.PlaySound(SoundType.Lose);
         }
         Debug.Log(count);
    }

    public void Reset()
    {
        LoadComponent();
    }

    private void LoadComponent()
    {
        LevelPrefabs.Clear();
        LevelPrefabs = Resources.LoadAll<LevelSO>("Prefabs/Levels/LevelsSO").ToList();
        UIManager = FindObjectOfType<UIManager>();
        countDown = FindObjectOfType<CountDown>();
        grid = FindObjectOfType<Grid>();
        
    }

}
 [Serializable]
public class DataMove 
{
    [SerializeReference] public StateHexa firstHex;
    [SerializeReference] public Hexagon secondHex;

    public DataMove()
    {
        firstHex = new StateHexa();
        secondHex = null;
    }

}

public enum State
{
    None,
    Playing,
    Stop,
    GameOver,
    Freeze,

}
