using UnityEngine;

public class BlockMover : MonoBehaviour
{
    
    private bool ascendingStarted;
    private bool descendingStarted;

    [SerializeField] private GameObject brush;

    public int blockMatrixI;
    public int blockMatrixJ;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!descendingStarted && ascendingStarted && transform.position.y < 1f)
        {
            if (transform.position.y + Time.deltaTime * 2f > 1f)
            {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
            }
            else
            {
                transform.position += new Vector3(0f, Time.deltaTime * 2f, 0f); 
            }
            
        } else if (transform.position.y >= 1f)
        {
            ascendingStarted = false;
        }
        
        if (!ascendingStarted && descendingStarted && transform.position.y > 0)
        {
            if (transform.position.y - Time.deltaTime * 2f < 0 )
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            else
            {
                transform.position -= new Vector3(0f, Time.deltaTime * 2f, 0f); 
            }
            
        } else if (transform.position.y <= 0 && descendingStarted)
        {
            brush.GetComponent<BrushController>().matrix[blockMatrixI, blockMatrixJ] = 0;
            descendingStarted = false;
        }
    }

    public void startAscending()
    {
        ascendingStarted = true;
        descendingStarted = false;
    }

    public void startDescending()
    {
        descendingStarted = true;
        ascendingStarted = false;
    }

    
}
