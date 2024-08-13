using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHomeManager : MonoBehaviour
{
    public static UIHomeManager Instance;
    public Transform HomePanel;
    public Transform LevelPanel;
    public Transform transition;

    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        HomePanel.gameObject.SetActive(true);
        LevelPanel.gameObject.SetActive(false);
    }

    public IEnumerator HomeToLevel()
    {
        StartCoroutine(Transition());   
        yield return new WaitForSeconds(0.25f);
        HomePanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        LevelPanel.gameObject.SetActive(true);
    }

    public IEnumerator LevelToHome()
    {
        StartCoroutine(Transition());
        yield return new WaitForSeconds(0.25f);
        HomePanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        LevelPanel.gameObject.SetActive(false);
    }

    public IEnumerator Transition()
    {
        transition.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        transition.gameObject.SetActive(false);
    }
}
