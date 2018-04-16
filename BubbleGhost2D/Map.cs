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
        public Vector2 SpawnPointBubble => spawnPointBubble;
        public Vector2 SpawnPointGhost  => spawnPointGhost;

        private SpriteRenderer renderer;
        public static Vector2 spawnPointBubble;
        public static Vector2 spawnPointGhost;
        private BoxCollider2D levelCollider;
        private BoxCollider2D wallCollider;
        private const int size    = 32;
        private int blockLenght   = 32;
        private int halfBlockSize = 16;
        private Candle candle;
        private static int index;

        public Map( List<int> mapCells, int rows, int columns, int index ) : base( "Map" ) //base ( ( int )RenderLayer.Level )
        {
            Map.index           = index;
            renderer            = AddComponent<SpriteRenderer>(new SpriteRenderer(this,size,size));
            renderer.IsTile     = true;
            renderer.mapCells   = mapCells.ToArray();
            renderer.Rows       = rows;
            renderer.Columns    = columns;

            for ( int i = 0; i < mapCells.ToArray().Length; i++ )
            {
                switch ( mapCells[ i ] )
                {
                    case 1:
                        wallCollider        = new BoxCollider2D( new Vector2( i % ( columns - 1 ) * blockLenght + halfBlockSize, i / ( columns - 1 ) * blockLenght + halfBlockSize ), blockLenght, blockLenght, new Vector4( 255, 0, 0, 255 ), this ); // 15??????
                        break;          
                    case 2:
                        wallCollider        = new BoxCollider2D(new Vector2(i % (columns - 1) * blockLenght + halfBlockSize, i / (columns - 1) * blockLenght + halfBlockSize), 6, blockLenght - 5, new Vector4(255, 0, 0, 255), this); // 15??????
                        candle              = new Candle("Candle", new Vector2(i % (columns - 1) * blockLenght + halfBlockSize, i / (columns - 1) * blockLenght + halfBlockSize), this);
                        break;
                    case 3:             
                        spawnPointGhost     = new Vector2( i % ( columns - 1 ) * blockLenght + halfBlockSize, i / ( columns - 1 ) * blockLenght + halfBlockSize );
                        break;
                    case 4:
                        spawnPointBubble    = new Vector2( i % ( columns - 1 ) * blockLenght + halfBlockSize, i / ( columns - 1 ) * blockLenght + halfBlockSize );
                        break;
                    case 5:
                        levelCollider       = new BoxCollider2D( new Vector2( i % ( columns - 1 ) * blockLenght + halfBlockSize, i / ( columns - 1 ) * blockLenght + halfBlockSize ), 10, 32, new Vector4( 255, 255, 255, 255 ), this );
                        AddComponent(levelCollider );
                        Engine.Add2( levelCollider );
                        break;
                }
                AddBehaviour(wallCollider);
                Engine.Add(wallCollider);
            }
        }
    }
}
