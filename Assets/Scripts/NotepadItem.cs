using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotepadItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    private NotepadData data;
    private NotesManager notesManager;
    
    public void Init(NotepadData data, NotesManager notesManager)
    {
        this.data = data;
        this.notesManager = notesManager;
        title.text = data.title;
    }

    public void SelectNote()
    {
        notesManager.SelectNotepad(data);
    }
}
