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
    [SerializeField] private RectTransform _notepadContent;
    [SerializeField] private NotepadItem _notepadPrefab;
    [SerializeField] private GameObject _addNote;

    private PlayerNotes _playerNotes;
    private int noteIndex = 0;
    private ScreenOrientation _orientation;
    private bool _verticalRotation = false;
    
    public bool VerticalRotation { set => _verticalRotation = value; }

    public void SetValue(PlayerNotes pNotes)
    {
        _playerNotes = pNotes;

        if(_playerNotes.notepad.Count == 0)
            _playerNotes.notepad.Add(new NotepadData("", "Notas"));
            
        _notesText.text = _playerNotes.notepad[0].notes;
        _notesText.pointSize = _playerNotes.fontSize;
        _notesText.verticalScrollbar.value = 0;

        UpdateNotepads();
    }

    private void UpdateNotepads()
    {
        foreach (Transform note in _notepadContent)
        {
            Destroy(note.gameObject);
        }

        foreach (var notepad in _playerNotes.notepad)
        {
            Instantiate(_notepadPrefab, _notepadContent).Init(notepad, this);
        }
    }

    public void SelectNotepad(NotepadData notepad)
    {
        _notesText.text = notepad.notes;
        noteIndex = _playerNotes.notepad.IndexOf(notepad);
    }
    
    public void SaveNotepad()
    {
        Debug.Log("Anotações salvas!");
        _playerNotes.notepad[noteIndex].notes = _notesText.text;
    }

    public void AddNotepad(TMP_InputField input)
    {
        _playerNotes.notepad.Add(new NotepadData("", input.text));
        Instantiate(_notepadPrefab, _notepadContent).Init(_playerNotes.notepad[^1], this);
        SelectNotepad(_playerNotes.notepad[^1]);
        noteIndex = _playerNotes.notepad.Count - 1;
        _addNote.SetActive(false);
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

    public PlayerNotes GetValue() => _playerNotes;

    public void FontSize(int value)
    {
        if(_notesText.pointSize + value > 14 && _notesText.pointSize + value < 72)
        {
            _notesText.pointSize += value;
            _playerNotes.fontSize = (int)_notesText.pointSize;
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
