using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class selectLoad : MonoBehaviour
{
    public PlayerSelect PlayerSelectScript;
    public void load()
    {

    }

    public void menu()
    {
        SceneManager.LoadScene("MenuNew");
    }
}
