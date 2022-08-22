using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using System;
using TMPro;

public class UpdateDice : MonoBehaviour
{

    TextMeshProUGUI errorText;
    string Player;
    public InputField numbers;
    string num, nums;
    public Dice di;

    private void Start()
    {
        Player = di.Player;
        errorText = di.errorText;
    }
    public void StartUp()
    {
        StartCoroutine(TecnicsNumLoad());
    }
    public void UpdateNum()
    {
        StartCoroutine(UpdateDiceNum());
    }

    IEnumerator UpdateDiceNum()
    {
        Player = di.Player;
        errorText = di.errorText;

        string[] strings = Regex.Split(numbers.text, Environment.NewLine);
        num = String.Join(" ", strings);

        string[] tecName = num.Split('\n');
        for (int i = 0; i < tecName.Length; i++)
        {
            nums += tecName[i] + " ";
        }

        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("lifeCaosKey", nums));

        UnityWebRequest www = UnityWebRequest.Post("https://sergiom.dev/rpg/Tecnics/RPGset" + Player + ".php", wwwForm);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            errorText.text = www.error;
        }

        di.updateNum();

        num = "";
        nums = "";
    }

    IEnumerator TecnicsNumLoad()
    {
        Player = di.Player;
        errorText = di.errorText;

        numbers.text = "";
        UnityWebRequest www = UnityWebRequest.Get("https://sergiom.dev/rpg/Tecnics/RPGget" + Player + ".php");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            errorText.text = www.error;
        }
        else
        {
            string[] tecName = www.downloadHandler.text.Split(' ');
            for (int i = 0; i < tecName.Length; i++)
            {
                numbers.text += tecName[i] + "\n";
            }
        }
    }
}
