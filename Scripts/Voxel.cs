using Godot;
using System.Collections.Generic;

public enum FaceDir : byte {
    East,
    West,
    North,
    South,
    Top,
    Bottom
}

public enum VoxelType : byte {
    Air,
    Grass,
    Dirt,
    Stone
}

public class VoxelParameters {
    public Vector2 texEast;
    public Vector2 texWest;
    public Vector2 texNorth;
    public Vector2 texSouth;
    public Vector2 texTop;
    public Vector2 texBottom;
    public bool isSolid;

    public VoxelParameters(Vector2 texEast, Vector2 texWest, 
                    Vector2 texNorth, Vector2 texSouth, 
                    Vector2 texTop, Vector2 texBottom, 
                    bool isSolid) 
    {
        this.texEast = texEast;
        this.texWest = texWest;
        this.texNorth = texNorth;
        this.texSouth = texSouth;
        this.texTop = texTop;
        this.texBottom = texBottom;
        this.isSolid = isSolid;
    }
}

public static class Voxel {
    public static readonly Vector2 TEX_ATLAS_SIZE = new Vector2(3, 2);
    
    private static readonly Vector3[] EAST_VERTICES = {
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f)
    };

    private static readonly Vector3[] WEST_VERTICES = {
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f)
    };

    private static readonly Vector3[] NORTH_VERTICES = {
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f)
    };

    private static readonly Vector3[] SOUTH_VERTICES = {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 0.0f, 1.0f)
    };

    private static readonly Vector3[] TOP_VERTICES = {
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 1.0f)
    };

    private static readonly Vector3[] BOTTOM_VERTICES = {
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f)
    };

    public static readonly Vector3[][] VERTICES = {
        EAST_VERTICES,  WEST_VERTICES,
        NORTH_VERTICES, SOUTH_VERTICES,
        TOP_VERTICES,   BOTTOM_VERTICES
    };

    public static readonly Dictionary<VoxelType, VoxelParameters> VOXEL_LIST = new Dictionary<VoxelType, VoxelParameters>() {
        { VoxelType.Air, new VoxelParameters(new Vector2(0, 0),  new Vector2(0, 0), 
            new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector2(0, 0), 
            false) 
        },
        { VoxelType.Grass, new VoxelParameters(new Vector2(2, 0),  new Vector2(2, 0), 
            new Vector2(2, 0), new Vector2(2, 0),
            new Vector2(0, 0), new Vector2(1, 0), 
            true) 
        },
        { VoxelType.Dirt, new VoxelParameters(new Vector2(1, 0),  new Vector2(1, 0), 
            new Vector2(1, 0), new Vector2(1, 0),
            new Vector2(1, 0), new Vector2(1, 0), 
            true) 
        },
        { VoxelType.Stone, new VoxelParameters(new Vector2(0, 1),  new Vector2(0, 1), 
            new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(0, 1), new Vector2(0, 1), 
            true) 
        }
    };
}