using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            transform.Rotate(Time.deltaTime * speed * new Vector3(0, 4, 0), Space.Self);
        }
        else
        {
            transform.Rotate(Time.deltaTime * speed * new Vector3(0, 4, 0), Space.World);
        }
    }
}
