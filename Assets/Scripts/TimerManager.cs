using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour, TimeChange
{
    [SerializeField] private int _seconds;
    [SerializeField] private int _minutes;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private StatesButton _playButton;
    private int[] timerPreset = {2,5,10,15,20,30,45,0};

    private IEnumerator _timerCoroutine;
    
    private void Start()
    {
        _timerCoroutine = Timer();
        _minutes = 0;
        _seconds = 0;
        _timerText.text = $"{_minutes:00}:{_seconds:00}";
    }

    public void PlayPause(int play)
    {
        if (play == 0)
            StartCoroutine(_timerCoroutine);
        else
            StopCoroutine(_timerCoroutine);
    }

    public void SetTimerPreset(int i)
    {
        StopCoroutine(_timerCoroutine);
        _playButton.SetValue(0);

        if (timerPreset[i] != 0)
        {
            _minutes = timerPreset[i];
            _seconds = 0;
            _timerText.text = $"{_minutes:00}:{_seconds:00}";
        }
        else
        {
            _minutes = 59;
            _seconds = 59;
            _timerText.text = "1:00:00";
        }
    }
    
    public void ChangeTimer(int m, int s)
    {
        _minutes = m;
        _seconds = s;
        
        _timerText.text = $"{_minutes:00}:{_seconds:00}";
    }
    
    public void OpenChange(ChangeTimeValue ctv)
    {
        StopCoroutine(_timerCoroutine);
        _playButton.SetValue(0);
        ctv.OpenChange(this, _minutes, _seconds, false);
    }

    private IEnumerator Timer()
    {
        _timerText.text = $"{_minutes:00}:{_seconds:00}";
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _seconds--;
            if (_seconds < 0)
            {
                _seconds = 59;
                _minutes--;
                if (_minutes <= 0)
                {
                    _minutes = 0;
                }
            }
            
            _timerText.text = $"{_minutes:00}:{_seconds:00}";
            
            if (_seconds <= 0 && _minutes <= 0)
                StopCoroutine(_timerCoroutine);
        }
    }
}
