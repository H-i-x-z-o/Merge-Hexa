using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public PopUpManager PopUp;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI FreezeCountText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LoadLevel(int level)
    {
        HidePanel();
        textLevel.text = "Level " + (level + 1);
    }

    public void ShowWinPanel()
    {
        PopUp.ShowWinPanel();
    }

    public void ShowLosePanel()
    {
        PopUp.ShowLosePanel();
    }

    public void HidePanel()
    {
        PopUp.HidePanel();
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
}
