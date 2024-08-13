using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicAndSound : MonoBehaviour
{
    [SerializeField] private UISwitcher.UISwitcher switcher;
    [SerializeField] private Transform iconOff;
    [SerializeField] private Text onText;
    [SerializeField] private Text offText;


    private void Awake() 
    {
        switcher.OnValueChanged += OnValueChanged3;
    }

    private void OnValueChanged3(bool isOn) 
    {
        onText.enabled = isOn;
        offText.enabled = !isOn;
        iconOff.gameObject.SetActive(!isOn);
    }
}
