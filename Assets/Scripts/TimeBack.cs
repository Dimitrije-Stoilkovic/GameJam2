using Unity.VisualScripting;
using UnityEngine;

public class TimeBack : MonoBehaviour
{
    [SerializeField] private float recordingInterval = 0.1f; 
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private Transform clone;
    
    private Vector3[] targetPositions = new Vector3[50];
    
    private ParticleSystem particleSystem;
   
    private int currPosition = 0;
    private float lastRecordTime;
    private bool canRewind;
    private float cdRemaining;
    private bool isGrounded;
    private bool startShrinking = false;
    private bool startExpanding = false;


    private void Start()
    {
        cdRemaining = cooldown;
        particleSystem = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        cdRemaining -= Time.deltaTime;
        if (cdRemaining <= 0)
        {
            canRewind = true;
        }
        else
        {
            canRewind = false;
        }

        if (Time.time - lastRecordTime > recordingInterval)
        {
            targetPositions[currPosition] = transform.position;
            lastRecordTime = Time.time;
            currPosition = (currPosition + 1) % targetPositions.Length; 
        }
        
       
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            if (Physics.Raycast(clone.transform.position, Vector3.down, out RaycastHit hitinfo))
            {
                isGrounded = hitinfo.collider.transform.position.y <= 0;
            }

            if (targetPositions[(currPosition + 1) % targetPositions.Length] != Vector3.zero && canRewind && isGrounded)
            {
                particleSystem.Play();
                //transform.position = targetPositions[(currPosition + 1) % targetPositions.Length];
                canRewind = false;
                cdRemaining = cooldown;
                startShrinking = true;
                particleSystem.GetComponent<AudioSource>().Play();
            }
        }
        if (startShrinking)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f)*2;
        }

        if (transform.localScale == new Vector3(0f, 0f, 0f))
        {
            startShrinking = false;
            transform.position = targetPositions[(currPosition + 1) % targetPositions.Length];
            startExpanding = true;
        } else if (transform.localScale == new Vector3(1f, 1f, 1f))
        {
            startExpanding = false;
        }

        if (startExpanding)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f)*2;
        }
    }

    private void FixedUpdate()
    {
        if (canRewind)
        {
           // clone.GetComponent<MeshRenderer>().enabled = true;
           clone.gameObject.SetActive(true);
        }
        else
        {
            clone.gameObject.SetActive(false);
            //clone.GetComponent<MeshRenderer>().enabled = false;
        }

        if (targetPositions[(currPosition + 1) % targetPositions.Length] != Vector3.zero && canRewind)
        {
            clone.position = targetPositions[(currPosition + 1) % targetPositions.Length];
        }
    }
}

