using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNotes : MonoBehaviour
{
    [SerializeField] private TMP_InputField _notesText;

    public void SetValue(PlayerSheetData pSheet)
    {
        _notesText.text = pSheet.texts.Notes;
        _notesText.pointSize = pSheet.texts.fontSize;
        _notesText.verticalScrollbar.value = 0;
    }

    public void FontSize(int value)
    {
        if(_notesText.pointSize + value > 14 && _notesText.pointSize + value < 72)
        {
            _notesText.pointSize += value;
        }
    }
}
