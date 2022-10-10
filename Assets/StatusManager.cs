using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [Header("Player Image")]
    [SerializeField] private Image playerImage;
    [SerializeField] private List<Sprite> playerImages = new List<Sprite>();

    [Space]
    [Header("Player Status")]
    [SerializeField] private Toggle injured;
    [SerializeField] private Toggle insane;
    [SerializeField] private Toggle unconscious;

    public void SetValue(PlayerSheetData pSheet)
    {
        injured.isOn = pSheet.stats.injured;
        insane.isOn = pSheet.stats.insane;
        unconscious.isOn = pSheet.stats.unconscious;
        
        UpdateImage();
    }
    
    public void UpdateImage()
    {
        playerImage.color = Color.white;
        if (unconscious.isOn)
        {
            playerImage.sprite = playerImages[0];
            playerImage.color = Color.black;
        }
        else if (insane.isOn && injured.isOn)
        {
            playerImage.sprite = playerImages[3];
        }
        else if (insane.isOn)
        {
            playerImage.sprite = playerImages[2];
        }
        else if (injured.isOn)
        {
            playerImage.sprite = playerImages[1];
        }
        else
        {
            playerImage.sprite = playerImages[0];
        }
    }
}
