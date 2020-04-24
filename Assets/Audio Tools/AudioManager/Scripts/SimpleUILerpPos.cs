using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUILerpPos : MonoBehaviour
{
    enum PosState
    {
        In,
        Out
    }

    [SerializeField] private PosState posState = PosState.In;
    [SerializeField] Vector3 inPos = Vector3.zero;
    [SerializeField] Vector3 outPos = Vector3.zero;
    [SerializeField] private float lerpSpeed = .5f;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = (RectTransform) transform;
    }

    private void LateUpdate()
    {
        switch (posState)
        {
            case PosState.In:
                if (_rectTransform.anchoredPosition3D != inPos)
                {
                    _rectTransform.anchoredPosition3D = Vector3.Lerp(_rectTransform.anchoredPosition3D, inPos, Time.deltaTime * lerpSpeed);
                }
                break;
            case PosState.Out:
                if (_rectTransform.anchoredPosition3D != outPos)
                {
                    _rectTransform.anchoredPosition3D = Vector3.Lerp(_rectTransform.anchoredPosition3D, outPos, Time.deltaTime * lerpSpeed);
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
                break;
            case PosState.Out:
                posState = PosState.In;
                break;
        }
    }
}
