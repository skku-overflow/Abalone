using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public Ball ballPrefab;



    void Awake()
    {
        var whiteBalls = new Vector3Int[] {
            new Vector3Int(-3, 1, 0),
            new Vector3Int(-2, 1, 0),
            new Vector3Int(-2, 0, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, 0, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(2, 1, 0),
            new Vector3Int(2, 0, 0),
            new Vector3Int(-1, 2, 0),
            new Vector3Int(0, 2, 0),
            new Vector3Int(1, 2, 0),
        };

        foreach (Vector3Int pos in whiteBalls)
        {
            RenderBall(pos, false);
            var blackPos = pos;
            blackPos.y = 8 - blackPos.y;
            RenderBall(blackPos, true);
        }

    }
    private void RenderBall(Vector3Int pos, bool black)
    {
        var loc = grid.CellToLocal(pos);
        var ball = Instantiate<Ball>(ballPrefab);
        ball.transform.SetParent(grid.transform);
        ball.transform.localPosition = loc;
        if (black)
        {
            ball.GetComponent<MeshRenderer>().material.color = Color.black;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
