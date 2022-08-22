using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontSizeChange : MonoBehaviour
{
    private TextMeshProUGUI TextInput;
    void Start()
    {
        TryGetComponent(out TextInput);
    }

    public void PlusLower(bool Up)
    {
        if (Up)
        {
            TextInput.fontSize++;
        }
        else if(TextInput.fontSize > 5)
        {
            TextInput.fontSize--;
        }
    }
}
