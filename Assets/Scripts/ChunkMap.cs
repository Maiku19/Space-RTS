using Mike;
using UnityEngine;

public class ChunkMap : MonoBehaviour
{
    [System.Serializable]
    public struct Chunk
    {
        public SuperTransform2D[] objectsInChunk;
        public int x;
        public int y;

        public Chunk(Vector2Int pos)
        {
            objectsInChunk = new SuperTransform2D[0];
            x = pos.x;
            y = pos.y;
        }

        public void AddObject(SuperTransform2D st)
        {
            Debug.Log($"Added {st} from chunk: ({x}, {y})");
            MikeArray.Append(ref objectsInChunk, st);
        }

        public void RemoveObject(SuperTransform2D st)
        {
            Debug.Log($"Removed {st} from chunk: ({x}, {y})");
            MikeArray.RemoveByReference(ref objectsInChunk, ref st);
        }

        public void SetVisiblity(bool visibility)
        {
            if (objectsInChunk == null) { return; }
            foreach (SuperTransform2D st in objectsInChunk)
            {
                st.SetVisibility(visibility);
            }
        }
    }

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
                _chunks = new Chunk[mapSize.x * 2, mapSize.y * 2];
                SpawnChunks();
            }

            return _chunks;
        }
    }

    void Awake()
    {
        chunkSize = _chunkSize;

        if (_chunks == null)
        {
            _chunks = new Chunk[mapSize.x * 2, mapSize.y * 2];
            SpawnChunks();
        }
    }

    public static Chunk GetChunk(int x, int y) { return _chunks[x + mapSize.x, y + mapSize.y]; }

    private static void SpawnChunks()
    {
        for (int x = 0; x < mapSize.x * 2; x++)
        {
            for (int y = 0; y < mapSize.y * 2; y++)
            {
                _chunks[x, y] = new Chunk(new Vector2Int(x - mapSize.x, y - mapSize.y));
            }
        }
    }
}
