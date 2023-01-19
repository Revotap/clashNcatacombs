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
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Timers;
using System.Xml.Serialization;

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

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        //World
        private int surrounding_width = 20;

        private String[,] map;
        private Texture2D tileset;
        private List<TileEntry> tilemap;
        private int targetTextureResolution = 64;
        private Tile wall_leftcorner;
        private Tile wall_leftcorner_2;
        private Tile wall_rightcorner;
        private Tile wall_rightcorner_2;
        private Tile wall_top;
        private Tile wall_bottom;
        private Tile wall_left;
        private Tile wall_right;
        private Tile ground;
        private Tile background;
        private Texture2D chest_small;
        private Texture2D chest_medium;
        private Texture2D chest_large;
        private Texture2D horizontal_door_left;
        private Texture2D horizontal_door_right;
        private Texture2D vertical_door_left;
        private Texture2D vertical_door_right;
        private Tile peaks;

        private Tile[,] crossInteractableTiles;

        //Sounds
        SoundEffect chest_open;
        SoundEffect door_open;

        //Camera
        private Vector3 cameraPos;
        private Camera worldCamera;

        //Colliders
        List<Rectangle> collider_map;
        private Vector2 oldPlayerPosition;
        private Vector2 playerVelocity;
        private bool collisionDetected = false;

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

        private Vector2 debug_ui_player_position_vector = new Vector2(0, 70);
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

        private Vector2 debug_ui_player_velocity_vector = new Vector2(0, 370);

        //Spells
        Spell fireball;
        Texture2D fireball_texture;
        List<Spell> casted_spells;
        float maxDistanceOfCastedSpell = 2000;
        #endregion Variablen


        #region Initialization
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            if(player == null)
            {
                List<Texture2D> animation = new List<Texture2D>();
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_0"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_1"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_2"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_3"));
                player = new Player("Spieler", 6, 64, 112, new Vector2(-200, -200), animation, 4f, 
                    Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Hit_1"),
                    Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Killed"), null);
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

            map = new string[,] { { "wl", "wt", "wt", "dl", "dl", "wt", "wt", "wr" },
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr" },
                                    {"wl", "gr", "peaks", "gr", "gr", "c0", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    {"wl", "gr", "gr", "gr", "gr", "gr", "c1", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "c2", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "wl", "gr", "gr", "gr", "gr", "gr", "gr", "wr"},
                                    { "cl", "wb", "wb", "wb", "wb", "wb", "wb", "cr"}};

            //FILESYSTEM TEST
            /*XmlSerializer serializer = new XmlSerializer(typeof(string[,]));
            using(FileStream fs = new FileStream("level.xml", FileMode.Create) )
            {
                serializer.Serialize(fs, map);
            }*/

            //MySerializer.Serialize("level.xml", map);

            //map = MySerializer.Deserialize("level.xml");


            map = LevelManager.map_01;

            //tileset = Content.Load<Texture2D>(@"OurContent\Map\Dungeon_Tileset");
            tilemap = new List<TileEntry>();
            //Generate TileEntries
            wall_leftcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_left_corner"), true);
            wall_leftcorner_2 = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_left_corner_2"), true);
            wall_rightcorner = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_right_corner"), true);
            wall_rightcorner_2 = new Tile(Content.Load<Texture2D>(@"OurContent\Map\bottom_right_corner_2"), true);
            wall_top = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_top"), true);
            wall_bottom = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_bottom"), true);
            wall_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_left"), true);
            wall_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\wall_right"), true);
            ground = new Tile(Content.Load<Texture2D>(@"OurContent\Map\ground"), false);
            background = new Tile(Content.Load<Texture2D>(@"OurContent\Map\background"), false);

            horizontal_door_left = Content.Load<Texture2D>(@"OurContent\Map\horizontal_door_left");
            horizontal_door_right = Content.Load<Texture2D>(@"OurContent\Map\horizontal_door_right");
            vertical_door_left = Content.Load<Texture2D>(@"OurContent\Map\vertical_door_left");
            vertical_door_right = Content.Load<Texture2D>(@"OurContent\Map\vertical_door_right");

            //Loot tables for chests
            List<Item> loot_table_chest_small = new List<Item>();
            loot_table_chest_small.Add(silver_key);

            List<Item> loot_table_chest_medium = new List<Item>();
            loot_table_chest_medium.Add(golden_key);

            List<Item> loot_table_chest_large = new List<Item>();
            loot_table_chest_large.Add(diamond_key);

            chest_small = Content.Load<Texture2D>(@"OurContent\Map\chest_small");
            chest_medium = Content.Load<Texture2D>(@"OurContent\Map\chest_medium");
            chest_large = Content.Load<Texture2D>(@"OurContent\Map\chest_large");

            //Generate Chest Tiles
            /*chest_small = new ChestTile(Content.Load<Texture2D>(@"OurContent\Map\chest_small"), false, loot_table_chest_small);
            chest_small.SetIsInteractable(ground.texture(), null, chest_open);

            chest_medium = new ChestTile(Content.Load<Texture2D>(@"OurContent\Map\chest_medium"), false, loot_table_chest_medium);
            chest_medium.SetIsInteractable(ground.texture(), null, chest_open);

            chest_large = new ChestTile(Content.Load<Texture2D>(@"OurContent\Map\chest_large"), false, loot_table_chest_large);
            chest_large.SetIsInteractable(ground.texture(), null, chest_open);
            */
            /*door_left = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_left"), true);
            door_left.SetIsLocked(silver_key);
            door_right = new Tile(Content.Load<Texture2D>(@"OurContent\Map\door_right"), true);
            door_right.SetIsLocked(silver_key);

            door_left.SetIsInteractable(ground.texture(), door_right, door_open);
            door_right.SetIsInteractable(ground.texture(), door_left, door_open);*/

            peaks = new Tile(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_1"), false);
            List<Texture2D> peak_animation = new List<Texture2D>();
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_1"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_2"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_3"));
            //peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_4"));
            peaks.SetDoesDamage(1, null, 700);
            peaks.SetIsAnimated(peak_animation, 400, null);

            Vector2 playerStartingPos = new Vector2(0,0);

            crossInteractableTiles = new Tile[2, 20];
            
            for(int i = 0; i< 2; i++)
            {
                for(int x = 0; x < 20; x++)
                {
                    crossInteractableTiles[i, x] = null;
                }
            }

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    String phrase = map[x, y];
                    String[] words = phrase.Split('_');

                    if (words[0] == "wl")
                    {
                        tilemap.Add(new TileEntry(wall_left, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    } else if (words[0] == "wr")
                    {
                        tilemap.Add(new TileEntry(wall_right, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    } else if (words[0] == "wt")
                    {
                        tilemap.Add(new TileEntry(wall_top, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    } else if (words[0] == "wb")
                    {
                        tilemap.Add(new TileEntry(wall_bottom, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    } else if (words[0] == "cl")
                    {
                        tilemap.Add(new TileEntry(wall_leftcorner, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    } else if (words[0] == "cr")
                    {
                        tilemap.Add(new TileEntry(wall_rightcorner, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }
                    else if (words[0] == "cl2")
                    {
                        tilemap.Add(new TileEntry(wall_leftcorner_2, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }
                    else if (words[0] == "cr2")
                    {
                        tilemap.Add(new TileEntry(wall_rightcorner_2, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }
                    else if (words[0] == "gr")
                    {
                        tilemap.Add(new TileEntry(ground, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }
                    else if (words[0] == "bgrnd")
                    {
                        tilemap.Add(new TileEntry(background, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }
                    else if (words[0] == "dl")
                    {
                        tilemap.Add(new TileEntry(ground, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        if (words[1] == "h")
                        {
                            Tile tmp = new Tile(horizontal_door_left, true);
                            if (words.Length > 2)
                            {
                                crossInteractableTiles[0, int.Parse(words[2])] = tmp;
                            }
                            tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                        else if (words[1] == "v")
                        {
                            
                            Tile tmp = new Tile(vertical_door_left, true);
                            if (words.Length > 2)
                            {
                                crossInteractableTiles[0, int.Parse(words[2])] = tmp;
                            }
                            tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                    }
                    else if (words[0] == "dr")
                    {
                        tilemap.Add(new TileEntry(ground, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        if (words[1] == "h")
                        {
                            Tile tmp = new Tile(horizontal_door_right, true);
                            if (words.Length > 2)
                            {
                                crossInteractableTiles[1, int.Parse(words[2])] = tmp;
                            }
                            tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                        else if (words[1] == "v")
                        {
                            Tile tmp = new Tile(vertical_door_right, true);
                            if (words.Length > 2)
                            {
                                crossInteractableTiles[1, int.Parse(words[2])] = tmp;
                            }
                            tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                    }
                    else if (words[0] == "" || words[0] == "  ") {
                        tilemap.Add(new TileEntry(background, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                    }
                    else
                        {
                            tilemap.Add(new TileEntry(ground, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));

                            if (words[0] == "c0")
                            {
                                ChestTile tmp = new ChestTile(chest_small, false, loot_table_chest_small);
                                tmp.SetIsInteractable(ground.texture(), null, chest_open);
                                tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (words[0] == "c1")
                            {
                                ChestTile tmp = new ChestTile(chest_medium, false, loot_table_chest_medium);
                                tmp.SetIsInteractable(ground.texture(), null, chest_open);
                                tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (words[0] == "c2")
                            {
                                ChestTile tmp = new ChestTile(chest_large, false, loot_table_chest_large);
                                tmp.SetIsInteractable(ground.texture(), null, chest_open);
                                tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (words[0] == "pk")
                            {
                                tilemap.Add(new TileEntry(peaks, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (words[0] == "pl")
                            {
                                //Position the player directly without checking collisions when loading the game   
                                player.position = new Vector2(targetTextureResolution * y, targetTextureResolution * x);
                            }
                        }
                }
            }

            //Link cross interactables
            for(int i = 0; i < 20; i++)
            {
                if (crossInteractableTiles[0,i] != null && crossInteractableTiles[1,i] != null) {
                    crossInteractableTiles[0, i].SetIsInteractable(ground.texture(), crossInteractableTiles[1,i], door_open);
                    crossInteractableTiles[1, i].SetIsInteractable(ground.texture(), crossInteractableTiles[0, i], door_open);
                    //0,1 gold,6
                    if (i == 0 || i == 6)
                    {
                        crossInteractableTiles[0, i].SetIsLocked(silver_key);
                        crossInteractableTiles[1, i].SetIsLocked(silver_key);
                    } else if (i == 1)
                    {
                        crossInteractableTiles[0, i].SetIsLocked(golden_key);
                        crossInteractableTiles[1, i].SetIsLocked(golden_key);
                    } else if (i == 7)
                    {
                        crossInteractableTiles[0, i].SetIsLocked(diamond_key);
                        crossInteractableTiles[1, i].SetIsLocked(diamond_key);
                    }
                }
                else
                {
                    if(crossInteractableTiles[0, i] != null)
                    {
                        crossInteractableTiles[0, i].SetIsInteractable(ground.texture(), null, door_open);
                        crossInteractableTiles[0, i].SetIsLocked(diamond_key);
                    }
                    else if (crossInteractableTiles[1, i] != null)
                    {
                        crossInteractableTiles[1, i].SetIsInteractable(ground.texture(), null, door_open);
                        crossInteractableTiles[1, i].SetIsLocked(diamond_key);
                    }
                }
            }

            cameraPos = new Vector3(player.position.X, player.position.Y, 0);
            worldCamera = new Camera(viewport);

            collider_map = new List<Rectangle>();
            interactable_map = new List<TileEntry>();
            damage_tile_map= new List<TileEntry>();
            //generate collision map and interactable map
            foreach(TileEntry item in tilemap)
            {
                if (item.tile.getHasCollision())
                {
                    collider_map.Add(item.boundingBox);
                }
                if(item.tile.getIsInteractable())
                {
                    interactable_map.Add(item);
                }
                if (item.tile.getDoesDamage())
                {
                    damage_tile_map.Add(item);
                }
            }

            oldPlayerPosition = player.position;

            //deahtscreen
            deathscreen_wallpaper = Content.Load<Texture2D>(@"OurContent\Utility\you_died");
            deathscreen_sound = Content.Load<Song>(@"OurContent\Audio\SoundEffects\you_died_soundeffect");

            //Casted spells
            casted_spells = new List<Spell>();
            //fireball_texture = Content.Load<Texture2D>(@"OurContent\Spells\Flame\flamethrower_2_2");
            fireball_texture = Content.Load<Texture2D>(@"OurContent\Spells\Flame\fireball_test");

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

            currentMouseState = Mouse.GetState();

            if (!otherScreenHasFocus)
            {

            }

            //Interactables
            //Check and remove already interacted interactables
            for (int i = interactable_map.Count - 1; i >= 0; i--)
            {
                if (!interactable_map[i].tile.getIsInteractable())
                {
                    interactable_map.Remove(interactable_map[i]);
                }
            }
            foreach (TileEntry item in interactable_map)
            {
                if (item.boundingBox.Intersects(player.BoundingBox()))
                {
                    debug_ui_interactable_collision = true;
                    interactableNearby = item;
                    break;
                }
                interactableNearby = null;
                debug_ui_interactable_collision = false;
            }
            if (interactable_map.Count == 0)
            {
                interactableNearby = null;
                debug_ui_interactable_collision = false;
            }

            //Damage from world
            foreach (TileEntry item in damage_tile_map)
            {
                if (item.boundingBox.Intersects(player.BoundingBox()))
                {
                    debug_ui_damagingWorld_collision = true;
                    item.tile.attack(gameTime, player);
                    break;
                }
                debug_ui_damagingWorld_collision = false;
            }

            //Check collision with walls
            player.updatePosition(collider_map);


            //UI

            //HealthBar
            if (player.Health() >= 2)
            {
                healthbar_list[0] = heart_full;
                if (player.Health() >= 4)
                {
                    healthbar_list[1] = heart_full;
                    if (player.Health() >= 6)
                    {
                        healthbar_list[2] = heart_full;
                    }
                    else if (player.Health() == 5)
                    {
                        healthbar_list[2] = heart_half;
                    }
                    else
                    {
                        healthbar_list[2] = heart_empty;
                    }
                }
                else if (player.Health() == 3)
                {
                    healthbar_list[1] = heart_half;
                    healthbar_list[2] = heart_empty;
                }
                else
                {
                    healthbar_list[1] = heart_empty;
                    healthbar_list[2] = heart_empty;
                }
            }
            else if (player.Health() == 1)
            {
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

            if (player.Health() <= 0)
            {
                ScreenManager.RemoveScreen(this);
                MediaPlayer.Play(Content.Load<Song>(@"OurContent\Audio\SoundEffects\you_died_soundeffect"));
                ScreenManager.AddScreen(new DeathBackgroundScreen(), 0);
                ScreenManager.AddScreen(new DeathScreen(), 0);
            }

            cameraPos.X = (player.position.X - 580) * -1;
            cameraPos.Y = (player.position.Y - 260) * -1;

            //Update casted spells
            for (int i = casted_spells.Count - 1; i >= 0; i--)
            {
                casted_spells[i].Update(gameTime);
                if (Vector2.Distance(casted_spells[i].Position, casted_spells[i].originPosition) > maxDistanceOfCastedSpell)
                {
                    casted_spells.Remove(casted_spells[i]);
                }
            }
            player.Update(gameTime);
            peaks.Update(gameTime);

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

            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.

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
                        player.inventory.AddItem(interactableNearby.tile.Interact(player.inventory));
                    }
                }

                //Cast fireball
                if(currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
                {
                    // Get the mouse position in world coordinates
                    Vector2 mousePosition = new Vector2(Mouse.GetState().X - cameraPos.X, Mouse.GetState().Y - cameraPos.Y);

                    // Get the direction from the player to the mouse
                    Vector2 fireballDirection = new Vector2(mousePosition.X - player.position.X, mousePosition.Y - player.position.Y);
                    fireballDirection.Normalize();

                    float rotation = (float)Math.Atan2(fireballDirection.X, fireballDirection.Y);
                    //Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);

                    // Create a new fireball at the player's position
                    //fireball = new Fireball(fireball_texture, new Vector2(player.PositionX + player.Width/2, player.PositionY + player.Height/4*3), fireballDirection, 10f, rotation, player.Position, mousePosition);
                    fireball = new Fireball("fireball", fireball_texture,0, rotation, 10f);
                    Vector2 originPosition = new Vector2(player.position.X + player.Width() / 2, player.position.Y + player.Height() / 4 * 3);
                    casted_spells.Add(fireball.Cast(originPosition, rotation, fireballDirection, mousePosition, originPosition));

                    // Play the fireball sound
                    //fireballSound.Play();

                    previousMouseState = currentMouseState;
                }
                if(currentMouseState.LeftButton == ButtonState.Released)
                {
                    previousMouseState = currentMouseState;
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
                    _spriteBatch.Draw(background.texture(), new Rectangle(i * targetTextureResolution, y * targetTextureResolution, targetTextureResolution, targetTextureResolution), Color.White);
                }
            }
        }

        private void DrawMap()
        {
            foreach(TileEntry entry in tilemap)
            {
                _spriteBatch.Draw(entry.tile.texture(), new Rectangle((int)entry.drawVector.X, (int) entry.drawVector.Y, 64, 64), Color.White);
            }
        }

        private void DrawEnemy()
        {

        }
        
        private void DrawPlayer()
        {
            _spriteBatch.Draw(player.Texture(), new Rectangle((int)player.position.X, (int)player.position.Y, 64, 112), Color.White);
        }

        private void DrawCastedSpells()
        {
            foreach (Spell item in casted_spells)
            {
                if(item.targetPosition.X < item.originPosition.X)
                {
                    //_spriteBatch.Draw(item.texture, item.Position, new Rectangle(0, 0, targetTextureResolution, targetTextureResolution), Color.White, item.rotation, new Vector2(item.texture.Width / 2, item.texture.Height / 2), 1.0f, SpriteEffects.FlipHorizontally, 0);
                    _spriteBatch.Draw(item.texture, item.Position,null, Color.White, item.rotation, new Vector2(item.texture.Width / 2, item.texture.Height / 2), 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    _spriteBatch.Draw(item.texture, item.Position, null, Color.White, item.rotation, new Vector2(item.texture.Width / 2, item.texture.Height / 2), 1.0f, SpriteEffects.None, 0);
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
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.drawVector.X, (int)item.drawVector.Y, targetTextureResolution, targetTextureResolution), Color.Green);
            }
            foreach (TileEntry item in damage_tile_map)
            {
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.drawVector.X, (int)item.drawVector.Y, targetTextureResolution, targetTextureResolution), Color.Yellow);
            }
            _spriteBatch.DrawString(spriteFont, "player_location: [X:" + player.position.X + ",Y:" + player.position.Y, new Vector2(debug_ui_player_position_vector.X - cameraPos.X, debug_ui_player_position_vector.Y - cameraPos.Y), Color.White);

            _spriteBatch.DrawString(spriteFont, "wall_collision:" + debug_ui_wall_collision, new Vector2(debug_ui_wall_collision_vector.X - cameraPos.X, debug_ui_wall_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "interactable_collision:" + debug_ui_interactable_collision, new Vector2(debug_ui_interactable_collision_vector.X - cameraPos.X, debug_ui_interactable_collision_vector.Y -(int)cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "enemy_collision:" + debug_ui_enemy_collision, new Vector2(debug_ui_enemy_collision_vector.X - cameraPos.X, debug_ui_enemy_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "damagingWorld_collision:" + debug_ui_damagingWorld_collision, new Vector2(debug_ui_damagingWorld_collision_vector.X - cameraPos.X, debug_ui_damagingWorld_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "health: " + player.Health(), new Vector2(debug_ui_testing_value_vector.X - cameraPos.X, debug_ui_testing_value_vector.Y - cameraPos.Y), Color.White);

            _spriteBatch.DrawString(spriteFont, "inventory_0: " + player.inventory.GetItemName(0), new Vector2(debug_ui_inventory_0_vector.X - cameraPos.X, debug_ui_inventory_0_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_1: " + player.inventory.GetItemName(1), new Vector2(debug_ui_inventory_1_vector.X - cameraPos.X, debug_ui_inventory_1_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_2: " + player.inventory.GetItemName(2), new Vector2(debug_ui_inventory_2_vector.X - cameraPos.X, debug_ui_inventory_2_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_3: " + player.inventory.GetItemName(3), new Vector2(debug_ui_inventory_3_vector.X - cameraPos.X, debug_ui_inventory_3_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "velocity: [" + player.GetVelocity().X + "," + player.GetVelocity().Y + "]", new Vector2(debug_ui_player_velocity_vector.X - cameraPos.X, debug_ui_player_velocity_vector.Y - cameraPos.Y), Color.White);

        }
    }
    #endregion
}