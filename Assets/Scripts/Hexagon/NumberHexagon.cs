using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scripts;
using UnityEngine;
using Grid = Scripts.Grid;

public class NumberHexagon : Hexagon
{
    public int number;
    public bool isMoving = false;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> numberSprites = new List<Sprite>();

    public override void Start()
    {
        base.Start();
        UpdateSprite();
    }

    public IEnumerator MergeNumHexagons(NumberHexagon otherNum, Vector3 firstHexPos, bool skipCheck = false)
    {
            
        if(!CanMergeNum(otherNum, firstHexPos, skipCheck)) yield break;
        // Debug.Log("Merge");
        AddDataMove(otherNum);
        number++;    // Increase the number of hexa1 by 1
        StartCoroutine(Move(otherNum.transform.position));
        yield return new WaitUntil(() => isMoving);
        UpdateSprite();
        otherNum.gameObject.SetActive(false);    // Deactivate hexa2

    }
    public bool CanMergeNum(NumberHexagon otherNum, Vector3 firstHexPos, bool skipCheck = false)
    {                                                                                   
        if(skipCheck)
        {
            if (number != otherNum.number)
            {
                // transform.position = firstHexPos;   // return hexa1 to its original position
                StartCoroutine(Move(firstHexPos));
                return false;
            }
            return true;
        }

        if (number != otherNum.number || transform == otherNum.transform || !neighbors.Contains(otherNum.transform)) 
        {
            // Debug.Log(isMoving);
            // if the numbers of hexa1 and hexa2 are different or hexa1 and hexa2 are the same, return
            StartCoroutine(Move(firstHexPos));   // return hexa1 to its original position
            return false;
        }
        return true;
    }

    public IEnumerator Move(Vector3 targetPos)
    {
        // yield return new WaitUntil(() => isMoving == false);
        transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        targetPos = new Vector3(targetPos.x, targetPos.y, -5);
        isMoving = true;
        transform.DOMove(targetPos,0.3f).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(0.3f);
        // UpdateSprite();
        isMoving = false;
        yield return new WaitForSeconds(0.005f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // Debug.Log("Move done");
    }

    public void AddDataMove(Hexagon otherNum)
    {
        Grid.Instance.AddDataMove(otherNum);
    }

    public void UpdateSprite()
    {
        spriteRenderer.sprite = numberSprites[number - 1];
    }
}