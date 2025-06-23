using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{

    [SerializeField] private AudioSource winAudio;
    [SerializeField] private InputActionReference move;
    [SerializeField] private float speed;
    private Animator animator;
    private Vector2 direction;
    void Start()
    {
        animator =  transform.GetChild(0).GetComponent<Animator>();
    }


    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitinfo))
        {
            if (hitinfo.collider.CompareTag("Goal"))
            {
                //winAudio.Play();
                //ovde aktiviraj ending cutscene
                SceneManager.LoadScene("SaturnWon");
            }
        }

        direction = move.action.ReadValue<Vector2>();
        if (direction.magnitude > .2f)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        transform.Translate(Time.deltaTime * speed * new Vector3(direction.x, 0, direction.y), Space.World);
        transform.position = new Vector3(Math.Clamp(transform.position.x, 0, 20), transform.position.y,Math.Clamp(transform.position.z, 0, 20));
    }
}
