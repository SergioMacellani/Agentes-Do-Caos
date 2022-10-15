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
    private List<string> imagesURL = new List<string>(4);

    [SerializeField]
    private UnityEvent OnPlayerSheetConverted;
    private ConvertCSVData ConvertCsvData => GetComponent<ConvertCSVData>();
    private float h, s, v;
    public void Awake()
    {
        Color.RGBToHSV(characterColor, out h, out s, out v);
        List<ColorKey> colorKeys = new List<ColorKey>();
        colorKeys.Add(new ColorKey("Light", Color.HSVToRGB(h,Mathf.Clamp01(s-.35f),v)));
        colorKeys.Add(new ColorKey("Normal", characterColor));
        colorKeys.Add(new ColorKey("Dark", Color.HSVToRGB(h,s,Mathf.Clamp01(v-.30f))));
        colorKeys.Add(new ColorKey("Menu", Color.HSVToRGB(h,s,.25f)));

        ColorPaletteManager.CreatePalette(colorKeys.ToArray());
    }

    public void GetCSV()
    {
        string path = SaveLoadSystem.OpenFileExplorer("Selecione a sua ficha", "csv");
        if (path == null) return;
        
        ConvertCsvData.ConvertPlayer(ref pSheet, path);
        OpenCSVConvert();
    }

    private void OpenCSVConvert()
    {
        characterName.text = pSheet.playerName.showName;
        convertMenu.SwitchMenu(1);
    }
    
    public void ConvertCSV()
    {
        StartCoroutine(ConvertCharacterAsync());
    }
    
    private IEnumerator ConvertCharacterAsync()
    {
        string playerName = pSheet.playerName.firstName.ToLower();

        SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"chardata", "chaos",$"characters/{playerName}/");
        convertText.text = $"Importando... 1/5";
        int i = 0;
        if (statusAvatar)
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
            for (int j = 0; j < 4; j++)
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
        string path = SaveLoadSystem.OpenFileExplorer("Selecione o seu avatar", "png");
        if (path == null) return;
        
        characterImages[i].sprite = SaveLoadSystem.LoadImage("", path, false);
        imagesURL[i] = path;
    }
}