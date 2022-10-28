using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour, TimeChange
{
    [SerializeField]
    private int _hour;
    [SerializeField]
    private int _minute;
    [SerializeField]
    private int _second;
    [SerializeField]
    private float _timeScale = 1f;
    [SerializeField]
    private TextMeshProUGUI _clockText;
    [SerializeField]
    private StatesButton _playButton;
    [SerializeField]
    private Image[] _timeScaleImages;
    private float[] _timeScaleValues = { .25f,.5f, 1f, 2f, 4f};
    
    private IEnumerator _clockCoroutine;
    private Color _activeColor = new Color(1,1,1,.25f);
    private Color _inactiveColor = new Color(.35f,.35f,.35f,.25f);
    
    private void Start()
    {
        _clockCoroutine = Clock();
        ChangeTimeScale(2);
        _hour = 0;
        _minute = 0;
        _second = 0;
        _clockText.text = $"{_hour:00}:{_minute:00}<size=30%>{_second:00}";
    }

    public void PlayPause(int play)
    {
        if (play == 0)
            StartCoroutine(_clockCoroutine);
        else
            StopCoroutine(_clockCoroutine);
    }
    
    public void ChangeTimeScale(int timeScale)
    {
        _timeScale = _timeScaleValues[timeScale];
        for (int i = 0; i < _timeScaleImages.Length; i++)
        {
            _timeScaleImages[i].color = i == timeScale ? _activeColor : _inactiveColor;
        }
    }
    
    public void AddHour()
    {
        _hour++;
        if (_hour >= 24)
        {
            _hour = 0;
        }
        _clockText.text = $"{_hour:00}:{_minute:00}<size=30%>{_second:00}";
    }
    
    public void AddMinute()
    {
        _minute++;
        if (_minute >= 60)
        {
            _minute = 0;
            _hour++;
        }
        _clockText.text = $"{_hour:00}:{_minute:00}<size=30%>{_second:00}";
    }

    public void ChangeTimer(int m, int s)
    {
        _hour = m;
        _minute = s;
        _second = 0;
        
        _clockText.text = $"{_hour:00}:{_minute:00}<size=30%>{_second:00}";
    }

    public void OpenChange(ChangeTimeValue ctv)
    {
        StopCoroutine(_clockCoroutine);
        _playButton.SetValue(0);
        ctv.OpenChange(this, _hour, _minute, true);
    }
    
    private IEnumerator Clock()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / _timeScale);
            _second++;
            if (_second >= 60)
            {
                _second = 0;
                _minute++;
                if (_minute >= 60)
                {
                    _minute = 0;
                    _hour++;
                    if (_hour >= 24)
                    {
                        _hour = 0;
                    }
                }
            }
            _clockText.text = $"{_hour:00}:{_minute:00}<size=30%>{_second:00}";
        }
    }

}
