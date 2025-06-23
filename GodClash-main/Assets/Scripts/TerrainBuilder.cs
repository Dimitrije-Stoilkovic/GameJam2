using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Assign a Cube prefab or the built-in Cube
    public int dimension;
    public float spacing; // Slight spacing to avoid overlapping

    void Start()
    {
        GeneratePlatform();
    }

    void GeneratePlatform()
    {
        for (int x = 0; x < dimension; x++)
        {
            for (int z = 0; z < dimension; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject createdBlock = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                createdBlock.GetComponent<BlockMover>().blockMatrixI = x;
                createdBlock.GetComponent<BlockMover>().blockMatrixJ = z;
            }
        }
    }
}