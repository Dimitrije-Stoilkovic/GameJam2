using System;
using Unity.VisualScripting;
using UnityEngine;

public class RingsFade : MonoBehaviour
{

    private int circleId;
    private Color currentColor;
    private Material currentMaterial;
    private Renderer currentRenderer;

    [SerializeField] private float fadingSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleId = 7;
        currentRenderer = transform.GetChild(circleId).GetComponent<Renderer>();
        currentMaterial = currentRenderer.material;
        currentColor = currentMaterial.GetColor("_BaseColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRenderer != null)
        {
            if (currentColor.a <= 0 && circleId > 0)
            {
                circleId--;
                Destroy(currentRenderer.gameObject);
                currentRenderer = transform.GetChild(circleId).GetComponent<Renderer>();
                currentMaterial = currentRenderer.material;
                currentColor = currentMaterial.GetColor("_BaseColor");
            }
            else if (currentColor.a > 0)
            {
                currentColor.a -= Time.deltaTime * fadingSpeed;
                currentMaterial.SetColor("_BaseColor", currentColor);
            }

            if (currentColor.a <= 0 && circleId == 0)
            {
                Destroy(currentRenderer.gameObject);
                currentRenderer = null;
            }
        }
    }
}
