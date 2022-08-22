using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayerOrder : MonoBehaviour
{
    public List<Sprite> PlayerPieces;
    public List<Color> PiecesColors;
    public List<Image> PiecesLocal;
    void Start()
    {
        ColorPieces();
        SpritePieces();
    }

    public void ColorPieces()
    {
        PiecesLocal[0].color = PiecesColors[0];
        PiecesLocal[1].color = PiecesColors[5];
        PiecesLocal[2].color = PiecesColors[1];
        PiecesLocal[3].color = PiecesColors[1];
        PiecesLocal[4].color = Color.white;
        PiecesLocal[5].color = PiecesColors[2];
        PiecesLocal[6].color = PiecesColors[6];
        PiecesLocal[7].color = PiecesColors[3];
        PiecesLocal[8].color = PiecesColors[0];
        PiecesLocal[9].color = PiecesColors[4];
    }

    public void SpritePieces()
    {
        for (int i = 0; i < PiecesLocal.Count - 1; i++)
        {
            PiecesLocal[i].sprite = PlayerPieces[i];
        }
    }

    public void UpdateSprite(string pieceName, Sprite spr)
    {
        if (pieceName == "Face")
        {
            PiecesLocal[0].sprite = spr;
        }
        else if (pieceName == "Mouth")
        {
            PiecesLocal[1].sprite = spr;
        }
        else if (pieceName == "Eyebrow")
        {
            PiecesLocal[7].sprite = spr;
        }
        else if (pieceName == "Ears")
        {
            PiecesLocal[8].sprite = spr; 
        }
        else if (pieceName == "Clothes")
        {
            PiecesLocal[9].sprite = spr;
        }

        UpdateSprite();
    }
    
    public void UpdatePlusSprite(string pieceName, Sprite spr1, Sprite spr2, Sprite spr3)
    {
        if (pieceName == "Hair")
        {
            PiecesLocal[2].sprite = spr1;
            PiecesLocal[3].sprite = spr2;
        }
        else if (pieceName == "Eye")
        {
            PiecesLocal[4].sprite = spr1;
            PiecesLocal[5].sprite = spr2;
            PiecesLocal[6].sprite = spr3;
        }

        UpdateSprite();
    }

    public void UpdateSprite()
    {
        for (int i = 0; i < PlayerPieces.Count; i++)
        {
            PlayerPieces[i] = PiecesLocal[i].sprite;
        }
    }
}
