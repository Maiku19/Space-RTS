[System.Serializable]
public struct SuperVector2
{
    public float x;
    public float y;

    public int chunkX;
    public int chunkY;

    public double FullX { get { return ChunkMap.chunkSize.x + x + chunkX * ChunkMap.chunkSize.x; } }
    public double FullY { get { return ChunkMap.chunkSize.y + y + chunkY * ChunkMap.chunkSize.y; } }

    public SuperVector2(float x, float y, int chunkX, int chunkY)
    {
        this.x = x;
        this.y = y;
        this.chunkX = chunkX;
        this.chunkY = chunkY;
    }
}
