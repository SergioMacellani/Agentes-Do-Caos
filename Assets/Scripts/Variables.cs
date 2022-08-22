using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Variables : MonoBehaviour
{
    [HideInInspector]
    public int loading = 0;
    [HideInInspector]
    public string[] Stat = new string[18], Inventory = new string[20];
    public string About;
    public GameObject Essenciais, Pocoes, Inventario, Historia, Status;

    void Start()
    {
        Stat = new string[19];
    }
    void Update()
    {

        Stat[0] = Essenciais.transform.GetChild(0).GetChild(1).GetComponent<InputField>().text;
        Stat[1] = Essenciais.transform.GetChild(0).GetChild(2).GetComponent<InputField>().text;
        Stat[2] = Essenciais.transform.GetChild(1).GetChild(1).GetComponent<InputField>().text;
        Stat[3] = Essenciais.transform.GetChild(1).GetChild(2).GetComponent<InputField>().text;

        Stat[4] = Pocoes.transform.GetChild(0).GetChild(0).GetComponent<InputField>().text;
        Stat[5] = Pocoes.transform.GetChild(1).GetChild(0).GetComponent<InputField>().text;
        Stat[6] = Pocoes.transform.GetChild(2).GetChild(0).GetComponent<InputField>().text;
        Stat[7] = Pocoes.transform.GetChild(3).GetChild(0).GetComponent<InputField>().text;
        Stat[8] = Pocoes.transform.GetChild(4).GetChild(0).GetComponent<InputField>().text;
        Stat[9] = Pocoes.transform.GetChild(5).GetChild(0).GetComponent<InputField>().text;

        Stat[10] = Inventario.transform.GetChild(1).GetComponent<InputField>().text;
        Stat[11] = Inventario.transform.GetChild(2).GetComponent<InputField>().text;

        Stat[12] = (Status.transform.GetChild(0).GetChild(0).GetComponent<Toggle>().isOn) ? "1" : "0";
        Stat[13] = (Status.transform.GetChild(1).GetChild(0).GetComponent<Toggle>().isOn) ? "1" : "0";
        Stat[14] = (Status.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn) ? "1" : "0";

        Stat[15] = Essenciais.transform.GetChild(2).GetChild(1).GetComponent<InputField>().text;
        Stat[16] = Essenciais.transform.GetChild(2).GetChild(2).GetComponent<InputField>().text;
        Stat[17] = Essenciais.transform.GetChild(3).GetChild(1).GetComponent<InputField>().text;
        Stat[18] = Essenciais.transform.GetChild(3).GetChild(2).GetComponent<InputField>().text;

        About = Historia.transform.GetChild(0).GetComponent<InputField>().text;

        Inventory[0] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<InputField>().text;
        Inventory[1] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text;
        Inventory[2] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<InputField>().text;
        Inventory[3] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetComponent<InputField>().text;
        Inventory[4] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(4).GetComponent<InputField>().text;
        Inventory[5] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<InputField>().text;
        Inventory[6] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(6).GetComponent<InputField>().text;
        Inventory[7] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(7).GetComponent<InputField>().text;
        Inventory[8] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(8).GetComponent<InputField>().text;
        Inventory[9] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(9).GetComponent<InputField>().text;
        Inventory[10] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(10).GetComponent<InputField>().text;
        Inventory[11] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(11).GetComponent<InputField>().text;
        Inventory[12] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(12).GetComponent<InputField>().text;
        Inventory[13] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(13).GetComponent<InputField>().text;
        Inventory[14] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(14).GetComponent<InputField>().text;
        Inventory[15] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(15).GetComponent<InputField>().text;
        Inventory[16] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(16).GetComponent<InputField>().text;
        Inventory[17] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(17).GetComponent<InputField>().text;
        Inventory[18] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(18).GetComponent<InputField>().text;
        Inventory[19] = Inventario.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(19).GetComponent<InputField>().text;
    }

    public void back()
    {
        SceneManager.LoadScene("Menu");
    }
}
