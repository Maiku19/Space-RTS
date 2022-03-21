using UnityEngine;

public class SuperTransform2D : MonoBehaviour
{
	[SerializeField] private SpriteRenderer sr;
	[SerializeField] private Component[] cloneComponents;

	[HideInInspector] public GameObject[] clones = new GameObject[3];


	[SerializeField] Vector2 _position;
	[SerializeField] ChunkMap.Chunk _chunk;


	public ChunkMap.Chunk Chunk { get { return _chunk; } set { SetPosition(new Vector2Int(value.x, value.y), ChunkPosition); } }

	public Vector2 ChunkPosition { get { return _position; } set { SetPosition(new Vector2Int(Chunk.x, Chunk.y), value); } }

	public double FullPositionX { get { return _position.x + _chunk.x * ChunkMap.chunkSize.x; } }
	public double FullPositionY { get { return _position.y + _chunk.y * ChunkMap.chunkSize.y; } }


	// ----------------------------------------

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		InitializeClones();
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
			Vector2Int newChunk = new Vector2Int(Chunk.x, Chunk.y) + chunkIncrement;

			SetPosition(newChunk, newPosition);
		}
	}

	public void SetVisibility(bool visible)
	{
		if (visible)
		{
			sr.enabled = true;

			foreach (GameObject clone in clones)
			{
				clone.SetActive(true); // faster than finding and turning off the spriterender.
			}
		}
		else
		{
			sr.enabled = false;

			foreach (GameObject clone in clones)
			{
				clone.SetActive(false); // faster than finding and turning off the spriterender.
			}
		}
	}

	virtual public void SetPosition(Vector2Int newChunk, Vector2 newPosition)
	{
		_position = newPosition;
		ChangeChunk(newChunk);

		transform.position = newPosition;

		UpdateClonePositions();
	}

	void ChangeChunk(Vector2Int newChunkPosition)
	{
		_chunk.RemoveObject(this);
		_chunk = ChunkMap.GetChunk(newChunkPosition.x, newChunkPosition.y);
		_chunk.AddObject(this);
	}

	void UpdateClonePositions()
	{
		if(clones[0] == null) { return; }

		// Offsets clones twards the nearest edges

		Vector2 mulVector = new Vector2(Mathf.Sign(transform.position.x), Mathf.Sign(transform.position.y));

		clones[0].transform.position = (Vector2)transform.position + ChunkMap.chunkSize * 2 * new Vector2(-mulVector.x, 0);
		clones[1].transform.position = (Vector2)transform.position + ChunkMap.chunkSize * 2 * new Vector2(0, -mulVector.y);
		clones[2].transform.position = (Vector2)transform.position + ChunkMap.chunkSize * 2 * -mulVector;

        foreach (GameObject clone in clones)
        {
			clone.transform.localScale = Vector2.one;
			clone.transform.rotation = transform.rotation;
        }
	}

	public virtual void InitializeClones()
	{
		for (int i = 0; i < 3; i++)
		{
			// Create gameobject, add it to clones array, name it and parent it
			clones[i] = new GameObject();
			clones[i].name = $"{gameObject.name} [clone {i + 1}]";
			clones[i].transform.parent = transform;
			clones[i].layer = 7 + i;

			// Copy all whitlisted (cloneComponents) components to clones
			foreach (Component component in cloneComponents)
			{
				CopyComponent(clones[i].AddComponent(component.GetType()), component);
			}
		}

		UpdateClonePositions();
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
}
