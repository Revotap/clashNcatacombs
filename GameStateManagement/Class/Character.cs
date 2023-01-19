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
        protected String name;
        protected int health;
        protected int baseDamage = 1;
        protected Rectangle boundingBox;
        protected float movementSpeed = 2f;
        protected Vector2 velocity { get; set; } = Vector2.Zero;
        public Vector2 position { get; set; }
        protected int width;
        protected int height;

        protected List<Texture2D> textures;
        protected int nextTexture;
        protected int timeSinceLastFrame = 0;
        protected int frameSpeed = 150;

        protected int timeSinceLastAttack = 0;
        protected int attackSpeed = 700;

        protected SoundEffect damageReceivedSound;
        protected SoundEffect attackWithNoWeaponSound;
        protected SoundEffect deathSound;

        protected Item equiptedItem;
        #endregion

        #region Constructor
        public Character(String name, int health, int width, int height, Vector2 position, List<Texture2D> textures, float movementSpeed, SoundEffect damageReceivedSound, SoundEffect deathSound, SoundEffect attackWithNoWeaponSound) {
            this.name = name;
            this.health = health;
            this.width = width;
            this.height = height;
            this.position = position;
            this.textures = textures;
            this.movementSpeed = movementSpeed;
            this.damageReceivedSound = damageReceivedSound;
            this.deathSound = deathSound;
            this.attackWithNoWeaponSound= attackWithNoWeaponSound;
            this.boundingBox = new Rectangle((int)position.X, (int)position.Y + height / 2, width, height / 2);
        }
        #endregion

        #region Methods
        public abstract void updatePosition(List<Rectangle> collisionObjects);
        public abstract bool isTouchingLeft(Rectangle item);
        public abstract bool isTouchingRight(Rectangle item);
        public abstract bool isTouchingUp(Rectangle item);
        public abstract bool isTouchingDown(Rectangle item);
        public abstract void Update(GameTime gameTime);
        public abstract void moveUp();
        public abstract void moveDown();
        public abstract void moveLeft();
        public abstract void moveRight();
        public abstract void attack(GameTime gameTime, Character target);
        public abstract void receiveDamage(Character source, int damage);
        public String Name() { return name; }
        public int Health() { return health; }
        public Rectangle BoundingBox() { return boundingBox; }
        public int Width() { return width; }
        public int Height() { return height; }
        public Texture2D Texture() { 
            if(nextTexture < textures.Count)
            {
                return textures[nextTexture];
            }
            return null;
        }
        public Item EquiptedItem() { return equiptedItem; }
        public Vector2 GetVelocity() { return velocity; }
        #endregion

    }
}
