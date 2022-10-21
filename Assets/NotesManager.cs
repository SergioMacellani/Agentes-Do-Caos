using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _notesText;
    [SerializeField] private Scrollbar _scrollbar;
    
    private ScreenOrientation _orientation;
    private bool _verticalRotation = false;
    
    public bool VerticalRotation { set => _verticalRotation = value; }

    public void SetValue(PlayerSheetData pSheet)
    {
        _notesText.text = pSheet.texts.Notes;
        _notesText.pointSize = pSheet.texts.fontSize;
        _notesText.verticalScrollbar.value = 0;
    }

    public void RotateScreen(bool rotate)
    {
        if(!_verticalRotation) return;
        
        if (rotate)
        {
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            _orientation = Screen.orientation;
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = _orientation;
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            StartCoroutine(ScrollToNotes());
        }
    }
    
    public PlayerText GetValue()
    {
        PlayerText pText = new PlayerText
        {
            Notes = _notesText.text,
            fontSize = (int)_notesText.pointSize
        };
        
        return pText;
    }

    public void FontSize(int value)
    {
        if(_notesText.pointSize + value > 14 && _notesText.pointSize + value < 72)
        {
            _notesText.pointSize += value;
        }
    }
    
    private IEnumerator ScrollToNotes()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForEndOfFrame();
            _scrollbar.value = .25f;
        }
    }
}
