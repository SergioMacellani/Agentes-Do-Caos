using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class AJAX : MonoBehaviour
{
    [Header("Scripts de Outros Objetos")]
    public InventoryLoad Inventory;
    public loading LoadingCanvas;
    private PlayerInfo playerInfo;
    private PlayerLoad playerLoad;
    private Canvas canvas;

    [Header("Itens da Ficha")]
    public TMP_InputField NotesText;
    public TMP_InputField InventoryMax;
    public TMP_InputField InventoryAtt;
    public List<TMP_InputField> StatsBar;
    public List<TMP_InputField> PotionsInput;
    public List<Toggle> StatsToggle;

    [Header("Micelanias")]
    public GameObject SaveIcon;
    private Dictionary<string, string> playerSelected = new Dictionary<string, string>();
    private bool saveCreate = false;

    public int LoadStats;

    public string playerName;
    readonly string URL = "https://sergiom.dev/rpg/";
    string setURL, getURL, stringSend;
    Text errorText;

    private void Start()
    {
        TryGetComponent(out canvas);
        TryGetComponent(out playerLoad);
        canvas.enabled = false;
        playerInfo = LoadingCanvas.playerInfo[PlayerPrefs.GetInt("PlayerSelect")];
        playerName = playerInfo.name.Replace("Player", "");

        if (PlayerPrefs.GetInt("OfflineMode") == 0)
        {
            StartCoroutine(GetRequestHistory());
        }
        else if(PlayerPrefs.GetInt("OfflineMode") == 1)
        {
            StartCoroutine(OfflineGetPlayer());
        }
        else
        {
            StartCoroutine(LoadCreatedFile());
        }

    }

    public void UpdateStats(string updater)
    {
        setURL = URL + updater + "/RPGset";
        getURL = URL + updater + "/RPGget";
        StartCoroutine(SetRequest(updater));
    }

    public IEnumerator LoadComplete()
    {
        LoadStats++;
        if (LoadStats >= 4)
        {
            canvas.enabled = true;
            SceneManager.UnloadSceneAsync("Loading");
        }
        yield break;
    }

    #region Online
    
    IEnumerator GetRequestHistory()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + "About/RPGget" + playerName + ".php");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.url);
            errorText.text = www.error;
            StartCoroutine(errorTimer());
        }
        else
        {
            NotesText.text = www.downloadHandler.text;
        }
        StartCoroutine(LoadComplete());
        StartCoroutine(GetRequestInventory());
    }

    IEnumerator GetRequestInventory()
    {
        string[] split = { "+!*!+", " - " };

        UnityWebRequest www = UnityWebRequest.Get(URL + "Inventory/RPGget" + playerName + ".php");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
            errorText.text = www.error;
            StartCoroutine(errorTimer());
        }
        else
        {
            string[] inv = www.downloadHandler.text.Split(split, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < inv.Length-1; i++)
            {
                if (i % 2 == 0)
                {
                    Inventory.itemName.Add(inv[i]);
                }
                else
                {
                    Inventory.itemWeight.Add(inv[i].Replace('.',','));
                }
            }
        }
        StartCoroutine(Inventory.LoadInventory());
        StartCoroutine(LoadComplete());
        StartCoroutine(GetRequestStats());
    }

    IEnumerator GetRequestStats()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + "Stats/RPGget" + playerName + ".php");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
            errorText.text = www.error;
            StartCoroutine(errorTimer());
        }
        else
        {
            string[] stats = www.downloadHandler.text.Split(' ');
            
            //Life
            StatsBar[0].text = stats[0];
            StatsBar[1].text = stats[1];
            
            //Caos
            StatsBar[2].text = stats[2];
            StatsBar[3].text = stats[3];
            
            //Armor
            StatsBar[4].text = stats[15];
            StatsBar[5].text = stats[16];
            
            //Life Armor
            StatsBar[6].text = stats[17];
            StatsBar[7].text = stats[18];

            PotionsInput[0].text = stats[4];
            PotionsInput[1].text = stats[5];
            PotionsInput[2].text = stats[6];
            PotionsInput[3].text = stats[7];
            PotionsInput[4].text = stats[8];
            PotionsInput[5].text = stats[9];

            InventoryMax.text = stats[10];

            StatsToggle[0].isOn = (stats[12] == "1") ? true : false;
            StatsToggle[1].isOn = (stats[13] == "1") ? true : false;
            StatsToggle[2].isOn = (stats[14] == "1") ? true : false;
            
            StartCoroutine(LoadComplete());
        }
    }

    IEnumerator SetRequest(string up)
    {
        SaveIcon.SetActive(true);
        if (PlayerPrefs.GetInt("OfflineMode") == 0)
        {
            
            stringSend = "";
            if (up == "About")
            {
                stringSend = NotesText.text;
            }
            else if (up == "Stats")
            {
                for (int i = 0; i < 4; i++)
                {
                    stringSend += StatsBar[i].text + " ";
                }

                for (int i = 0; i < 6; i++)
                {
                    stringSend += PotionsInput[i].text + " ";
                }

                stringSend += InventoryMax.text + " ";
                stringSend += InventoryAtt.text + " ";

                for (int i = 0; i < 3; i++)
                {
                    stringSend += ((StatsToggle[i].isOn == true) ? 1 : 0) + " ";
                }

                for (int i = 4; i < 8; i++)
                {
                    stringSend += StatsBar[i].text + " ";
                }
            }
            else if (up == "Inventory")
            {
                Transform inv = Inventory.transform;
                for (int i = 0; i < inv.childCount; i++)
                {
                    TMP_InputField invSlot = inv.GetChild(i).GetComponent<TMP_InputField>();
                    if (invSlot.text != null && invSlot.text != " " && invSlot.text != "")
                    {
                        stringSend +=
                            $"{invSlot.text} - {inv.GetChild(i).GetChild(0).GetComponent<TMP_InputField>().text}+!*!+";
                    }
                    else
                    {
                        stringSend += "Slot Vazio - 0,0+!*!+";
                    }
                }
            }

            List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
            wwwForm.Add(new MultipartFormDataSection("lifeCaosKey", stringSend));

            UnityWebRequest www = UnityWebRequest.Post(setURL + playerName + ".php", wwwForm);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                errorText.text = www.error;
                StartCoroutine(errorTimer());
            }
        }
        else if (PlayerPrefs.GetInt("OfflineMode") == 1)
        {
            if (up == "About")
            {
                playerInfo.Notes = NotesText.text;
            }
            else if (up == "Stats")
            {
                //Life
                playerInfo.Life = new Vector2(float.Parse(StatsBar[0].text), float.Parse(StatsBar[1].text));
            
                //Caos
                playerInfo.Caos = new Vector2(float.Parse(StatsBar[2].text), float.Parse(StatsBar[3].text));
            
                //Armor
                playerInfo.Armor = new Vector2(float.Parse(StatsBar[4].text), float.Parse(StatsBar[5].text));
            
                //Life Armor
                playerInfo.ArmorLife = new Vector2(float.Parse(StatsBar[5].text), float.Parse(StatsBar[6].text));

                //Potions
                playerInfo.Potions1 = new Vector3(float.Parse(PotionsInput[0].text), float.Parse(PotionsInput[1].text),
                    float.Parse(PotionsInput[2].text));
                playerInfo.Potions2 = new Vector3(float.Parse(PotionsInput[3].text), float.Parse(PotionsInput[4].text),
                    float.Parse(PotionsInput[5].text));
        
                //Status
                playerInfo.Status = new bool4(StatsToggle[0].isOn, StatsToggle[1].isOn,StatsToggle[2].isOn,false);
                
                //InventoryWeight
                playerInfo.MaxWeight = int.Parse(InventoryMax.text);
            }
            else if (up == "Inventory")
            {
                Transform inv = Inventory.transform;
                for (int i = 0; i < inv.childCount; i++)
                {
                    TMP_InputField invSlot = inv.GetChild(i).GetComponent<TMP_InputField>();
                    if (invSlot.text != null && invSlot.text != " " && invSlot.text != "")
                    {
                        playerInfo.Inventory[i] =
                            $"{invSlot.text} - {inv.GetChild(i).GetChild(0).GetComponent<TMP_InputField>().text}";
                    }
                    else
                    {
                        playerInfo.Inventory[i] = "Slot Vazio - 0,0";
                    }
                }
            }
        }
        else if(!saveCreate)
        {
            saveCreate = true;
            
            string save = $"#\n" +
                          $"# Player Info\n" +
                          $"#\n" +
                          $"name={SlotDic()["name"]}\n" +
                          $"specie={SlotDic()["specie"]}\n" +
                          $"occupation={SlotDic()["occupation"]}\n" +
                          $"biotype={SlotDic()["biotype"]}\n" +
                          $"height={SlotDic()["height"]}\n" +
                          $"#\n" +
                          $"# Player Essentials\n" +
                          $"#\n" +
                          $"life_max={StatsBar[1].text}\n" +
                          $"life_att={StatsBar[0].text}\n" +
                          $"caos_max={StatsBar[3].text}\n" +
                          $"caos_att={StatsBar[2].text}\n" +
                          $"armor_max={StatsBar[5].text}\n" +
                          $"armor_att={StatsBar[4].text}\n" +
                          $"lifearmor_max={StatsBar[7].text}\n" +
                          $"lifearmor_att={StatsBar[6].text}\n" +
                          $"#\n" +
                          $"# Player Stats\n" +
                          $"#\n" +
                          $"ferido={StatsToggle[0].isOn.GetHashCode()}\n" +
                          $"insano={StatsToggle[1].isOn.GetHashCode()}\n" +
                          $"inconsciente={StatsToggle[2].isOn.GetHashCode()}\n" +
                          $"#\n" +
                          $"# Player Tecnics\n" +
                          $"#\n" +
                          $"tecnics=[\n" +
                          $"{SlotDic()["tecnics"]}\n";
        /*
        foreach (TecnicsBar tec in PlayerTecnics.tecnics)
        {
            writer.WriteLine(tec.tecnicDiceNum);
        }
        */
        save += $"]\n" +
                $"#\n" +
                $"# Player Magic\n" +
                $"#\n" +
                $"magic=[\n" +
                $"{SlotDic()["magic"]}\n";
        /*
        foreach (int magic in PlayerMagic.MagicDiceNum)
        {
            writer.WriteLine(magic);
        }
        */
        save += $"]\n" +
                $"#\n" +
                $"# Player Potions\n" +
                $"#\n" +
                $"potions=[\n";
        
        foreach (var pot in PotionsInput)
        {
            save += $"{pot.text}\n";
        }
        
        save += $"]\n" +
                $"#\n" +
                $"# Player Inventory\n" +
                $"#\n" +
                $"inventory=[\n";
        
        int i = 0;
        foreach (string item in Inventory.itemName)
        {
            save += $"{item} - {Inventory.itemWeight[i]}\n";
            i++;
        }

        save += $"]\n" +
                $"inventory_maxweight={InventoryMax.text}\n" +
                $"inventory_weight={Inventory.weightUpdate.WeightTotal}\n" +
                $"#\n" +
                $"# Player Story\n" +
                $"#\n" +
                $"notes=[\n" +
                $"{NotesText.text}\n" +
                $"]\n" +
                $"#\n" +
                $"# Player Design\n" +
                $"#\n" +
                $"design=[\n";
        foreach (Sprite spr in playerLoad.PlayerCreateIcon.PlayerPieces)
        {
            save += $"{spr.name}\n";
        }

        save += $"]\n" +
                $"#\n" +
                $"# Player Colors\n" +
                $"#\n" +
                $"colors=[\n";
        
        foreach (Color col in playerLoad.PlayerCreateIcon.PiecesColors)
        {
            save += $"{ColorUtility.ToHtmlStringRGB(col)}\n";
        }
        
        save += $"]\n" +
                $"color_ex={SlotDic()["color_ex"]}\n" +
                $"#\n" +
                $"# Player File End\n" +
                $"#";

        PlayerPrefs.SetString($"Save{PlayerPrefs.GetInt("PlayerCreateSelect")}", save);
        
        saveCreate = false;
        }

        SaveIcon.SetActive(false);
    }
    
    #endregion

    #region Offline

    IEnumerator OfflineGetPlayer()
    {
        
        //Life
        StatsBar[0].text = playerInfo.Life.x.ToString();
        StatsBar[1].text = playerInfo.Life.y.ToString();
            
        //Caos
        StatsBar[2].text = playerInfo.Caos.x.ToString();
        StatsBar[3].text = playerInfo.Caos.y.ToString();
            
        //Armor
        StatsBar[4].text = playerInfo.Armor.x.ToString();
        StatsBar[5].text = playerInfo.Armor.y.ToString();
            
        //Life Armor
        StatsBar[6].text = playerInfo.ArmorLife.x.ToString();
        StatsBar[7].text = playerInfo.ArmorLife.y.ToString();

        //Potions
        PotionsInput[0].text = playerInfo.Potions1.x.ToString();
        PotionsInput[1].text = playerInfo.Potions1.y.ToString();
        PotionsInput[2].text = playerInfo.Potions1.z.ToString();
        PotionsInput[3].text = playerInfo.Potions2.x.ToString();
        PotionsInput[4].text = playerInfo.Potions2.y.ToString();
        PotionsInput[5].text = playerInfo.Potions2.z.ToString();
        
        //Status
        StatsToggle[0].isOn = playerInfo.Status.x;
        StatsToggle[1].isOn = playerInfo.Status.y;
        StatsToggle[2].isOn = playerInfo.Status.z;
        
        //Notes
        NotesText.text = playerInfo.Notes;
        
        //Inventory
        string[] split = { " - " };
        for (int i = 0; i < playerInfo.Inventory.Count-1; i++)
        {
            string[] inv = playerInfo.Inventory[i].Split(split, System.StringSplitOptions.RemoveEmptyEntries);
            Inventory.itemName.Add(inv[0]);
            Inventory.itemWeight.Add(inv[1].Replace('.',','));
        }

        InventoryMax.text = playerInfo.MaxWeight.ToString();
        StartCoroutine(Inventory.LoadInventory());
        
        LoadStats = 4;
        StartCoroutine(LoadComplete());
        
        yield break;
    }

    #endregion

    #region Created

    private IEnumerator LoadCreatedFile()
    {
        int i = 0;
        bool addLine = false;
        string valueLine = "";
        string keyLine = "";
        List<TextAsset> playerCreated = Resources.LoadAll<TextAsset>("PlayersCreated").ToList();
        foreach (string ln in System.Text.RegularExpressions.Regex.Split(PlayerPrefs.GetString($"Save{PlayerPrefs.GetInt("PlayerCreateSelect")}"), "\r\n|\r|\n"))
        {
            if(ln.StartsWith("#"))
                continue;
            if(ln.Trim().Length == 0)
                continue;
                
            string line = ln.TrimStart();

            if (line.Contains("=[") || addLine)
            {
                if (addLine)
                {
                    if (!line.StartsWith("]"))
                    {
                        valueLine += $"{line}\n";
                    }
                    else
                    {
                        playerSelected.Add(keyLine, valueLine);
                        keyLine = "";
                        valueLine = "";
                        addLine = false;
                    }
                }
                else
                {
                    string[] s = line.Split(new char[] {'='}, 2);
                    keyLine = s[0];
                    addLine = true;
                }
            }
            else
            {
                string[] s = line.Split(new char[] {'='}, 2);

                if (s.Length == 2)
                {
                    if (s[0].Length == 0)
                    {
                        Debug.LogError("Key Error! Line:" + i);
                    }
                    else
                    {
                        playerSelected.Add(s[0], s[1]);
                    }
                }
                else
                {
                    if (s[0].Length == 0)
                    {
                        Debug.LogError("Key Error! Line:" + i);
                    }
                    else
                    {
                        playerSelected.Add(s[0], s[1]);
                    }
                }
            }

            i++;
        }

        StartCoroutine(CreatedGetPlayer(playerSelected));
        yield break;
    }

    private IEnumerator CreatedGetPlayer(Dictionary<string, string> slot)
    {
        //Name
        playerName = slot["name"];
        
        //Life
        StatsBar[0].text = slot["life_att"];
        StatsBar[1].text = slot["life_max"];
            
        //Caos
        StatsBar[2].text = slot["caos_att"];
        StatsBar[3].text = slot["caos_max"];
            
        //Armor
        StatsBar[4].text = slot["armor_att"];
        StatsBar[5].text = slot["armor_max"];
            
        //Life Armor
        StatsBar[6].text = slot["lifearmor_att"];
        StatsBar[7].text = slot["lifearmor_max"];

        //Potions
        string[] potions = slot["potions"].Split(new char[]{'\n', '\r'});
        for (int i = 0; i < PotionsInput.Count; i++)
        {
            PotionsInput[i].text = potions[i];
        }
        
        //Status
        StatsToggle[0].isOn = (slot["ferido"] == "1") ? true : false;
        StatsToggle[1].isOn = (slot["insano"] == "1") ? true : false;
        StatsToggle[2].isOn = (slot["inconsciente"] == "1") ? true : false;
        
        //Notes
        NotesText.text = slot["notes"];
        
        //Inventory
        string[] split = { " - " };
        string[] inventory = slot["inventory"].Split(new char[]{'\n', '\r'});
        for (int i = 0; i < inventory.Length-1; i++)
        {
            string[] inv = inventory[i].Split(split, System.StringSplitOptions.RemoveEmptyEntries);
            Inventory.itemName.Add(inv[0]);
            Inventory.itemWeight.Add(inv[1].Replace('.',','));
        }

        InventoryMax.text = slot["inventory_maxweight"];

        LoadStats = 4;
        StartCoroutine(LoadComplete());
        
        yield break;
    }
    
    public Dictionary<string, string> SlotDic()
    {
        return playerSelected;
    }

    #endregion

    IEnumerator errorTimer()
    {
        yield return new WaitForSeconds(5);
        errorText.text = "";
    }
}
