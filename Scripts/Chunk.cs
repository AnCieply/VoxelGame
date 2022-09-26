using System;
using Godot;

[Tool]
public class Chunk : MeshInstance
{
	public static readonly int WIDTH = 16;
	public static readonly int HEIGHT = 256;
	public static readonly int DEPTH = 16;

	public VoxelType[,,] voxels;

	// Mesh generation.
	private SurfaceTool st;
	private ArrayMesh mesh;
	private SpatialMaterial voxelTextureMat;
	
	public override void _Ready() {
		st = new SurfaceTool();
		voxelTextureMat = (SpatialMaterial)ResourceLoader.Load("res://Textures/VoxelTexturesMat.tres");
	}
	
	public override void _Process(float delta) {
		editorDebug();
		
		generateVoxels();
		generateChunkMesh();
	}

	private void editorDebug() {
		voxelTextureMat = (SpatialMaterial)ResourceLoader.Load("res://Textures/VoxelTexturesMat.tres");
	} 

	// Generates a value for each voxel in the chunk.
	// No mesh is generated here.
	private void generateVoxels() {
		voxels = new VoxelType[WIDTH, HEIGHT, DEPTH];
		
		for (int x = 0; x < WIDTH; x++)
			for (int y = 0; y < HEIGHT; y++)
				for (int z = 0; z < DEPTH; z++) {
					if (y <= 6) voxels[x, y, z] = VoxelType.Dirt;
					if (y == 7) voxels[x, y, z] = VoxelType.Grass;
				}
	}

	private void generateChunkMesh() {
		st.Begin(Mesh.PrimitiveType.Triangles);

		for (int x = 0; x < WIDTH; x++)
			for (int y = 0; y < HEIGHT; y++)
				for (int z = 0; z < DEPTH; z++)
					genVoxel(new Vector3(x, y, z));

		st.GenerateNormals();
		st.SetMaterial(voxelTextureMat);
		
		mesh = st.Commit();
		this.Mesh = mesh;
	}

	private bool checkTransparent(Vector3 position) {
		if  (position.x >= 0 && position.x < WIDTH && 
			 position.y >= 0 && position.y < HEIGHT &&
			 position.z >= 0 && position.z < DEPTH) {
			if (Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].isSolid) 
				return false;
		}
		return true;
	}
	
	private void genVoxel(Vector3 position) {
		if (voxels[(int)position.x, (int)position.y, (int)position.z] == VoxelType.Air) return; 
		
		if (checkTransparent(position + new Vector3( 0, 0,-1)))  
			genFace(FaceDir.East,   position, Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].texEast);
		
		if (checkTransparent(position + new Vector3( 0, 0, 1)))  
			genFace(FaceDir.West,   position, Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].texWest);
		
		if (checkTransparent(position + new Vector3( 1, 0, 0)))  
			genFace(FaceDir.North,  position, Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].texNorth);
		
		if (checkTransparent(position + new Vector3(-1, 0, 0)))  
			genFace(FaceDir.South,  position, Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].texSouth);
		
		if (checkTransparent(position + new Vector3( 0, 1, 0)))  
			genFace(FaceDir.Top,    position, Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].texTop);
		
		if (checkTransparent(position + new Vector3( 0,-1, 0)))  
			genFace(FaceDir.Bottom, position, Voxel.voxelList[voxels[(int)position.x, (int)position.y, (int)position.z]].texBottom);
		
	}
	
	private void genFace(FaceDir direction, Vector3 offset, Vector2 texOffset) {
		var a = Voxel.vertices[(int)direction][0] + offset;
		var b = Voxel.vertices[(int)direction][1] + offset;
		var c = Voxel.vertices[(int)direction][2] + offset;
		var d = Voxel.vertices[(int)direction][3] + offset;
		
		Vector3[] tri1 = {
			a, b, c
		};

		Vector3[] tri2 = {
			a, c, d
		};

		var uvOffset = texOffset / Voxel.TEX_ATLAS_SIZE;
		var height = 1.0f / Voxel.TEX_ATLAS_SIZE.y;
		var width = 1.0f / Voxel.TEX_ATLAS_SIZE.x;

		var uvA = uvOffset + new Vector2(width, height);
		var uvB = uvOffset + new Vector2(width, 0);
		var uvC = uvOffset + new Vector2(0, 0);
		var uvD = uvOffset + new Vector2(0, height);

		Vector2[] uv1 = {
			uvA, uvB, uvC
		};
		
		Vector2[] uv2 = {
			uvA, uvC, uvD
		};
			
		st.AddTriangleFan(tri1, uv1);
		st.AddTriangleFan(tri2, uv2);
	}
}
