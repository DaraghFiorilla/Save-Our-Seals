using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RopeMinigame : MonoBehaviour
{
    public Rope[] ropes;
    public Knot[] knots;
    private List<int> knotsList = new List<int>();
    [SerializeField] private GameObject failureWindow;
    [SerializeField] private GameObject successWindow;
    private HashSet<Rope> cutRopes = new HashSet<Rope>();
    private bool gameOver;

    private void Awake()
    {
        knots = FindObjectsByType<Knot>(FindObjectsSortMode.None);
        for (int i = 0; i < knots.Length; i++)
        {
            knotsList.Add(i);
        }
        AssignKnotTypes();
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (Input.GetMouseButtonDown(0)) { ProcessCut(); }
        }
    }

    void ProcessCut()
    {
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up);
        if (hit.collider == null) { return; }
        Rope rope = hit.collider.GetComponent<Rope>();
        if (rope == null || cutRopes.Contains(rope)) { return; }
        else
        {
            hit.collider.GetComponent<Rope>().Cut();
            cutRopes.Add(rope);
        }
    }

    void Fail(string failMessage)
    {
        gameOver = true;
        failureWindow.SetActive(true);
        Debug.Log("Fail with message: " + failMessage);
        failureWindow.GetComponentInChildren<TextMeshProUGUI>().text = failMessage;
    }
    void Succeed()
    {
        Debug.Log("won");
        successWindow.SetActive(true);
    }

    public void Restart()
    {

    }

    public void Finish()
    {
        Destroy(this.gameObject);
    }

    public void ValidateRopeLayout()
    {
        var touchedCounts = new Dictionary<Knot, int>();
        foreach (var rope in cutRopes)
        {
            if (!touchedCounts.ContainsKey(rope.knotA)) { touchedCounts[rope.knotA] = 0; }
            if (!touchedCounts.ContainsKey(rope.knotB)) { touchedCounts[rope.knotB] = 0; }

            touchedCounts[rope.knotA]++;
            touchedCounts[rope.knotB]++;
            if (rope.knotA.myKnotType == Knot.KnotType.KnotTight || rope.knotB.myKnotType == Knot.KnotType.KnotTight)
            {
                Debug.Log("Hit rope = " + rope + ", and hit knots = " + rope.knotA.myKnotType + " & " + rope.knotB.myKnotType);
                Fail("You cut a tight knot!");
                return;
            }
        }
        Debug.Log("passed first loop");

        foreach (var knot in knots)
        {
            if (knot.myKnotType == Knot.KnotType.KnotFrayed)
            {
                if (touchedCounts.TryGetValue(knot, out int count) && count >= 2) { continue; }

                Fail("A frayed knot isn't cut enough!");
                return;
            }
        }
        Debug.Log("passed second loop");

        int expectedCuts = 0;
        foreach (var knot in knots)
        {
            if (knot.myKnotType == Knot.KnotType.KnotFrayed)
            {
                expectedCuts += touchedCounts.ContainsKey(knot) ? touchedCounts[knot] : 0;
            }
        }
        Debug.Log("passed third loop");
        expectedCuts /= 2;

        if (cutRopes.Count >= expectedCuts)
        {
            Succeed();
        }
        else
        {
            Debug.Log("so close mate");
        }
    }

    void AssignKnotTypes()
    {
        ShuffleKnotsList();
        // Assigning 5 regular, 4 frayed and 3 tight knots
        // 4/4/4 was causing too many unsolveable layouts
        for (int i = 0; i < 5; i++)
        {
            knots[knotsList[i]].myKnotType = Knot.KnotType.Knot;
        }
        for (int i = 5; i < 9; i++)
        {
            knots[knotsList[i]].myKnotType = Knot.KnotType.KnotFrayed;
        }
        for (int i = 9; i < 12; i++)
        {
            knots[knotsList[i]].myKnotType = Knot.KnotType.KnotTight;
        }

        foreach (Knot knot in knots)
        {
            knot.AssignSprite();
        }

        // Validate current layout, if invalid then reshuffle
        if (!ValidateKnotLayout())
        {
            Debug.LogWarning("Invalid layout generated, reshuffling");
            AssignKnotTypes();
        }
        else { Debug.Log("Valid layout generated"); }
    }

    void ShuffleKnotsList()
    {
        for (int i = 0; i < knotsList.Count; i++)
        {
            int randomIndex = Random.Range(i, knotsList.Count);
            int temp = knotsList[i];
            knotsList[i] = knotsList[randomIndex];
            knotsList[randomIndex] = temp;
        }
    }

    bool ValidateKnotLayout()
    {
        List<int> frayedKnots = new List<int>();
        HashSet<int> tightKnots = new HashSet<int>();

        // Collecting frayed and tight knot indexes
        for (int i = 0; i < knots.Length; i++)
        {
            if (knots[i].myKnotType == Knot.KnotType.KnotFrayed) { frayedKnots.Add(i); }
            else if (knots[i].myKnotType == Knot.KnotType.KnotTight) { tightKnots.Add(i); }
        }

        // Building knot graph temporarily excluding tight knots
        Dictionary<int, List<int>> fullGraph = BuildKnotGraph();

        // Check if frayed knots are correctly connected and not isolated
        foreach (int frayedIndex in frayedKnots)
        {
            if (!fullGraph.ContainsKey(frayedIndex)) { return false; }

            int validNeighborCount = 0;
            foreach (int neighbor in fullGraph[frayedIndex])
            {
                if (!tightKnots.Contains(neighbor)) { validNeighborCount++; }
            }

            // if a frayed knot has less than 2 valid neighbours, it is completely unsolveable
            if (validNeighborCount < 2) { return false; }
        }

        // Check if frayed knots are all reachable
        Dictionary<int, List<int>> filteredGraph = BuildKnotGraph(tightKnots);
        HashSet<int> visited = new HashSet<int>();
        DepthFirstSearch(frayedKnots[0], filteredGraph, visited);

        foreach (int frayedIndex in frayedKnots)
        {
            if (!visited.Contains(frayedIndex)) { return false; }
        }

        return true; // If this return statement is run then the layout is detected as valid
    }

    Dictionary<int, List<int>> BuildKnotGraph()
    {
        return BuildKnotGraph(new HashSet<int>());
    }

    Dictionary<int, List<int>> BuildKnotGraph(HashSet<int> blockedKnots)
    {
        Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();

        foreach (Rope rope in ropes)
        {
            int a = System.Array.IndexOf(knots, rope.knotA);
            int b = System.Array.IndexOf(knots, rope.knotB);

            // Skip if the rope contains blocked knots
            if (blockedKnots.Contains(a) || blockedKnots.Contains(b)) { continue; }

            if (!graph.ContainsKey(a)) graph[a] = new List<int>();
            if (!graph.ContainsKey(b)) graph[b] = new List<int>();

            graph[a].Add(b);
            graph[b].Add(a);
        }

        return graph;
    }

    void DepthFirstSearch(int current, Dictionary<int, List<int>> graph, HashSet<int> visited)
    {
        if (visited.Contains(current)) return;
        visited.Add(current);

        if (!graph.ContainsKey(current)) return;

        foreach (int neighbor in graph[current]) { DepthFirstSearch(neighbor, graph, visited); }
    }
}