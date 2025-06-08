using UnityEngine;

public class Levitate : MonoBehaviour
{
    private float factor = -0.2f;

    private float timer = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * factor);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            factor *= -1;
            timer = 1;
        }
    }
}
