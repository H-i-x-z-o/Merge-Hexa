using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform unlock;
    [SerializeField] private Transform Locked;

    public void Unlock()
    {
        unlock.gameObject.SetActive(true);
        Locked.gameObject.SetActive(false);
    }
    
    public void Lock()
    {
        unlock.gameObject.SetActive(false);
        Locked.gameObject.SetActive(true);
    }
    
}
