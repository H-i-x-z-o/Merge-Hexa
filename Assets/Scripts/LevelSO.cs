using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "LevelSO")]
public class LevelSO : ScriptableObject
{
    public Transform Prefab;
    public int Time;
    public int FreezeCount;
}
