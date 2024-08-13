using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public Transform WinPanel;
    public Transform LosePanel;

    public void ShowWinPanel()
    {
        gameObject.SetActive(true);
        WinPanel.gameObject.SetActive(true);
        LosePanel.gameObject.SetActive(false);
    }

    public void ShowLosePanel()
    {
        gameObject.SetActive(true);
        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(true);
    }
    
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

}
