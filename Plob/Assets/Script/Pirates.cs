using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirates : MonoBehaviour
{
    [Header("Setting")]
    public string Name;

    [Space]
    [Header("Reference")]
    [SerializeField] private Animator anim;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!anim)
            anim = GetComponent<Animator>();
    }
#endif

    public void Animating(string _trigger) => anim.SetTrigger(_trigger);
}
