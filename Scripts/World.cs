using Godot;
using System.Collections.Generic;

public class World : Spatial {
	private PackedScene chunkScene;
	public Dictionary<Vector2, Chunk> chunks;

	public override void _Ready() {
		// Will eventually replace all this for automatic and procedural generation.
		// However I need to fix the issue of faces rendering between chunks first.
		chunkScene = (PackedScene)GD.Load("res://Scenes/Chunk.tscn");
		chunks = new Dictionary<Vector2, Chunk>();
		chunks.Add(new Vector2(0, 0), new Chunk()); 
		AddChild(chunks[new Vector2(0, 0)]);
		chunks.Add(new Vector2(0, 1), new Chunk());
		AddChild(chunks[new Vector2(0, 1)]);
		chunks.Add(new Vector2(1, 1), new Chunk());
		AddChild(chunks[new Vector2(1, 1)]);
		chunks.Add(new Vector2(1, 0), new Chunk());
		AddChild(chunks[new Vector2(1, 0)]);
		chunks.Add(new Vector2(1, -1), new Chunk());
		AddChild(chunks[new Vector2(1, -1)]);
		chunks.Add(new Vector2(0, -1), new Chunk());
		AddChild(chunks[new Vector2(0, -1)]);
		chunks.Add(new Vector2(-1, -1), new Chunk());
		AddChild(chunks[new Vector2(-1, -1)]);
		chunks.Add(new Vector2(-1, 0), new Chunk());
		AddChild(chunks[new Vector2(-1, 0)]);
		chunks.Add(new Vector2(-1, 1), new Chunk());
		AddChild(chunks[new Vector2(-1, 1)]);

		

		foreach (KeyValuePair<Vector2,Chunk> c in chunks) {
			//AddChild(c.Value);
			c.Value.setPosition((int)c.Key.x, (int)c.Key.y);
		}
	}
	
	public override void _Process(float delta) {
		
	}
	
	// Returns chunk at the position in the chunk grid.
	public Chunk getChunk(int x, int z) {
		return chunks[new Vector2(x, z)];
	}

	public VoxelType getBlock(int x, int y, int z) {
		// Finds coords of the block in its chunk.
		// If the coords are negative in anyway, they must be flipped.
		Vector3 intraChunkBlockPos = new Vector3(x % Chunk.WIDTH, y, z % Chunk.DEPTH);
		if (intraChunkBlockPos.x < 0)
			intraChunkBlockPos.x = Chunk.WIDTH - Mathf.Abs(intraChunkBlockPos.x);
		if (intraChunkBlockPos.z < 0)
			intraChunkBlockPos.z = Chunk.DEPTH - Mathf.Abs(intraChunkBlockPos.z);

		Vector2 chunkPos = new Vector2(x / (Chunk.WIDTH + 1), z / (Chunk.DEPTH + 1));

		return getChunk((int)chunkPos.x, (int)chunkPos.y).voxels[(int)intraChunkBlockPos.x, y, (int)intraChunkBlockPos.z]; // The issue is i think im giving a negative index to.
	}
}
