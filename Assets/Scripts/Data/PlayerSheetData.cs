using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ninito.UnityExtended;
using Ninito.UsualSuspects;

[CreateAssetMenu(fileName = "PlayerSheet", menuName = "ADC/New Player Sheet")]
public class PlayerSheetData : ScriptableObject
{
    [Header("Info")]
    public PlayerName playerName;
    public PlayerImages playerImages;
    public Color playerColors;

    [Header("Essential")] 
    public PlayerEssential essential;

    [Header("Stats")] 
    public PlayerStats stats;

    [Header("Potions")] 
    public PlayerPotions potions = new PlayerPotions();
    
    [Header("Abilities")]
    public int abilityPoints = 5;

    [Header("Skills")]
    public PlayerSkills skills = new PlayerSkills();
    
    [Header("Techniques")] 
    public PlayerTechniques techniques = new PlayerTechniques();

    [Header("Magics")] 
    public PlayerMagics magics = new PlayerMagics();

    [Header("Inventory")] 
    public PlayerInv inventory;

    [Header("Text")] 
    public PlayerNotes notes;
    
    [Header("Documents")]
    public List<PlayerDocument> documents;
    
    public string DataName => playerName.dataName;

    public void SetData(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
    }

    public List<Sprite> GetImages()
    {
        List<Sprite> images = new List<Sprite>();

        for (int i = 0; i < 4; i++)
        {
            try
            {
                images.Add(SaveLoadSystem.LoadImage($"{i}.png", $"characters/{playerName.dataName}/"));
            }
            catch
            {
                images.Add(SaveLoadSystem.LoadImage("0.png", $"characters/{playerName.dataName}/"));
            }
        }

        return images;
    }

    public void SetEssentials(float[] values)
    {
        essential.playerLife.SetValue((int)values[0]);
        essential.playerChaos.SetValue(10,(int)values[1]);
        essential.playerArmor.SetValue((int)values[2]);
        essential.playerArmorLife.SetValue((int)values[2]*3);
    }
    
    public void SetPotions(float[] values)
    {
        potions.Potions = SetDictionary(potions.Potions, values);
    }

    public void SetSkills(float[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = MathF.Floor(values[i] / 50);
        }
        
        skills.psique = (int)values[0];
        skills.combate = (int)values[1];
        skills.ocultismo = (int)values[2];
        skills.sentidos = (int)values[3];
    }
    
    public void SetTechniques(float[] values)
    {
        techniques.Techniques = SetDictionary(techniques.Techniques, values);
    }
    
    public void SetTechniques(int[] values)
    {
        techniques.Techniques = SetDictionary(techniques.Techniques, values);
    }

    public void SetMagics(float[] values)
    {
        magics.Magics = SetDictionary(magics.Magics, values);
    }

    public void SetNotes(string[] notes, string[] traumas, string[] habilidades)
    {
        var notesText = notes.Aggregate("", (current, text) => current + (text + "\n"));
        var traumasText = traumas.Aggregate("", (current, text) => current + (text + "\n"));
        var habilidadesText = habilidades.Aggregate("", (current, text) => current + (text + "\n"));
        
        this.notes.SetNotepad(new string[3]{notesText, habilidadesText, traumasText}, new string[3]{"Notas", "Habilidades", "Traumas"});
    }

    public SerializedDictionary<string, int> SetDictionary(SerializedDictionary<string, int> dic, float[] values)
    {
        SerializedDictionary<string, int> createdDic = new SerializedDictionary<string, int>();
        int i = 0;
        
        foreach (var d in dic.Keys.ToList())
        {
            createdDic.Add(d, (int)values[i]);
            i++;
        }

        return createdDic;
    }
    
    public SerializedDictionary<string, int> SetDictionary(SerializedDictionary<string, int> dic, int[] values)
    {
        SerializedDictionary<string, int> createdDic = new SerializedDictionary<string, int>();
        int i = 0;
        
        foreach (var d in dic.Keys.ToList())
        {
            createdDic.Add(d, values[i]);
            i++;
        }

        return createdDic;
    }
}

[System.Serializable]
public class PlayerName
{
    public string fullName;
    public string firstName;
    public string lastName;

    public string showName => $"{firstName} {lastName}";
    public string dataName => $"{firstName.ToLower()}{lastName.ToLower()}";
    public void SetName(string pName)
    {
        fullName = pName;
        string[] nameArray = pName.Split(' ');
        firstName = nameArray[0];
        lastName = nameArray.Length > 1 ? nameArray[^1] : "";
    }
    public void SetName(PlayerName pName)
    {
        fullName = pName.fullName;
        firstName = pName.firstName;
        lastName = pName.lastName;
    }
    
}

[System.Serializable]
public class PlayerImages
{
    public List<string> imageLink;
}

[System.Serializable]
public class PlayerEssential
{
    public StatsValue playerLife;
    public StatsValue playerChaos;
    public StatsValue playerArmor;
    public StatsValue playerArmorLife;
}

[System.Serializable]
public class PlayerStats
{
    public bool injured;
    public bool insane;
    public bool unconscious;
}

[System.Serializable]
public class PlayerPotions
{
    public SerializedDictionary<string, int> Potions = new SerializedDictionary<string, int>()
    {
        {"regeneration",0},
        {"strength",0},
        {"invisibility",0},
        {"truth",0},
        {"poison",0},
        {"resistance",0}
    };
}

[System.Serializable]
public class PlayerSkills
{
    public int psique;
    public int combate;
    public int ocultismo;
    public int sentidos;
}

[System.Serializable]
public class PlayerTechniques
{
    public SerializedDictionary<string, int> Techniques = new SerializedDictionary<string, int>()
    {
        {"Intimidar",5},
        {"Mentira",5},
        {"Psicologia",5},
        {"Inteligencia",5},
        {"Medicina",5},
        {"Hacking",5},
        {"Historia",5},
        {"Tiro",5},
        {"Arremeçar",5},
        {"Furtividade",5},
        {"Atletismo",5},
        {"Força",5},
        {"Luta",5},
        {"Constituição",5},
        {"Magia",5},
        {"Sel. Magica",5},
        {"Cristalologia",5},
        {"Alquimia",5},
        {"Linguagem",5},
        {"Bestiario",5},
        {"Procurar",5},
        {"Escutar",5},
        {"Cheirar",5},
        {"Analisar",5}
    };
}

[System.Serializable]
public class PlayerMagics
{
    public SerializedDictionary<string, int> Magics = new SerializedDictionary<string, int>()
    {
        {"Caos",0},
        {"Tempo",0},
        {"Vazio",0},
        {"Fogo",0},
        {"Energia",0},
        {"Mente",0},
        {"Vida",0},
        {"Gelo",0},
        {"Ar",0},
        {"Sangue",0},
        {"Absoluto",0},
        {"Climatico",0},
        {"Água",0}
    };
}

[System.Serializable]
public class PlayerInv
{
    public StatsValueFloat inventoryWeight;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(20);
    
    public void SetValues(string[] itemsName, float[] itemsWeight, StatsValueFloat weight)
    {
        inventoryWeight = weight;
        inventorySlots.Clear();
        for (int i = 0; i < 20; i++)
        {
            InventorySlot inv = new InventorySlot();
            inv.SetValue(itemsName[i], itemsWeight[i]);
            inventorySlots.Add(inv);
        }
    }
}

[System.Serializable]
public class PlayerNotes
{
    public int fontSize = 40;
    public List<NotepadData> notepad = new List<NotepadData>();
    
    public void SetNotepad(string[] text, string[] title)
    {
        notepad.Clear();
        for (int i = 0; i < text.Length; i++)
        {
            notepad.Add(new NotepadData(text[i], title[i]));
        }
    }
}

[System.Serializable]
public class NotepadData
{
    public string title = "Sem Titulo";
    [TextArea()] public string notes;
    
    public NotepadData(string notes, string title = "Sem Titulo")
    {
        this.notes = notes;
        this.title = title ?? "Sem Titulo";
    }
}

[System.Serializable]
public class PlayerDocument
{
    public string Name;
    public string Code;
    
    public PlayerDocument(DocumentData data)
    {
        Name = data.Name;
        Code = data.Code;
    }
}

//Variaveis

[System.Serializable]
public struct StatsValue
{
    public int current;
    public int max;
    
    public StatsValue(int current, int max)
    {
        this.current = current;
        this.max = max;
    }
    
    public StatsValue(int max)
    {
        this.current = max;
        this.max = max;
    }
    
    public void SetValue(int c, int m)
    {
        current = c;
        max = m;
    }
    public void SetValue(int m)
    {
        current = m;
        max = m;
    }
}

[System.Serializable]
public struct StatsValueFloat
{
    public float current;
    public float max;
    
    public void SetValue(float c, float m)
    {
        current = c;
        max = m;
    }
    public void SetValue(float m)
    {
        current = m;
        max = m;
    }
}

[System.Serializable]
public struct InventorySlot
{
    public string itemName;
    public float itemWeight;

    public InventorySlot(string name, float weight)
    {
        itemName = name;
        itemWeight = weight;
    }
    
    public void SetValue(string itemName, float itemWeight)
    {
        this.itemName = itemName;
        this.itemWeight = itemWeight;
    }
    
    public void SetValue(InventorySlot inv)
    {
        this.itemName = inv.itemName;
        this.itemWeight = inv.itemWeight;
    }
}