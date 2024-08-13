using System;
using System.Linq;
using UnityEngine;
using Grid = Scripts.Grid;
[Serializable]
public class Hexagon : MonoBehaviour
{
    public Type type;
    public Transform blackBorder;
    public Transform whiteBorder;
    public Transform[] neighbors = new Transform[6];

    public virtual void Start()
    {
        UpdateNeighbors();
    }


    public void ShowBorder(bool isBlack, Hexagon other = null)
    {
        blackBorder.gameObject.SetActive(isBlack);
        if(!isBlack && neighbors.Contains(other.transform)) whiteBorder.gameObject.SetActive(!isBlack);
    }

    public void HideBorder()
    {
        blackBorder.gameObject.SetActive(false);
        whiteBorder.gameObject.SetActive(false);
    }


    public void UpdateNeighbors()
    {
        // if(this.name == "Tele2") Debug.Log("Update in Hexagon", gameObject);
        neighbors = new Transform[6];
        var hexagons = Grid.Instance.hexagons;
        foreach (var hexa in hexagons)
        {
            if(!hexa.gameObject.activeSelf) continue;
            for (int i = 0; i < 6; i++)
            {
                Vector2 neighborPos = transform.position + Grid.Instance.DirOffset[i];
                Vector2 checkPos = hexa.transform.position;
                     
                if(Vector2.Distance(neighborPos,checkPos) < 0.1f)
                {
                    neighbors[i] = hexa.transform;
                }
            }
        }
    }
}

public enum Type
{
    Number,
    Move,
}