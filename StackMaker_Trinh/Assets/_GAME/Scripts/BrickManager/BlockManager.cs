using System.Net.NetworkInformation;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float      offsetY         = 1f;
    [SerializeField] private int        initialPoolSize = 50;

    public static BlockManager Instance              { get; private set; }
    public        int                 TotalBrickCount       { get; private set; }
    public        Vector3?            FirstBrickPosition    { get; private set; }

    public Vector3? FirstBrickCenterPosition { get; private set; }

    public ObjectPool brickObjectPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        this.brickObjectPool = new ObjectPool(brickPrefab, initialPoolSize);
        this.SpawnBricks();
        Debug.Log(CountGroundBlocks());
        Debug.Log(CountBridgeBlocks());
    }

    private void Start()
    {
    }

    private void SpawnBricks()
    {
        this.TotalBrickCount = 0;

        var groundObjects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var ground in groundObjects)
        {

            var groundHeight = GetColliderHeight(ground);

            var spawnPos = ground.transform.position + Vector3.up * (groundHeight);

            var brick = this.brickObjectPool.Get();
            brick.transform.position = spawnPos;

            if (this.TotalBrickCount == 0)
            {
                this.FirstBrickPosition = spawnPos;

                this.FirstBrickCenterPosition = new Vector3(
                    Mathf.Floor(spawnPos.x) + 0.5f,
                    spawnPos.y,
                    Mathf.Floor(spawnPos.z) + 0.5f
                );
            }

            this.TotalBrickCount++;
        }
        Debug.Log($"[BlockManager] Total bricks spawned: {TotalBrickCount}");
    }

    private static float GetColliderHeight(GameObject obj)
    {
        var col = obj.GetComponentInChildren<Collider>();
        if (col != null)
        {
            return col.bounds.size.y;
        }

        var rend = obj.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            return rend.bounds.size.y;
        }

        return 1f; // fallback
    }

    private Vector3 GetBlockCenter(GameObject block)
    {
        var col = block.GetComponent<Collider>();
        if (col != null)
        {
            return col.bounds.center;
        }

        var rend = block.GetComponent<Renderer>();
        if (rend != null)
        {
            return rend.bounds.center;
        }

        return block.transform.position;
    }


    private static int CountGroundBlocks()
    {
        var groundLayer      = LayerMask.NameToLayer("Ground");
        var groundBlockCount = 0;
        var groundObjects    = GameObject.FindGameObjectsWithTag("Ground");

        foreach (var ground in groundObjects)
        {
            if (ground.layer == groundLayer)
            {
                groundBlockCount++;
            }
        }

        return groundBlockCount;
    }

    private static int CountBridgeBlocks()
    {
        var bridgeBlockCount = 0;
        var bridgeObjects    = GameObject.FindGameObjectsWithTag("Bridge");

        foreach (var bridge in bridgeObjects)
        {

            bridgeBlockCount++;

        }
        return bridgeBlockCount;
    }
}