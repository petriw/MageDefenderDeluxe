using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MageDefenderDeluxe.GameObjects
{
    public class Enemy
    {
        Matrix render, view, projection;

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

        int particleSystemId;

        public int ParticleSystemId
        {
            get { return particleSystemId; }
            set { particleSystemId = value; }
        }

        public bool IsColliding(Spell s)
        {
            bool collision = false;
            Vector3 distance = this.Position - s.Position;
            distance.Y = 0.0f; // Bypass detection on Y

            if (distance.Length() < (this.Radius + s.Radius))
            {
                collision = true;
            }

            return collision;
        }

        public bool IsColliding(Vector3 playerPos)
        {
            //playerPos.Y = 300;

            bool collision = false;
            Vector3 distance = this.Position - playerPos;
            distance.Y = 0.0f; // Bypass detection on Y

            if (distance.Length() < (this.Radius + 200))
            {
                collision = true;
            }

            return collision;
        }

        bool animated;

        public bool Animated
        {
            get { return animated; }
            set { animated = value; }
        }

        int radius;

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        Vector3 position;

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
        int modelID;

        public int ModelID
        {
            get { return modelID; }
            set { modelID = value; }
        }

        int baseDamage;

        public int BaseDamage
        {
            get { return baseDamage; }
            set { baseDamage = value; }
        }
        float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        GameObjects.EnemyHandler.AI ai;

        public GameObjects.EnemyHandler.AI AI
        {
            get { return ai; }
            set { ai = value; }
        }

        int loot;

        public int Loot
        {
            get { return loot; }
            set { loot = value; }
        }
        int xp;

        public int Xp
        {
            get { return xp; }
            set { xp = value; }
        }
        int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        float health;

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        bool isDOT;

        public bool IsDOT
        {
            get { return isDOT; }
            set { isDOT = value; }
        }

        List<Spell.Alignments> alignmentsList;

        public List<Spell.Alignments> AlignmentsList
        {
            get { return alignmentsList; }
            set { alignmentsList = value; }
        }
        List<Spell.Alignments> imuneAlignments;

        public List<Spell.Alignments> ImuneAlignments
        {
            get { return imuneAlignments; }
            set { imuneAlignments = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Name</param>
        /// <param name="mID">ModelID</param>
        /// <param name="hP">Hit points</param>
        /// <param name="bD">Base damage</param>
        /// <param name="s">Speed(x,y)</param>
        /// <param name="r">radius</param>
        /// <param name="p">Start position</param>
        /// <param name="aiMode">AI</param>
        /// <param name="l">loot gold</param>
        /// <param name="x">xp when killed</param>
        /// <param name="sc">score</param>
        public Enemy(string n, bool anim, int mID, int hP, int bD, Vector2 s, int r, Vector3 p, GameObjects.EnemyHandler.AI aiMode, int l, int x, int sc, int partSysId)
        {
            alignmentsList = new List<Spell.Alignments>();
            imuneAlignments = new List<Spell.Alignments>();

            Random rndSpeed = new Random();

            particleSystemId = partSysId;
            speed = (float)(s.X + (rndSpeed.NextDouble() * s.Y));
            baseDamage = bD;
            modelID = mID;
            position = p;
            active = true;
            radius = r;
            ai = aiMode;
            loot = l;
            xp = x;
            score = sc;
            health = hP;
            maxHealth = hP;
            name = n;
            isDOT = false;
            animated = anim;
        }

        public void Update(Matrix view, Matrix projection, GameTime gameTime)
        {
            Matrix objectMatrix = Matrix.CreateRotationX(0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(position.X, position.Y, position.Z);
            Matrix renderMatrix = Matrix.CreateScale(0.0080f);
            this.render = objectMatrix * renderMatrix;

            this.view = view;
            this.projection = projection;
        }
    }
}