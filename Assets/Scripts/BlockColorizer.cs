using UnityEngine;

public class BlockColorizer : MonoBehaviour
{
    
    [SerializeField] private Material materialDeselected;
    [SerializeField] private Material materialSelected;
    [SerializeField] private Material materialHeightened;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool heightened, selected;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        heightened = transform.position.y > 0;
        if (heightened)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialHeightened;
        }
        else if (!selected)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialDeselected;
        }
        if(selected)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialSelected;
        }
    }
    
    public void select()
    {
        selected = true;
    }

    public void deselect()
    {
        selected = false;
    }
}
