using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MageDefenderDeluxe.Interfaces;

namespace MageDefenderDeluxe.GameObjects
{
    public class Spell
    {
        MageDefenderDeluxe.GameObjects.SpellHandler.Spells type;

        public MageDefenderDeluxe.GameObjects.SpellHandler.Spells Type
        {
            get { return type; }
            set { type = value; }
        }

        Vector3 position, target;

        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }
        Matrix render, view, projection;
        bool active = true;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix Render
        {
            get { return render; }
            set { render = value; }
        }


        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float PositionZ
        {
            get { return position.Z; }
            set { position.Z = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        float baseDamage;

        public float BaseDamage
        {
            get { return baseDamage; }
            set { baseDamage = value; }
        }
        int minLevel;

        public int MinLevel
        {
            get { return minLevel; }
            set { minLevel = value; }
        }
        bool destoryOnHit;

        public bool DestoryOnHit
        {
            get { return destoryOnHit; }
            set { destoryOnHit = value; }
        }

        Vector3 rotation;

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float RotationX
        {
            get { return rotation.X; }
            set { rotation.X = value; }
        }
        public float RotationY
        {
            get { return rotation.Y; }
            set { rotation.Y = value; }
        }
        public float RotationZ
        {
            get { return rotation.Z; }
            set { rotation.Z = value; }
        }

        Alignments alignment;

        public Alignments Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        int modelId;

        public int ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }
        float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public enum Alignments { Damage = 1, Poison = 2, Slow };
        int mana;

        float moveXPosToTarget = 0;

        public float MoveXPosToTarget
        {
            get { return moveXPosToTarget; }
            set { moveXPosToTarget = value; }
        }
        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        int price;

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        int radius;

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public void Update(Matrix view, Matrix projection, GameTime gameTime, float thumbSticksRightX)
        {
            switch(type)
            {
                case MageDefenderDeluxe.GameObjects.SpellHandler.Spells.Fireball:
                {
                    this.position.Z += this.speed * gameTime.ElapsedGameTime.Milliseconds;
                    break;
                }
                case MageDefenderDeluxe.GameObjects.SpellHandler.Spells.Slowball:
                {
                    this.position.Z += this.speed * gameTime.ElapsedGameTime.Milliseconds;
                    this.RotationY = this.PositionZ / 500;
                    break;
                }
                case MageDefenderDeluxe.GameObjects.SpellHandler.Spells.PoisonBall:
                {
                    this.position.Z += this.speed * gameTime.ElapsedGameTime.Milliseconds;
                    break;
                }
                case MageDefenderDeluxe.GameObjects.SpellHandler.Spells.MagicMissile:
                {
                    this.PositionZ += this.Speed * (gameTime.ElapsedGameTime.Milliseconds);

                    float diff = Math.Abs(this.PositionX - this.Target.X);

                    if (this.PositionX < this.Target.X)
                    {
                        this.MoveXPosToTarget += (gameTime.ElapsedGameTime.Milliseconds / 20.0f) * (diff / 1000.0f);
                    }
                    if (this.PositionX > this.Target.X)
                    {
                        this.MoveXPosToTarget -= (gameTime.ElapsedGameTime.Milliseconds / 20.0f) * (diff / 1000.0f);
                    }

                    this.PositionX += this.MoveXPosToTarget;
                    break;
                }
                case MageDefenderDeluxe.GameObjects.SpellHandler.Spells.EnergyBall:
                {

                    this.PositionZ += this.Speed * (gameTime.ElapsedGameTime.Milliseconds);
                    this.PositionX -= thumbSticksRightX * 10;
                    break;
                }
                default:
                {
                    this.position.Z += this.speed * gameTime.ElapsedGameTime.Milliseconds;
                    break;
                }
            }


            if (position.Z >= 6200)
            {
                active = false;
            }

            Matrix objectMatrix = Matrix.CreateRotationX(0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(position.X, position.Y, position.Z);
            Matrix renderMatrix = Matrix.CreateScale(0.0080f);
            this.render = objectMatrix * renderMatrix;

            this.view = view;
            this.projection = projection;
        }

        /// <param name="t">Spell name</param>
        /// <param name="a">Aligments</param>
        /// <param name="mID">ModelID</param>
        /// <param name="m">Mana req.</param>
        /// <param name="bD">Base damage</param>
        /// <param name="mL">Minimum Level</param>
        /// <param name="s">Speed</param>
        /// <param name="dOH">Is the spell destroyd when hitting something?</param>
        /// <param name="r">radius</param>
        /// <param name="p">position( like players hand )</param>
        /// <param name="price">Price in shop</param>
        /// <param name="desc">SHORT description of the spell</param>
        /// 
        //public Spell(MageDefenderDeluxe.GameObjects.SpellHandler.Spells type, int modelId, float speed, Vector3 pos, Vector3 target)

        public Spell(GameObjects.SpellHandler.Spells t, Alignments a, int mID, int m, float bD, int mL, float s, bool dOH, int r, Vector3 p, int price, string desc, Vector3 target)
        {
            destoryOnHit = dOH;
            speed = s;
            minLevel = mL;
            baseDamage = bD;
            modelId = mID;
            position = p;
            active = true;
            radius = r;
            type = t;
            mana = m;
            rotation = new Vector3(0, 0, 0);
            alignment = a;
            name = t.ToString();
            this.price = price;
            description = desc;
            this.target = target;
            moveXPosToTarget = 0.0f;
        }
    }
}
