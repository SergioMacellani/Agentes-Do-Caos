using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStory : MonoBehaviour
{
    public TextAsset BackstoryPreset;
    public TMP_InputField HistoryText;
    public TMP_InputField NotesText;
    public Animator Canvas;

    public void RandomBackstory()
    {
        string[] bStory = BackstoryPreset.text.Split(new string[] {"\n"}, System.StringSplitOptions.RemoveEmptyEntries);
        HistoryText.text = bStory[Random.Range(0, bStory.Length)];
    }
    
    public void NextPage()
    {
        if (HistoryText.text.ToCharArray().Length >= 250 && NotesText.text.ToCharArray().Length >= 50)
        {
            Canvas.Play("StoryReview");
        }
    }
}
