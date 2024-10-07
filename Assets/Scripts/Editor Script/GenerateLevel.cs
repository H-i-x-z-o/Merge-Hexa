using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = Scripts.Grid;

namespace Editor
{
[CreateAssetMenu(fileName = "GenerateLevel", menuName = "GenerateLevel", order = 1)]
public partial class GenerateLevel : ScriptableObject
{
    [Header("Properties")]
    public string PathSavePrefabs;
    public int TotalNumber;
    public int TotalHexa;

    [Header("Move Hexagon")]
    public bool HasMoveHexa;
    public int TotalMoveHexa;
    private int cntMoveHexa;

    [Header("Mod Hexagon")]
    public bool HasModHexa;
    public int TotalModHexa;
    private int cntModHexa;

    [Header("Teleport Hexagon")]
    public bool HasTeleportHexa;


    private Hexagon previousHexa;
    private int rateMod;
    private int rateMove;
    public void GenLevel()
    {
        if (!Directory.Exists(PathSavePrefabs))
        {
            Directory.CreateDirectory(PathSavePrefabs);
        }

        rateMod = 0;

        GameObject parent = new GameObject();
        Stack<int> st = new Stack<int>();
        st.Push(TotalNumber);
        while (st.Count > 0)
        {
            int number = st.Pop();
            if (number > 1)
            {
                st.Push(number - 1);
                st.Push(number - 1);
            }
            else
            {
                  NumberHexagon numChild = CreateNumChild(number,parent);
                  updateNeighbors(parent,numChild);
                  if(HasModHexa && cntModHexa < TotalModHexa)
                  {
                      int randomMod = Random.Range(0, TotalHexa * 2);
                      if(randomMod <= rateMod)
                      {
                          Hexagon mod = GenModHex(parent);
                          updateNeighbors(parent,mod);
                          cntModHexa++;
                      }
                      rateMod++;
                  }

                  if (HasMoveHexa && cntMoveHexa < TotalMoveHexa)
                  {
                      int randomMove = Random.Range(0, TotalHexa * 2);
                      if(randomMove <= rateMove)
                      {
                          Hexagon move = GenMoveHex(parent);
                          updateNeighbors(parent,move);
                          cntMoveHexa++;
                      }
                      rateMove++;
                  }
                  
            }
            if(parent.transform.childCount >= TotalHexa)
            {
                break;
            }
        }

        parent.name = "New Level";
        string pathPrefab = PathSavePrefabs + parent.name + ".prefab";
        // PrefabUtility.SaveAsPrefabAsset(parent,pathPrefab);
        // DestroyImmediate(parent);
        previousHexa = null;
    }

    private void updateNeighbors(GameObject parent, Hexagon hexa)
    {
        for (int i = 0; i < 6; i++)
        {
            if(hexa.neighbors[i] == null)
            {
                foreach (Transform child in parent.transform)
                {
                    if(Vector3.Distance(child.position,hexa.transform.position + DirOffset[i]) < 0.1f)
                    {
                        hexa.neighbors[i] = child.GetComponent<Transform>();
                        child.GetComponent<Hexagon>().neighbors[(i + 3) % 6] = hexa.transform;
                    }
                }
            }
        }
    }


    private NumberHexagon CreateNumChild(int number, GameObject parent)
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Hexagon/NumberHexa")) as GameObject;
        NumberHexagon numberHexa = go.GetComponent<NumberHexagon>();
        numberHexa.number = number;
        go.transform.parent = parent.transform;
        go.name = parent.transform.childCount.ToString();
        if (previousHexa == null)
        {
            numberHexa.transform.position = Vector3.zero;
            previousHexa = numberHexa;
        }
        else
        {
            numberHexa.transform.position = randomNewPos();
            previousHexa = numberHexa;
        }
        return numberHexa;
    }

    private Vector3 randomNewPos()
    {
        Vector3 newPos = Vector3.zero;   // new position of numberHexa

        // find new position for numberHexa
        List<KeyValuePair<int,int>> a = new List<KeyValuePair<int, int>>();
        int total = 0;
        for (int i = 0; i < 6; i++)
        {
            if(previousHexa.neighbors[i] == null)
            {
                a.Add(new KeyValuePair<int, int>((1 + i) * 100, i));
                total += (1 + i) * 100;
            }
        }
        a.OrderByDescending(pair => pair.Key);
        int random = Random.Range(0, total);
        for(int i = 0; i < a.Count; i++)
        {
            if(random <= a[i].Key)
            {
                // found new position
                newPos = previousHexa.transform.position + DirOffset[a[i].Value];
                break;
            }
            random -= a[i].Key;
            
        }
        return newPos;

    }

    private List<Vector3> DirOffset = new List<Vector3>()
    {
        new Vector3(0,0.56f,0),new Vector3(0.48f,0.28f,0), new Vector3(0.48f,-0.28f,0),
        new Vector3(0,-0.56f,0), new Vector3(-0.48f,-0.28f,0), new Vector3(-0.48f,0.28f,0)
    };
}

}