using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class SimpleFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField] private float faderSpeed;
    private bool _isOpen;
    private bool _process;
    private bool _messageReceived;

    public UnityEvent onShowComplete;
    public UnityEvent onHideComplete;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void LateUpdate()
    {
        if (!_process) return;
        if (_isOpen)
        {
            if (_canvasGroup.alpha < 1)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1.1f, Time.deltaTime * faderSpeed);
            }
            else
            {
                _process = false;
                
                if (_messageReceived)
                {
                    _messageReceived = false;
                    Hide();
                }
                
                onShowComplete.Invoke();
            }
        }
        else
        {
            if (_canvasGroup.alpha > 0)
            {
                _canvasGroup.interactable = false;
                _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, -.1f, Time.deltaTime * faderSpeed);
            }
            else
            {
                _process = false;
                
                onHideComplete.Invoke();
            }
        }
    }

    public void Show()
    {
        _canvasGroup.blocksRaycasts = true;
        
        _isOpen = true;
        _process = true;
        _messageReceived = false;
    }

    public void Hide()
    {
        if (_process)
        {
            _messageReceived = true;
        }
        else
        {
            _canvasGroup.blocksRaycasts = false;
            _isOpen = false;
            _process = true;
        }
    }
}
