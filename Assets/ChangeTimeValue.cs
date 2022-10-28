using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeTimeValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    private TimeChange _timeChange;
    private int hour;
    private int minute;
    private bool isHour = true;

    public void OpenChange(TimeChange tc, int hour, int minute, bool isHour = true)
    {
        this.hour = hour;
        this.minute = minute;
        this.isHour = isHour;
        _timeChange = tc;
        
        gameObject.SetActive(true);
        _text.text = $"{hour:00}:{minute:00}";
    }

    public void ChangeHour(int i)
    {
        hour += i;
        if ((isHour && hour >= 24) || (!isHour && hour >= 60))
            hour = 0;
        else if (hour < 0)
            hour = isHour ? 23 : 59;
        
        _text.text = $"{hour:00}:{minute:00}";
    }
    
    public void ChangeMinute(int i)
    {
        minute += i;
        if (minute >= 60)
            minute = 0;
        else if (minute < 0)
            minute = 59;
        
        _text.text = $"{hour:00}:{minute:00}";
    }

    public void SaveChange()
    {
        _timeChange.ChangeTimer(hour,minute);
        gameObject.SetActive(false);
    }

    public void CancelChange()
    {
        gameObject.SetActive(false);
    }
}

public interface TimeChange
{
    public abstract void ChangeTimer(int m, int s);
}
