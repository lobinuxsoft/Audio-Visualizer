using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UISimpleFader : MonoBehaviour
{
    enum StateFader
    {
        In,
        Out
    }
    
    private CanvasGroup _canvasGroup;

    [SerializeField] private float faderSpeed = 2;
    [SerializeField] private AnimationCurve inLerpBehavior;
    [SerializeField] private AnimationCurve outLerpBehavior;
    [SerializeField] private StateFader stateFader = StateFader.Out;
    [SerializeField] private bool _process;
    [SerializeField] private float lerp = 0;
    //private bool _messageReceived;

    public UnityEvent onShowComplete;
    public UnityEvent onHideComplete;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        switch (stateFader)
        {
            case StateFader.In:
                lerp = 1;
                break;
            case StateFader.Out:
                lerp = 0;
                break;
        }
    }

    private void LateUpdate()
    {
        if (!_process) return;

        switch (stateFader)
        {
            case StateFader.In:
                
                if (lerp < 1)
                {
                    lerp = Mathf.Lerp(lerp, 1.1f, faderSpeed * Time.deltaTime);
                    
                    _canvasGroup.interactable = true;
                    _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, inLerpBehavior.Evaluate(lerp));
                }
                else
                {
                    _process = false;
                
                    // if (_messageReceived)
                    // {
                    //     _messageReceived = false;
                    //     Hide();
                    // }
                
                    onShowComplete.Invoke();
                }
                
                break;
            case StateFader.Out:
                
                if (lerp > 0)
                {
                    lerp = Mathf.Lerp(lerp, -.1f, faderSpeed * Time.deltaTime);
                    
                    _canvasGroup.interactable = false;
                    _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0, outLerpBehavior.Evaluate(lerp));
                }
                else
                {
                    _process = false;
                
                    onHideComplete.Invoke();
                }
                
                break;
        }
    }

    public void Show()
    {
        _canvasGroup.blocksRaycasts = true;

        stateFader = StateFader.In;
        _process = true;
        //_messageReceived = false;
    }

    public void Hide()
    {
        _canvasGroup.blocksRaycasts = false;
        stateFader = StateFader.Out;
        _process = true;
        
        // if (_process)
        // {
        //     _messageReceived = true;
        // }
        // else
        // {
        //     _canvasGroup.blocksRaycasts = false;
        //     stateFader = StateFader.Out;
        //     _process = true;
        // }
    }
}
