using System.Collections;
using System.Linq;
using UnityEngine;
using Grid = Scripts.Grid;

public class ModNumHexagon : Hexagon
{
    public int mod;
    private void Awake()
    {
        type = Type.Number;
    }
    public IEnumerator ModHexNum(NumberHexagon numHex,  Vector3 firstHexPos, bool skipCheck = false)
    {
        StartCoroutine(numHex.Move(transform.position));     
        yield return new WaitUntil(() => !numHex.isMoving);
        if(!CanModNum(numHex, firstHexPos,skipCheck)) yield break;
        AddDataMove(this);
        // Debug.Log("Mod");
        gameObject.SetActive(false);
        ModNumber(numHex);
        numHex.UpdateSprite();
    }

    public bool CanModNum(NumberHexagon numHex,  Vector3 firstHexPos, bool skipCheck = false)
    {
        // Debug.Log(numHex.number);
        // if skipCheck is true only check if sum of number and mod is greater than 0
        if (skipCheck)
        {
            if (this.mod < 0 && numHex.number <= 1)
            {
                // return hexa1 to its original position
                StartCoroutine(numHex.Move(firstHexPos));
                return false;
            }
            return true;
        }
        if((numHex.number <= 1 && mod < 0) || !numHex.neighbors.Contains(transform))
        {
            StartCoroutine(numHex.Move(firstHexPos));
            return false;
        }
        return true;
    }

    public void AddDataMove(Hexagon otherNum)
    {
        Grid.Instance.AddDataMove(otherNum);
    }

    public void ModNumber(NumberHexagon numHex)
    {
        numHex.number += mod;
    }
}