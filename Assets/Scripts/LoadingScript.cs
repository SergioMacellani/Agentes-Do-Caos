using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    private void Start()
    {
        StartCoroutine(LoadGame());
        RenderSettings.skybox.SetColor("_Tint", GameInfo.PlayerSheetData.playerColors.colorNormal.color);
    }

    IEnumerator LoadGame()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("PlayerFichaNew", LoadSceneMode.Additive);
        //load.allowSceneActivation = false;
        while (!load.isDone)
        {
            //Debug.Log(load.progress);
            float percentage = load.progress * 100;
            loadingText.text = "Loading: " + percentage.ToString("F0") + "%";
            yield return null;
        }
        yield break;
    }
}
