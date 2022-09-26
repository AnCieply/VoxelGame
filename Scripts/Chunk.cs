using Godot;

[Tool]
public class Chunk : MeshInstance
{
	// Chunk size.
	private static readonly int WIDTH = 16;
	private static readonly int HEIGHT = 256;
	private static readonly int DEPTH = 16;
	private VoxelType[,,] voxels;

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
	
	// Remove later.
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
					if (y <= 64) voxels[x, y, z] = VoxelType.Stone;
					if (y <= 127) voxels[x, y, z] = VoxelType.Dirt;
					if (y == 128) voxels[x, y, z] = VoxelType.Grass;
				}
	}

	private void generateChunkMesh() {
		st.Begin(Mesh.PrimitiveType.Triangles);

		for (int x = 0; x < WIDTH; x++)
			for (int y = 0; y < HEIGHT; y++)
				for (int z = 0; z < DEPTH; z++)
					genVoxel(x, y, z);

		st.GenerateNormals();
		st.SetMaterial(voxelTextureMat);
		
		mesh = st.Commit();
		this.Mesh = mesh;
	}

	private bool checkTransparent(int x, int y, int z) {
		if  (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT && z >= 0 && z < DEPTH) {
			if (Voxel.VOXEL_LIST[voxels[x, y, z]].isSolid) 
				return false;
		}
		return true;
	}
	
	private void genVoxel(int x, int y, int z) {
		if (voxels[x, y, z] == VoxelType.Air) return; 
		
		if (checkTransparent(x, y, z - 1 ))  
			genFace(FaceDir.East, new Vector3(x, y, z), 
				Voxel.VOXEL_LIST[voxels[x, y, z]].texEast);
		
		if (checkTransparent(x, y, z + 1))  
			genFace(FaceDir.West, new Vector3(x, y, z), 
				Voxel.VOXEL_LIST[voxels[x, y, z]].texWest);
		
		if (checkTransparent(x + 1, y, z))  
			genFace(FaceDir.North, new Vector3(x, y, z), 
				Voxel.VOXEL_LIST[voxels[x, y, z]].texNorth);
		
		if (checkTransparent(x - 1, y, z))  
			genFace(FaceDir.South, new Vector3(x, y, z), 
				Voxel.VOXEL_LIST[voxels[x, y, z]].texSouth);
		
		if (checkTransparent(x, y + 1, z))  
			genFace(FaceDir.Top, new Vector3(x, y, z), 
				Voxel.VOXEL_LIST[voxels[x, y, z]].texTop);
		
		if (checkTransparent(x, y - 1, z))  
			genFace(FaceDir.Bottom, new Vector3(x, y, z), 
				Voxel.VOXEL_LIST[voxels[x, y, z]].texBottom);
	}
	
	private void genFace(FaceDir direction, Vector3 offset, Vector2 texOffset) {
		// Position vertices.
		var a = Voxel.VERTICES[(int)direction][0] + offset;
		var b = Voxel.VERTICES[(int)direction][1] + offset;
		var c = Voxel.VERTICES[(int)direction][2] + offset;
		var d = Voxel.VERTICES[(int)direction][3] + offset;
		
		Vector3[] tri1 = { a, b, c };
		Vector3[] tri2 = { a, c, d };

		// UVs.
		var uvOffset = texOffset / Voxel.TEX_ATLAS_SIZE;
		var height = 1.0f / Voxel.TEX_ATLAS_SIZE.y;
		var width = 1.0f / Voxel.TEX_ATLAS_SIZE.x;

		var uvA = uvOffset + new Vector2(width, height);
		var uvB = uvOffset + new Vector2(width, 0);
		var uvC = uvOffset + new Vector2(0, 0);
		var uvD = uvOffset + new Vector2(0, height);

		Vector2[] uv1 = { uvA, uvB, uvC };
		Vector2[] uv2 = { uvA, uvC, uvD };
			
		// Constructs the final face.
		st.AddTriangleFan(tri1, uv1);
		st.AddTriangleFan(tri2, uv2);
	}
}
