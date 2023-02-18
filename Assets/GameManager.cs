using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ninito.UsualSuspects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private PlayerSheetData pSheet;
    [SerializeField]
    private ColorPaletteData cPalette;

    [Space]
    [Header("CSV")]
    [SerializeField] private ConvertCSVData csv;
    [SerializeField] private bool generateNewCsvData = false;
    
    [Space]
    [Header("UI")]
    [SerializeField] private Image savingIcon;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TMP_InputField abilityPoints;
    
    [Space]
    [Header("Managers")]
    [SerializeField] private DiceManager diceScript;
    [SerializeField] private EssentialsManager essentialsScript;
    [SerializeField] private StatusManager statusScript;
    [SerializeField] private SkillsManager skillsScript;
    [SerializeField] private MagicsManager magicsScript;
    [SerializeField] private InventoryManager inventoryScript;
    [SerializeField] private PotionsManager potionsScript;
    [SerializeField] private NotesManager notesScript;
    [SerializeField] private DocumentManager docsScript;
    
    [Space]
    [Header("Colors")]
    [SerializeField] private Material skyboxMaterial;
    
    [Space]
    [Header("Updaters")]
    [SerializeField] private UpdateTechniques updateTechniquesScript;

    //create a instance
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        #if UNITY_EDITOR
        if(generateNewCsvData) 
        {
            csv.ConvertPlayer(ref pSheet);
            return;
        }
        if(GameInfo.PlayerSheetData == null)
            GameInfo.PlayerSheetData = pSheet;
        #endif
        
        pSheet = GameInfo.PlayerSheetData;
    }

    private void Start()
    {
        SetSheetValues();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void LoadData(string data)
    {
        pSheet.SetData(data);

        SetSheetValues();
    }

    private void SetSheetValues()
    {
        playerName.text = pSheet.playerName.showName;
        abilityPoints.text = pSheet.abilityPoints.ToString();
        
        essentialsScript.SetValue(pSheet);
        inventoryScript.SetValue(pSheet);
        notesScript.SetValue(pSheet.notes);
        potionsScript.SetValue(pSheet.potions);
        statusScript.SetValue(pSheet);
        diceScript.SetValue(pSheet);
        skillsScript.SetSkills(pSheet.skills);
        magicsScript.SetMagicPoints(pSheet.magics);
        docsScript.SetDocument(pSheet.documents);
        BackgroundColor();
        StartCoroutine(SaveTimer());
    }

    public void SaveData(bool saveExit = false)
    {
        StartCoroutine(SavePlayerSheetAsync(saveExit));
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) SaveData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerSheet();
    }

    private void SavePlayerSheet()
    {
        pSheet.essential = essentialsScript.GetValue();
        pSheet.stats = statusScript.GetValue();
        pSheet.inventory = inventoryScript.GetValue();
        pSheet.potions = potionsScript.GetValue();
        pSheet.notes = notesScript.GetValue();
        pSheet.documents = docsScript.GetDocuments;
        
        SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"chardata", "chaos",$"characters/{pSheet.playerName.dataName}/");
    }

    private IEnumerator SavePlayerSheetAsync(bool saveExit)
    {
        savingIcon.gameObject.SetActive(true);
        Debug.Log("<color=blue>Saving...</color>");
        var waitForSeconds = new WaitForSeconds(0);
        
        try
        {
            pSheet.essential = essentialsScript.GetValue();
            pSheet.stats = statusScript.GetValue();
            pSheet.inventory = inventoryScript.GetValue();
            pSheet.potions = potionsScript.GetValue();
            pSheet.notes = notesScript.GetValue();
            pSheet.documents = docsScript.GetDocuments;
            
            Debug.Log(pSheet.techniques.Techniques["Tiro"]);
        
            SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"chardata", "chaos",$"characters/{pSheet.playerName.dataName}/");
            
            Debug.Log("<color=green>Saved!</color>");
        }
        catch (Exception e)
        {
            Debug.LogException(new Exception("<color=red>Saving Error: </color>" + e), this);

            savingIcon.GetComponent<Animator>().Play("Error");
            waitForSeconds = new WaitForSeconds(1f);
        }
        
        yield return waitForSeconds;
        savingIcon.gameObject.SetActive(false);
        
        if(saveExit) GameInfo.LoadScene("Menu");
    }
    
    private void BackgroundColor()
    {
        skyboxMaterial.SetColor("_Tint", ColorPaletteManager.GetColor("Normal"));
    }

    public void ImportCSV(string path)
    {
        csv.ConvertPlayer(ref pSheet, path);
    }

    public void ImportChaos(string path)
    {
        
    }

    public void DownloadChaos(string path)
    {
        
    }

    public void ShareChaos()
    {
#if PLATFORM_ANDROID || PLATFORM_IOS
        new NativeShare() 
            .SetTitle("Agentes do Caos RPG - Character Sheet")
            .AddFile(SaveLoadSystem.path + $"/characters/{pSheet.playerName.dataName}/chardata.chaos")
            .SetSubject("Chaos Character")
            .SetText("Check out my character!")
            .Share();
#endif
    }

    public void UpdateTechniques()
    {
        updateTechniquesScript.Initialize(pSheet);
    }
    
    public void SetTechniques(SerializedDictionary<string, int> tecnics)
    {
        pSheet.SetTechniques(tecnics.Values.ToArray());
        Debug.Log("Tecnics Updated");
        Debug.Log(pSheet.techniques.Techniques["Tiro"]);
    }
    
    private IEnumerator SaveTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(120);
            SaveData();
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
