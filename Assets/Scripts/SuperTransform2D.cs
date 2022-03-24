using UnityEngine;

public class SuperTransform2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private TrailRenderer tlr;


    [SerializeField] Vector2 _position;
    [SerializeField] Chunk _chunk;


    public Chunk CurrentChunk
    {
        get
        {
            if (_chunk != null)
            {
                return _chunk;
            }

            ChunkMap.RegisterObject(gameObject, Vector2Int.zero, Vector2.zero);

            if (_chunk != null)
            {
                return _chunk;
            }

            return _chunk;
        }

        set
        {
            SetPosition(new Vector2Int(value.x, value.y), _position);
        }
    }
    public Vector2 ChunkPosition
    {
        get
        {
            if (_position != null)
            {
                return _position;
            }

            ChunkMap.RegisterObject(gameObject, Vector2Int.zero, Vector2.zero);
            return _position;
        }

        set
        {
            SetPosition(new Vector2Int(_chunk.x, _chunk.y), value);
        }
    }

    public double FullPositionX { get { return ChunkPosition.x + CurrentChunk.x * ChunkMap.chunkSize.x; } }
    public double FullPositionY { get { return ChunkPosition.y + CurrentChunk.y * ChunkMap.chunkSize.y; } }


    // ----------------------------------------

    void Start()
    {
        TryGetComponent(out sr);
    }

    // ----------------------------------------

    public void Move(double xIncrement, double yIncrement)
    {
        // This doesn't work properly

        Vector2 chunkOffset = new Vector2(ChunkPosition.x, ChunkPosition.y);
        Vector2Int sign = new Vector2Int(xIncrement + chunkOffset.x >= 0 ? 1 : -1, yIncrement + chunkOffset.y >= 0 ? 1 : -1);
        double xAfterIncrementAbs = AbsD(xIncrement + chunkOffset.x);
        double yAfterIncrementAbs = AbsD(yIncrement + chunkOffset.y);


        if (xAfterIncrementAbs < ChunkMap.chunkSize.x &&
            yAfterIncrementAbs < ChunkMap.chunkSize.y)
        {
            ChunkPosition += new Vector2((float)xIncrement, (float)yIncrement);
        }
        else // There is propably a better way to do this
        {
            Vector2Int chunkIncrement = Vector2Int.zero;

            while (xAfterIncrementAbs > ChunkMap.chunkSize.x)
            {
                xAfterIncrementAbs -= ChunkMap.chunkSize.x * 2;
                chunkIncrement += Vector2Int.right * sign.x;
            }

            while (yAfterIncrementAbs > ChunkMap.chunkSize.y)
            {
                yAfterIncrementAbs -= ChunkMap.chunkSize.y * 2;
                chunkIncrement += Vector2Int.up * sign.y;
            }

            Vector2 newPosition = new Vector2((float)xAfterIncrementAbs, (float)yAfterIncrementAbs) * sign;
            Vector2Int newChunk = new Vector2Int(CurrentChunk.x, CurrentChunk.y) + chunkIncrement;

            SetPosition(newChunk, newPosition);
        }
    }

    public void SetVisibility(bool visibility)
    {
        if (sr != null) sr.enabled = visibility;
        if (tlr != null) tlr.enabled = visibility;

        Debug.Log($"Changed visibility of {this} from chunk: ({_chunk.x}, {_chunk.y}) to {visibility}");
    }

    virtual public void SetPosition(Vector2Int newChunk, Vector2 newPosition)
    {
        _position = newPosition;
        ChangeChunk(newChunk);

        transform.position = newPosition;
    }

    public void SetPositionRaw(Chunk newChunk, Vector2 newPosition)
    {
        _position = newPosition;
        _chunk = newChunk;
    }

    void ChangeChunk(Vector2Int newChunkPosition)
    {
        if (new Vector2Int(CurrentChunk.x, CurrentChunk.y) == newChunkPosition) { return; }
        CurrentChunk.RemoveObject(this);

        _chunk = ChunkMap.GetChunk(newChunkPosition.x, newChunkPosition.y);
        CurrentChunk.AddObject(this);

        UpdateVisibility();
    }

    public void CopyComponent<T>(Component comp, T other)
    {
        System.Type type = comp.GetType();
        if (type != other.GetType()) return; // type mis-match
        System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.DeclaredOnly;
        System.Reflection.PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        System.Reflection.FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
    }

    double AbsD(double d)
    {
        if (d < 0)
        {
            return d * -1;
        }

        return d;
    }

    public void UpdateVisibility()
    {
        int distX = Mathf.Abs(_chunk.x - SuperTransformCamera.Instance.CurrentChunk.x);
        int distY = Mathf.Abs(_chunk.y - SuperTransformCamera.Instance.CurrentChunk.y);

        if (distX > 1 || distY > 1)
        {
            SetVisibility(false);
        }
        else if (distX <= 1 && distY <= 1)
        {
            SetVisibility(true);

            if(distX == 1 || distX == 1)
            {
                gameObject.layer = 6;
            }
            else
            {
                gameObject.layer = 7;
            }
        }
    }
}