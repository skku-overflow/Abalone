using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Grid grid;
    public Ball ballPrefab;
    public GameObject selectedPrefab;


    private bool isBlackTurn = true;
    private Vector3Int clicked = new Vector3Int(0, 0, -1);
    private GameObject selected;

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
        if (selected != null)
        {
            selected.transform.Rotate(new Vector3(0, 0, 1));
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        if (selected != null)
        {
            Destroy(selected);
            selected = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;

        var cell = grid.WorldToCell(hit.transform.position);

        if (clicked.z == -1)
        {
            clicked = cell;
            selected = Instantiate(selectedPrefab);
            var pos = grid.CellToLocal(cell);

            selected.transform.SetParent(grid.transform);
            pos.z = 1;
            selected.transform.localPosition = pos;

            return;
        }

        var distance = Vector3Int.Distance(clicked, cell);

        // 거리가 1또는 sqrt(2) 인 경우
        if (clicked != cell && distance <= 1.5f)
        {
            // 이동
        }

        clicked = new Vector3Int(0, 0, -1);
    }
}
