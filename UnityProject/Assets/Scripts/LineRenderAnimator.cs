using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRenderAnimator : MonoBehaviour
{
    public float scrollRate = 2.5f;

    LineRenderer lineRenderer;
    Material material;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        material = lineRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += Vector2.left * (Time.deltaTime / scrollRate);
    }
}
