using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary> 
    /// </summary>
    public Grid grid;
    public Ball ballPrefab;
    public GameObject selectedPrefab;
    public Cell cellPrefab;

    private Ball[,] balls;
    private bool isBlackTurn = true;
    private int leftMove = 3;
    private int whiteDeadCount, blackDeadCount;

    private Vector3Int clicked = new Vector3Int(0, 0, -1);
    private GameObject selected;

    private static readonly Vector3Int[] validPoints = new Vector3Int[] {
            new Vector3Int(0, 3, 0),
            new Vector3Int(0, 4, 0),
            new Vector3Int(0, 5, 0),

            new Vector3Int(1, 1, 0),
            new Vector3Int(1, 2, 0),
            new Vector3Int(1, 3, 0),
            new Vector3Int(1, 4, 0),
            new Vector3Int(1, 5, 0),
            new Vector3Int(1, 6, 0),
            new Vector3Int(1, 7, 0),

            new Vector3Int(2, 0, 0),
            new Vector3Int(2, 1, 0),
            new Vector3Int(2, 2, 0),
            new Vector3Int(2, 3, 0),
            new Vector3Int(2, 4, 0),
            new Vector3Int(2, 5, 0),
            new Vector3Int(2, 6, 0),
            new Vector3Int(2, 7, 0),
            new Vector3Int(2, 8, 0),

            new Vector3Int(3, 0, 0),
            new Vector3Int(3, 1, 0),
            new Vector3Int(3, 2, 0),
            new Vector3Int(3, 3, 0),
            new Vector3Int(3, 4, 0),
            new Vector3Int(3, 5, 0),
            new Vector3Int(3, 6, 0),
            new Vector3Int(3, 7, 0),
            new Vector3Int(3, 8, 0),

            new Vector3Int(4, 0, 0),
            new Vector3Int(4, 1, 0),
            new Vector3Int(4, 2, 0),
            new Vector3Int(4, 2, 0),
            new Vector3Int(4, 3, 0),
            new Vector3Int(4, 4, 0),
            new Vector3Int(4, 5, 0),
            new Vector3Int(4, 6, 0),
            new Vector3Int(4, 7, 0),
            new Vector3Int(4, 8, 0),

            new Vector3Int(5, 0, 0),
            new Vector3Int(5, 1, 0),
            new Vector3Int(5, 2, 0),
            new Vector3Int(5, 3, 0),
            new Vector3Int(5, 4, 0),
            new Vector3Int(5, 5, 0),
            new Vector3Int(5, 6, 0),
            new Vector3Int(5, 7, 0),
            new Vector3Int(5, 8, 0),

            new Vector3Int(6, 0, 0),
            new Vector3Int(6, 1, 0),
            new Vector3Int(6, 2, 0),
            new Vector3Int(6, 3, 0),
            new Vector3Int(6, 4, 0),
            new Vector3Int(6, 5, 0),
            new Vector3Int(6, 6, 0),
            new Vector3Int(6, 7, 0),
            new Vector3Int(6, 8, 0),

            new Vector3Int(7, 2, 0),
            new Vector3Int(7, 3, 0),
            new Vector3Int(7, 4, 0),
            new Vector3Int(7, 5, 0),
            new Vector3Int(7, 6, 0),

            new Vector3Int(8, 4, 0),
       };

    void Awake()
    {
        balls = new Ball[10, 10];

        // 흰 공 좌표. 
        var whiteBalls = new Vector3Int[] {
            new Vector3Int(1, 1, 0),
            new Vector3Int(2, 1, 0),
            new Vector3Int(2, 0, 0),
            new Vector3Int(3, 0, 0),
            new Vector3Int(3, 1, 0),
            new Vector3Int(3, 2, 0),
            new Vector3Int(4, 0, 0),
            new Vector3Int(4, 1, 0),
            new Vector3Int(4, 2, 0),
            new Vector3Int(5, 0, 0),
            new Vector3Int(5, 1, 0),
            new Vector3Int(5, 2, 0),
            new Vector3Int(6, 0, 0),
            new Vector3Int(6, 1, 0),
        };

        foreach (Vector3Int pos in whiteBalls)
        {
            CreateBall(pos, false);
            var blackPos = pos;
            blackPos.y = 8 - blackPos.y;
            CreateBall(blackPos, true);
        }




        foreach (Vector3Int pos in validPoints)
        {
            CreateCell(pos);
        }
    }

    private void CreateCell(Vector3Int pos)
    {
        var cell = Instantiate<Cell>(cellPrefab);
        cell.transform.SetParent(grid.transform);
        var loc = grid.CellToLocal(pos);
        cell.transform.localPosition = loc;
    }

    /// <summary>
    /// HexGrid 내부에 공을 생성합니다.
    /// </summary>
    /// <param name="pos"><c>Vector3Int</c> 형태의 좌표입니다.</param>
    /// <param name="black"><c>true</c>일 경우 검은 공으로, 아니면 하얀 공으로 초기화합니다.</param>
    private void CreateBall(Vector3Int pos, bool black)
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
        Debug.Log("선택: " + cell);

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
            Debug.Log("Move(" + clicked + " -> " + cell + ")");
            MoveBall(grid.CellToLocal(clicked), grid.CellToLocal(cell));
        }

        clicked = new Vector3Int(0, 0, -1);
    }

    private void MoveBall(Vector3 from, Vector3 to)
    {
        var castDir = to - from;
        var distance = castDir.magnitude;

        var last = grid.LocalToCell(from);

        RaycastHit hit;
        Physics.Raycast(from, castDir, out hit, distance);

        // 이동하는 공의 개수
        int myBallCount = 1;
        var ballsToMove = new List<Vector3Int>
        {
            last
        };

        while (hit.collider != null)
        {
            var curCell = grid.WorldToCell(hit.collider.transform.position);

            Debug.Log("Hit: " + curCell);
            var ball = hit.collider.GetComponent<Ball>();
            if (ball == null && hit.collider.GetComponent<Cell>() == null)
            {
                Debug.Log("ball == null && cell == null");
                break;
            }

            // Cell이 Raycast에 먼저 맞은 경우 처리
            if (ball == null)
            {
                ball = balls[curCell.x, curCell.y];
                if (ball == null)
                {
                    Debug.Log("ball == null");
                    break;
                }
            }

            ballsToMove.Insert(0, curCell);

            var isBlack = ball.GetComponent<MeshRenderer>().material.color == Color.black;
            var isMyBall = isBlack == isBlackTurn;


            if (isMyBall)
            {
                myBallCount++;
            }

            if (myBallCount > leftMove)
            {
                Debug.Log("ballCount > leftMove");
                // TODO(kdy1): 남은 공 이동 횟수가 충분하지 않다는 에러 메시지 표시 
                return;
            }




            last = curCell;
            Physics.Raycast(hit.collider.transform.position, castDir, out hit, distance);
        }

        if (ballsToMove.Count >= 6)
        {
            // 이동 불가능
            Debug.Log("6개 이상의 공을 움직일 수 없습니다");
            // TODO(kdy1): 6개 이상 움직일 수 없다는 에러 메시지 표시
            return;
        }

        if (ballsToMove.Count > myBallCount * 2)
        {
            // 이동 불가능
            Debug.Log(myBallCount + "개의 공으로 " + (ballsToMove.Count - myBallCount) + "개의 공을 밀 수 없습니다");
            // TODO(kdy1): 에러 메시지 표시
            return;
        }




        // 공을 이동시킵니다.
        foreach (var curCell in ballsToMove)
        {
            var newLoc = grid.CellToLocal(curCell) + castDir;
            var newCell = grid.LocalToCell(newLoc);
            var moveToValid = IsValid(newCell);

            Debug.Log("이동: " + curCell + " -> " + newCell);

            var ball = balls[curCell.x, curCell.y];
            if (moveToValid)
            {
                ball.transform.localPosition = ball.transform.localPosition + castDir;

                balls[newCell.x, newCell.y] = ball;
            }
            else
            {
                var isBlack = ball.GetComponent<MeshRenderer>().material.color == Color.black;
                if (isBlack)
                {
                    blackDeadCount++;
                }
                else
                {
                    whiteDeadCount++;
                }

                Destroy(ball);
            }
            balls[curCell.x, curCell.y] = null;


            if (blackDeadCount >= 6)
            {
                Debug.Log("흰색이 이겼습니다");
                // TODO: UI
            }
            else if (whiteDeadCount >= 6)
            {
                Debug.Log("검은색이 이겼습니다");
                // TODO: UI
            }

        }

        Debug.Log(myBallCount + "개 이동");

        leftMove -= myBallCount;
        Debug.Assert(leftMove >= 0, "leftMove는 0 이상으로 유지돼야 합니다");

        if (leftMove == 0)
        {
            leftMove = 3;
            isBlackTurn = !isBlackTurn;
        }
    }


    private bool IsValid(Vector3Int cell)
    {
        foreach (var point in validPoints)
        {
            if (point == cell) return true;
        }
        return false;
    }

}
