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
        public static String[,] map_01 { get; } = {{"  ","  ","  ","  ","  ","  ","wl","wt","dl_h_7","wt","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","  ","  ","wl","gr_x","gr_x","gr_x","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"wl","wt","wt","wt","wt","wt","wt","gr_x","gr_x","gr_x","wt","wt","wt","wt","wt","wt","wr","  ","  ","  ","  ","  ","  ","wl","wt","wt","wt","wt","wt","wr","  ","  ","  ","wl","wt","wt","wt","wt","wt","wr"},
                                                 {"wl","pk","gr_x","pk","gr_x","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","gr_x","pk","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","boss","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","wl","wt","wt","wt","wr","wl","gr_x","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","wl","gr_x","c0","gr","wt","wt","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","wl","gr_x","gr","gr","dl_v_4","gr","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","wl","gr_x","gr","gr","dr_v_4","gr","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","wl","gr_x","gr","gr","cr2","cl2","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","wl","gr_x","gr","gr_x","wr","wl","gr_x","gr","gr","gr","gr_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","wl","gr_x","gr_x","gr_x","wr","wl","gr_x","gr","gr","gr","gr","wb","wb","wb","wb","wb","gr","gr","gr","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","cl","wb","wb","wb","cr","wl","gr_x","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr","gr_x","gr_x","gr","gr","gr","gr","gr","gr","gr","gr_x","gr_x","gr","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr","cr2","wb","cl2","gr","gr","gr","gr","gr","cr2","wb","cl2","gr","gr_x","wr"},
                                                 {"wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr_x","wr","  ","wl","gr_x","gr","gr","gr","gr_x","wr","  ","wl","gr_x","gr_x","wr"},
                                                 {"wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr_x","wr","  ","wl","gr_x","gr","gr","gr","gr_x","wr","  ","wl","gr_x","gr_x","wr"},
                                                 {"wl","gr_x","pk","gr_x","gr_x","gr_x","pk","gr_x","pk","gr_x","pk","gr_x","pk","gr_x","pk","gr_x","wr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr_x","wr","  ","wl","gr_x","gr","gr","gr","gr_x","wr","  ","wl","gr_x","gr_x","wr"},
                                               {"cl","wb","cl2","gr_x","gr_X","cr2","wb","wb","wb","wb","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","wl","gr_x","gr_x","wr","  ","wl","gr_x","gr","gr","gr","gr_x","wr","  ","wl","gr_x","gr_x","wr"},
                                                 {"  ","wl","wt","dl_h_1","dr_h_1","wt","wt","wt","wt","wt","wt","wt","wt","wt","wt","wt","wr","wl","wt","wr","  ","  ","  ","wl","gr_x","gr","wt","wt","wt","gr","gr","gr","gr","gr","wt","wt","wt","gr","gr_x","wr"},
                                                 {"  ","wl","gr_x","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr_x","wl_x","c2_x","wr","  ","  ","  ","wl","gr_x","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr_x","wr"},
                                                 {"  ","wl","gr_x","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","wr_x","wl_x","gr_x","wr","  ","  ","  ","wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr"},
                                                 {"  ","wl","gr_x","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr_x","wr","cl","wb","cr","  ","  ","  ","cl","wb","wb","wb","wb","wb","wb","wb","wb","wb","cl2","gr_x","gr_x","cr2","wb","wb","cr"},
                                               {"  ","wl","gr_x","gr_x","cr2","cl2","gr_x","gr_x","gr_x","gr_x","gr_x","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","wl","gr_x","gr_x","wr","  ","  ","  "},
                                                 {"  ","wl","gr_x","gr_x","wr","wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","wl","dl_h_3","dr_h_3","wr","  ","  ","  "},
                                               {"  ","wl","gr_x","gr_x","wr","wl","gr_x","gr_x","gr_x","cr2","cl2","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","  ","  ","wl","wt","wt","wt","wt","wt","wt","wr","  ","  ","  ","wl","gr_x","gr_x","wr","  ","  ","  "},
                                                 {"  ","wl","gr_x","gr_x","wr","wl","gr_x","gr_x","gr_x","wr","wl","gr_x","gr","gr","gr","gr","wt","wt","wt","wt","wt","wt","wt","gr","gr","gr","c1","gr","gr","wt","wt","wt","wt","wt","gr","gr_x","wr","  ","  ","  "},
                                                 {"  ","wl","gr_x","gr_x","wt","wt","gr_x","gr_x","gr_x","wr","wl","gr_x","gr","gr","gr","gr","gr","dl_v_5","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr","gr_x","wr","  ","  ","  "},
                                                 {"  ","wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","wl","gr_x","gr","gr","gr","gr","gr","dr_v_5","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr","gr","gr","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","  ","  ","  "},
                                                {"  ","wl","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","wr","wl","gr_x","gr_x","gr_x","gr_x","gr_x","cr2","wb","wb","wb","wb","wb","cl2","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","cr2","wb","wb","wb","wb","wb","wb","cr","  ","  ","  "},
                                             {"  ","cl","wb","wb","wb","cl2","gr_x","gr_x","cr2","wb","wb","cl2","gr","gr","cr2","wb","cr","  ","  ","  ","  ","  ","cl","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","wt","dl_h_0","dr_h_0","wt","wr","wl","wt","dl_h_2","dr_h_2","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr_x","gr_x","gr_x","gr_x","wr","wl","wt","gr","gr","wt","wt","wt","wt","wt","wt","wt","wt","wr","  ","  ","  ","  ","  ","wl","wt","wt","wt","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr_x","gr_x","pl","gr_x","wr","wl","pk","gr","gr","gr","gr","gr","gr","pk","gr","gr","gr","wt","wt","wt","wt","wt","wt","wt","c2_x","gr","c2","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","gr_","gr_x","gr_x","gr_x","wr","wl","pk","gr","gr","gr","gr","pk","gr","pk","gr","pk","gr","gr","dl_v_6","gr","gr","gr","gr","gr","gr","gr","gr_x","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","wl","c0","gr_x","gr_x","gr_x","wr","wl","pk","gr","gr","gr","gr","pk","gr","gr","gr","pk","gr","gr","dr_v_6","gr_x","gr_x","gr_x","gr_x","ggr_xr","gr_x","gr_x","gr_x","wr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","cl","wb","wb","wb","wb","cr","wl","pk","gr","gr","gr","gr","pk","gr","pk","gr","gr","gr","cr2","wb","wb","wb","wb","wb","wb","wb","wb","wb","cr","  ","  ","  ","  ","  ","  ","  "},
                                                 {"  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","wl","pk","gr_x","gr_x","gr_x","gr_x","gr_x","gr_x","pk","gr_x","pk","gr_x","wr","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  ","  "},
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
