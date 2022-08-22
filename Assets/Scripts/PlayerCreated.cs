using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCreated : MonoBehaviour
{
    [Header("Player Info")]
    public string Name;

    enum SpeciesType
    {
        Elfo,
        Bruxo,
        Feiticeiro,
        Demoni,
        Anjo,
        Goblin,
        Felino,
        Suine,
        Reptiliano,
        Ogro,
        Fada,
        Pequenino,
        Espectrai,
        Ursal,
        Lobulo,
        ElfoNegro,
        Mutagenico,
        Anunnaki,
        Elformo,
        Nino
    }

    [SerializeField]
    private SpeciesType Species;
    public string SpecieEnum => Species.ToString();
    public string Specie;
    public string Occupation;
    public string Biotype;
    public string Height;

    [Header("Player Design")] 
    public List<Sprite> PlayerSprites;
    public List<Color> PlayerColors;
    public Dictionary<int, Sprite> PlayerSpritesAdd;
    
    [Header("Player Sheet")] 
    public Vector2 Life;
    public Vector2 Caos;
    public Vector2 Armor;
    public Vector2 ArmorLife;
    public bool4 Status;
    public Vector3 Potions1;
    public Vector3 Potions2;
    public int MaxWeight;
    public List<string> Inventory;
    public List<int> DiceTecnics;
    [Multiline]
    public string Notes;
}
