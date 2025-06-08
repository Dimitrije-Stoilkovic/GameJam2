using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

public class PortalCreator : MonoBehaviour
{
    
    [SerializeField] private GameObject brush;

    private GameObject selectedBlock;
    
    private int cnt = 0;
    
    private bool switcher = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedBlock = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitBlock, 3f))
        {
            if (hitBlock.collider.gameObject.CompareTag("Terrain"))
            {
                
                BlockMover newBlock = hitBlock.collider.gameObject.GetComponent<BlockMover>();
                
                if (selectedBlock != null)
                {
                    BlockMover oldBlock = selectedBlock.GetComponent<BlockMover>();
                    brush.GetComponent<BrushController>().matrix[oldBlock.blockMatrixI, oldBlock.blockMatrixJ] = 0;
                }

                selectedBlock = newBlock.gameObject;
                brush.GetComponent<BrushController>().matrix[newBlock.blockMatrixI, newBlock.blockMatrixJ] = 3;
                brush.GetComponent<BrushController>().playerX = newBlock.blockMatrixI;
                brush.GetComponent<BrushController>().playerY = newBlock.blockMatrixJ;
            }
        }
    }
}