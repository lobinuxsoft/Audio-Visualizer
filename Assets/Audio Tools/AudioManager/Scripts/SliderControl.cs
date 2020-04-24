using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderControl : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private float value = 50;
    [SerializeField] private string labelText;
    [SerializeField] private TextMeshProUGUI label;

    [SerializeField] private Button lessButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Slider slider;

    [HideInInspector] public FloatEvent onValueChange;

    private void Start()
    {
        label.text = $"{labelText}";
        
        lessButton.onClick.AddListener(delegate { AddValue(-1); });
        plusButton.onClick.AddListener(delegate { AddValue(1); });

        slider.maxValue = 100;
        slider.minValue = 0;
        slider.value = value;
        
        slider.onValueChanged.AddListener(SetValueFromSlider);
    }

    private void SetValueFromSlider(float sliderValue)
    {
        value = sliderValue;

        onValueChange.Invoke(value/100);
    }

    private void AddValue(int add)
    {
        value += add;
        if (value > 100)
        {
            value = 100;
        }
        else if(value < 0)
        {
            value = 0;
        }
        
        slider.value = value;
        
        onValueChange.Invoke(value/100);
    }

    public float GetValue()
    {
        return value / 100f;
    }

    public void SetValue(float newValue)
    {
        value = newValue * 100f;
        
        slider.value = value;
        
        onValueChange.Invoke(value/100);
    }
}

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }