using System.Collections;
using System.Collections.Generic;
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
    private Text NumText;
    private int roladas = 0, numRand, DiceNum;
    private bool rol;
    private float timer = .55f;
    void Start()
    {
        
        NumText = transform.GetChild(0).GetComponent<Text>();
        if (!_customDice)
        {
            DiceNum = int.Parse(this.name.Trim(new char[] {' ', 'D', '.'}));
        }
        else
        {
            DiceNum = _diceNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rol)
        {
            if (timer >= .75f)
            {
                if (roladas < 6)
                {
                    numRand = Random.Range(1, DiceNum + 1);
                    NumText.text = numRand.ToString();
                    roladas++;
                    timer = 0;
                }
                else
                {
                    numRand = Random.Range(1, DiceNum + 1);
                    NumText.text = numRand.ToString();
                    roladas = 0;
                    rol = false;
                }
            }
            timer += 10 * Time.deltaTime;
        }
        
    }

    public void DiceRoll()
    {
        rol = true;
        timer = .75f;
    }
}
