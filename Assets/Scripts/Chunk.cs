using UnityEngine;
using Mike;

[System.Serializable]
public class Chunk
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
        MikeArray.Append(ref objectsInChunk, st);
        Debug.Log($"Added {st} from chunk: ({x}, {y})");
    }

    public void RemoveObject(SuperTransform2D st)
    {
        MikeArray.RemoveByReference(ref objectsInChunk, ref st);
        Debug.Log($"Removed {st} from chunk: ({x}, {y})");
    }

    public void UpdateVisibility()
    {
        if (objectsInChunk == null) { return; }
        foreach (SuperTransform2D st in objectsInChunk)
        {
            st.UpdateVisibility();
        }
    }
}
