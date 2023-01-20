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
using System.Collections.Generic;
using System;
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
using System.Text.RegularExpressions;

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
        private int prevDirection;

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

        //Decorations walls
        private Texture2D banner;
        private Texture2D chains_1;
        private Texture2D chains_2;
        private Texture2D spider_web_1;
        private Texture2D spider_web_2;
        private Texture2D wall_torch;

        //Sounds
        SoundEffect chest_open;
        SoundEffect door_open;
        Song game_finished;
        SoundEffect step_sound;

        //Camera
        private Vector3 cameraPos;

        //Colliders
        List<TileEntry> collider_map;
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
        private Vector2 healthbar_vector = new Vector2(0, 0);
        private List<Texture2D> healthbar_list;

        private Vector2 ui_interact_string_vector = new Vector2(550, 400);

        private Vector2 ui_inventory_equipted = new Vector2(30, 630);
        private Vector2 ui_inventory_slot_0_vector = new Vector2(354,630);
        private Texture2D ui_inventory_slot_empty;
        private Texture2D ui_inventory_slot_selected;

        private Vector2 ui_inventory_drop_text_vector = new Vector2(1000,660);
        private Vector2 ui_inventory_use_text_vector = new Vector2(1100,660);

        private Vector2 ui_xpbar_vector = new Vector2(85, 77);
        private Vector2 ui_playerLevel_vector = new Vector2(5, 70);
        private Texture2D ui_xpbar_empty;
        private Texture2D ui_xpbar_filler;
        private Texture2D coin_texture;
        private Vector2 ui_gold_symbol_vector = new Vector2(5,90);
        private Vector2 ui_gold_text_vector = new Vector2(60, 120);

        //Debug UI
        private bool debug_mode_active = true;
        Texture2D debug_border;

        private static Vector2 debug_ui_vector = new Vector2(0, 120);
        private static int lineHeight = 30;
        private static Vector2 debug_ui_player_position_vector = new Vector2(debug_ui_vector.X+0, debug_ui_vector.Y+lineHeight);
        private bool debug_ui_wall_collision = false;
        private static Vector2 debug_ui_wall_collision_vector = new Vector2(debug_ui_player_position_vector.X+0, debug_ui_player_position_vector.Y + lineHeight);
        private bool debug_ui_interactable_collision = false;
        private static Vector2 debug_ui_interactable_collision_vector = new Vector2(debug_ui_wall_collision_vector.X + 0, debug_ui_wall_collision_vector.Y + lineHeight);
        private bool debug_ui_enemy_collision = false;
        private static Vector2 debug_ui_enemy_collision_vector = new Vector2(debug_ui_interactable_collision_vector.X + 0, debug_ui_interactable_collision_vector.Y + lineHeight);
        private bool debug_ui_damagingWorld_collision = false;
        private static Vector2 debug_ui_damagingWorld_collision_vector = new Vector2(debug_ui_enemy_collision_vector.X + 0, debug_ui_enemy_collision_vector.Y + lineHeight);

        private static Vector2 debug_ui_inventory_0_vector = new Vector2(debug_ui_damagingWorld_collision_vector.X + 0, debug_ui_damagingWorld_collision_vector.Y + lineHeight);
        private static Vector2 debug_ui_inventory_1_vector = new Vector2(debug_ui_inventory_0_vector.X + 0, debug_ui_inventory_0_vector.Y + lineHeight);
        private static Vector2 debug_ui_inventory_2_vector = new Vector2(debug_ui_inventory_1_vector.X + 0, debug_ui_inventory_1_vector.Y + lineHeight);
        private static Vector2 debug_ui_inventory_3_vector = new Vector2(debug_ui_inventory_2_vector.X + 0, debug_ui_inventory_2_vector.Y + lineHeight);
        private static Vector2 debug_ui_player_velocity_vector = new Vector2(debug_ui_inventory_3_vector.X + 0, debug_ui_inventory_3_vector.Y + lineHeight);
        private static Vector2 debug_ui_player_level_vector = new Vector2(debug_ui_player_velocity_vector.X + 0, debug_ui_player_velocity_vector.Y + lineHeight);
        private static Vector2 debug_ui_player_currentXP_vector = new Vector2(debug_ui_player_level_vector.X + 0, debug_ui_player_level_vector.Y + lineHeight);
        private static Vector2 debug_ui_player_maxXP_vector = new Vector2(debug_ui_player_currentXP_vector.X + 0, debug_ui_player_currentXP_vector.Y + lineHeight);
        private static Vector2 debug_ui_player_equippedItem_vector = new Vector2(debug_ui_player_maxXP_vector.X + 0, debug_ui_player_maxXP_vector.Y + lineHeight);
        private static Vector2 debug_ui_playerHealth_vector = new Vector2(debug_ui_player_equippedItem_vector.X + 0, debug_ui_player_equippedItem_vector.Y + lineHeight);

        private Texture2D debug_ui_beamTexture;
        //Spells
        Spell fireball;
        Texture2D fireball_texture;
        Texture2D iceball_texture;
        List<Spell> casted_spells;
        float maxDistanceOfCastedSpell = 2000;

        //Enemies
        List<Enemy> enemy_map= new List<Enemy>();
        //Loot tables
        List<Item> enemyLootTable_small= new List<Item>();
        List<Item> enemyLootTable_medium = new List<Item>();
        List<Item> enemyLootTable_boss = new List<Item>();
        //Animations
        List<Texture2D> skeleton_animation = new List<Texture2D>();
        List<Texture2D> skull_animation = new List<Texture2D>();
        List<Texture2D> vampire_animation = new List<Texture2D>();
        //Sounds
        SoundEffect skeleton_damageReceivedSound;
        SoundEffect skeleton_deathSound;
        SoundEffect skeleton_attackWithNoWeaponSound;
        SoundEffect skull_damageReceivedSound;
        SoundEffect skull_deathSound;
        SoundEffect skull_attackWithNoWeaponSound;
        SoundEffect vampire_damageReceivedSound;
        SoundEffect vampire_deathSound;
        SoundEffect vampire_attackWithNoWeaponSound;
        SoundEffect spell_sound;
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

            ui_inventory_slot_empty = Content.Load<Texture2D>(@"OurContent\Utility\Inventory\item");
            ui_inventory_slot_selected = Content.Load<Texture2D>(@"OurContent\Utility\Inventory\item_selected");
            debug_ui_beamTexture = Content.Load<Texture2D>(@"OurContent\Utility\beam");
            if (player == null)
            {
                List<Texture2D> animation = new List<Texture2D>();
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_0"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_1"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_2"));
                animation.Add(Content.Load<Texture2D>(@"OurContent\Player\Wizard\wizard_idle_3"));
                player = new Player("Spieler", 6, 64, 112, new Vector2(-200, -200), animation, 4f, 
                    Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Hit_1"),
                    Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Killed"), null, new Inventory(ui_inventory_slot_0_vector, ui_inventory_slot_empty, ui_inventory_slot_selected));
            }

            // Ein SpriteBatch zum Zeichnen
            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            // Viewport speichern
            viewport = ScreenManager.GraphicsDevice.Viewport;

            // Font laden
            spriteFont = Content.Load<SpriteFont>(@"OurContent\Utility\Silver");

            //Sound effects
            SoundEffect.MasterVolume = 0.05f;
            chest_open = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Coins");
            door_open = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\door_wood_open");
            skeleton_damageReceivedSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Hit_2");
            skeleton_attackWithNoWeaponSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\melee_attack");
            skeleton_deathSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Killed");
            skull_damageReceivedSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Hit_2");
            skull_attackWithNoWeaponSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\melee_attack");
            skull_deathSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\skull_explosion");
            vampire_damageReceivedSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Hit_2");
            vampire_attackWithNoWeaponSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\melee_attack");
            vampire_deathSound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\Player_Killed");
            game_finished = Content.Load<Song>(@"OurContent\Audio\SoundEffects\quest_complete");
            spell_sound = Content.Load<SoundEffect>(@"OurContent\Audio\SoundEffects\fireball_cast");


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

            chest_small = Content.Load<Texture2D>(@"OurContent\Map\chest_small");
            chest_medium = Content.Load<Texture2D>(@"OurContent\Map\chest_medium");
            chest_large = Content.Load<Texture2D>(@"OurContent\Map\chest_large");

            peaks = new Tile(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_1"), false);
            List<Texture2D> peak_animation = new List<Texture2D>();
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_1"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_2"));
            peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_3"));
            //peak_animation.Add(Content.Load<Texture2D>(@"OurContent\Map\Peaks\peaks_4"));
            peaks.SetDoesDamage(1, null, 700);
            peaks.SetIsAnimated(peak_animation, 400, null);

            banner = Content.Load<Texture2D>(@"OurContent\Map\banner");
            chains_1 = Content.Load<Texture2D>(@"OurContent\Map\chains_1");
            chains_2 = Content.Load<Texture2D>(@"OurContent\Map\chains_2");
            spider_web_1 = Content.Load<Texture2D>(@"OurContent\Map\spider_web_1");
            spider_web_2 = Content.Load<Texture2D>(@"OurContent\Map\spider_web_2");
            wall_torch = Content.Load<Texture2D>(@"OurContent\Map\wall_torch");

            Vector2 playerStartingPos = new Vector2(0,0);

            //UI
            ui_xpbar_empty = Content.Load<Texture2D>(@"OurContent\Utility\XPBar\xpbar_empty");
            ui_xpbar_filler = Content.Load<Texture2D>(@"OurContent\Utility\XPBar\xpbar_filler");

            //Animations
            skeleton_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skeleton\v1\skeleton_v1_1"));
            skeleton_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skeleton\v1\skeleton_v1_2"));
            skeleton_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skeleton\v1\skeleton_v1_3"));
            skeleton_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skeleton\v1\skeleton_v1_4"));

            skull_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skull\v1\skull_v1_1"));
            skull_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skull\v1\skull_v1_2"));
            skull_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skull\v1\skull_v1_3"));
            skull_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Skull\v1\skull_v1_4"));

            vampire_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Vampire\v1\vampire_v1_1"));
            vampire_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Vampire\v1\vampire_v1_2"));
            vampire_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Vampire\v1\vampire_v1_3"));
            vampire_animation.Add(Content.Load<Texture2D>(@"OurContent\Enemies\Vampire\v1\vampire_v1_4"));

            //Items
            Key silver_key = new Key("Silver Key", 0, Content.Load<Texture2D>(@"OurContent\Map\silver_key"), 16, 1);
            Key golden_key = new Key("Golden Key", 1, Content.Load<Texture2D>(@"OurContent\Map\golden_key"), 16, 1);
            Key diamond_key = new Key("Diamond Key", 2, Content.Load<Texture2D>(@"OurContent\Map\diamond_key"), 16, 1);
            iceball_texture = Content.Load<Texture2D>(@"OurContent\Spells\ice_spell");
            fireball_texture = Content.Load<Texture2D>(@"OurContent\Spells\Flame\fireball_test");
            Texture2D kinetic_ball_texture = Content.Load<Texture2D>(@"OurContent\Spells\kinetic_spell");
            coin_texture = Content.Load<Texture2D>(@"OurContent\Map\coin");
            Spell ice_spell = new Spell("Ice Spell", iceball_texture, 2, 2.0f, 1, 15f, spell_sound);
            Spell fire_spell = new Spell("Fire Spell", fireball_texture, 1, 2.0f, 1,10f, spell_sound);
            Spell kinetic_spell = new Spell("Kinetic Spell", kinetic_ball_texture, 3, 2.0f,2, 20f, spell_sound);
            Item gold_coin = new Item("Coin", coin_texture, 1, 1, 2.0f);
            Item health_potion = new Item("Health Potion", Content.Load<Texture2D>(@"OurContent\Map\health_flask"), 1, 4, 1.0F);

            player.equipItem(fire_spell);

            //Loot tables for chests
            List<Item> loot_table_chest_small = new List<Item>();
            loot_table_chest_small.Add(silver_key);

            List<Item> loot_table_chest_medium = new List<Item>();
            loot_table_chest_medium.Add(ice_spell);

            List<Item> loot_table_chest_large_item = new List<Item>();
            loot_table_chest_large_item.Add(kinetic_spell);

            List<Item> loot_table_chest_large = new List<Item>();
            loot_table_chest_large.Add(golden_key);

            //Loot tables for enemies
            enemyLootTable_small.Add(gold_coin);
            enemyLootTable_small.Add(health_potion);
            enemyLootTable_medium.Add(gold_coin);
            enemyLootTable_medium.Add(health_potion);
            enemyLootTable_boss.Add(diamond_key);

            crossInteractableTiles = new Tile[2, 20];
            
            for(int i = 0; i< 2; i++)
            {
                for(int x = 0; x < 20; x++)
                {
                    crossInteractableTiles[i, x] = null;
                }
            }

            //Generate Map
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    String phrase = map[x, y];
                    String[] words = phrase.Split('_');

                    if (words[0] == "wl")
                    {
                        if (words.Length > 1)
                        {
                            Tile tmp = new Tile(wall_left.texture(), false);
                            tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                        else
                        {
                            tilemap.Add(new TileEntry(wall_left, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                    } else if (words[0] == "wr")
                    {
                        if (words.Length > 1)
                        {
                            Tile tmp = new Tile(wall_right.texture(), false);
                            tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                        else
                        {
                            tilemap.Add(new TileEntry(wall_right, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                    } else if (words[0] == "wt")
                    {
                        tilemap.Add(new TileEntry(wall_top, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));

                        if(random.Next(0,4) == 0)
                        {
                            int tmp = random.Next(0,6);
                            if(tmp == 0)
                            {
                                tilemap.Add(new TileEntry(new Tile(banner, false), new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }else if(tmp == 1)
                            {
                                tilemap.Add(new TileEntry(new Tile(chains_1, false), new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (tmp == 2)
                            {
                                tilemap.Add(new TileEntry(new Tile(chains_2, false), new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (tmp == 3)
                            {
                                tilemap.Add(new TileEntry(new Tile(spider_web_1, false), new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (tmp == 4)
                            {
                                tilemap.Add(new TileEntry(new Tile(spider_web_2, false), new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                            else if (tmp == 5)
                            {
                                tilemap.Add(new TileEntry(new Tile(wall_torch, false), new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                        }
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

                        if(words.Length == 1)
                        {
                            Random random = new Random();
                            if (random.Next(0, 50) <= 9)
                            {
                                int tmp = random.Next(0, 9);
                                if (0 <= tmp && tmp < 2)
                                {
                                    enemy_map.Add(new Enemy("Skull", 3, 64, 112, new Vector2(targetTextureResolution * y, targetTextureResolution * x), skull_animation, 3.5f, skull_damageReceivedSound, skull_deathSound, skull_attackWithNoWeaponSound, enemyLootTable_small, 2));

                                }
                                else if (3 <= tmp && tmp < 6)
                                {
                                    enemy_map.Add(new Enemy("Skeleton", 6, 64, 112, new Vector2(targetTextureResolution * y, targetTextureResolution * x), skeleton_animation, 2.0f, skeleton_damageReceivedSound, skeleton_deathSound, skeleton_attackWithNoWeaponSound, enemyLootTable_medium, 2, fire_spell, 1200));
                                }
                            }
                        }

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
                            if(words.Length > 1)
                            {
                                if (words[1] == "x")
                                {
                                    ChestTile tmp = new ChestTile(chest_large, false, loot_table_chest_large_item);
                                    tmp.SetIsInteractable(ground.texture(), null, chest_open);
                                    tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                                }
                            }
                            else
                            {
                                ChestTile tmp = new ChestTile(chest_large, false, loot_table_chest_large);
                                tmp.SetIsInteractable(ground.texture(), null, chest_open);
                                tilemap.Add(new TileEntry(tmp, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                            }
                        }
                        else if (words[0] == "pk")
                        {
                            tilemap.Add(new TileEntry(peaks, new Vector2(targetTextureResolution * y, targetTextureResolution * x), 64));
                        }
                        else if (words[0] == "pl")
                        {
                            //Position the player directly without checking collisions when loading the game   
                            player.position = new Vector2(targetTextureResolution * y, targetTextureResolution * x);
                        }else if (words[0] == "boss")
                        {
                            enemy_map.Add(new Enemy("Vampire", 50, 128, 224, new Vector2(targetTextureResolution * y, targetTextureResolution * x), vampire_animation, 1.0f, vampire_damageReceivedSound, vampire_deathSound, vampire_attackWithNoWeaponSound, enemyLootTable_boss, 2, fire_spell));
                        }
                    }
                }
            }

            //Link cross interactables
            for(int i = 0; i < 20; i++)
            {
                if (crossInteractableTiles[0,i] != null && crossInteractableTiles[1,i] != null) {
                        crossInteractableTiles[0, i].SetIsInteractable(ground.texture(), crossInteractableTiles[1, i], door_open);
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
                        crossInteractableTiles[0, i].SetIsInteractable(ground.texture(), null, game_finished, false);
                        crossInteractableTiles[0, i].SetIsLocked(diamond_key);
                    }
                    else if (crossInteractableTiles[1, i] != null)
                    {
                        crossInteractableTiles[1, i].SetIsInteractable(ground.texture(), null, game_finished, false);
                        crossInteractableTiles[1, i].SetIsLocked(diamond_key);
                    }
                }
            }

            cameraPos = new Vector3(player.position.X, player.position.Y, 0);

            collider_map = new List<TileEntry>();
            interactable_map = new List<TileEntry>();
            damage_tile_map= new List<TileEntry>();

            //generate collision map and interactable map
            foreach(TileEntry item in tilemap)
            {
                if (item.tile.getHasCollision())
                {
                    collider_map.Add(item);
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
            //fireball_texture = Content.Load<Texture2D>(@"OurContent\Spells\Flame\fireball_test");

            //UI
            heart_empty = Content.Load<Texture2D>(@"OurContent\Utility\Heart\heart_empty");
            heart_half = Content.Load<Texture2D>(@"OurContent\Utility\Heart\heart_half");
            heart_full = Content.Load<Texture2D>(@"OurContent\Utility\Heart\heart_full");
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
            currentMouseState = Mouse.GetState();

            if (!otherScreenHasFocus)
            {

            }

            //Check if collision map has changed
            for(int i = collider_map.Count - 1; i >= 0; i--)
            {
                if (!collider_map[i].tile.getHasCollision())
                {
                    collider_map.Remove(collider_map[i]);
                }
            }


            //Interactables
            //Check and remove already interacted interactables
            for (int i = interactable_map.Count - 1; i >= 0; i--)
            {
                if (!interactable_map[i].tile.getIsInteractable())
                {
                    if (interactable_map[i].tile.hasInteractionSong())
                    {
                        ScreenManager.RemoveScreen(this);
                        MediaPlayer.Volume = 0.1F;
                        MediaPlayer.Play(Content.Load<Song>(@"OurContent\Audio\SoundEffects\quest_complete"));
                        ScreenManager.AddScreen(new GameFinishedBackgroundScreen(), 0);
                        ScreenManager.AddScreen(new GameFinishedScreen(), 0);
                    }
                    interactable_map.Remove(interactable_map[i]);
                }
            }
            foreach (TileEntry item in interactable_map)
            {
                //Increase size of player boundingBox temporarily to check for interaction
                if (item.boundingBox.Intersects(new Rectangle(player.BoundingBox().X - 20, player.BoundingBox().Y - 20, player.BoundingBox().Width + 40, player.BoundingBox().Height + 40)))
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
                else
                {
                    foreach(TileEntry entry in collider_map)
                    {
                        if (entry.hit(casted_spells[i].Position))
                        {
                            casted_spells.Remove(casted_spells[i]);
                            break;
                        }
                    }
                    if(i < casted_spells.Count)
                    {
                        if (casted_spells[i].caster.GetType() == typeof(Player))
                        {
                            for (int x = enemy_map.Count - 1; x >= 0; x--)
                            {
                                if (enemy_map[x].hit(casted_spells[i].Position))
                                {
                                    enemy_map[x].receiveDamage(player, player.EquiptedItem().value);
                                    casted_spells.Remove(casted_spells[i]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (player.hit(casted_spells[i].Position))
                            {
                                player.receiveDamage(casted_spells[i].caster, casted_spells[i].value);
                                casted_spells.Remove(casted_spells[i]);
                            }
                        }
                    }
                }
            }

            //Update enemies´and check enemie collision with player
            for (int x = enemy_map.Count - 1; x >= 0; x--)
            {
                /*int tmpMove = enemy_map[x].getNextMove();
                if (tmpMove == 0)
                {
                    enemy_map[x].moveUp();
                }else if (tmpMove == 1)
                {
                    enemy_map[x].moveRight();
                }
                else if(tmpMove == 2)
                {
                    enemy_map[x].moveDown();
                }
                else if(tmpMove == 3)
                {
                    enemy_map[x].moveLeft();
                }*/

                /*if (LineIntersectsRectangle(enemy_map[x].position, player.position, player.BoundingBox()))
                {
                    throw new Exception("true");
                }*/

                Vector2 tmpPlayerCenter = new Vector2(player.position.X, player.position.Y);
                Rectangle newBox = new Rectangle((int)tmpPlayerCenter.X - player.Width()*4, (int)tmpPlayerCenter.Y - player.Height()*4, player.Width()*8, player.Height()*8);
                if (enemy_map[x].BoundingBox().Intersects(newBox))
                {
                    if (enemy_map[x].equiptedItem != null)
                    {
                        enemy_map[x].attackWithSpell(gameTime, player, cameraPos, casted_spells);
                    }

                    Vector2 tmpPosition = Vector2.Lerp(enemy_map[x].position, player.position, enemy_map[x].movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (tmpPosition.X < enemy_map[x].position.X)
                    {
                        enemy_map[x].moveLeft();
                    }
                    else if (tmpPosition.X > enemy_map[x].position.X)
                    {
                        enemy_map[x].moveRight();
                    }
                    if (tmpPosition.Y > enemy_map[x].position.Y)
                    {
                        enemy_map[x].moveDown();
                    }
                    else if (tmpPosition.Y < enemy_map[x].position.Y)
                    {
                        enemy_map[x].moveUp();
                    }
                }

                enemy_map[x].updatePosition(collider_map);
                enemy_map[x].Update(gameTime);

                if (enemy_map[x].BoundingBox().Intersects(player.BoundingBox()))
                {
                    enemy_map[x].attack(gameTime, player);
                    player.calculatePushBack(enemy_map[x].BoundingBox(), collider_map);
                }

                if (enemy_map[x].Health() <= 0)
                {
                    Item loot = enemy_map[x].dropItem();
                    List<Item> item = new List<Item>();
                    item.Add(loot);
                    ChestTile tmp_tile = new ChestTile(loot.texture, false, item);
                    tmp_tile.SetIsInteractable(ground.texture(), null, chest_open);
                    TileEntry tmp = new TileEntry(tmp_tile, enemy_map[x].position, targetTextureResolution);
                    interactable_map.Add(tmp) ;
                    enemy_map.Remove(enemy_map[x]);
                }
            }

            //Check collision with walls
            player.updatePosition(collider_map);

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

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    player.moveLeft();
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    player.moveRight();
                }
                
                if(keyboardState.IsKeyDown(Keys.W))
                {
                    player.moveUp();
                }

                if (keyboardState.IsKeyDown(Keys.S))
                {
                    player.moveDown();
                }

                if (keyboardState.IsKeyDown(Keys.D1))
                {
                    player.inventory.selectItemNumber(0);
                }
                if (keyboardState.IsKeyDown(Keys.D2))
                {
                    player.inventory.selectItemNumber(1);
                }
                if (keyboardState.IsKeyDown(Keys.D3))
                {
                    player.inventory.selectItemNumber(2);
                }
                if (keyboardState.IsKeyDown(Keys.D4))
                {
                    player.inventory.selectItemNumber(3);
                }
                if (keyboardState.IsKeyDown(Keys.D5))
                {
                    player.inventory.selectItemNumber(4);
                }
                if (keyboardState.IsKeyDown(Keys.D6))
                {
                    player.inventory.selectItemNumber(5);
                }
                if (keyboardState.IsKeyDown(Keys.D7))
                {
                    player.inventory.selectItemNumber(6);
                }
                if (keyboardState.IsKeyDown(Keys.D8))
                {
                    player.inventory.selectItemNumber(7);
                }

                if(keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                {
                    player.inventory.equipSelectedItem(player);
                }

                if (keyboardState.IsKeyDown(Keys.Q) && previousKeyboardState.IsKeyUp(Keys.Q))
                {
                    Item loot = player.inventory.dropSelectedItem();

                    if(loot != null)
                    {
                        List<Item> item = new List<Item>();
                        item.Add(loot);
                        ChestTile tmp_tile = new ChestTile(loot.texture, false, item);
                        tmp_tile.SetIsInteractable(ground.texture(), null, chest_open);
                        TileEntry tmp = new TileEntry(tmp_tile, player.position, targetTextureResolution);
                        interactable_map.Add(tmp);
                    }
                }

                if (keyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E))
                {
                    if(interactableNearby != null)
                    {
                        Item loot = interactableNearby.tile.Interact(player.inventory);

                        if(loot != null)
                        {
                            if (!player.inventory.invenotryFull() || loot.name == "Coin")
                            {
                                player.inventory.AddItem(loot);
                                interactableNearby.tile.PlayInteractionSound();
                            }
                            else
                            {
                                List<Item> item = new List<Item>();
                                item.Add(loot);
                                ChestTile tmp_tile = new ChestTile(loot.texture, false, item);
                                tmp_tile.SetIsInteractable(ground.texture(), null, null);
                                TileEntry tmp = new TileEntry(tmp_tile, new Vector2(interactableNearby.boundingBox.X, interactableNearby.boundingBox.Y), targetTextureResolution);
                                interactable_map.Add(tmp);
                            }
                        }
                        else
                        {
                            if (interactableNearby.tile.hasInteractionSong())
                            {
                                interactableNearby.tile.PlayInteractionSong();
                            }
                            else
                            {
                                interactableNearby.tile.PlayInteractionSound();
                            }
                        }
                    }
                    previousKeyboardState = keyboardState;
                }

                //Cast fireball
                if(currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
                {
                    // Get the mouse position in world coordinates
                    Vector2 mousePosition = new Vector2(Mouse.GetState().X - cameraPos.X, Mouse.GetState().Y - cameraPos.Y);
                    Vector2 playerPositonForCast = new Vector2(player.position.X + player.Width() / 2, player.position.Y + player.Height() / 4 * 3);
                    // Get the direction from the player to the mouse
                    Vector2 spellDirection = new Vector2(mousePosition.X - playerPositonForCast.X, mousePosition.Y - playerPositonForCast.Y);
                    spellDirection.Normalize();

                    float rotation = (float)Math.Atan2(spellDirection.X, spellDirection.Y);

                    // Create a new spell at the player's position
                    Spell tmpSpell = (Spell) player.EquiptedItem();
                    Spell spell = new Spell(player.EquiptedItem().name, player.EquiptedItem().texture, player.EquiptedItem().rarity, rotation, player.EquiptedItem().value, player.EquiptedItem().Speed + (player.getLevel() * 2), player, tmpSpell.sound);
                    casted_spells.Add(spell.Cast(playerPositonForCast, rotation, spellDirection, mousePosition, playerPositonForCast));

                    // Play the fireball sound
                    //fireballSound.Play();

                    if(player.equiptedItem.name == "Kinetic Spell")
                    {
                        //LEFT
                        // Get the direction from the player to the mouse
                        //double angle = 45;
                        //Vector2 spellDirectionLeft = new Vector2(spellDirection.X * (int)Math.Sin(angle), spellDirection.Y * (int)Math.Cos(angle));
                        spellDirection = new Vector2(mousePosition.X - playerPositonForCast.X + 100, mousePosition.Y - playerPositonForCast.Y + 100);
                        spellDirection.Normalize();

                        rotation = (float)Math.Atan2(spellDirection.X, spellDirection.Y);

                        // Create a new spell at the player's position
                        spell = new Spell(player.EquiptedItem().name, player.EquiptedItem().texture, player.EquiptedItem().rarity, rotation, player.EquiptedItem().value, player.EquiptedItem().Speed + (player.getLevel() * 2), player, tmpSpell.sound);
                        casted_spells.Add(spell.Cast(playerPositonForCast, rotation, spellDirection, mousePosition, playerPositonForCast));

                        //RIGHT
                        // Get the direction from the player to the mouse
                        spellDirection = new Vector2(mousePosition.X - playerPositonForCast.X - 100, mousePosition.Y - playerPositonForCast.Y - 100);
                        spellDirection.Normalize();

                        rotation = (float)Math.Atan2(spellDirection.X, spellDirection.Y);

                        // Create a new spell at the player's position
                        spell = new Spell(player.EquiptedItem().name, player.EquiptedItem().texture, player.EquiptedItem().rarity, rotation, player.EquiptedItem().value, player.EquiptedItem().Speed + (player.getLevel() * 2), player, tmpSpell.sound);
                        casted_spells.Add(spell.Cast(playerPositonForCast, rotation, spellDirection, mousePosition, playerPositonForCast));
                    }

                    previousMouseState = currentMouseState;
                }
                if(currentMouseState.LeftButton == ButtonState.Released)
                {
                    previousMouseState = currentMouseState;
                }
                previousKeyboardState = keyboardState;
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

            if(debug_mode_active)
            {
                DrawDebugUI();
            }

            DrawUi();

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

            foreach (TileEntry entry in interactable_map)
            {
                _spriteBatch.Draw(entry.tile.texture(), new Rectangle((int)entry.drawVector.X, (int)entry.drawVector.Y, 64, 64), Color.White);
            }
        }

        private void DrawEnemy()
        {
            foreach(Enemy item in enemy_map)
            {
                _spriteBatch.Draw(item.Texture(), new Rectangle((int)item.position.X, (int)item.position.Y, item.Width(), item.Height()), Color.White);
            }
        }
        
        private void DrawPlayer()
        {
            if((player.GetVelocity().X < 0))
            {
                //invert player texture here
                //_spriteBatch.Draw(player.Texture(), player.position, new Rectangle(0, 0, 64, 112), Color.White, 1.0f, Vector2.Zero, 1.0F, SpriteEffects.FlipVertically, 0); ;
                _spriteBatch.Draw(player.Texture(), new Rectangle((int)player.position.X, (int)player.position.Y, 64, 112), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                prevDirection = (int) player.GetVelocity().X;
            }
            else if(player.GetVelocity().X > 0)
            {
                _spriteBatch.Draw(player.Texture(), new Rectangle((int)player.position.X, (int)player.position.Y, 64, 112), Color.White);
                prevDirection = (int)player.GetVelocity().X;
            }
            else
            {
                if(prevDirection < 0)
                {
                    _spriteBatch.Draw(player.Texture(), new Rectangle((int)player.position.X, (int)player.position.Y, 64, 112), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    _spriteBatch.Draw(player.Texture(), new Rectangle((int)player.position.X, (int)player.position.Y, 64, 112), Color.White);
                }
            }
        }

        private void DrawCastedSpells()
        {
            foreach (Spell item in casted_spells)
            {
                if(item.targetPosition.X < item.originPosition.X)
                {
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

            player.inventory.Draw(_spriteBatch,spriteFont, cameraPos, targetTextureResolution);

            _spriteBatch.DrawString(spriteFont, "Equipped", new Vector2((int)ui_inventory_equipted.X - (int)cameraPos.X - 10, (int)ui_inventory_equipted.Y - (int)cameraPos.Y - 30), Color.White);
            _spriteBatch.Draw(ui_inventory_slot_empty, new Rectangle((int)ui_inventory_equipted.X - (int)cameraPos.X, (int)ui_inventory_equipted.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
            if(player.EquiptedItem() != null)
            {
                _spriteBatch.Draw(player.EquiptedItem().texture, new Rectangle((int)ui_inventory_equipted.X - (int)cameraPos.X, (int)ui_inventory_equipted.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
            }
            if (interactableNearby != null)
            {
                if(interactableNearby.tile.getRequiredItem() != null)
                {
                    _spriteBatch.DrawString(spriteFont, "Interact [E][" + interactableNearby.tile.getRequiredItem().name + "]", new Vector2(ui_interact_string_vector.X - cameraPos.X, ui_interact_string_vector.Y - cameraPos.Y), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(spriteFont, "Interact [E]", new Vector2(ui_interact_string_vector.X - cameraPos.X, ui_interact_string_vector.Y - cameraPos.Y), Color.White);
                }
            }
            _spriteBatch.DrawString(spriteFont, "Drop [Q]", new Vector2(ui_inventory_drop_text_vector.X - cameraPos.X, ui_inventory_drop_text_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "Use/Equipt [Space]", new Vector2(ui_inventory_use_text_vector.X - cameraPos.X, ui_inventory_use_text_vector.Y - cameraPos.Y), Color.White);

            //Xp Bar
            _spriteBatch.DrawString(spriteFont, "Level: " + player.getLevel(), new Vector2(ui_playerLevel_vector.X - cameraPos.X, ui_playerLevel_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.Draw(ui_xpbar_empty, new Rectangle((int)ui_xpbar_vector.X - (int)cameraPos.X, (int)ui_xpbar_vector.Y - (int)cameraPos.Y, 128, 16), Color.White);

            float tmpWidth = (player.getCurrentXP() + 0.01F) / player.getMaxXForCurrentLevel();
            _spriteBatch.Draw(ui_xpbar_filler, new Rectangle((int)ui_xpbar_vector.X - (int)cameraPos.X, (int)ui_xpbar_vector.Y - (int)cameraPos.Y, (int) Math.Floor(128 * tmpWidth), 16), Color.White);
            //Gold
            _spriteBatch.Draw(coin_texture, new Rectangle((int) ui_gold_symbol_vector.X - (int)cameraPos.X, (int) ui_gold_symbol_vector.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
            _spriteBatch.DrawString(spriteFont, player.inventory.getGold().ToString(), new Vector2((int)ui_gold_text_vector.X - (int)cameraPos.X, (int)ui_gold_text_vector.Y - (int)cameraPos.Y), Color.White);

            foreach(Enemy enemy in enemy_map)
            {
                float tmpHealth = (enemy.Health() + 0.01F) / enemy.maxHealth;
                _spriteBatch.Draw(ui_xpbar_empty, new Rectangle((int)enemy.position.X, (int)enemy.position.Y - 10, enemy.Width(), 16), Color.White);
                _spriteBatch.Draw(ui_xpbar_filler, new Rectangle((int)enemy.position.X, (int)enemy.position.Y - 10, (int)Math.Floor(enemy.Width() * tmpHealth), 16), Color.White);
                
            }
        }

        private void DrawDebugUI()
        {
            _spriteBatch.Draw(debug_border, new Rectangle(player.BoundingBox().X - 20, player.BoundingBox().Y - 20, player.BoundingBox().Width + 40, player.BoundingBox().Height + 40), Color.Purple);

            foreach(TileEntry item in collider_map)
            {
                _spriteBatch.Draw(debug_border, item.boundingBox, Color.White);
            }

            foreach(TileEntry item in interactable_map)
            {
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.drawVector.X, (int)item.drawVector.Y, targetTextureResolution, targetTextureResolution), Color.Green);
            }
            foreach (TileEntry item in damage_tile_map)
            {
                _spriteBatch.Draw(debug_border, new Rectangle((int)item.drawVector.X, (int)item.drawVector.Y, targetTextureResolution, targetTextureResolution), Color.Yellow);
            }
            foreach(Enemy enemy in enemy_map)
            {
                _spriteBatch.DrawString(spriteFont, enemy.Health().ToString(), new Vector2(enemy.position.X, enemy.position.Y - 40), Color.Red);
            }

            _spriteBatch.DrawString(spriteFont, "player_location: [X:" + player.position.X + ",Y:" + player.position.Y, new Vector2(debug_ui_player_position_vector.X - cameraPos.X, debug_ui_player_position_vector.Y - cameraPos.Y), Color.White);

            _spriteBatch.DrawString(spriteFont, "wall_collision:" + debug_ui_wall_collision, new Vector2(debug_ui_wall_collision_vector.X - cameraPos.X, debug_ui_wall_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "interactable_collision:" + debug_ui_interactable_collision, new Vector2(debug_ui_interactable_collision_vector.X - cameraPos.X, debug_ui_interactable_collision_vector.Y -(int)cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "enemy_collision:" + debug_ui_enemy_collision, new Vector2(debug_ui_enemy_collision_vector.X - cameraPos.X, debug_ui_enemy_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "damagingWorld_collision:" + debug_ui_damagingWorld_collision, new Vector2(debug_ui_damagingWorld_collision_vector.X - cameraPos.X, debug_ui_damagingWorld_collision_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "health: " + player.Health(), new Vector2(debug_ui_playerHealth_vector.X - cameraPos.X, debug_ui_playerHealth_vector.Y - cameraPos.Y), Color.White);

            _spriteBatch.DrawString(spriteFont, "inventory_0: " + player.inventory.GetItemName(0), new Vector2(debug_ui_inventory_0_vector.X - cameraPos.X, debug_ui_inventory_0_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_1: " + player.inventory.GetItemName(1), new Vector2(debug_ui_inventory_1_vector.X - cameraPos.X, debug_ui_inventory_1_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_2: " + player.inventory.GetItemName(2), new Vector2(debug_ui_inventory_2_vector.X - cameraPos.X, debug_ui_inventory_2_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "inventory_3: " + player.inventory.GetItemName(3), new Vector2(debug_ui_inventory_3_vector.X - cameraPos.X, debug_ui_inventory_3_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "velocity: [" + player.GetVelocity().X + "," + player.GetVelocity().Y + "]", new Vector2(debug_ui_player_velocity_vector.X - cameraPos.X, debug_ui_player_velocity_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "level: " + player.getLevel(), new Vector2(debug_ui_player_level_vector.X - cameraPos.X, debug_ui_player_level_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "current_xp: " + player.getCurrentXP(), new Vector2(debug_ui_player_currentXP_vector.X - cameraPos.X, debug_ui_player_currentXP_vector.Y - cameraPos.Y), Color.White);
            _spriteBatch.DrawString(spriteFont, "max_xp: " + player.getMaxXForCurrentLevel(), new Vector2(debug_ui_player_maxXP_vector.X - cameraPos.X, debug_ui_player_maxXP_vector.Y - cameraPos.Y), Color.White);
            if(player.EquiptedItem() != null)
            {
                _spriteBatch.DrawString(spriteFont, "equipped_item: " + player.EquiptedItem().name, new Vector2(debug_ui_player_equippedItem_vector.X - cameraPos.X, debug_ui_player_equippedItem_vector.Y - cameraPos.Y), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(spriteFont, "equipped_item: none", new Vector2(debug_ui_player_equippedItem_vector.X - cameraPos.X, debug_ui_player_equippedItem_vector.Y - cameraPos.Y), Color.White);
            }

            //Drawing of casted spell lines is not working
            /*foreach(Spell spell in casted_spells)
            {
                Vector2 casterPos = new Vector2(spell.caster.position.X + spell.caster.Width()/2, spell.caster.position.Y + spell.caster.Height()/2);
                DrawLine(debug_ui_beamTexture, casterPos, spell.Direction * 100, Color.White);
            }*/

            DrawLine(debug_ui_beamTexture, new Vector2(player.position.X + player.Width() / 2 + 10, player.position.Y + player.Height() / 2 + 20), new Vector2(Mouse.GetState().Position.X - cameraPos.X, Mouse.GetState().Position.Y - cameraPos.Y), Color.Blue);
            
            /*foreach(Enemy enemy in enemy_map)
            {
                DrawLine(debug_ui_beamTexture, new Vector2(enemy.position.X + enemy.Width()/2 + 10, enemy.position.Y + enemy.Height()/2 + 20), new Vector2(player.position.X + player.Width()/2 + 10, player.position.Y + player.Height() / 2 + 20), Color.Red);
            }*/
        }

        public void DrawLine(Texture2D texture, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(distance, thickness);

            _spriteBatch.Draw(texture, point1, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

        public static bool IsVectorOnLine(Vector2 start, Vector2 end, Vector2 check)
        {
            float dot = Vector2.Dot(check - start, end - start);
            if (dot > 0 && dot < Vector2.Dot(end - start, end - start))
            {
                var collinear = (check.X - start.X) * (end.Y - start.Y) - (check.Y - start.Y) * (end.X - start.X);
                return Math.Abs(collinear) < float.Epsilon;
            }
            return false;
        }

        public static bool LineIntersectsRectangle(Vector2 lineStart, Vector2 lineEnd, Rectangle rectangle)
        {
            Vector2 lineDirection = lineEnd - lineStart;
            Vector2 lineNormal = new Vector2(-lineDirection.Y, lineDirection.X);

            // Check if the line is parallel to the rectangle
            float dot = Vector2.Dot(lineNormal, new Vector2(rectangle.X, rectangle.Y) - lineStart);
            if (Math.Abs(dot) < float.Epsilon)
            {
                return false;
            }

            // Check if the line intersects with any of the rectangle's edges
            Vector2[] rectanglePoints = new Vector2[]
            {
            new Vector2(rectangle.X, rectangle.Y),
            new Vector2(rectangle.X + rectangle.Width, rectangle.Y),
            new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
            new Vector2(rectangle.X, rectangle.Y + rectangle.Height)
            };

            Vector2 intersectionPoint = Vector2.Zero;
            for (int i = 0; i < 4; i++)
            {
                Vector2 rectangleEdgeStart = rectanglePoints[i];
                Vector2 rectangleEdgeEnd = rectanglePoints[(i + 1) % 4];

                if (LineIntersection(lineStart, lineEnd, rectangleEdgeStart, rectangleEdgeEnd, out intersectionPoint))
                {
                    return true;
                }
            }

            // If the line doesn't intersect with any of the rectangle's edges, check if the line is inside the rectangle
            dot = Vector2.Dot(lineDirection, rectanglePoints[1] - lineStart);
            if (dot > 0)
            {
                Vector2 nearestPoint = Vector2.Zero;
                float distance = float.MaxValue;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 point = rectanglePoints[i];
                    float tempDistance = Vector2.DistanceSquared(lineStart, point);
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        nearestPoint = point;
                    }
                }

                dot = Vector2.Dot(lineDirection, nearestPoint - lineStart);
                if (dot > 0 && dot < Vector2.Dot(lineDirection, lineDirection))
                {
                    return rectangle.Contains((int)nearestPoint.X, (int)nearestPoint.Y);
                }
            }

            return false;
        }
        public static bool LineIntersectsRectangle2(Vector2 lineStart, Vector2 lineEnd, Rectangle rectangle)
        {
            Vector2[] lines = new Vector2[4]
            {
            new Vector2(rectangle.X, rectangle.Y),
            new Vector2(rectangle.Right, rectangle.Y),
            new Vector2(rectangle.Right, rectangle.Bottom),
            new Vector2(rectangle.X, rectangle.Bottom)
            };

            Vector2[] directions = new Vector2[4]
            {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1)
            };

            for (int i = 0; i < 4; i++)
            {
                Vector2 intersection;
                if (LineIntersection(lineStart, lineEnd, lines[i], lines[i] + directions[i], out intersection))
                {
                    if (rectangle.Contains((int)intersection.X, (int)intersection.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool LineIntersectsRectangle3(Vector2 lineStart, Vector2 lineEnd, Rectangle rectangle)
        {
            if (rectangle.Contains((int)lineStart.X, (int)lineStart.Y) || rectangle.Contains((int)lineEnd.X, (int)lineEnd.Y))
            {
                return true;
            }

            //throw new Exception(lineStart.ToString() +"," +lineEnd.ToString() +","+ rectangle.X + "," + rectangle.Y + "," + rectangle.Width +", " + rectangle.Height );

            Vector2[] lines = new Vector2[4]
            {
            new Vector2(rectangle.X, rectangle.Y),//5,-5
            new Vector2(rectangle.Right, rectangle.Y), //15,-5
            new Vector2(rectangle.Right, rectangle.Bottom),
            new Vector2(rectangle.X, rectangle.Bottom)
            };

            //throw new Exception();

            Vector2[] directions = new Vector2[4]
            {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1)
            };

            for (int i = 0; i < 4; i++)
            {
                Vector2 intersection;
                if (LineIntersection(lineStart, lineEnd, lines[i], lines[i] + directions[i], out intersection))
                {
                    throw new Exception();
                    if (rectangle.Contains((int)intersection.X, (int)intersection.Y))
                    {
                        return true;
                    }
                }
                throw new Exception();
            }
            return false;
        }
        public static bool LineIntersectsRectangle4(Vector2 lineStart, Vector2 lineEnd, Rectangle rectangle)
        {
            Vector2[] lines = new Vector2[4]
            {
            new Vector2(rectangle.X, rectangle.Y),
            new Vector2(rectangle.Right, rectangle.Y),
            new Vector2(rectangle.Right, rectangle.Bottom),
            new Vector2(rectangle.X, rectangle.Bottom)
            };

            Vector2[] directions = new Vector2[4]
            {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1)
            };

            for (int i = 0; i < 4; i++)
            {
                Vector2 intersection;
                if(LineIntersection(lineStart, lineEnd, lines[i], lines[i] + directions[i], out intersection))
            {
                    return true;
                }
            }
            return false;
        }

        private static bool LineIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out Vector2 intersection)
        {
            intersection = Vector2.Zero;
            float denominator = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);
            if (Math.Abs(denominator) < float.Epsilon)
            {
                throw new Exception();
                return false;
            }

            float u = ((D.X - C.X) * (A.Y - C.Y) - (D.Y - C.Y) * (A.X - C.X)) / denominator;
            float v = ((B.X - A.X) * (A.Y - C.Y) - (B.Y - A.Y) * (A.X - C.X)) / denominator;
            if (u >= 0 && u <= 1 && v >= 0 && v <= 1)
            {
                intersection = A + u * (B - A);
                return true;
            }
            throw new Exception();
            return false;
        }
    }
    #endregion
}
