using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoRun : MonoBehaviour
{
    [SerializeField] private UnityEvent autorun;
    [SerializeField] private bool destroyObjectAfter;

    private void Start()
    {
        autorun.Invoke();
        if (destroyObjectAfter) Destroy(gameObject);
        else Destroy(this);
    }
}
