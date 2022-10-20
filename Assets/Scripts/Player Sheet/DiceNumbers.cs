using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Icon("Assets/Images/UI/randomDiceIcon.png")]
public class DiceNumbers : MonoBehaviour
{
    [SerializeField]
    private bool _customDice = false;
    [SerializeField]
    private int _diceNumber;
    
    //Basic
    public TextMeshProUGUI NumText;
    protected int roladas = 0;
    protected int numRand;
    protected int DiceNum;
    protected bool rol;
    protected float timer = .55f;

    private void Awake()
    {
        NumText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        DiceNum = !_customDice ? int.Parse(this.name.Trim(new char[] {' ', 'D', '.'})) : _diceNumber;
    }

    public void DiceRoll()
    {
        rol = false;
        roladas = 0;
        
        StartCoroutine(RollingDice());
    }

    protected virtual IEnumerator RollingDice()
    {
        rol = true;
        timer = .01f;
        
        while (rol)
        {
            if (roladas < 6)
            {
                numRand = Random.Range(1, DiceNum + 1);
                NumText.text = numRand.ToString();
                roladas++;
                timer += .02f;
            }
            else
            {
                numRand = Random.Range(1, DiceNum + 1);
                NumText.text = numRand.ToString();
                roladas = 0;
                rol = false;
            }
            
            yield return new WaitForSeconds(timer);
        }
    }
}
