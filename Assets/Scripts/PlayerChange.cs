using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChange : MonoBehaviour
{
    public Sprite[] Player;
    public Toggle Eye;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Eye.isOn)
        {
            GetComponent<Image>().sprite = Player[1];
        }
        else
        {
            GetComponent<Image>().sprite = Player[0];
        }
    }
}
