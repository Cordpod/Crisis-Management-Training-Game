using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatsRadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;

    private Stats stats;
    private CanvasRenderer radarMeshCanvasRenderer;
    // private CanvasRenderer baselineRadarMeshCanvasRenderer;

    private void Awake(){
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats) {
        //this.stats = stats;
        // stats = StatsManager.instance.GetStats(); // Retrieve latest stats
        // stats.OnStatsChanged += Stats_OnStatsChanged;
        // UpdateStatsVisual();
        if (StatsManager.instance == null) {
            Debug.LogError("StatsManager instance is null in SetStats()!");
            return;
        }
        Debug.Log("Before assignment, stats: " + (this.stats != null ? "Not Null" : "Null"));
        this.stats = StatsManager.instance.GetStats(); // Retrieve latest stats
        Debug.Log("After assignment, stats: " + (this.stats != null ? "Not Null" : "Null"));

        if (stats == null) {
            Debug.LogError("Stats retrieved from StatsManager is null!");
            return;
        }

        this.stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();

    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e){
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual(){
        if (stats == null) {
            Debug.LogError("Stats is null in UpdateStatsVisual()!");
            return;
        }

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 147f;

        Vector3 heuristicsVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Heuristics);
        int heuristicsVertexIndex = 1;
        Vector3 mentalmodelsVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Mentalmodels);
        int mentalmodelsVertexIndex = 2;
        Vector3 infoclarityVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Infoclarity);
        int infoclarityVertexIndex = 3;
        Vector3 cognitiveloadVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Cognitiveload);
        int cognitiveloadVertexIndex = 4;
        Vector3 externalaidVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Externalaid);
        int externalaidVertexIndex = 5;

        vertices[0] = Vector3.zero;
        vertices[heuristicsVertexIndex] = heuristicsVertex;
        vertices[mentalmodelsVertexIndex] = mentalmodelsVertex;
        vertices[infoclarityVertexIndex] = infoclarityVertex;
        vertices[cognitiveloadVertexIndex] = cognitiveloadVertex;
        vertices[externalaidVertexIndex] = externalaidVertex;

        uv[0] = Vector2.zero;
        uv[heuristicsVertexIndex] = Vector2.one;
        uv[mentalmodelsVertexIndex] = Vector2.one;
        uv[infoclarityVertexIndex] = Vector2.one;
        uv[cognitiveloadVertexIndex] = Vector2.one;
        uv[externalaidVertexIndex] = Vector2.one;

        triangles[0] = 0;
        triangles[1] = heuristicsVertexIndex;
        triangles[2] = mentalmodelsVertexIndex;

        triangles[3] = 0;
        triangles[4] = mentalmodelsVertexIndex;
        triangles[5] = infoclarityVertexIndex;

        triangles[6] = 0;
        triangles[7] = infoclarityVertexIndex;
        triangles[8] = cognitiveloadVertexIndex;

        triangles[9] = 0;
        triangles[10] = cognitiveloadVertexIndex;
        triangles[11] = externalaidVertexIndex;

        triangles[12] = 0;
        triangles[13] = externalaidVertexIndex;
        triangles[14] = heuristicsVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture2D);

    }

    
}
