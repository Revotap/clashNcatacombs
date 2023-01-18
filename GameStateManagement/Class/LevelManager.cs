﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameStateManagement.Class
{
    internal class LevelManager {

        //One map has to have 5 rooms
        public static String[,] map_01 { get; } = {{"  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","wt","wt","wt","wt","wt","wt","wt","wt","wr","wt","wt","wt","wt","wt","wt","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                               {"cl","wb","cl2","gr","gr","cr2","wb","wb","wb","wb","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","wt","dl","dr","wt","wt","wt","wt","wt","wt","wt","wt","wt","wt","wt","wr","wl","wt","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","wl","c1","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","wl","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","cl","wb","cr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                               {"  ","wl","gr","gr","cr2","cl2","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","wr","wl","rg","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                               {"  ","wl","gr","gr","wr","wl","gr","gr","gr","cr2","cl2","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","wl","wt","wt","wt","wt","wt","wt","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","wr","wl","gr","gr","gr","wr","wl","gr","gr","gr","gr","gr","wt","wt","wt","wt","wt","wt","wt","gr","gr","gr","gr","gr","gr","wt","wt","wt","wt","wt","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","wt","wt","gr","gr","gr","wr","wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","  ","  ","  ","  ","  ","  "},
                                                 {"  ","wl","gr","gr","gr","gr","gr","gr","gr","wr","wl","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","  ","  ","  ","  ","  ","  "},
                                                {"  ","wl","gr","gr","gr","gr","gr","gr","gr","wr","wl","gr","gr","gr","gr","gr","cr2","wb","wb","wb","wb","wb","cl2","gr","gr","gr","gr","gr","gr","cr2","wb","wb","wb","wb","  ","  ","  ","  ","  ","  "},
                                             {"  ","cl","wb","wb","wb","cl2","gr","gr","cr2","wb","wb","cl2","gr","gr","cr2","wb","cr","  ","  ","  ","  ","  ","cl","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","wt","dl","dr","wt","wr","wl","wt","dl","dr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr","gr","gr","c0","wr","wl","wt","gr","gr","wt","wt","wt","wt","wt","wt","wt","wt","wr","  ","  ","  ","  ","  ","wl","wt","wt","wt","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr","gr","pl","gr","wr","wl","pk","gr","gr","gr","gr","gr","gr","pk","gr","gr","gr","wt","wt","wt","wt","wt","wt","wt","c0","gr","c1","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr","gr","gr","gr","wr","wl","pk","gr","gr","gr","gr","pk","gr","pk","gr","pk","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr","gr","gr","gr","wr","wl","pk","gr","gr","gr","gr","pk","gr","gr","gr","pk","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","cl","wb","wb","wb","wb","cr","wl","pk","gr","gr","gr","gr","pk","gr","pk","gr","gr","gr","cr2","wb","wb","wb","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","wl","pk","gr","gr","gr","gr","gr","gr","pk","gr","pk","gr","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","cl","wb","wb","wb","wb","wb","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "}};


        public static String[,] Deserialize(String path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));

            String[] import;

            using (FileStream fs = new FileStream("level.xml", FileMode.Open))
            {
                import = (String[])serializer.Deserialize(fs);
            }

            int lineCnt = 0;
            int maxLetterCnt = 0;
            foreach(String line in import)
            {
                lineCnt++;
                if(line.Length > maxLetterCnt)
                {
                    maxLetterCnt = line.Length;
                }
            }

            String[,] map = new string[1, maxLetterCnt];
            for(int i = 0; i < map.GetLength(0); i++)
            {
                map[0, i] = import[i];
            }

            return map;
        }

        public static void Serialize(String path, String[,] level) {
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));

            using (FileStream fs = new FileStream("level.xml", FileMode.Create))
            {
                for(int i = 0; i < level.GetLength(0); i++)
                {
                    String[] temp = new string[level.GetLength(1)];

                    for(int y = 0; y < level.GetLength(1); y++)
                    {
                        temp[y] = level[i, y];
                    }

                    serializer.Serialize(fs, temp);
                }
            }
        }
    }
}
