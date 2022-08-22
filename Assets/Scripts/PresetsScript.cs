using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetsScript : MonoBehaviour
{
    [HideInInspector]
    public bool Show = false;

    public bool HaveIcon;

    public string StyleType;
    public List<Sprite> SpriteOrder;
    public Sprite SpriteIcon;
    public Image imagePrefab;
    [HideInInspector]
    public CreatePlayerOrder playerOrder;
    void Start()
    {
        if (Show)
        {
            foreach (Sprite spr in SpriteOrder)
            {
                Instantiate(imagePrefab, transform).GetComponent<Image>().sprite = spr;
            }
        }
        else
        {
            if (HaveIcon)
            {
                Instantiate(imagePrefab, transform).GetComponent<Image>().sprite = SpriteIcon;
            }
            else
            {
                Instantiate(imagePrefab, transform).GetComponent<Image>().sprite = SpriteOrder[0];
            }
        }
    }

    public void SetStyle()
    {
        if (SpriteOrder.Count == 1)
        {
            playerOrder.UpdateSprite(StyleType, SpriteOrder[0]);
        }
        else if (SpriteOrder.Count == 2)
        {
            playerOrder.UpdatePlusSprite(StyleType, SpriteOrder[0], SpriteOrder[1], SpriteOrder[1]);
        }
        else
        {
            playerOrder.UpdatePlusSprite(StyleType, SpriteOrder[0], SpriteOrder[1], SpriteOrder[2]);
        }

    }
}
