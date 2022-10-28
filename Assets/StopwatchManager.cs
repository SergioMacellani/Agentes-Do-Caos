using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StopwatchManager : MonoBehaviour
{
    [SerializeField]
    private float _seconds;
    [SerializeField]
    private int _minutes;
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField] 
    private StatesButton _playButton;
    [SerializeField]
    private GameObject _stopButton;
    private float _microseconds => (_seconds - Mathf.Floor(_seconds))*100;
    private bool _isRunning;

    private void Start()
    {
        _seconds = 0;
        _minutes = 0;
        _text.text = $"{_minutes:00}:{Mathf.Floor(_seconds):00}<size=30%>{_microseconds:00}";
    }
    
    public void PlayPause(int state)
    {
        if (state == 0)
        {
            _isRunning = true;
            _stopButton.SetActive(true);
        }
        else
        {
            _isRunning = false;
        }
    }
    
    public void Stop()
    {
        _isRunning = false;
        _playButton.SetValue(0);
        _stopButton.SetActive(false);
        _seconds = 0;
        _minutes = 0;
        _text.text = $"{_minutes:00}:{Mathf.Floor(_seconds):00}<size=30%>{_microseconds:00}";
    }

    private void FixedUpdate()
    {
        if(!_isRunning) return;
        var microseconds = _microseconds;
        if (_microseconds >= 99) microseconds = 99;
        
        _seconds += Time.deltaTime;
        if (_seconds >= 60)
        {
            _seconds = 0;
            _minutes++; 
        }
        _text.text = $"{_minutes:00}:{Mathf.Floor(_seconds):00}<size=30%>{microseconds:00}";
    }
}
