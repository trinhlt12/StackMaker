using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private float      offsetY         = 1f;
    [SerializeField] private int        initialPoolSize = 50;

    public static BlockManager Instance                 { get; private set; }
    public        int          TotalBrickCount          { get; set; }
    public        Vector3?     FirstBrickPosition       { get; private set; }
    public        GameObject   FirstBrick               { get; private set; }
    public        Vector3?     FirstBrickCenterPosition { get; private set; }

    public GameObject FirstGroundBlock { get; private set; }

    public ObjectPool brickObjectPool;

    public List<GameObject> groundList = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Init()
    {
        this.brickObjectPool = new ObjectPool(brickPrefab, initialPoolSize);
        this.SpawnBricks();
    }

    public void SpawnBricks()
    {
        if(Instance == null)
        {
            return;
        }

        this.TotalBrickCount = 0;

        var brickList = new List<GameObject>();
        groundList.Clear();

        // Get the current level instance from LevelManager
        var levelManager         = _GAME.Scripts.Level.LevelManager.Instance;
        var currentLevelInstance = levelManager._currentLevelInstance;

        if (currentLevelInstance != null)
        {
            // Find only ground objects that are children of the current level instance
            foreach (Transform child in currentLevelInstance.GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("Ground"))
                {
                    groundList.Add(child.gameObject);
                }
            }
        }

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
        Debug.Log("Total ground blocks: " + groundList.Count);
    }

    public void ClearBricks()
    {
        var bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach (var brick in bricks)
        {
            brickObjectPool.Return(brick);
        }
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
}