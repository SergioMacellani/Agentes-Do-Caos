using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CSVImportManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSheetData pSheet;
    [SerializeField]
    private Color characterColor;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private TextMeshProUGUI convertText;
    [SerializeField]
    private MobileToggle statusAvatar;
    [SerializeField]
    private WindowManager convertMenu;
    [SerializeField]
    private WindowManager csvMenu;

    [SerializeField]
    private List<Image> characterImages = new List<Image>();
    [SerializeField]
    private List<string> imagesURL = new List<string>(4);

    [SerializeField]
    private UnityEvent OnPlayerSheetConverted;
    private ConvertCSVData ConvertCsvData => GetComponent<ConvertCSVData>();
    private float h, s, v;

    public TextMeshProUGUI t;
    public void Awake()
    {
        ColorPaletteManager.SetPallete(characterColor);
    }

    public void GetCSV()
    {
        string path = SaveLoadSystem.OpenFileExplorer("Selecione a sua ficha", new string[]{"csv"});
        if (path == null) return;
        
        ConvertCsvData.ConvertPlayer(ref pSheet, path);
        OpenCSVConvert();
    }

    private void OpenCSVConvert()
    {
        characterName.text = pSheet.playerName.showName;
        convertMenu.SwitchMenu(1);
    }

    public void SetColor(Color col)
    {
        pSheet.playerColors = col;
    }
    
    public void ConvertCSV()
    {
        StartCoroutine(ConvertCharacterAsync());
    }
    
    private IEnumerator ConvertCharacterAsync()
    {
        string playerName = pSheet.DataName;

        SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"chardata", "chaos",$"characters/{playerName}/");
        convertText.text = $"Importando... 1/5";
        int i = 0;
        if (statusAvatar.isOn)
        {
            foreach (var img in characterImages)
            {
                SaveLoadSystem.SaveFile(img.sprite.texture.EncodeToPNG(), i.ToString(), "png",
                    $"characters/{playerName}/");
                convertText.text = $"Importando... {i+2}/5";
                i++;
            }
        }
        else
        {
            for (i = 0; i < 4; i++)
            {
                SaveLoadSystem.SaveFile(characterImages[i].sprite.texture.EncodeToPNG(), i.ToString(), "png",
                    $"characters/{playerName}/");
                convertText.text = $"Importando... {i+2}/5";
            }
        }
        
        OnPlayerSheetConverted.Invoke();
        
        yield break;
    }

    public void SetAvatar(int i)
    {
        string path = SaveLoadSystem.OpenFileExplorer("Selecione o seu avatar", new string[]{"png","jpg","jpeg","bmp"});
        t.text += "    " + path;

        if (path == null) return;
        
        characterImages[i].sprite = SaveLoadSystem.LoadImage("", path, false);
        imagesURL[i] = path;
    }
}
