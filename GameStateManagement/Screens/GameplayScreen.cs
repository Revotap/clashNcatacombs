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

        private PlayerOld player;

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
            }

            // Ein SpriteBatch zum Zeichnen
            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            // Viewport speichern
            viewport = ScreenManager.GraphicsDevice.Viewport;

            // Font laden
            spriteFont = Content.Load<SpriteFont>("Verdana");

            //Sound effects
            SoundEffect.MasterVolume = 0.05f;

            //World
            Room r1 = new Room(24, 24);
            //map = r1.Map;

            map = new string[,] { { "wl", "wt", "wt", "door_left", "door_right", "wt", "wt", "wr" },
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr" },
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "cl", "wb", "wb", "wb", "wb", "wb", "wb", "cr"}};

            tileset = Content.Load<Texture2D>(@"OurContent\Map\Dungeon_Tileset");
            tilemap = new List<TileEntry>();
            //Generate TileEntries
            wall_leftcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_left_corner"), 16, new Vector2(0,0), false);
            wall_rightcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_right_corner"), 16, new Vector2(0, 0), false);
            wall_top = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_top"), 16, new Vector2(0, 0), false);
            wall_bottom = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_bottom"), 16, new Vector2(0, 0), false);
            wall_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_left"), 16, new Vector2(0, 0), false);
            wall_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_right"), 16, new Vector2(0, 0), false);
            ground = new Tile(Content.Load<Texture2D>(@"OurContent\Map\ground"), 16, new Vector2(0, 0), false);
            background = new Tile(Content.Load<Texture2D>(@"OurContent\Map\background"), 16, new Vector2(0, 0), false);
            chest_small = new Tile(Content.Load<Texture2D>(@"OurContent\Map\chest_small"), 16, new Vector2(0, 0), false);
            chest_medium = new Tile(Content.Load<Texture2D>(@"OurContent\Map\chest_medium"), 16, new Vector2(0, 0), false);
            chest_large = new Tile(Content.Load<Texture2D>(@"OurContent\Map\chest_large"), 16, new Vector2(0, 0), false);
            door_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_left"), 16, new Vector2(0, 0), false);
            door_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_right"), 16, new Vector2(0, 0), false);

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
                    }else if (map[x,y] == "chest_small")
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

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            Content.Unload();
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            currentKeyboardState = Keyboard.GetState();


            previousKeyboardState = currentKeyboardState;

            if (!otherScreenHasFocus)
            {

            }

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
                    //MoveShipLeft();
                    //player.MoveShipLeft();
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    //MoveShipRight();
                    //player.MoveShipRight();
                }
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred,null, SamplerState.PointClamp, null, null, null, null);

            // Hintergrund zeichnen
            DrawBackground();

            DrawMap();

            // Feinde zeichnen
            DrawEnemy();

            DrawDebugUI();

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

        }

        private void DrawMap()
        {
            //_spriteBatch.Draw(door_left, new Rectangle(0, 0, 64, 64), new Rectangle(0,0,16,16), Color.White);
            foreach(TileEntry entry in tilemap)
            {
                _spriteBatch.Draw(entry.Tile.Texture, new Rectangle((int)entry.DrawVector.X, (int) entry.DrawVector.Y, 64, 64), Color.White);
            }
        }

        private void DrawEnemy()
        {

        }
        
        public void DrawUi()
        {

        }

        public void DrawDebugUI()
        {

        }
    }
    #endregion
}