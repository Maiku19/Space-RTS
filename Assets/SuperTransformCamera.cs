using UnityEngine;

public class SuperTransformCamera : SuperTransform2D
{

    [SerializeField] Camera[] cams;
    [SerializeField] float speed;
    [SerializeField] float zoomSpeed;

    private ChunkMap.Chunk[,] currentChunks = new ChunkMap.Chunk[3, 3];

    private static SuperTransformCamera _instance;

    public delegate void OnChunkChanged(ChunkMap.Chunk newChunk);

    public static SuperTransformCamera Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SuperTransformCamera();
            }

            return _instance;
        }
    }
    public static OnChunkChanged chunkUpdate;
    
    Camera cam;

    private void Update()
    {
        Move(Input.GetAxis("Horizontal") * cam.orthographicSize * speed, Input.GetAxis("Vertical") * cam.orthographicSize * speed);

        cam.orthographicSize += -Input.mouseScrollDelta.y * cam.orthographicSize * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 2_500);
        UpdateCameras(cam.orthographicSize);
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        GetSurroundingChunks();
    }

    // -------------

    public override void InitializeClones()
    {
        
    }

    void UpdateCameras(float orthographicSize)
    {
        foreach (Camera clone in cams)
        {
            clone.GetComponent<Camera>().orthographicSize = orthographicSize;
        }

        // Offsets clones twards the nearest edge
        Vector2 mulVector = new Vector2(Mathf.Sign(transform.position.x), Mathf.Sign(transform.position.y));

        cams[0].transform.position = (Vector2)transform.position + ChunkMap.chunkSize * 2 * new Vector2(-mulVector.x, 0);
        cams[1].transform.position = (Vector2)transform.position + ChunkMap.chunkSize * 2 * new Vector2(0, -mulVector.y);
        cams[2].transform.position = (Vector2)transform.position + ChunkMap.chunkSize * 2 * -mulVector;
    }

    private void GetSurroundingChunks()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y < 1; y++)
            {
                currentChunks[x + 1, y + 1] = ChunkMap.GetChunk(Chunk.x + x, Chunk.y + y);
            }
        }
    }

    public override void SetPosition(Vector2Int newChunk, Vector2 newPosition)
    {
        // This can be optimized
        if (newChunk != new Vector2Int(Chunk.x, Chunk.y))
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    currentChunks[x + 1, y + 1].SetVisiblity(false);

                    ChunkMap.GetChunk(Chunk.x + x, Chunk.y + y).SetVisiblity(true);

                    currentChunks[x + 1, y + 1] = ChunkMap.GetChunk(Chunk.x + x, Chunk.y + y);
                }
            }

        }

        base.SetPosition(newChunk, newPosition);
    }
}