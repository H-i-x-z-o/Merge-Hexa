using System.Collections;
using System.Linq;
using UnityEngine;
using Grid = Scripts.Grid;

public class MoveHexArrow : Hexagon
{
    public Direction direction;
    public Transform icon;

    public override void Start()
    {
        base.Start();
        type = Type.Move;
        int iconDir = (int)direction - 3;
        icon.rotation = Quaternion.Euler(0, 0, -60 * iconDir);
    }

    public IEnumerator MoveArrow(NumberHexagon num,  Vector3 firstHexPos, bool skipCheck = false)
    {
        // Debug.Log("Move");
        if(!IsNeighbor(num, skipCheck)) yield break;
        StartCoroutine(num.Move(transform.position));
        yield return new WaitUntil((() => num.isMoving == false));
        if(!CanMoveArrow(num, firstHexPos, skipCheck)) yield break;
        Move(num);
        yield return new WaitForSeconds(0.2f);
        // yield return new WaitUntil((() => num.isMoving == false));
        int inDir = ((int)direction + 3) % 6;
        StartCoroutine(Grid.Instance.AfteMove(num,inDir));
        // Grid.Instance.AfteMove(num,inDir);
        // yield return new WaitUntil((() => num.isMoving == false));
        // Grid.Instance.UpdateNeighbors();
    }

    public bool IsNeighbor(NumberHexagon num, bool skipCheck = false)
    {
        return num.neighbors.Contains(transform) || skipCheck;
    }

    public bool CanMoveArrow(NumberHexagon num,  Vector3 firstHexPos, bool skipCheck = false)
    {
        if (num.transform == neighbors[(int)direction] && !skipCheck)
        {
            Debug.Log("Can't Move Arrow");
            StartCoroutine(BackToOriginalPos(num, firstHexPos));
            return false;
        }
        return true;
    }

    public bool Move(NumberHexagon num)
    {
        // if(neighbors[(int)direction] != null) return false;
        Vector3 offset = Grid.Instance.DirOffset[(int)direction];
        StartCoroutine(num.Move(transform.position + offset));
        // num.transform.position = transform.position + offset;
        return true;
    }

    public IEnumerator BackToOriginalPos(NumberHexagon num, Vector3 firstHexPos)
    {
        Debug.Log("Back To Original Pos");
        yield return new WaitUntil((() => num.isMoving == false));
        StartCoroutine(num.Move(firstHexPos));
    }
}