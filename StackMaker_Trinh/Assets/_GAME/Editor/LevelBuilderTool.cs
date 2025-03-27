using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelBuilderTool : EditorWindow
{
    private enum BlockType
    {
        None,
        Ground,
        Brick,
        Obstacle,
        Base
    }

    private BlockType currentBlockType = BlockType.None;

    private GameObject groundBlockPrefab;
    private GameObject brickBlockPrefab;
    private GameObject obstacleBlockPrefab;
    private GameObject baseBlockPrefab;

    private Vector3? lastPlacedPosition = null;

    [MenuItem("Tools/Level Builder")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilderTool>("Level Builder");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("🛠️ Level Builder Tool", EditorStyles.boldLabel);

        currentBlockType = (BlockType)EditorGUILayout.EnumPopup("Block Type", currentBlockType);

        GUILayout.Space(10);
        GUILayout.Label("🔧 Block Prefabs", EditorStyles.boldLabel);
        groundBlockPrefab    = (GameObject)EditorGUILayout.ObjectField("Ground Prefab", groundBlockPrefab, typeof(GameObject), false);
        brickBlockPrefab     = (GameObject)EditorGUILayout.ObjectField("Brick Prefab", brickBlockPrefab, typeof(GameObject), false);
        obstacleBlockPrefab  = (GameObject)EditorGUILayout.ObjectField("Obstacle Prefab", obstacleBlockPrefab, typeof(GameObject), false);
        this.baseBlockPrefab = (GameObject)EditorGUILayout.ObjectField("Base Prefab", this.baseBlockPrefab, typeof(GameObject), false);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.alt && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            TryEraseBlockAtMousePosition(e);
        }
        else if (e.shift && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            TryPlaceBlockAtMousePosition(e);
        }

        if (e.type == EventType.MouseUp && e.button == 0)
        {
            lastPlacedPosition = null;
        }
    }

    private void TryPlaceBlockAtMousePosition(Event e)
    {
        GameObject prefabToPlace = GetPrefabForCurrentBlockType();
        if (prefabToPlace == null) return;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector3 gridPos = new Vector3(
                Mathf.Floor(point.x) + 0.5f,
                GetBlockYByType(currentBlockType),
                Mathf.Floor(point.z) + 0.5f);

            float blockHeight = GetBlockHeight(prefabToPlace);

            int safety = 100;
            while (Physics.CheckBox(gridPos, Vector3.one * 0.45f) && safety-- > 0)
            {
                gridPos.y += blockHeight;
            }



            if (lastPlacedPosition == null || Vector3.Distance(gridPos, lastPlacedPosition.Value) > 0.01f)
            {
                GameObject newBlock = (GameObject)PrefabUtility.InstantiatePrefab(prefabToPlace);
                newBlock.transform.position = gridPos;
                Undo.RegisterCreatedObjectUndo(newBlock, "Place Block");

                ApplySettingsForBlockType(newBlock);

                lastPlacedPosition = gridPos;
                e.Use();
            }
        }
    }

    private float GetBlockHeight(GameObject prefab)
    {
        if (prefab.TryGetComponent<Collider>(out var collider))
        {
            return collider.bounds.size.y;
        }

        return 1f;
    }


    private GameObject GetPrefabForCurrentBlockType()
    {
        return currentBlockType switch
        {
            BlockType.Ground   => groundBlockPrefab,
            BlockType.Brick    => brickBlockPrefab,
            BlockType.Obstacle => obstacleBlockPrefab,
            BlockType.Base     => this.baseBlockPrefab,
            _                  => null
        };
    }

    private float GetBlockYByType(BlockType type)
    {
        return type switch
        {
            BlockType.Ground => 0f,
            BlockType.Brick => 0.5f,
            BlockType.Obstacle => 0.5f,
            BlockType.Base => 0.5f,
            _ => 0f
        };
    }

    private void ApplySettingsForBlockType(GameObject block)
    {
        string parentName = currentBlockType.ToString(); // Ground, Brick, etc.
        string layerName = parentName;

        var root = GameObject.Find(parentName);
        if (root == null)
        {
            root = new GameObject(parentName);
            Undo.RegisterCreatedObjectUndo(root, "Create Root Group");
        }

        block.transform.SetParent(root.transform);
        SetLayerRecursively(block, LayerMask.NameToLayer(layerName));
    }

    private static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private void TryEraseBlockAtMousePosition(Event e)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector3 gridPos = new Vector3(
                Mathf.Floor(point.x) + 0.5f,
                Mathf.Floor(point.y) + 0.5f,
                Mathf.Floor(point.z) + 0.5f);

            Collider[] colliders = Physics.OverlapBox(gridPos, Vector3.one * 0.45f);

            foreach (var col in colliders)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(col.gameObject))
                {
                    Undo.DestroyObjectImmediate(col.gameObject);
                    e.Use();
                    break;
                }
            }
        }
    }
}