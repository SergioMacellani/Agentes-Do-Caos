using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageBackgroundChange : MonoBehaviour
{
    public Sprite[] backgroundImage;
    void Start()
    {
        this.GetComponent<Image>().sprite = backgroundImage[Random.Range(0,backgroundImage.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
