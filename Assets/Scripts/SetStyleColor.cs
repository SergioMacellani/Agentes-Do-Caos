using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStyleColor : MonoBehaviour
{
    public CreatePlayerOrder PlayerOrder;
    public ColorPicker ColorPicker;
    public GameObject DoubleColor;
    
    private int ColorOrder = 0;
    private int ColorOrder2 = 5;

    private void Start()
    {
        SetColorOrder("0");
    }

    public void UpdateColor(Color col, Color col2)
    {
        if (col != col2)
        {
            PlayerOrder.PiecesColors[ColorOrder] = col;
            PlayerOrder.PiecesColors[ColorOrder2] = col2;
        }
        else
        {
            PlayerOrder.PiecesColors[ColorOrder] = col;
            PlayerOrder.PiecesColors[ColorOrder2] = col;
        }
        PlayerOrder.ColorPieces();
    }

    public void SetColorOrder(string i)
    {
        string[] split = { "-" };
        string[] f = i.Split(split, System.StringSplitOptions.RemoveEmptyEntries);
        ColorOrder = int.Parse(f[0]);
        if (f.Length > 1)
        {
            ColorOrder2 = int.Parse(f[1]);
            DoubleColor.SetActive(true);
        }
        else
        {
            ColorOrder2 = int.Parse(f[0]);
            ColorPicker.DoubleColorGameObject.SetActive(false);
            DoubleColor.SetActive(false);
        }
    }
}
