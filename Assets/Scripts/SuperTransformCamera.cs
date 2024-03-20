using UnityEngine;

public class SuperTransformCamera : SuperTransform2D
{

    [SerializeField] Camera[] cams;
    [SerializeField] float speed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float maxZoomIn;
    [SerializeField] float maxZoomOut;

    private Chunk[,] currentChunks = new Chunk[3, 3];

    private static SuperTransformCamera _instance;
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
    
    Camera cam;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        Move(Input.GetAxis("Horizontal") * cam.orthographicSize * speed * Time.deltaTime, Input.GetAxis("Vertical") * cam.orthographicSize * speed * Time.deltaTime);

        cam.orthographicSize += -Input.mouseScrollDelta.y * cam.orthographicSize * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, maxZoomIn, maxZoomOut);
        UpdateCameras(cam.orthographicSize);
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        GetSurroundingChunks();
    }

    // -------------


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
            for (int y = -1; y <= 1; y++)
            {
                currentChunks[x + 1, y + 1] = ChunkMap.GetChunk(CurrentChunk.x + x, CurrentChunk.y + y);
            }
        }
    }

    /*public override void UpdateVisibility()
    {
        return;
    }*/

    public override void SetPosition(Vector2Int newChunk, Vector2 newPosition)
    {
        // This can be optimized
        if (newChunk != new Vector2Int(CurrentChunk.x, CurrentChunk.y))
        {
            base.SetPosition(newChunk, newPosition);
            UpdateChunkVisibility();
            return;
        }

        base.SetPosition(newChunk, newPosition);
    }

    void UpdateChunkVisibility()
    {
        // TODO: This can be optimised

        foreach (Chunk chunk in currentChunks)
        {
            chunk.UpdateVisibility();
        }

        currentChunks = new Chunk[currentChunks.GetLength(0), currentChunks.GetLength(1)];
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                ChunkMap.GetChunk(CurrentChunk.x + x, CurrentChunk.y + y).UpdateVisibility();

                currentChunks[x + 1, y + 1] = ChunkMap.GetChunk(CurrentChunk.x + x, CurrentChunk.y + y);
            }
        }
    }
}