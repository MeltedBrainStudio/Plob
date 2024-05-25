using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Setting")]
    public float ParallaxEffect;
    public float Offset;

    [Header("Reference")]
    [SerializeField] private Transform camPos;

    public float length, startPos;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!camPos)
            camPos = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
#endif

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x - Offset;
    }

    private void FixedUpdate()
    {
        float _temp = (camPos.position.x * (1 - ParallaxEffect));
        float _dist = (camPos.position.x * ParallaxEffect);
        transform.position = new Vector3(startPos + _dist, transform.position.y, transform.position.z);

        //Debug.Log(_temp + " ++ Comparing ++ " + startPos + length);

        if (_temp > startPos + length) startPos += length;
        else if (_temp < startPos - length) startPos -= length;
    }
}
