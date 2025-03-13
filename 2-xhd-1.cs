// 柏林噪声地形生成器
public class IslandGenerator : MonoBehaviour {
    [SerializeField] private int seed = 0;
    [SerializeField] private float noiseScale = 0.5f;
    [SerializeField] private int octaves = 6;

    public Mesh GenerateIsland(IslandType type) {
        PerlinNoise noise = new PerlinNoise(seed);
        Vector3[] vertices = mesh.vertices;
        
        for(int i=0; i<vertices.Length; i++){
            float elevation = 0;
            float frequency = 1;
            float amplitude = 1;
            
            for(int o=0; o<octaves; o++){
                Vector3 point = vertices[i] * frequency * noiseScale;
                elevation += noise.GetNoise(point.x, point.y, point.z) * amplitude;
                frequency *= 2;
                amplitude *= 0.5f;
            }
            
            vertices[i].y = ApplyBiomeCurve(type, elevation);
        }
        
        return BuildMesh(vertices);
    }

    private float ApplyBiomeCurve(IslandType type, float rawValue){
        switch(type){
            case IslandType.Volcano:
                return Mathf.Pow(rawValue, 3) * 10f;
            case IslandType.Rainforest:
                return Mathf.Sin(rawValue * Mathf.PI) * 8f;
            // 其他生态区曲线...
        }
    }
}

// 动态加载系统
public class IslandStreamer : MonoBehaviour {
    private Dictionary<Vector2Int, IslandChunk> loadedChunks = new Dictionary<Vector2Int, IslandChunk>();
    
    void Update(){
        Vector2Int currentChunk = GetCurrentChunkCoord();
        LoadSurroundingChunks(currentChunk);
        UnloadDistantChunks(currentChunk);
    }

    void LoadSurroundingChunks(Vector2Int center){
        for(int x=-1; x<=1; x++){
            for(int y=-1; y<=1; y++){
                Vector2Int coord = new Vector2Int(center.x+x, center.y+y);
                if(!loadedChunks.ContainsKey(coord)){
                    StartCoroutine(GenerateChunkAsync(coord));
                }
            }
        }
    }
}