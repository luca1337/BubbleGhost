﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;
using BehaviourEngine;

namespace BubbleGhostGame2D
{
    public class Level
    {
        private static Dictionary<string, Level> instances;
        private List<int>             currentMap;
        private int                   rows;
        private int                   columns;
        private int                   index;
        
      
        static Level()
        {
            instances = new Dictionary<string, Level>();
        }

        public Level(string fileName, string levelName, int index)
        {
            currentMap        = new List<int>();
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
                        currentMap.Add(value);
                }
            }
        }

        private void LoadMap()
        {
            GameObject.Spawn(new Map( currentMap, rows, columns, index ) );
        }

        public void NextLevel( bool next )
        {
            //TODO destroy previous map

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