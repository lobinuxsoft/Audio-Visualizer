using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISimpleLerpPos : MonoBehaviour
{
    enum PosState
    {
        In,
        Out
    }

    [SerializeField] private PosState posState = PosState.In;
    [SerializeField] Vector3 inPos = Vector3.zero;
    [SerializeField] Vector3 outPos = Vector3.zero;
    [SerializeField] private float lerpSpeed = 2;
    [SerializeField] private AnimationCurve inLerpBehavior;
    [SerializeField] private AnimationCurve outLerpBehavior;

    private RectTransform _rectTransform;
    [SerializeField] private float lerp = 0;

    private bool _process;

    private void Start()
    {
        _rectTransform = (RectTransform) transform;

        switch (posState)
        {
            case PosState.In:
                lerp = 1;
                break;
            case PosState.Out:
                lerp = 0;
                break;
        }
    }

    private void LateUpdate()
    {
        if (!_process) return;
        
        switch (posState)
        {
            case PosState.In:
                if (lerp < 1)
                {
                    lerp = Mathf.Lerp(lerp, 1.1f, lerpSpeed * Time.deltaTime);
                    _rectTransform.anchoredPosition3D = Vector3.Lerp(_rectTransform.anchoredPosition3D, inPos, inLerpBehavior.Evaluate(lerp));
                }
                else
                {
                    _process = false;
                }
                break;
            case PosState.Out:
                if (lerp > 0)
                {
                    lerp = Mathf.Lerp(lerp, -.1f, lerpSpeed * Time.deltaTime);
                    _rectTransform.anchoredPosition3D = Vector3.Lerp(_rectTransform.anchoredPosition3D, outPos, outLerpBehavior.Evaluate(lerp));
                }
                else
                {
                    _process = false;
                }
                break;
        }
    }

    public void ChangeState()
    {
        switch (posState)
        {
            case PosState.In:
                posState = PosState.Out;
                _process = true;
                break;
            case PosState.Out:
                posState = PosState.In;
                _process = true;
                break;
        }
    }
}
