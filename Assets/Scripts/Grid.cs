using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Grid : MonoBehaviour
    {
        public static Grid Instance;
        [Header("Grid properties")]
        public Camera mainCamera;
        public List<Vector3> DirOffset = new List<Vector3>()
        {
            new Vector3(0,0.56f,0),new Vector3(0.48f,0.28f,0), new Vector3(0.48f,-0.28f,0),
            new Vector3(0,-0.56f,0), new Vector3(-0.48f,-0.28f,0), new Vector3(-0.48f,0.28f,0)
        };

        public Transform pools;
        public List<Hexagon> hexagons;
        [Header("Hexagon picked properties")]
        public NumberHexagon hexa1;
        public Hexagon hexa2;
        public Vector3 firstHexPos;
        public StateHexa stateHexa = new StateHexa();

        private void Awake()
        {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
            
        }

        public void LoadPool()
        {
            hexagons = new List<Hexagon>();
            foreach (Transform Hexagon in pools)
            {
                hexagons.Add(Hexagon.GetComponent<Hexagon>());
            }
        }

        public void TouchScreen()
        {
            if(LevelManager.Instance.state != State.Playing) return;

            if(Input.touches.Length == 1)
            {
                // Debug.Log("Touching");
                
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (touch.phase == TouchPhase.Began)
                {
                    if (touchedCollider != null && touchedCollider.TryGetComponent<NumberHexagon>(out hexa1) )
                    {
                        firstHexPos = hexa1.transform.position;
                        hexa1.ShowBorder(true);
                        stateHexa.Hexa = hexa1;
                        stateHexa.PreNum = hexa1.number;
                        stateHexa.PrePos = hexa1.transform.position;
                        SoundManager.PlaySound(SoundType.HexagonClick);
                        // Debug.Log(stateHexa.Hexa + " " + stateHexa.PreNum + " " + stateHexa.PrePos);
                    }
                }
                else if (touch.phase == TouchPhase.Moved && hexa1 != null)
                {
                    Hexagon tmp = hexa2;
                    TeleHexagon telePre = hexa2 as TeleHexagon;
                    
                    if (touchedCollider != null && touchedCollider.TryGetComponent<Hexagon>(out hexa2))
                    {                                                     
                        // if hexa1 different from hexa2 then show border
                        if(hexa1.transform != hexa2.transform)
                        {
                            hexa2.ShowBorder(false, hexa1); // show white border if they are neighbors

                            TeleHexagon tele = hexa2 as TeleHexagon;
                            if(tele != null && tele != telePre)    tele.PreviewTele(hexa1);
                        }

                    }
                    // touch outside of hexa2 previous 
                    if(tmp != null && tmp != hexa2 && tmp != hexa1) tmp.HideBorder();
                    if(telePre != null && telePre != hexa2) telePre.StopShowPreview();
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    StartCoroutine(Merge());
                    
                }
                
            }
            // else
            // {
            //     if(hexa1!= null && hexa1.isMoving)  return;
            //     hexa1 = null;
            //     hexa2 = null;
            // }
        }

        public IEnumerator Merge()
        {
            if (hexa1 == null || hexa2 == null) yield break;
            switch (hexa2)
            {
                case NumberHexagon otherNum:
                    StartCoroutine(hexa1.MergeNumHexagons(otherNum,firstHexPos));
                    break;
                case ModNumHexagon modNum:
                    StartCoroutine(modNum.ModHexNum(hexa1,firstHexPos));
                    break;
                case MoveHexArrow moveHexArrow:
                    StartCoroutine(moveHexArrow.MoveArrow(hexa1,firstHexPos));
                    break;
                case TeleHexagon teleHex:
                    StartCoroutine(teleHex.Tele(hexa1));
                    break;

            }

            yield return new WaitUntil(() => hexa1.isMoving == false);
            
            hexa1.HideBorder();
            hexa2.HideBorder();
            if(hexa2 as TeleHexagon != null) (hexa2 as TeleHexagon).StopShowPreview();
            hexa1 = null;
            hexa2 = null;
            // Debug.Log("Update Neighbors merge");
            LevelManager.Instance.CheckWinCondition();
            UpdateNeighbors();
        }

        public IEnumerator AfteMove(NumberHexagon num, int inDir = -1)
        {   
            Hexagon hexa = null;
            foreach(Hexagon hexagon in hexagons)
            {
                if(hexagon != num && Vector2.Distance(hexagon.transform.position, num.transform.position) < 0.1f && hexagon.gameObject.activeSelf)
                {
                    hexa = hexagon;
                    break;
                }
            }
            if(hexa == null) yield break;
            // if(hexa == null) return;
            // Debug.Log(hexa.name);
            switch (hexa)
            {
                case NumberHexagon numHex:
                    StartCoroutine(num.MergeNumHexagons(numHex, firstHexPos, true));
                    break;
                case ModNumHexagon modHex:
                    StartCoroutine(modHex.ModHexNum(num, firstHexPos, true));
                    break;
                case MoveHexArrow moveArrow:
                    StartCoroutine(moveArrow.MoveArrow(num, firstHexPos, true));
                    break;
                case TeleHexagon teleHex:
                    StartCoroutine(teleHex.Tele(num, inDir, true));
                    break;
            }

            yield return new WaitUntil((() => num.isMoving == false));
            // yield return new WaitForSeconds(0.3f);
            Debug.Log("Update Neighbors after move");
            UpdateNeighbors();
        }

        public void DeleteConnection(Hexagon hexa1, Hexagon hexa2)
        {
            for (int i = 0; i < 6; i++)
            {
                if (hexa1.neighbors[i] == hexa2.transform)
                {
                    hexa1.neighbors[i] = null;
                    hexa2.neighbors[(i + 3) % 6] = null;
                    break;
                }
            }
        }

        public void UpdateNeighbors()
        {
            foreach (var hexa in hexagons)
            {
                hexa.UpdateNeighbors();
            }
        }

        public void AddDataMove(Hexagon otherHexa)
        {

            DataMove dataMove = new DataMove();
            if (stateHexa.Hexa == null)
            {
                Debug.Log("can't add data move because stateHexa is null");
                return;
            }
            dataMove.firstHex = stateHexa;
            dataMove.secondHex = otherHexa;
            LevelManager.Instance.dataMoves.Add(dataMove);
            stateHexa = new StateHexa();
        }
    }

    


}
public enum Direction
{
    Top,
    TopRight,
    BotRight,
    Bot,
    BotLeft,
    TopLeft,
}
