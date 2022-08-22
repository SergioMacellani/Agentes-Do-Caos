using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("Player Info")]
    public string Name;
    public List<Sprite> playerIcons;
    public List<Color> playerColors;

    [Header("Local Save")] 
    public Vector2 Life;
    public Vector2 Caos;
    public Vector2 Armor;
    public Vector2 ArmorLife;
    public bool4 Status;
    public Vector3 Potions1;
    public Vector3 Potions2;
    public int MaxWeight;
    public List<string> Inventory;
    public List<string> DiceTecnics;
    [TextArea]
    public string Notes;
}
