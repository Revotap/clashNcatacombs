using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal abstract class Character
    {
        #region Variables
        private String name;
        private String t_class;
        private int healthPoints;
        private int baseDamage;
        //Placeholder for inventory
        //placeholder for weapon
        private Rectangle boundingBox;

        //Not in UML
        private float speed;
        private Texture2D texture;
        private Vector2 position;

        //Neu
        private int width;
        private int height;

        private List<Texture2D> textureList;
        private int nextTexture = 1;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 150;

        private int timeSinceLastAttack = 0;
        private int attackSpeed = 700;

        private SoundEffect damageReceivedSound;
        private SoundEffect attackSound;
        private SoundEffect deathSound;
        
        #endregion

        #region Constructor
        public Character()
        {
        }
        #endregion

        #region Methods
        public abstract void Update(GameTime gameTime);
        public abstract void moveUp();
        public abstract void moveDown();
        public abstract void moveLeft();
        public abstract void moveRight();
        public abstract void attack(GameTime gameTime, Character target);

        public abstract void doDamage(int damage);

        //Getters and Setters
        protected string Name { get => name; set => name = value; }
        protected string T_class { get => t_class; set => t_class = value; }
        public int HealthPoints { get => healthPoints; set => healthPoints = value; }
        protected Rectangle BoundingBox { get => boundingBox; set => boundingBox = value; }
        protected int BoundingBoxX { get => boundingBox.X; set => boundingBox.X = value; }
        protected int BoundingBoxY { get => boundingBox.Y; set => boundingBox.Y = value; }
        protected float Speed { get => speed; set => speed = value; }
        protected Texture2D Texture { get => texture; set => texture = value; }
        public Vector2 Position { get =>  position; set => position = value; }
        public float PositionX { get => position.X; set => position.X = value; }
        public float PositionY { get => position.Y; set => position.Y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public List<Texture2D> TextureList { get => textureList; set => textureList = value; }
        public int NextTexture { get => nextTexture; set => nextTexture = value; }
        public int TimeSinceLastFrame { get => timeSinceLastFrame; set => timeSinceLastFrame = value; }
        public int MillisecondsPerFrame { get => millisecondsPerFrame; set => millisecondsPerFrame = value; }
        public int BaseDamage { get => baseDamage; set => baseDamage = value; }
        public int AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        public int TimeSinceLastAttack { get => timeSinceLastAttack; set => timeSinceLastAttack = value; }
        public SoundEffect DamageReceivedSound { get => damageReceivedSound; set => damageReceivedSound = value; }
        public SoundEffect AttackSound { get => attackSound; set => attackSound = value; }
        public SoundEffect DeathSound { get => deathSound; set => deathSound = value; }
        #endregion

    }
}
