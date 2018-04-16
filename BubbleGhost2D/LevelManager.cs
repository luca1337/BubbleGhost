using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;
using BehaviourEngine.Test;

namespace BubbleGhostGame2D
{
    public enum RenderLayer
    {
        None        = -1,
        Gui         = 0,
        Pawn        = 1,
        Level       = 100,
        Background  = 1000
    }
    public class Level
    {
        private static Dictionary<string, Level> instances;
        private List<int>             map;
        private int                   rows;
        private int                   columns;
        private int                   index;
        private static GameObject     gameObj;

        static Level()
        {
            instances = new Dictionary<string, Level>();
        }

        public Level(string fileName, string levelName, int index)
        {
            map        = new List<int>();
            this.index = index;
            ReadFromFile(fileName);
            instances.Add(levelName, this);
        }

        private void ReadFromFile(string csvFileName)
        {
            string[] lines = File.ReadAllLines(csvFileName);
            rows      = lines.Length;

            foreach (string t1 in lines)
            {
                string[] values  = t1.Trim().Split(',');
                if (columns == 0)
                    columns = values.Length ;

                foreach (string t in values)
                {
                    int value;
                    string currentVal = t.Trim();
                    bool success      = int.TryParse(currentVal, out value);
                    if (success)
                        map.Add(value);
                }
            }
        }

        private void LoadMap()
        {
            gameObj = Engine.Spawn( new Map( map, rows, columns, index ) );
        }

        public void NextLevel( bool next )
        {
            Engine.Destroy(gameObj);
            if (next)
                index++;
            else
                index--;
            string level = "Base" + index;
            Get(level).LoadMap();
        }

        public static Level Get(string levelName)
        {
            if(instances.ContainsKey(levelName))
            return instances[levelName];
            return null;
        }

        public static void Load(string levelName)
        {
            try
            {
                instances[levelName].LoadMap();
            }
            catch(KeyNotFoundException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}