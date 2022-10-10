using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsArrow : MonoBehaviour
{
    public List<string> optionsList;
    public int optionSelect;
    private TextMeshProUGUI textOptions;
    void Start()
    {
        TryGetComponent(out textOptions);
    }

    public void NextBack(bool next)
        {
            if (!next)
            {
                if (optionSelect < optionsList.Count-1)
                {
                    optionSelect++;
                }
                else
                {
                    optionSelect = 0;
                }
            }
            else
            {
                if (optionSelect > 0)
                {
                    optionSelect--;
                }
                else
                {
                    optionSelect = optionsList.Count-1;
                }
            }

            textOptions.text = optionsList[optionSelect];
        }
}
