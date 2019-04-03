﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary> 
    /// </summary>
    public Grid grid;
    public Ball ballPrefab;
    public GameObject selectedPrefab;

    private Ball[,] balls;
    private bool isBlackTurn = true;
    private int leftMove = 3;

    private Vector3Int clicked = new Vector3Int(0, 0, -1);
    private GameObject selected;

    void Awake()
    {
        balls = new Ball[10, 10];

        // 흰 공 좌표. 
        var whiteBalls = new Vector3Int[] {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(2, 0, 0),
            new Vector3Int(2, 1, 0),
            new Vector3Int(2, 2, 0),
            new Vector3Int(3, 0, 0),
            new Vector3Int(3, 1, 0),
            new Vector3Int(3, 2, 0),
            new Vector3Int(4, 0, 0),
            new Vector3Int(4, 1, 0),
            new Vector3Int(4, 2, 0),
            new Vector3Int(5, 0, 0),
            new Vector3Int(5, 1, 0),
        };

        foreach (Vector3Int pos in whiteBalls)
        {
            RenderBall(pos, false);
            var blackPos = pos;
            blackPos.y = 8 - blackPos.y;
            RenderBall(blackPos, true);
        }
    }

    /// <summary>HexGrid 상에 공을 생성하는 함수입니다.
    /// </summary>
    /// <param name="pos"><c>Vector3Int</c> 형태의 좌표입니다.</param>
    /// <param name="black"><c>true</c>일 경우 검은 공으로, 아니면 하얀 공으로 초기화합니다.</param>
    private void RenderBall(Vector3Int pos, bool black)
    {
        var ball = Instantiate<Ball>(ballPrefab);
        // 공을 grid의 child로 만듦
        ball.transform.SetParent(grid.transform);

        // 공의 position
        var loc = grid.CellToLocal(pos);
        ball.transform.localPosition = loc;
        if (black)
        {
            ball.GetComponent<MeshRenderer>().material.color = Color.black;
        }
        balls[pos.x, pos.y] = ball;
    }

    void Update()
    {
        if (selected != null)
        {
            // 선택한 공이 있으면 회전 애니메이션 표시
            selected.transform.Rotate(new Vector3(0, 0, 1));
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }


    /// <summary>
    /// 마우스 클릭을 처리합니다.
    /// </summary>
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

        // 클릭한 좌표 -> Hex 좌표
        var cell = grid.WorldToCell(hit.transform.position);

        // 만약 현재 클릭된 공이 없다면
        if (clicked.z == -1)
        {
            var ball = balls[cell.x, cell.y];
            // 빈 공간 선택 금지
            if (ball == null)
            {
                return;
            }

            // 턴이 아닌 경우 클릭 금지
            var isBlack = ball.GetComponent<MeshRenderer>().material.color == Color.black;
            if (isBlack != isBlackTurn)
            {
                return;
            }

            // 공 선택
            clicked = cell;
            selected = Instantiate(selectedPrefab);
            selected.transform.SetParent(grid.transform);

            // 공 뒤에 보이도록 z 값 설정
            var pos = grid.CellToLocal(cell);
            pos.z = 1;
            selected.transform.localPosition = pos;

            return;
        }

        var distance = Vector3Int.Distance(clicked, cell);

        // 거리가 1또는 sqrt(2) 인 경우
        if (clicked != cell && distance <= 1.5f)
        {
            // 이동
            MoveBall(grid.WorldToLocal(cell), grid.WorldToLocal(clicked));
        }

        clicked = new Vector3Int(0, 0, -1);
    }

    private void MoveBall(Vector3 from, Vector3 to)
    {
        Debug.Log("Move (" + from + ") -> (" + to + ")");

        var castDir = to - from;

        var last = grid.LocalToCell(from);

        RaycastHit hit;
        Physics.Raycast(from, castDir, out hit);

        // 이동하는 공의 개수
        int ballCount = 1;

        while (hit.collider != null)
        {
            Debug.Log("Hit" + last);
            var ball = hit.collider.GetComponent<Ball>();
            if (ball == null)
            {
                Debug.Log("ball == null");
                break;
            }
            ballCount++;

            if (ballCount > leftMove)
            {
                Debug.Log("ballCount > leftMove");
                // TODO(kdy1): 남은 공 이동 횟수가 충분하지 않다는 에러 메시지 표시 
                break;
            }




            last = grid.WorldToCell(hit.transform.position);
            Physics.Raycast(hit.collider.transform.position, castDir, out hit);
        }


    }
}
