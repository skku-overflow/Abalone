using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abalone.Assets.Scripts
{
    public class Direction
    {

        /// <summary>Grid 상에서 오른쪽 방향</summary>
        public static readonly Direction RIGHT = new Direction(XWise.POS, YWise.NONE);
        /// <summary>Grid 상에서 왼쪽 방향</summary>
        public static readonly Direction LEFT = new Direction(XWise.NEG, YWise.NONE);
        /// <summary>Grid 상에서 오른쪽 위 방향</summary>
        public static readonly Direction UPPER_RIGHT = new Direction(XWise.HALF_POS, YWise.POS);
        /// <summary>Grid 상에서 왼쪽 위 방향</summary>
        public static readonly Direction UPPER_LEFT = new Direction(XWise.HALF_NEG, YWise.POS);
        /// <summary>Grid 상에서 오른쪽 아래 방향</summary>
        public static readonly Direction LOWER_RIGHT = new Direction(XWise.HALF_POS, YWise.NEG);
        /// <summary>Grid 상에서 왼쪽 아래 방향</summary>
        public static readonly Direction LOWER_LEFT = new Direction(XWise.HALF_NEG, YWise.NEG);

        private enum XWise {
            NEG = -1,
            POS = 1,
            HALF_POS,
            HALF_NEG,
        }
        private enum YWise {
            POS = 1,
            NONE = 0,
            NEG = -1,
        }

        private XWise x;
        private YWise y;

        private Direction (XWise x, YWise y) {
            this.x = x; this.y = y;
        }

        /// <summary>주어진 위치에서 `Direction`에 해당되는 방향으로 이동하기 위해 `Vector3Int` 형태로 변환하는 함수.</summary>
        /// <param name="where">격자 상에서의 위치. 전역 좌표가 아님에 유의.</param>
        /// 
        /// <example>
        /// 사용법은 다음과 같다.
        /// <code>
        /// Ball ball = SOME_BALL;
        /// Vector3Int pos = ball.position_on_grid;
        /// Direction dir = Direction.SOME_DIRECTION;
        /// 
        /// pos += dir.toVector3Int(pos);
        /// ball.position_on_grid = pos;
        /// </code>
        /// </example>
        public Vector3Int toVector3Int (Vector3Int where) {
            Vector3Int result = new Vector3Int();

            switch (this.x) {
                case XWise.HALF_POS:
                    result.x = where.y % 2;
                    break;
                case XWise.HALF_NEG:
                    result.x = where.y % 2 - 1;
                    break;
                default:
                    result.x = (int) this.x;
                    break;
            }
            
            result.y = (int) this.y;

            return result;
        }
    }
}