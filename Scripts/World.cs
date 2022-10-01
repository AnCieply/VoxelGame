using System;
using Godot;
using System.Collections.Generic;
using System.Threading;

public class World : Spatial {
	private PackedScene chunkScene;
	public Dictionary<Vector2, Chunk> chunks;

	private int genDistance = 3;
	private int renderDistance = 2;

	private Player player;

	public override void _Ready() {
		// Will eventually replace all this for automatic and procedural generation.
		// However I need to fix the issue of faces rendering between chunks first.
		chunkScene = (PackedScene)GD.Load("res://Scenes/Chunk.tscn");
		chunks = new Dictionary<Vector2, Chunk>();

		player = (Player)GetChild(0);
		player.chunkPos = new Vector2((int)(player.Translation.x / Chunk.WIDTH),
			(int)(player.Translation.z / Chunk.DEPTH));
	}
	
	public override void _Process(float delta) {
		// Every frame, the game checks whether or not there are any chunks that
		// should be re-meshed, loaded, or unloaded. If there is a chunk that fits this
		// criteria, a thread should be dispatched to complete that job.
		
		player.chunkPos = new Vector2((int)(player.Translation.x / Chunk.WIDTH),
			(int)(player.Translation.z / Chunk.DEPTH));
		
		// Generates voxel values.
		for (int x = (int)player.chunkPos.x - genDistance; x <= (int)player.chunkPos.x + genDistance; x++)
			for (int z = (int)player.chunkPos.y - genDistance; z <= (int)player.chunkPos.y + genDistance; z++) {
				if (chunks.ContainsKey(new Vector2(x, z)))
					break;

				Chunk newChunk = new Chunk();
				newChunk.position = new Vector2(x, z);
				newChunk.Translation = new Vector3(x * Chunk.WIDTH, 0, z * Chunk.DEPTH);
				chunks.Add(newChunk.position, newChunk);
				AddChild(newChunk);

				if (!newChunk.isGenerated) {
					newChunk.generateVoxels();
					newChunk.isGenerated = true;
				}
			}
		
		// Generates chunk mesh.
		for (int x = (int)player.chunkPos.x - renderDistance; x <= (int)player.chunkPos.x + renderDistance; x++)
			for (int z = (int)player.chunkPos.y - renderDistance; z <= (int)player.chunkPos.y + renderDistance; z++) {
				Vector2 chunkPos = new Vector2(x, z);
				
				if (!chunks.ContainsKey(chunkPos))
					continue;
				
				Chunk c = chunks[chunkPos];
				
				if (c.isModified) {
					c.generateChunkMesh();
					c.isModified = false;
				}
					
			}
	}
	
	// Returns chunk at the position in the chunk grid.
	public Chunk getChunk(int x, int z) {
		Vector2 pos = new Vector2(x, z);
		if (!chunks.ContainsKey(pos))
			return chunks[Vector2.Zero];
		
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
