using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BehaviourEngine;
using OpenTK;

namespace BubbleGhostGame2D
{
    public class Map : GameObject
    {
        public static Vector2 SpawnPointBubble => spawnPointBubble;
        public static Vector2 SpawnPointGhost => spawnPointGhost;

        private float halfBlockSize = 0.5f;
        private Candle candle;
        private Wall wall;
        private int[] mapCells;
        private int rows;
        private int columns;
        private static Vector2 spawnPointBubble;
        private static Vector2 spawnPointGhost;

        public Map(List<int> mapCells, int rows, int columns, int index) : base("Map")
        {
            this.mapCells = mapCells.ToArray();
            this.rows = rows;
            this.columns = columns;

            for (int i = 0; i < mapCells.ToArray().Length; i++)
            {
                switch (mapCells[i])
                {
                    case 1:
                        wall = new Wall();
                        wall.Transform.Position = new Vector2(i % (columns - 1) + halfBlockSize, i / (columns - 1) + halfBlockSize);
                        Spawn(wall);
                        break;
                    case 2:
                        candle = new Candle();
                        candle.Transform.Position = new Vector2(i % (columns - 1) + halfBlockSize, i / (columns - 1) + halfBlockSize);
                        Spawn(candle);
                        break;
                    case 3:
                        spawnPointGhost = new Vector2(i % (columns - 1) + halfBlockSize, i / (columns - 1) + halfBlockSize);
                        break;
                    case 4:
                        spawnPointBubble = new Vector2(i % (columns - 1) + halfBlockSize, i / (columns - 1) + halfBlockSize);
                        break;
                }
            }
        }
    }
}
