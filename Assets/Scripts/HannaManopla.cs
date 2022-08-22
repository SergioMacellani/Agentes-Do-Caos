using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HannaManopla : MonoBehaviour
{
    public InputField CaosLvl;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int manoplaTotal = 100 - System.Convert.ToInt32(CaosLvl.text);
        GetComponent<InputField>().text = manoplaTotal.ToString();
    }
}
