using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class versionUpdate : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "V: " + Application.version;
    }
}
