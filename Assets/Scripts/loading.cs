using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour
{
    public List<PlayerInfo> playerInfo;
    private List<TextAsset> playerCreated;
    void Start()
    {
        StartCoroutine(LoadGame());
        if (PlayerPrefs.GetInt("OfflineMode") != 2)
        {
            RenderSettings.skybox.SetColor("_Tint", playerInfo[PlayerPrefs.GetInt("PlayerSelect")].playerColors[0]);
        }
        else
        {
            playerCreated = Resources.LoadAll<TextAsset>("PlayersCreated").ToList();
            int i = 0;
            bool addLine = false;
            string valueLine = "";
            string keyLine = "";
            Color col;
            Dictionary<string, string> playerSelected = new Dictionary<string, string>();
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
            ColorUtility.TryParseHtmlString("#"+playerSelected["color_ex"], out col);
            RenderSettings.skybox.SetColor("_Tint", col);
        }
    }

    IEnumerator LoadGame()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("PlayerFicha", LoadSceneMode.Additive);
        //load.allowSceneActivation = false;
        while (!load.isDone)
        {
            //Debug.Log(load.progress);
            float percentage = load.progress * 100;
            Debug.Log(percentage);
            yield return null;
        }
        yield break;
    }
}
