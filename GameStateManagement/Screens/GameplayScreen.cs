#region File Description

//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using GameStateManagement.Class;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;

#endregion Using Statements

namespace GameStateManagement
{
    internal class GameplayScreen : GameScreen
    {
        #region Fields

        private ContentManager Content;
        private SpriteFont gameFont;

        private Random random = new Random();

        private float pauseAlpha;

        #endregion Fields

        #region Variablen

        private Player player;

        // Grafische Ausgabe
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Font
        private SpriteFont spriteFont;

        // Viewport
        private Viewport viewport;

        // Tastatur abfragen
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        //World
        private int surrounding_width = 20;

        private String[,] map;
        private Texture2D tileset;
        private List<TileEntry> tilemap;
        private int targetTextureResolution = 64;
        private Tile wall_leftcorner;
        private Tile wall_rightcorner;
        private Tile wall_top;
        private Tile wall_bottom;
        private Tile wall_left;
        private Tile wall_right;
        private Tile ground;
        private Tile background;
        private Tile chest_small;
        private Tile chest_medium;
        private Tile chest_large;
        private Tile door_left;
        private Tile door_right;

        //Sounds
        SoundEffect chest_open;
        SoundEffect door_open;

        //Camera
        private Vector3 cameraPos;

        //Colliders
        List<Rectangle> collider_map;
        private Vector2 oldPlayerPosition;

        //Interactables
        List<TileEntry> interactable_map;

        //UI

        //Debug UI
        private bool debug_mode_active = true;
        Texture2D debug_border;

        #endregion Variablen


        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            if(player == null)
            {
                //player = new PlayerOld(Content.Load<Texture2D>("ship"), Content.Load<Texture2D>("laser"), Content.Load<SoundEffect>("laserfire"));
                List<Texture2D> animation = new List<Texture2D>();
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_0"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_1"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_2"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_3"));
                player = new Player("Spieler", animation, 64, 112);
            }

            // Ein SpriteBatch zum Zeichnen
            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            // Viewport speichern
            viewport = ScreenManager.GraphicsDevice.Viewport;

            // Font laden
            spriteFont = Content.Load<SpriteFont>("Verdana");

            //Sound effects
            SoundEffect.MasterVolume = 0.05f;
            chest_open = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Coins");
            door_open = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\door_wood_open");

            //Items
            Key silver_key = new Key("Silver Key", 0, null, 16);
            Key golden_key = new Key("Golden Key", 1, null, 16);
            Key diamond_key = new Key("Diamond Key", 2, null, 16);

            //World
            Room r1 = new Room(24, 24);
            //map = r1.Map;

            map = new string[,] { { "wl", "wt", "wt", "door_left", "door_right", "wt", "wt", "wr" },
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr" },
                                    {"wl", "gr", "gr", "gr", "gr", "chest_small", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "chest_medium", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "chest_large", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "cl", "wb", "wb", "wb", "wb", "wb", "wb", "cr"}};

            tileset = Content.Load<Texture2D>(@"OurContent\Map\Dungeon_Tileset");
            tilemap = new List<TileEntry>();
            //Generate TileEntries
            wall_leftcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_left_corner"), 16, new Vector2(0,0), true);
            wall_rightcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_right_corner"), 16, new Vector2(0, 0), true);
            wall_top = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_top"), 16, new Vector2(0, 0), true);
            wall_bottom = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_bottom"), 16, new Vector2(0, 0), true);
            wall_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_left"), 16, new Vector2(0, 0), true);
            wall_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_right"), 16, new Vector2(0, 0), true);
            ground = new Tile(Content.Load<Texture2D>(@"OurContent\Map\ground"), 16, new Vector2(0, 0), false);
            background = new Tile(Content.Load<Texture2D>(@"OurContent\Map\background"), 16, new Vector2(0, 0), false);

            chest_small = new Tile(Content.Load<Texture2D>(@"OurContent\Map\chest_small"), 16, new Vector2(0, 0), false, true,chest_open);
            chest_medium = new Tile(Content.Load<Texture2D>(@"OurContent\Map\chest_medium"), 16, new Vector2(0, 0), false, true, chest_open);
            chest_large = new Tile(Content.Load<Texture2D>(@"OurContent\Map\chest_large"), 16, new Vector2(0, 0), false, true, chest_open);
            door_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_left"), 16, new Vector2(0, 0),true,true, ground,true, silver_key, door_open);
            door_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_right"), 16, new Vector2(0, 0), true, true, ground, true, silver_key, door_open);

            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x,y] == "wl")
                    {
                        tilemap.Add(new TileEntry(wall_left, new Vector2(targetTextureResolution*y,targetTextureResolution*x),64));
                    }else if (map[x,y] == "wr")
                    {
                        tilemap.Add(new TileEntry(wall_right, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x,y] == "wt")
                    {
                        tilemap.Add(new TileEntry(wall_top, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x, y] == "wb")
                    {
                        tilemap.Add(new TileEntry(wall_bottom, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x, y] == "cl")
                    {
                        tilemap.Add(new TileEntry(wall_leftcorner, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x, y] == "cr")
                    {
                        tilemap.Add(new TileEntry(wall_rightcorner, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x,y] == "gr")
                    {
                        tilemap.Add(new TileEntry(ground, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x,y] == "bgrnd")
                    {
                        tilemap.Add(new TileEntry(background, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x,y] == "door_left")
                    {
                        tilemap.Add(new TileEntry(door_left, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else if (map[x,y] == "door_right")
                    {
                        tilemap.Add(new TileEntry(door_right, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }else
                    {
                        tilemap.Add(new TileEntry(ground, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));

                        if (map[x,y] == "chest_small")
                        {
                            tilemap.Add(new TileEntry(chest_small, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }else if (map[x,y] == "chest_medium")
                        {
                            tilemap.Add(new TileEntry(chest_medium, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }else if (map[x,y] == "chest_large")
                        {
                            tilemap.Add(new TileEntry(chest_large, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                    }
                }
            }
            player.PositionX = map.GetLength(0)/2 * targetTextureResolution;
            player.PositionY = map.GetLength(1)/2 * targetTextureResolution;

            cameraPos = new Vector3(player.Position.X, player.PositionY, 0);

            collider_map = new List<Rectangle>();
            interactable_map = new List<TileEntry>();
            //generate collision map and interactable map
            foreach(TileEntry item in tilemap)
            {
                if (item.Tile.HasCollision)
                {
                    collider_map.Add(item.BoundingBox);
                }
                if(item.Tile.IsInteractable)
                {
                    interactable_map.Add(item);
                }
            }

            oldPlayerPosition= player.Position;

            //UI

            //Debug UI
            debug_border = Content.Load<Texture2D>(@"OurContent\Utility\outline");
        }

        public override void UnloadContent()
        {
            Content.Unload();
        }

        #endregion Initialization

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            currentKeyboardState = Keyboard.GetState();


            previousKeyboardState = currentKeyboardState;

            if (!otherScreenHasFocus)
            {

            }

            //Check collision
            foreach(Rectangle item in collider_map)
            {
                if (item.Intersects(player.BoundingBox))
                {
                    player.Position = oldPlayerPosition;
                    break;
                }
            }
            
            oldPlayerPosition = player.Position;


            player.Update(gameTime);
            cameraPos.X = (player.PositionX - 580) * -1;
            cameraPos.Y = (player.PositionY - 260) * -1;
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    player.moveLeft();
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    player.moveRight();
                }
                
                if(keyboardState.IsKeyDown(Keys.Up))
                {
                    player.moveUp();
                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    player.moveDown();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred,null, SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(cameraPos));

            DrawBackground();

            DrawMap();

            DrawEnemy();

            DrawPlayer();

            if(debug_mode_active)
            {
                DrawDebugUI();
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion Update and Draw

        #region Methods
        public void CreateEnemies()
        {
            
        }

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                    !previousKeyboardState.IsKeyDown(key);
        }

        private void DrawBackground()
        {
            for(int i = -surrounding_width; i < map.GetLength(1) + surrounding_width; i++)
            {
                for(int y = -surrounding_width; y < map.GetLength(0) + surrounding_width; y++)
                {
                    _spriteBatch.Draw(background.Texture, new Rectangle(i * targetTextureResolution, y * targetTextureResolution, targetTextureResolution, targetTextureResolution), Color.White);
                }
            }
        }

        private void DrawMap()
        {
            foreach(TileEntry entry in tilemap)
            {
                _spriteBatch.Draw(entry.Tile.Texture, new Rectangle((int)entry.DrawVector.X, (int) entry.DrawVector.Y, 64, 64), Color.White);
            }
        }

        private void DrawEnemy()
        {

        }
        
        private void DrawPlayer()
        {
            _spriteBatch.Draw(player.Texture, new Rectangle((int)player.PositionX, (int)player.PositionY, 64, 112), Color.White);
        }
        private void DrawUi()
        {

        }

        private void DrawDebugUI()
        {
            foreach(Rectangle item in collider_map)
            {
                _spriteBatch.Draw(debug_border, item, Color.White);
            }

            foreach(TileEntry item in interactable_map)
            {
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.DrawVector.X, (int)item.DrawVector.Y, targetTextureResolution, targetTextureResolution), Color.Red);
            }
        }
    }
    #endregion
}