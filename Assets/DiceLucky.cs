using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLucky : DiceNumbers
{
    protected override IEnumerator RollingDice()
    {
        rol = true;
        timer = .01f;

        while (rol)
        {
            if (roladas < 6)
            {
                numRand = Random.Range(1, DiceNum + 1);
                NumText.text = numRand % 2 == 0 ? "Sorte" : "Azar";
                roladas++;
                timer += .02f;
            }
            else
            {
                numRand = Random.Range(1, DiceNum + 1);
                NumText.text = numRand % 2 == 0 ? "Sorte" : "Azar";
                roladas = 0;
                rol = false;
            }

            yield return new WaitForSeconds(timer);
        }
    }
}
