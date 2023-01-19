using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Room
    {
        private bool hasLeftNeightbor = false;
        private bool hasTopNeighbor = false;
        private bool hasRightNeightbor = false;
        private bool hasBottomNeighbor = false;

        private Room leftNeightbor;
        private Room topNeighbor;
        private Room rightNeightbor;
        private Room bottomNeighbor;

        private String[,] map;
        private string[,] _gameworld;
        private string[,] _gameworld2;
        int width;
        int height;

        public Room(int max_width, int max_height) {
            this.width = max_width;
            this.height = max_height;
            map = new String[max_width, max_height];

            int rand_num = new Random().Next(1, 15);
            BitArray ba = new BitArray(new int[] { rand_num });
            if (ba.Get(0)) { this.hasLeftNeightbor = true; }
            if(ba.Get(1)) { this.hasTopNeighbor= true; }
            if(ba.Get(2)) { this.hasRightNeightbor= true; }
            if(ba.Get(3)) { this.hasBottomNeighbor= true; }

            generateMap();
            GenerateGameworld();
            GenerateGameworld2();
        }

        public string[,] Map { get => map; }
        public string[,] Gameworld { get => _gameworld; set => _gameworld = value; }

        private void generateMap()
        {
            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    if(x == 0)
                    {
                        if (y == 0)
                        {
                            //Upper left corner
                            map[x, y] = "wl";
                        }
                        else if (y > 0 && y < height - 1)
                        {
                            //Left wall
                            map[x, y] = "wt";
                        }
                        else
                        {
                            //Bottom left corner
                            map[x, y] = "wr";
                        }
                    }else if(x == width - 1)
                    {
                        if (y == 0)
                        {
                            //Upper left corner
                            map[x, y] = "wr";
                        }
                        else if (y > 0 && y < height - 1)
                        {
                            //Left wall
                            map[x, y] = "wr";
                        }
                        else if (y == height - 1)
                        {
                            //Bottom left corner
                            map[x, y] = "cr";
                        }
                    }
                    else
                    {
                        if(y == 0)
                        {
                            //Top Wall
                            map[x, y] = "wt";
                        }else if(y > 0 && y < height-1)
                        {
                            //Empty space in the middle
                            map[x, y] = "g";
                        }
                        else
                        {
                            //Bottom wall
                            map[x, y] = "wb";
                        }
                    }
                }
            }
        }

        private void GenerateGameworld()
        {
            // Set the size of the gameworld
            int rows = 20;
            int columns = 20;
            _gameworld = new string[rows, columns];

            // Fill the gameworld with empty spaces
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _gameworld[i, j] = " ";
                }
            }

            // Create a room in the middle of the gameworld
            int roomRows = 5;
            int roomColumns = 5;
            int roomRowStart = (rows / 2) - (roomRows / 2);
            int roomColumnStart = (columns / 2) - (roomColumns / 2);
            for (int i = roomRowStart; i < roomRowStart + roomRows; i++)
            {
                for (int j = roomColumnStart; j < roomColumnStart + roomColumns; j++)
                {
                    _gameworld[i, j] = "gr";
                }
            }

            // Add walls around the room
            for (int i = roomRowStart - 1; i < roomRowStart + roomRows + 1; i++)
            {
                _gameworld[i, roomColumnStart - 1] = "wt";
                _gameworld[i, roomColumnStart + roomColumns] = "wt";
            }
            for (int j = roomColumnStart; j < roomColumnStart + roomColumns; j++)
            {
                _gameworld[roomRowStart - 1, j] = "wt";
                _gameworld[roomRowStart + roomRows, j] = "wt";
            }

            // Add a door to the room
            _gameworld[roomRowStart + roomRows, (roomColumnStart + roomColumns) / 2] = "door_left";

            // Create a bigger room
            int bigRoomRows = 10;
            int bigRoomColumns = 10;
            int bigRoomRowStart = (rows / 2) - (bigRoomRows / 2);
            int bigRoomColumnStart = (columns / 2) - (bigRoomColumns / 2);
            for (int i = bigRoomRowStart; i < bigRoomRowStart + bigRoomRows; i++)
            {
                for (int j = bigRoomColumnStart; j < bigRoomColumnStart + bigRoomColumns; j++)
                {
                    _gameworld[i, j] = "gr";
                }
            }

            // Add walls around the big room
            for (int i = bigRoomRowStart - 1; i < bigRoomRowStart + bigRoomRows + 1; i++)
            {
                _gameworld[i, bigRoomColumnStart - 1] = "wt";
                _gameworld[i, bigRoomColumnStart + bigRoomColumns] = "wt";
            }
            for (int j = bigRoomColumnStart; j < bigRoomColumnStart + bigRoomColumns; j++)
            {
                _gameworld[bigRoomRowStart - 1, j] = "wt";
                _gameworld[bigRoomRowStart + bigRoomRows, j] = "wt";
            }

            // Add a door to the big room
            _gameworld[bigRoomRowStart + bigRoomRows, (bigRoomColumnStart + bigRoomColumns) / 2] = "door_left";
        }
        private void GenerateGameworld2()
        {
            Random random = new Random();
            const int roomSize = 20;
            const int mapWidth = 80;
            const int mapHeight = 40;
            const int maxRooms = 4;
            List<Rectangle> rooms = new List<Rectangle>();

            _gameworld2 = new string[mapWidth, mapHeight];

            // Initialize map with empty spaces
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    _gameworld2[x, y] = ".";
                }
            }

            // Generate rooms
            for (int i = 0; i < maxRooms; i++)
            {
                int roomWidth = random.Next(roomSize, roomSize + 6);
                int roomHeight = random.Next(roomSize, roomSize + 6);
                int roomX = random.Next(0, mapWidth - roomWidth - 1);
                int roomY = random.Next(0, mapHeight - roomHeight - 1);
                Rectangle room = new Rectangle(roomX, roomY, roomWidth, roomHeight);

                // Check for room overlapping
                bool overlapped = false;
                foreach (Rectangle otherRoom in rooms)
                {
                    if (room.Intersects(otherRoom))
                    {
                        overlapped = true;
                        break;
                    }
                }

                if (!overlapped)
                {
                    // Draw room on map
                    for (int x = room.X; x < room.X + room.Width; x++)
                    {
                        for (int y = room.Y; y < room.Y + room.Height; y++)
                        {
                            _gameworld2[x, y] = " ";
                        }
                    }

                    // Add room to list
                    rooms.Add(room);
                }
                else
                {
                    i--;
                }
            }

            // Generate doors between rooms
            for (int i = 0; i < rooms.Count; i++)
            {
                // Connect current room to next room
                if (i < rooms.Count - 1)
                {
                    Rectangle currentRoom = rooms[i];
                    Rectangle nextRoom = rooms[i + 1];

                    // Random point on current room
                    int currentRoomDoorX = random.Next(currentRoom.X + 1, currentRoom.X + currentRoom.Width - 1);
                    int currentRoomDoorY = random.Next(currentRoom.Y + 1, currentRoom.Y + currentRoom.Height - 1);

                    // Random point on next room
                    int nextRoomDoorX = random.Next(nextRoom.X + 1, nextRoom.Y + nextRoom.Height - 1);
                    int nextRoomDoorY = random.Next(nextRoom.Y + 1, nextRoom.Y + nextRoom.Height - 1);

                    // Draw door symbol on current room and next room
                    _gameworld2[currentRoomDoorX, currentRoomDoorY] = "+";
                    _gameworld2[nextRoomDoorX, nextRoomDoorY] = "+";

                    // Connect the two door points with a path
                    int x = currentRoomDoorX;
                    int y = currentRoomDoorY;
                    while (x != nextRoomDoorX)
                    {
                        _gameworld2[x, y] = "#";
                        x += x < nextRoomDoorX ? 1 : -1;
                    }
                    while (y != nextRoomDoorY)
                    {
                        _gameworld2[x, y] = "#";
                        y += y < nextRoomDoorY ? 1 : -1;
                    }
                }
            }
        }

        public static string[,] GenerateGameworld3(int min_width, int min_height, int max_width, int max_height, int door_cnt)
        {
            Random random = new Random();
            string[,] map;
            const int roomSize = 8;
            const int mapWidth = 80;
            const int mapHeight = 40;
            const int maxDoors = 3;
            List<Rectangle> rooms = new List<Rectangle>();

            map = new string[mapWidth, mapHeight];

            //Initialize map with empty spaces
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    map[x, y] = " ";
                }
            }

            // Generate a random room
            int roomWidth = random.Next(roomSize, roomSize + 6);
            int roomHeight = random.Next(roomSize, roomSize + 6);
            int roomX = random.Next(1, mapWidth - roomWidth - 1);
            int roomY = random.Next(1, mapHeight - roomHeight - 1);
            Rectangle room = new Rectangle(roomX, roomY, roomWidth, roomHeight);

            // Draw walls around room
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    if (x == room.X)
                    {
                        // Left wall
                        map[x, y] = "wl";
                    }
                    else if (x == room.X + room.Width - 1)
                    {
                        // Right wall
                        map[x, y] = "wr";
                    }
                    else if (y == room.Y)
                    {
                        // Top wall
                        map[x, y] = "wt";
                    }
                    else if (y == room.Y + room.Height - 1)
                    {
                        // Bottom wall
                        map[x, y] = "wb";
                    }

                    // Bottom left corner
                    if (x == room.X && y == room.Y + room.Height - 1)
                    {
                        map[x, y] = "cl";
                    }
                    // Bottom right corner
                    if (x == room.X + room.Width - 1 && y == room.Y + room.Height - 1)
                    {
                        map[x, y] = "cr";
                    }
                }
            }

            /*// Generate doors
            int numDoors = random.Next(1, maxDoors + 1);
            for (int i = 0; i < numDoors; i++)
            {
                int doorX = random.Next(room.X + 1, room.X + room.Width - 1);
                int doorY = random.Next(room.Y + 1, room.Y + room.Height - 1);

                // Draw door_left
                map[doorX, doorY] = tileMap["door_left"];
            }*/

            return map;
        }
    }

}

