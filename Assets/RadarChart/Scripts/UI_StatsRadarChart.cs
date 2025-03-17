using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatsRadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;

    [SerializeField] private Material baselineRadarMaterial;
    [SerializeField] private Texture2D baselineRadarTexture2D;

    // public Text timeText; 

    private Stats stats;
    private CanvasRenderer radarMeshCanvasRenderer;
    private CanvasRenderer baselineRadarMeshCanvasRenderer;

    private void Awake(){
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
        baselineRadarMeshCanvasRenderer = transform.Find("baselineRadarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats) {
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
        CreateBaselineRadarMesh();

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

        float heuristicsPercent = (float)stats.GetStatAmount(Stats.Type.Heuristics) / stats.GetMaxScore(Stats.Type.Heuristics);
        float mentalmodelsPercent = (float)stats.GetStatAmount(Stats.Type.Mentalmodels) / stats.GetMaxScore(Stats.Type.Mentalmodels);
        float infoclarityPercent = (float)stats.GetStatAmount(Stats.Type.Infoclarity) / stats.GetMaxScore(Stats.Type.Infoclarity);
        float cognitiveloadPercent = (float)stats.GetStatAmount(Stats.Type.Cognitiveload) / stats.GetMaxScore(Stats.Type.Cognitiveload);
        float externalaidPercent = (float)stats.GetStatAmount(Stats.Type.Externalaid) / stats.GetMaxScore(Stats.Type.Externalaid);

        // Vector3 heuristicsVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Heuristics);
        // int heuristicsVertexIndex = 1;
        // Vector3 mentalmodelsVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Mentalmodels);
        // int mentalmodelsVertexIndex = 2;
        // Vector3 infoclarityVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Infoclarity);
        // int infoclarityVertexIndex = 3;
        // Vector3 cognitiveloadVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Cognitiveload);
        // int cognitiveloadVertexIndex = 4;
        // Vector3 externalaidVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Externalaid);
        // int externalaidVertexIndex = 5;
        Vector3 heuristicsVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * heuristicsPercent * 0.5f;
        int heuristicsVertexIndex = 1;
        Vector3 mentalmodelsVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * mentalmodelsPercent * 0.5f;
        int mentalmodelsVertexIndex = 2;
        Vector3 infoclarityVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * infoclarityPercent * 0.5f;
        int infoclarityVertexIndex = 3;
        Vector3 cognitiveloadVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * cognitiveloadPercent * 0.5f;
        int cognitiveloadVertexIndex = 4;
        Vector3 externalaidVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * externalaidPercent * 0.5f;
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

    private void CreateBaselineRadarMesh(){
        Stats baselineStats = new Stats();
        baselineStats.SetStatAmount(Stats.Type.Heuristics, 12); 
        baselineStats.SetStatAmount(Stats.Type.Mentalmodels, 10);
        baselineStats.SetStatAmount(Stats.Type.Infoclarity, 11);
        baselineStats.SetStatAmount(Stats.Type.Cognitiveload, 12);
        baselineStats.SetStatAmount(Stats.Type.Externalaid, 10);

        if (baselineStats == null) {
            Debug.LogError("Stats is null in CreateBaselineRadarMesh()!");
            return;
        }

        Mesh baselineMesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 147f;

        float heuristicsPercent = (float)baselineStats.GetStatAmount(Stats.Type.Heuristics) / baselineStats.GetMaxScore(Stats.Type.Heuristics);
        float mentalmodelsPercent = (float)baselineStats.GetStatAmount(Stats.Type.Mentalmodels) / baselineStats.GetMaxScore(Stats.Type.Mentalmodels);
        float infoclarityPercent = (float)baselineStats.GetStatAmount(Stats.Type.Infoclarity) / baselineStats.GetMaxScore(Stats.Type.Infoclarity);
        float cognitiveloadPercent = (float)baselineStats.GetStatAmount(Stats.Type.Cognitiveload) / baselineStats.GetMaxScore(Stats.Type.Cognitiveload);
        float externalaidPercent = (float)baselineStats.GetStatAmount(Stats.Type.Externalaid) / baselineStats.GetMaxScore(Stats.Type.Externalaid);

        // Vector3 heuristicsVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * baselineStats.GetStatAmountNormalized(Stats.Type.Heuristics);
        // int heuristicsVertexIndex = 1;
        // Vector3 mentalmodelsVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * baselineStats.GetStatAmountNormalized(Stats.Type.Mentalmodels);
        // int mentalmodelsVertexIndex = 2;
        // Vector3 infoclarityVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * baselineStats.GetStatAmountNormalized(Stats.Type.Infoclarity);
        // int infoclarityVertexIndex = 3;
        // Vector3 cognitiveloadVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * baselineStats.GetStatAmountNormalized(Stats.Type.Cognitiveload);
        // int cognitiveloadVertexIndex = 4;
        // Vector3 externalaidVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * baselineStats.GetStatAmountNormalized(Stats.Type.Externalaid);
        // int externalaidVertexIndex = 5;
        Vector3 heuristicsVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * heuristicsPercent * 0.5f;
        int heuristicsVertexIndex = 1;
        Vector3 mentalmodelsVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * mentalmodelsPercent * 0.5f;
        int mentalmodelsVertexIndex = 2;
        Vector3 infoclarityVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * infoclarityPercent * 0.5f;
        int infoclarityVertexIndex = 3;
        Vector3 cognitiveloadVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * cognitiveloadPercent * 0.5f;
        int cognitiveloadVertexIndex = 4;
        Vector3 externalaidVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * externalaidPercent * 0.5f;
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

        baselineMesh.vertices = vertices;
        baselineMesh.uv = uv;
        baselineMesh.triangles = triangles;

        baselineRadarMeshCanvasRenderer.SetMesh(baselineMesh);
        baselineRadarMeshCanvasRenderer.SetMaterial(baselineRadarMaterial, baselineRadarTexture2D);

    }
    
}
