using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float      offsetY         = 1f;
    [SerializeField] private int        initialPoolSize = 50;

    public static BlockManager Instance                 { get; private set; }
    public        int          TotalBrickCount          { get; private set; }
    public        Vector3?     FirstBrickPosition       { get; private set; }
    public        GameObject   FirstBrick               { get; private set; }
    public        Vector3?     FirstBrickCenterPosition { get; private set; }

    public GameObject FirstGroundBlock    { get; private set; }
    public Vector3    FirstGroundPosition => FirstGroundBlock != null ? FirstGroundBlock.transform.position : Vector3.zero;

    public ObjectPool brickObjectPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        this.brickObjectPool = new ObjectPool(brickPrefab, initialPoolSize);
        this.SpawnBricks();
    }

    private void SpawnBricks()
    {
        this.TotalBrickCount = 0;

        var brickList  = new List<GameObject>();
        var groundList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ground"));

        // Sắp xếp Ground trước (Z tăng dần, X tăng dần)
        groundList.Sort((a, b) =>
        {
            float zA = a.transform.position.z;
            float zB = b.transform.position.z;

            if (!Mathf.Approximately(zA, zB))
            {
                return zA.CompareTo(zB);
            }

            float xA = a.transform.position.x;
            float xB = b.transform.position.x;

            return xA.CompareTo(xB);
        });

        if (groundList.Count > 0)
        {
            this.FirstGroundBlock = groundList[0];
        }

        foreach (var ground in groundList)
        {
            var groundHeight = GetColliderHeight(ground);
            var spawnPos     = ground.transform.position + Vector3.up * groundHeight;

            var brick = this.brickObjectPool.Get();
            brick.transform.position = spawnPos;

            brickList.Add(brick);
            this.TotalBrickCount++;
        }

        brickList.Sort((a, b) =>
        {
            float zA = a.transform.position.z;
            float zB = b.transform.position.z;

            if (!Mathf.Approximately(zA, zB))
            {
                return zA.CompareTo(zB);
            }

            float xA = a.transform.position.x;
            float xB = b.transform.position.x;

            return xA.CompareTo(xB);
        });

        if (brickList.Count > 0)
        {
            var firstBrick = brickList[0];
            this.FirstBrickPosition = firstBrick.transform.position;

            this.FirstBrickCenterPosition = new Vector3(
                Mathf.Floor(firstBrick.transform.position.x) + 0.5f,
                firstBrick.transform.position.y,
                Mathf.Floor(firstBrick.transform.position.z) + 0.5f
            );
        }

        Debug.Log($"[BlockManager] Total bricks spawned: {TotalBrickCount}");
        Debug.Log($"[BlockManager] First Ground Position: {FirstGroundPosition}");
    }

    public void ReturnAllRemainingBricks()
    {
        this.brickObjectPool.ReturnAll();
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