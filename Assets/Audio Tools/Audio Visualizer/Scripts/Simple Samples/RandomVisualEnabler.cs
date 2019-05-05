using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVisualEnabler : MonoBehaviour
{
    [SerializeField] List<GameObject> effectsList = new List<GameObject>();
    GameObject activeEffect = null;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Today.Second);
        ActivateRandomEffect();
    }

    public void ActivateRandomEffect()
    {
        if (effectsList.Count <= 0)
            return;

        DeactivateAllEffects();

        index = Random.Range(0, effectsList.Count);

        activeEffect = effectsList[index];

        activeEffect.SetActive(true);
    }

    public void DeactivateAllEffects()
    {
        if (effectsList.Count > 0)
        {
            for (int i = 0; i < effectsList.Count; i++)
            {
                effectsList[i].SetActive(false);
            }
        }
    }
}
