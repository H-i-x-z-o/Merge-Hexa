using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Grid = Scripts.Grid;

public class TeleHexagon : Hexagon
{
    public TeleHexagon TeleOut;
    public Transform PreOut;
    public bool isShowing = false;


    public IEnumerator Tele(NumberHexagon num, int inDir = -1 , bool skipCheck = false)
    {
        if(inDir == -1)
        {
            for (int i = 0; i < 6; i++)
            {
                if (num.transform == neighbors[i])
                {
                    inDir = i;
                    break;
                }
            }
        }
        if(inDir == -1) yield break;
        StartCoroutine(num.Move(transform.position));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(MoveToTeleOut(num));
        yield return new WaitUntil(() => num.transform.position == TeleOut.transform.position);
        // Debug.Log("inDir: " + inDir);
            
        int outDir = (inDir + 3) % 6;
        Vector3 OutPos = TeleOut.transform.position + Grid.Instance.DirOffset[outDir];
        StartCoroutine(num.Move(OutPos));
        // if(!CanTele(num, skipCheck)) yield break;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Grid.Instance.AfteMove(num));
        yield return new WaitForSeconds(0.3f);
        if(neighbors[outDir] == null)   Grid.Instance.AddDataMove(null);
        Debug.Log("Update Neighbors after Tele");
        Grid.Instance.UpdateNeighbors();
    }



    public bool CanTele(NumberHexagon num, bool skipCheck = false)
    {
        if (!neighbors.Contains(num.transform) && !skipCheck)
        {
            StartCoroutine(BackToOriginalPos(num, Grid.Instance.firstHexPos));
            StopShowPreview();
        }
        Debug.Log("Can Tele");
        return true;
    }

    public IEnumerator MoveToTeleOut(NumberHexagon num)
    {
        num.transform.DOScale(0, 0.2f).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(0.2f);
        num.transform.position = TeleOut.transform.position;
        num.transform.DOScale(1, 0.2f).SetEase(Ease.InBounce);
        StartCoroutine(num.Move(TeleOut.transform.position));
    }

    public IEnumerator BackToOriginalPos(NumberHexagon num, Vector3 firstHexPos)
    {
        // Debug.Log("Back To Original Pos");
        yield return new WaitUntil((() => num.isMoving == false));
        StartCoroutine(num.Move(firstHexPos));
            
    }

    public void PreviewTele(Hexagon hexa)
    {
        int OutDir = -1;
        for(int i = 0; i < 6; i++)
        {
            if(neighbors[i] == hexa.transform)
            {
                OutDir = i;
                // OutDir = (i + 3) % 6;
                break;
            }
        }

        if (OutDir == -1) return;
        // Debug.Log(OutDir);
        TeleOut.PreOut.rotation = Quaternion.Euler(0, 0, TeleOut.PreOut.rotation.z - (60 * OutDir));
        StartCoroutine(TeleOut.ShowPreview());
    }

    public IEnumerator ShowPreview()
    {
        isShowing = true;
        while(isShowing)
        {
            bool state = PreOut.gameObject.activeSelf;
            PreOut.gameObject.SetActive(!state);
            yield return new WaitForSeconds(0.5f);
        }
        PreOut.gameObject.SetActive(false);
        PreOut.rotation = Quaternion.Euler(0, 0,-180);
    }

    public void StopShowPreview()
    {
        TeleOut.isShowing = false;
    }
}