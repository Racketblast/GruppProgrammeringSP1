using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, -10f);
    [SerializeField] private float Smoothing = 3.5f;
    private SpriteRenderer rend;
    private float horizontalValue;


    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, Smoothing * Time.deltaTime);
        transform.position = newPosition;

        horizontalValue = Input.GetAxis("Horizontal");

        if (horizontalValue < 0)
        {
            offset = new Vector3(-2, 0.5f, -10f);
        }
        else if (horizontalValue > 0)
        {
            offset = new Vector3(2, 0.5f, -10f);
        }
        else
        {
            offset = new Vector3(0, 0.5f, -10f);
        }
    }
}
