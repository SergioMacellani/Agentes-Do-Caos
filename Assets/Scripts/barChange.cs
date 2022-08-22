using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class barChange : MonoBehaviour
{
    public Image bar;
    public TMP_InputField MaxNum;
    public TMP_InputField AttNum;
    void Start()
    {
        UpdateBar();
    }
    public void UpdateBar()
    {
        bar.fillAmount = float.Parse(AttNum.text) / float.Parse(MaxNum.text);
    }
}
