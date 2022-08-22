using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Linq;
using TMPro;

public class Dice : MonoBehaviour
{
    public TMP_Dropdown TecnicSelect;
    public TextMeshProUGUI DiceText, numName, loadingText, errorText;
    public GameObject updateDice;
    public TMP_InputField chaos;
    public List<string> optionsDropdown;
    public AJAX ajax;
    public TextAsset TecnicsNameText;
    public TextAsset MagicNameText;

    public string Player;
    public float timer = .55f;
    int rool;
    int diceMaxNum;

    public bool DiceRool = false;

    private List<int> TecnicsNum = new List<int>();
    private List<string> tecnicsName = new List<string>();
    private List<string> magicName = new List<string>();

    void Start()
    {
        Player = ajax.GetComponent<PlayerLoad>().playerInfo[PlayerPrefs.GetInt("PlayerSelect")].name
            .Replace("Player", "");
        StartCoroutine(TecnicsNameLoad());
    }

    public void LoadRPGDice()
    {
        StartCoroutine(TecnicsNameLoad());
    }

    IEnumerator TecnicsNameLoad()
    {
        string[] split = { "\n" };
        tecnicsName = TecnicsNameText.text.Trim('\r').Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
        magicName = MagicNameText.text.Trim('\r').Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();

        StartCoroutine(TecnicsNumLoad());
        yield break;
    }

    public void updateNum() 
    {
        StartCoroutine(TecnicsNumLoad());
    }

    IEnumerator TecnicsNumLoad()
    {
        if (PlayerPrefs.GetInt("OfflineMode") != 2)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://sergiom.dev/rpg/Tecnics/RPGget" + Player + ".php");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.url);
                errorText.text = www.error;
                StartCoroutine(errorTimer());
            }
            else
            {
                string[] tecName = www.downloadHandler.text.Split(' ');
                for (int i = 0; i < 28; i++)
                {
                    TecnicsNum.Add(int.Parse(tecName[i]));
                }
            }
        }
        else
        {
            while (!ajax.SlotDic().ContainsKey("magic"))
            {
                Debug.Log("Loading...");
            }
            string[] TecnicsString = ajax.SlotDic()["tecnics"].Split(new char[]{'\n', '\r'});
            string[] MagicString = ajax.SlotDic()["magic"].Split(new char[]{'\n', '\r'});
            foreach (string tec in TecnicsString)
            {
                if(tec != "")
                    TecnicsNum.Add(int.Parse(tec));
            }
            foreach (string mag in MagicString)
            {
                if(mag != "")
                    TecnicsNum.Add(int.Parse(mag));
            }
        }
        DropdownUpdate();
        //updateDice.SetActive(false);
    }

    private void DropdownUpdate()
    {
        int i = 0;
        foreach (string tec in tecnicsName)
        {
            optionsDropdown.Add((tec.Replace("\r", "") + ": " + TecnicsNum[i]).ToString());
            i++;
        }
        foreach (string mag in magicName)
        {
            optionsDropdown.Add((mag.Replace("\r", "") + ": " + TecnicsNum[i]).ToString());
            i++;
        }
        optionsDropdown.Add("Caos: " + chaos.text);
        TecnicSelect.AddOptions(optionsDropdown);
        StartCoroutine(ajax.LoadComplete());
    }

    public void openUpdateDice()
    {
        updateDice.SetActive(true);
        updateDice.GetComponent<UpdateDice>().StartUp();
    }

    public void diceChangeChaos()
    {
        //TecnicSelect.options[28].text = "Caos: " + chaos.text;
    }

    public void DiceOn()
    {
        DiceRool = true;
        TecnicSelect.options[28].text = "Caos: " + chaos.text;
    }
    
    void Update()
    {
        if (DiceRool)
        {
            if (timer >= .75f && rool < 6)
            {
                numName.color = new Color(1, 1, 1, 0);
                diceMaxNum = 100;
                DiceText.text = UnityEngine.Random.Range(1, diceMaxNum + 1).ToString();
                timer = 0;
                rool++;
            }
            if (timer >= .75f && rool >= 6)
            {
                diceMaxNum = 100;
                DiceText.text = UnityEngine.Random.Range(1, diceMaxNum + 1).ToString();
                if (diceMaxNum == 100)
                {
                    numName.color = new Color(1, 1, 1, 1);
                    double habNum;
                    if (TecnicSelect.value != 28)
                    {
                        habNum = TecnicsNum[TecnicSelect.value];
                    }
                    else
                    {
                        habNum = Int16.Parse(chaos.text);
                    }
                    int habExtremo = System.Convert.ToInt32(System.Math.Ceiling((15 * habNum) / 100));
                    int habBom = System.Convert.ToInt32(System.Math.Ceiling((50 * habNum) / 100));
                    int diceNum = int.Parse(DiceText.text);

                    if (diceNum == 1)
                    {
                        numName.text = "PERFEIÇÃO";
                    }
                    else if (diceNum <= habExtremo)
                    {
                        numName.text = "EXTREMO";
                    }
                    else if (diceNum <= habBom)
                    {
                        numName.text = "BOM";
                    }
                    else if (diceNum <= habNum)
                    {
                        numName.text = "NORMAL";
                    }
                    else if (diceNum == 100)
                    {
                        numName.text = "CATASTROFE";
                    }
                    else
                    {
                        numName.text = "FALHOU";
                    }
                }
                rool = 0;
                DiceRool = false;
            }
            timer += 10 * Time.deltaTime;
        }
    }

    IEnumerator errorTimer()
    {
        yield return new WaitForSeconds(5);
        errorText.text = "";
    }
}
