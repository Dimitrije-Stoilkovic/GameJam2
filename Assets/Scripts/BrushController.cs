using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BrushController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform player, goal;
    [SerializeField] private int maxWalls = 15;
    [SerializeField] private GameObject terrainBuilder;
    [SerializeField] private GameObject child;
    [SerializeField] private AudioSource blockAudio;
    [SerializeField] public float timer;
    
    public int[,] matrix;
    public bool[,] checkingMatrix; 

    private bool attacking, goalMoved,inRange, isAtGroundLevel;
    private Vector2 mousePosition;

    private GameObject selectedBlock;
    
    private Queue<int[]> queueBFS = new Queue<int[]>();
    private bool pathFound = false;

    private Queue<Transform> raisedBlocks = new Queue<Transform>();

    public int playerX;
    public int playerY;

    private int n;

    private void Start()
    {
        selectedBlock = null;

        n = terrainBuilder.GetComponent<PlatformGenerator>().dimension;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        //Inicijalizacija matrice nulama
        matrix = new int [n, n];
        checkingMatrix = new bool[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                checkingMatrix[i, j] = false;
                matrix[i, j] = 0;
            }
        }
        matrix[n - 1, n - 1] = 2;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            SceneManager.LoadScene("JupiterWon");
        }

        mousePosition = move.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(mousePosition.x, 0, mousePosition.y);
        transform.Translate(speed * Time.deltaTime * moveDirection , Space.World);
        transform.position = new Vector3(Math.Clamp(transform.position.x, 0, n-1), transform.position.y,Math.Clamp(transform.position.z, 0, n-1));


        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitinfo, 5f))
        {

            Debug.DrawRay(transform.position, Vector3.down * hitinfo.distance, Color.red);

            Transform target = hitinfo.collider.transform;



            inRange = Vector3.Distance(target.position, player.position) > 2.5f;
            isAtGroundLevel = target.position.y == 0f;

            if (hitinfo.collider.tag == "Terrain")
            {
                if (selectedBlock == null)
                {
                    selectedBlock = hitinfo.collider.gameObject;
                    
                    selectedBlock.transform.GetChild(0).transform.GetChild(0).GetComponent<BlockColorizer>().select();
                }
                else if (selectedBlock != hitinfo.collider.gameObject)
                {
                    selectedBlock.transform.GetChild(0).transform.GetChild(0).GetComponent<BlockColorizer>().deselect();
                    selectedBlock = hitinfo.collider.gameObject;
                    hitinfo.collider.gameObject.transform.GetChild(0).transform.GetChild(0)
                        .GetComponent<BlockColorizer>().select();
                }
            }

            if (target.gameObject.CompareTag("Terrain"))
            {
                if (attacking && inRange && isAtGroundLevel
                    && BFS(target.GetComponent<BlockMover>().blockMatrixI,
                    target.GetComponent<BlockMover>().blockMatrixJ))
                {
                    if (raisedBlocks.Count >= maxWalls)
                    {
                        Transform oldestBlock = raisedBlocks.Dequeue();
                        //oldestBlock.position = new Vector3(oldestBlock.position.x, 0f, oldestBlock.position.z);
                        oldestBlock.gameObject.GetComponent<BlockMover>().startDescending();
                        matrix[target.GetComponent<BlockMover>().blockMatrixI, 
                            target.GetComponent<BlockMover>().blockMatrixJ] = 1;
                    }

                    //target.Translate(Vector3.up); staro
                    target.gameObject.GetComponent<BlockMover>().startAscending();
                    matrix[target.GetComponent<BlockMover>().blockMatrixI,
                        target.GetComponent<BlockMover>().blockMatrixJ] = 1;
                    raisedBlocks.Enqueue(target);
                }

            }
        }
    }

    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attacking = true;
            blockAudio.Play();
        }
        else if (context.canceled)
        {
            attacking = false;
            
        }
    }

    public void OnMoveGoal(InputAction.CallbackContext context)
    {
        if (context.started && !goalMoved && isAtGroundLevel)
        {
            goalMoved = true;
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitinfo, 5f);
            goal.position = new Vector3(hitinfo.transform.position.x, 0.6f, hitinfo.transform.position.z);
            matrix[hitinfo.collider.GetComponent<BlockMover>().blockMatrixI, hitinfo.collider.GetComponent<BlockMover>().blockMatrixJ] = 2;
            matrix[n - 1, n - 1] = 0;
        }

    }

    bool BFS(int x, int y)
    {

        bool res = false;
        
        matrix[x, y] = 1;
        
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                checkingMatrix[i, j] = false;
            }
        }
        checkingMatrix[playerX, playerY] = true;
        queueBFS.Enqueue(new int[] { playerX, playerY });

        while (queueBFS.Count > 0)
        {
            int[] current = queueBFS.Dequeue();
            int currentX = current[0];
            int currentY = current[1];
          
            if (matrix[currentX, currentY] == 2)
            {
                res = true;
            } 
            else
            {
                int i = currentX - 1;
                int j = currentY;
                if (i >= 0 && i < n && j >= 0 && j < n && !checkingMatrix[i, j] && (matrix[i, j] == 0 || matrix[i, j] == 2))
                {
                    checkingMatrix[i, j] = true;
                    queueBFS.Enqueue(new int[] { i, j });
                }
                i = currentX + 1;
                j = currentY;
                if (i >= 0 && i < n && j >= 0 && j < n && !checkingMatrix[i, j] && (matrix[i, j] == 0 || matrix[i, j] == 2))
                {
                    checkingMatrix[i, j] = true;
                    queueBFS.Enqueue(new int[] { i, j });
                }
                i = currentX;
                j = currentY - 1;
                if (i >= 0 && i < n && j >= 0 && j < n && !checkingMatrix[i, j] && (matrix[i, j] == 0 || matrix[i, j] == 2))
                {
                    checkingMatrix[i, j] = true;
                    queueBFS.Enqueue(new int[] { i, j });
                }
                i = currentX;
                j = currentY + 1;
                if (i >= 0 && i < n && j >= 0 && j < n && !checkingMatrix[i, j] && (matrix[i, j] == 0 || matrix[i, j] == 2))
                {
                    checkingMatrix[i, j] = true;
                    queueBFS.Enqueue(new int[] { i, j });
                }
            }
        }
        
        matrix[x, y] = 0;
        return res;
    }
    
}
