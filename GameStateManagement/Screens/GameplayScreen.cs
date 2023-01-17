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
using GameStateManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
        private Tile peaks;

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

        //damaging Tiles
        List<TileEntry> damage_tile_map;

        //deahtscreen
        private Texture2D deathscreen_wallpaper;
        private Song deathscreen_sound;

        //Invenotry
        private TileEntry interactableNearby;

        //UI
        private Texture2D heart_empty;
        private Texture2D heart_half;
        private Texture2D heart_full;
        private Vector2 healthbar_vector;
        private List<Texture2D> healthbar_list;

        private Vector2 ui_interact_string_vector = new Vector2(550, 400);

        //Debug UI
        private bool debug_mode_active = true;
        Texture2D debug_border;

        private bool debug_ui_wall_collision = false;
        private Vector2 debug_ui_wall_collision_vector = new Vector2(0,100);
        private bool debug_ui_interactable_collision = false;
        private Vector2 debug_ui_interactable_collision_vector = new Vector2(0,130);
        private bool debug_ui_enemy_collision = false;
        private Vector2 debug_ui_enemy_collision_vector = new Vector2(0, 160);
        private bool debug_ui_damagingWorld_collision = false;
        private Vector2 debug_ui_damagingWorld_collision_vector = new Vector2(0, 190);
        private Vector2 debug_ui_testing_value_vector = new Vector2(0,220);

        private Vector2 debug_ui_inventory_0_vector = new Vector2(0,250);
        private Vector2 debug_ui_inventory_1_vector = new Vector2(0, 280);
        private Vector2 debug_ui_inventory_2_vector = new Vector2(0,310);
        private Vector2 debug_ui_inventory_3_vector = new Vector2(0, 340);

        //Spells
        Item fireball;
        Texture2D fireball_texture;
        List<Item> casted_spells;
        float maxDistanceOfCastedSpell = 2000;
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
                player.DamageReceivedSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Hit_1");
                player.DeathSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Killed");
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
            //Room r1 = new Room(24, 24);
            //map = r1.Map;

            map = new string[,] { { "wl", "wt", "wt", "door_left", "door_right", "wt", "wt", "wr" },
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr" },
                                    {"wl", "gr", "peaks", "gr", "gr", "chest_small", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "chest_medium", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "chest_large", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "cl", "wb", "wb", "wb", "wb", "wb", "wb", "cr"}};

            //map = Room.GenerateGameworld3();

            tileset = Content.Load<Texture2D>(@"OurContent\Map\Dungeon_Tileset");
            tilemap = new List<TileEntry>();
            //Generate TileEntries
            wall_leftcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_left_corner"), 16, new Vector2(0,0), true, false);
            wall_rightcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_right_corner"), 16, new Vector2(0, 0), true, false);
            wall_top = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_top"), 16, new Vector2(0, 0), true, false);
            wall_bottom = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_bottom"), 16, new Vector2(0, 0), true, false);
            wall_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_left"), 16, new Vector2(0, 0), true, false);
            wall_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_right"), 16, new Vector2(0, 0), true, false);
            ground = new Tile(Content.Load<Texture2D>(@"OurContent\Map\ground"), 16, new Vector2(0, 0), false, false);
            background = new Tile(Content.Load<Texture2D>(@"OurContent\Map\background"), 16, new Vector2(0, 0), false, false);

            chest_small = new ChestTile(Content.Load<Texture2D>(@"OurContent\Map\chest_small"), 16, new Vector2(0, 0), false, true, ground, silver_key,chest_open, false);
            chest_medium = new ChestTile(Content.Load<Texture2D>(@"OurContent\Map\chest_medium"), 16, new Vector2(0, 0), false, true, ground, golden_key, chest_open, false);
            chest_large = new ChestTile(Content.Load<Texture2D>(@"OurContent\Map\chest_large"), 16, new Vector2(0, 0), false, true, ground, diamond_key, chest_open, false);
            door_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_left"), 16, new Vector2(0, 0),true,true, ground,true, silver_key, door_open, false);
            door_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_right"), 16, new Vector2(0, 0), true, true, ground, true, silver_key, door_open, false);
            door_left.NeighborInteractable = door_right;
            door_right.NeighborInteractable = door_left;

            List<Texture2D> peak_animation = new List<Texture2D>();
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_1"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_2"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_3"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_4"));
            peaks = new Tile(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_1"), 16, new Vector2(0, 0), false, false, null, true, peak_animation);

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
                        }else if (map[x,y] == "peaks")
                        {
                            tilemap.Add(new TileEntry(peaks, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                    }
                }
            }
            player.PositionX = map.GetLength(0)/2 * targetTextureResolution;
            player.PositionY = map.GetLength(1)/2 * targetTextureResolution;

            cameraPos = new Vector3(player.Position.X, player.PositionY, 0);

            collider_map = new List<Rectangle>();
            interactable_map = new List<TileEntry>();
            damage_tile_map= new List<TileEntry>();
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
                if (item.Tile.DoesDamage)
                {
                    damage_tile_map.Add(item);
                }
            }

            oldPlayerPosition= player.Position;

            //deahtscreen
            deathscreen_wallpaper = Content.Load<Texture2D>(@"OurContent\Utility\you_died");
            deathscreen_sound = Content.Load<Song>(@"OurContent\Audio\SoundEffects\you_died_soundeffect");

            //Casted spells
            casted_spells = new List<Item>();
            fireball_texture = Content.Load<Texture2D>(@"OurContent\Spells\Flame\flamethrower_2_2");

            //UI
            heart_empty = Content.Load<Texture2D>(@"OurContent\Utility\Heart\heart_empty");
            heart_half = Content.Load<Texture2D>(@"OurContent\Utility\Heart\heart_half");
            heart_full = Content.Load<Texture2D>(@"OurContent\Utility\Heart\heart_full");
            healthbar_vector = new Vector2(0,0);
            healthbar_list = new List<Texture2D>();
            healthbar_list.Add(heart_empty);
            healthbar_list.Add(heart_empty);
            healthbar_list.Add(heart_empty);

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

            //Interactables
            foreach(TileEntry item in interactable_map)
            {
                if (item.BoundingBox.Intersects(player.BoundingBox))
                {
                    debug_ui_interactable_collision = true;
                    interactableNearby = item;
                    break;
                }
                interactableNearby = null;
                debug_ui_interactable_collision = false;
            }

            //Damage from world
            foreach(TileEntry item in damage_tile_map)
            {
                if (item.BoundingBox.Intersects(player.BoundingBox))
                {
                    debug_ui_damagingWorld_collision = true;
                    item.Tile.doDamage(gameTime, player);
                    break;
                }
                debug_ui_damagingWorld_collision = false;
            }

            //Check collision with walls
            foreach(Rectangle item in collider_map)
            {
                if (item.Intersects(player.BoundingBox))
                {
                    debug_ui_wall_collision = true;
                    //player.Position = oldPlayerPosition;
                    break;
                }
                debug_ui_wall_collision = false;
            }
            
            //UI

            //HealthBar
            if(player.HealthPoints >= 2)
            {
                healthbar_list[0] = heart_full;
                if(player.HealthPoints >= 4)
                {
                    healthbar_list[1] = heart_full;
                    if(player.HealthPoints >= 6)
                    {
                        healthbar_list[2] = heart_full;
                    }else if(player.HealthPoints == 5)
                    {
                        healthbar_list[2] = heart_half;
                    }
                    else
                    {
                        healthbar_list[2] = heart_empty;
                    }
                }else if(player.HealthPoints == 3)
                {
                    healthbar_list[1] = heart_half;
                    healthbar_list[2] = heart_empty;
                }
                else
                {
                    healthbar_list[1] = heart_empty;
                    healthbar_list[2] = heart_empty;
                }
            }else if(player.HealthPoints == 1) {
                healthbar_list[0] = heart_half;
                healthbar_list[1] = heart_empty;
                healthbar_list[2] = heart_empty;
            }
            else
            {
                healthbar_list[0] = heart_empty;
                healthbar_list[1] = heart_empty;
                healthbar_list[2] = heart_empty;
            }

            oldPlayerPosition = player.Position;


            player.Update(gameTime);
            peaks.Update(gameTime);

            if(player.HealthPoints <= 0)
            {
                ScreenManager.RemoveScreen(this);
                MediaPlayer.Play(Content.Load<Song>(@"OurContent\Audio\SoundEffects\you_died_soundeffect"));
                ScreenManager.AddScreen(new DeathBackgroundScreen(), 0);
                ScreenManager.AddScreen(new DeathScreen(), 0);
            }

            cameraPos.X = (player.PositionX - 580) * -1;
            cameraPos.Y = (player.PositionY - 260) * -1;

            //Update casted spells
            for(int i = casted_spells.Count-1; i > 0; i--)
            {
                casted_spells[i].Update(gameTime);
                if (Vector2.Distance(casted_spells[i].Position, casted_spells[i].originPosition) > maxDistanceOfCastedSpell)
                {
                    casted_spells.Remove(casted_spells[i]);

                }
            }
            /*foreach (Item item in casted_spells)
            {
                item.Update(gameTime);
                if(Vector2.Distance(item.Position, item.originPosition) > maxDistanceOfCastedSpell)
                {
                    casted_spells.Remove(item);
                }
            }*/

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
            MouseState mouseState = Mouse.GetState();
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
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    if(interactableNearby != null)
                    {
                        player.PlayerInventory.AddItem(interactableNearby.Tile.Interact(player.PlayerInventory));
                    }
                }
                //Cast fireball
                //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                if(mouseState.LeftButton == ButtonState.Pressed)
                {
                    // Get the mouse position in world coordinates
                    Vector2 mousePosition = new Vector2(Mouse.GetState().X - cameraPos.X, Mouse.GetState().Y - cameraPos.Y);

                    // Get the direction from the player to the mouse
                    Vector2 fireballDirection = mousePosition - player.Position;
                    fireballDirection.Normalize();

                    float rotation = (float)Math.Atan2(fireballDirection.X, fireballDirection.Y);
                    //Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);

                    // Create a new fireball at the player's position
                    fireball = new Fireball(fireball_texture, new Vector2(player.PositionX + player.Width/2, player.PositionY + player.Height/4*3), fireballDirection, 10f, rotation, player.Position, mousePosition);
                    casted_spells.Add(fireball);

                    // Play the fireball sound
                    //fireballSound.Play();
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

            DrawCastedSpells();

            DrawUi();

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

        private void DrawCastedSpells()
        {
            foreach (Item item in casted_spells)
            {
                if(item.target.X < item.originPosition.X)
                {
                    _spriteBatch.Draw(item.Texture, item.Position, null, Color.White, item.Rotation, new Vector2(item.Texture.Width / 2, item.Texture.Height / 2), 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    _spriteBatch.Draw(item.Texture, item.Position, null, Color.White, item.Rotation, new Vector2(item.Texture.Width / 2, item.Texture.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        private void DrawUi()
        {
            for(int i = 0; i < healthbar_list.Count; i++)
            {
                _spriteBatch.Draw(healthbar_list[i], new Rectangle((int)healthbar_vector.X - (int)cameraPos.X + targetTextureResolution * i, (int)healthbar_vector.Y - (int) cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
            }

            if (interactableNearby != null)
            {
                _spriteBatch.DrawString(spriteFont, "Interact [E]", new Vector2(ui_interact_string_vector.X - cameraPos.X, ui_interact_string_vector.Y - cameraPos.Y), Color.White);
            } 
        }

        private void DrawDebugUI()
        {
            foreach(Rectangle item in collider_map)
            {
                _spriteBatch.Draw(debug_border, item, Color.White);
            }

            foreach(TileEntry item in interactable_map)
            {
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.DrawVector.X, (int)item.DrawVector.Y, targetTextureResolution, targetTextureResolution), Color.Green);
            }
            foreach (TileEntry item in damage_tile_map)
            {
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.DrawVector.X, (int)item.DrawVector.Y, targetTextureResolution, targetTextureResolution), Color.Yellow);
            }
            _spriteBatch.DrawString(spriteFont, "wall_collision:" + debug_ui_wall_collision, new Vector2(debug_ui_wall_collision_vector.X - cameraPos.X, debug_ui_wall_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "interactable_collision:" + debug_ui_interactable_collision, new Vector2(debug_ui_interactable_collision_vector.X - cameraPos.X, debug_ui_interactable_collision_vector.Y -(int)cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "enemy_collision:" + debug_ui_enemy_collision, new Vector2(debug_ui_enemy_collision_vector.X - cameraPos.X, debug_ui_enemy_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "damagingWorld_collision:" + debug_ui_damagingWorld_collision, new Vector2(debug_ui_damagingWorld_collision_vector.X - cameraPos.X, debug_ui_damagingWorld_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "health: " + player.HealthPoints, new Vector2(debug_ui_testing_value_vector.X - cameraPos.X, debug_ui_testing_value_vector.Y - cameraPos.Y), Color.White);

            _spriteBatch.DrawString(spriteFont, "inventory_0: " + player.PlayerInventory.GetItemName(0), new Vector2(debug_ui_inventory_0_vector.X - cameraPos.X, debug_ui_inventory_0_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_1: " + player.PlayerInventory.GetItemName(1), new Vector2(debug_ui_inventory_1_vector.X - cameraPos.X, debug_ui_inventory_1_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_2: " + player.PlayerInventory.GetItemName(2), new Vector2(debug_ui_inventory_2_vector.X - cameraPos.X, debug_ui_inventory_2_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_3: " + player.PlayerInventory.GetItemName(3), new Vector2(debug_ui_inventory_3_vector.X - cameraPos.X, debug_ui_inventory_3_vector.Y - cameraPos.Y), Color.White);
        }
    }
    #endregion
}