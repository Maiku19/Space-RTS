using Mike;
using UnityEngine;

public class ChunkMap : MonoBehaviour
{
    // TODO: Update this script to exclude chunks at x = 0 and y = 0.

    [SerializeField] private Vector2 _chunkSize = Vector2.one * 10_000;
    [SerializeField] private Vector2Int _mapSize = Vector2Int.one * 100;

    public static Vector2 chunkSize = Vector2.one * 10_000;
    public static Vector2Int mapSize = Vector2Int.one * 100;


    static Chunk[,] _chunks;

    public static Chunk[,] Chunks
    {
        get
        {
            if (_chunks == null)
            {
                SpawnChunks();
            }

            return _chunks;
        }
    }

    void Awake()
    {
        InitializeVariables();

        if (_chunks == null)
        {
            SpawnChunks();
        }
    }

    void InitializeVariables()
    {
        chunkSize = _chunkSize;
        mapSize = _mapSize;
    }

    /// <summary>
    /// Gets the chunk in the grid array
    /// </summary>
    /// <param name="x">The x coordinate in the grid</param>
    /// <param name="y">The y coordinate in the grid</param>
    /// <returns>The chunk in the provided coordinates. will return null if <paramref name="x"/> or <paramref name="y"/> equal 0</returns>
    public static Chunk GetChunk(int x, int y) 
    { 
        if (Chunks == null) { SpawnChunks(); }
        return x == 0 || y == 0 ? null : Chunks[x + mapSize.x + 1, y + mapSize.y + 1]; 
    }

    private static void SpawnChunks()
    {
        _chunks = new Chunk[mapSize.x * 2, mapSize.y * 2];
        for (int x = 0; x < _chunks.GetLength(0); x++)
        {
            for (int y = 0; y < _chunks.GetLength(1); y++)
            {
                _chunks[x, y] = new Chunk(new Vector2Int(x - mapSize.x, y - mapSize.y));
            }
        }
    }

    public static void SpawnObject(GameObject go, Vector2Int chunkPosition, Vector2 positionInChunk, Quaternion? rot = null, Transform parent = null)
    {
        GameObject objectInstance = Instantiate(go, parent);
        if(rot != null) { objectInstance.transform.rotation = (Quaternion) rot; }
        if(objectInstance.TryGetComponent(out SuperTransform2D str)) { str = objectInstance.AddComponent<SuperTransform2D>(); };

        str.SetPosition(chunkPosition, positionInChunk);
    }

    public static void RegisterObject(GameObject go, Vector2Int chunkPosition, Vector2 positionInChunk, Quaternion? rot = null, Transform parent = null)
    {
        if (rot != null) { go.transform.rotation = (Quaternion)rot; }
        if (go.TryGetComponent(out SuperTransform2D str)) { str = go.AddComponent<SuperTransform2D>(); };

        str.SetPositionRaw(GetChunk(chunkPosition.x, chunkPosition.y), positionInChunk);
    }
}
