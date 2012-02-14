using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MageDefenderDeluxe.GameObjects;
using MageDefenderDeluxe.Engine;


namespace MageDefenderDeluxe.Screens
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ShopState : GameState
    {
        const int maxManaPotions = 9;
        const int maxHealthPotions = 9;

        int buySpellIndex = 0;
        float selectTimer = 100;

        Interfaces.ICastle castle;
        Interfaces.IPlayer player;
        Interfaces.ISpellHandler spellHandler;

        List<GameObjects.SpellHandler.Spells> spellsToBuy = new List<GameObjects.SpellHandler.Spells>();

        Texture2D texTest;
        SpriteBatch spriteBatch;
        SpriteFont m_Font;
        SpriteFont m_FontLarge;
        Texture2D selectorTexture;

        int itemSelected = 1;

        public ShopState(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            texTest = this.Game.Content.Load<Texture2D>("Textures\\shop");
            selectorTexture = this.Game.Content.Load<Texture2D>("Textures\\shopSelector");
            m_Font = Game.Content.Load<SpriteFont>(@"Comic");
            m_FontLarge = Game.Content.Load<SpriteFont>(@"ComicLarge");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            player = (Interfaces.IPlayer)Game.Services.GetService(typeof(Interfaces.IPlayer));
            spellHandler = (Interfaces.ISpellHandler)Game.Services.GetService(typeof(Interfaces.ISpellHandler));


            selectTimer -= gameTime.ElapsedGameTime.Milliseconds;

            GamePadState playerState = GamePad.GetState(player.PlayerIndexSaved);
            KeyboardState keyboard = Keyboard.GetState();

            if (selectTimer <= 0 && (playerState.Buttons.B == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape)))
            {
                selectTimer = 200;
                NextState = (int)MageDefenderStates.Level;
                ChangeState = true;
            }

            if (
                 selectTimer <= 0 &&
                 (
                        (playerState.DPad.Down == ButtonState.Pressed) || (playerState.ThumbSticks.Left.Y <= -1.0f) || (keyboard.IsKeyDown(Keys.Down))
                 )
               )
            {
                selectTimer = 200;
                itemSelected++;
            }

            if (
                 selectTimer <= 0 &&
                 (
                        (playerState.DPad.Up == ButtonState.Pressed) || (playerState.ThumbSticks.Left.Y >= 1.0f) || (keyboard.IsKeyDown(Keys.Up))
                 )
               )
            {
                selectTimer = 200;
                itemSelected--;
            }
            if (selectTimer <= 0 && ((playerState.DPad.Right == ButtonState.Pressed) || (playerState.ThumbSticks.Left.X >= 1.0f) || (keyboard.IsKeyDown(Keys.Right))) && itemSelected == 9 && spellsToBuy.Count > 0)
            {
                buySpellIndex++;
                if (buySpellIndex > spellsToBuy.Count - 1)
                    buySpellIndex = 0;
                selectTimer = 200;
            }
            if (selectTimer <= 0 && ((playerState.DPad.Left == ButtonState.Pressed) || (playerState.ThumbSticks.Left.X <= -1.0f) || (keyboard.IsKeyDown(Keys.Left))) && itemSelected == 9)
            {
                buySpellIndex--;
                if (buySpellIndex < 0)
                    buySpellIndex = spellsToBuy.Count - 1;
                selectTimer = 200;
            }

            if (selectTimer <= 0 && (playerState.Buttons.A == ButtonState.Pressed || (keyboard.IsKeyDown(Keys.Enter))))
            {
                switch (itemSelected)
                {
                    case 1:
                        {
                            if (player.Ap > 0)
                            {
                                player.Strength++;
                                player.Ap--;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (player.Ap > 0)
                            {
                                player.Constitution++;
                                player.Ap--;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (player.Ap > 0)
                            {
                                player.Agility++;
                                player.Ap--;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (player.Ap > 0)
                            {
                                player.Intelligence++;
                                player.Ap--;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (player.Ap > 0)
                            {
                                player.Wisdom++;
                                player.Ap--;
                            }
                            break;
                        }
                    case 6:
                        {
                            if (player.HealthPotions < maxHealthPotions)
                            {
                                if (player.Gold >= 100)
                                {
                                    player.HealthPotions++;
                                    player.Gold -= 100;
                                }
                            }
                            break;
                        }
                    case 7:
                        {
                            if (player.ManaPotions < maxManaPotions)
                            {
                                if (player.Gold >= 200)
                                {
                                    player.ManaPotions++;
                                    player.Gold -= 200;
                                }
                            }
                            break;
                        }
                    case 8:
                        {
                            if (castle.Upgrade < 6)
                            {
                                if (player.Gold >= castle.Upgrade * 500)
                                {
                                    castle.Upgrade++;
                                    player.Gold -= castle.Upgrade * 500;
                                }
                            }
                            break;
                        }
                    case 9:
                        {
                            if (spellsToBuy.Count > 0)
                            {
                                if (player.Gold >= spellHandler.GetSpellPrice(spellsToBuy[buySpellIndex]))
                                {
                                    spellHandler.LearnSpell(spellsToBuy[buySpellIndex]);
                                    player.Gold -= spellHandler.GetSpellPrice(spellsToBuy[buySpellIndex]);
                                }
                            }
                            break;
                        }
                }
                selectTimer = 200;
            }

            if (itemSelected <= 1)
                itemSelected = 1;
            if (itemSelected >= 9)
                itemSelected = 9;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            player = (Interfaces.IPlayer)Game.Services.GetService(typeof(Interfaces.IPlayer));
            spellHandler = (Interfaces.ISpellHandler)Game.Services.GetService(typeof(Interfaces.ISpellHandler));
            castle = (Interfaces.ICastle)Game.Services.GetService(typeof(Interfaces.ICastle));

            int width = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            spriteBatch.Begin();
            spriteBatch.Draw(texTest, new Rectangle(0, 0, Game.GraphicsDevice.PresentationParameters.BackBufferWidth, Game.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);

            string priceOnSelectedItem = "";
            string selectedItemType = "  AP";

            string desc1 = "";
            string desc2 = "";

            switch (itemSelected)
            {
                case 1:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.59f), Convert.ToInt32(height * 0.13f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = " 1/" + player.Ap;
                        selectedItemType = "  AP";

                        desc1 = "+Max Health";
                        break;
                    }
                case 2:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.595f), Convert.ToInt32(height * 0.19f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = " 1/" + player.Ap;
                        selectedItemType = "  AP";
                        desc1 = "+Mana regen";
                        break;
                    }
                case 3:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.6f), Convert.ToInt32(height * 0.25f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = " 1/" + player.Ap;
                        selectedItemType = "  AP";
                        desc1 = "+Speed";
                        break;
                    }
                case 4:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.605f), Convert.ToInt32(height * 0.31f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = " 1/" + player.Ap;
                        selectedItemType = "  AP";
                        desc1 = "+Spell damage";
                        break;
                    }
                case 5:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.61f), Convert.ToInt32(height * 0.36f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = " 1/" + player.Ap;
                        selectedItemType = "  AP";
                        desc1 = "+Max mana";
                        break;
                    }
                case 6:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.615f), Convert.ToInt32(height * 0.52f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = "100";
                        selectedItemType = " Gold";
                        desc1 = "Heals when consumed";
                        desc2 = "Max amount: 9";
                        break;
                    }
                case 7:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.62f), Convert.ToInt32(height * 0.58f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = "200";
                        selectedItemType = " Gold";
                        desc1 = "Returns some mana.";
                        desc2 = "Max amount: 9";
                        break;
                    }
                case 8:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.625), Convert.ToInt32(height * 0.64f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        priceOnSelectedItem = (castle.Level * 500).ToString();
                        selectedItemType = " Gold";
                        desc1 = "Increase castles lives.";
                        desc2 = "Max levels: 6";
                        break;
                    }
                case 9:
                    {
                        spriteBatch.Draw(selectorTexture, new Rectangle(Convert.ToInt32(width * 0.63), Convert.ToInt32(height * 0.72f), (int)Convert.ToInt32(width * 0.05f), Convert.ToInt32(height * 0.0888f)), Color.White);
                        if (spellsToBuy.Count > 0)
                        {
                            if (spellsToBuy.Count > buySpellIndex)
                            {
                                priceOnSelectedItem = spellHandler.GetSpellPrice(spellsToBuy[buySpellIndex]).ToString();
                                string spellDesc = spellHandler.GetSpellDescription(spellsToBuy[buySpellIndex]);
                                string[] spellDescSplit = spellDesc.Split('|');
                                desc1 = spellDescSplit[0];
                                desc2 = spellDescSplit[1];
                            }
                        }
                        else priceOnSelectedItem = "0";
                        selectedItemType = " Gold";

                        break;
                    }
            }

            /*
             Strength = max Helse
            Constitution = Mana Regen speed
            Intelligence = damage modifier
            Wisdom = max mana
             */
            //spriteBatch.DrawString(m_Font, "Abilities", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.107f)), Color.Red);
            //spriteBatch.DrawString(m_Font, "Ability points left: " + player.Ap, new Vector2((width * 0.62f), Convert.ToInt32(height * 0.14f)), Color.WhiteSmoke);
            //spriteBatch.DrawString(m_Font, "Constitution:   " + player.Constitution + "  (Mana regen)", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.17f)), Color.WhiteSmoke);
            //spriteBatch.DrawString(m_Font, "Intelligence:    " + player.Intelligence + "  (Spell damage)", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.20f)), Color.WhiteSmoke);
            //spriteBatch.DrawString(m_Font, "Strength:         " + player.Strength + "  (Increase max health)", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.23f)), Color.WhiteSmoke);
            //spriteBatch.DrawString(m_Font, "Wisdom:         " + player.Wisdom + "  (Increase max mana)", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.26f)), Color.WhiteSmoke);


            spriteBatch.DrawString(m_FontLarge, player.Strength.ToString(), new Vector2((width * 0.553f), Convert.ToInt32(height * 0.13f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.Strength.ToString(), new Vector2((width * 0.55f), Convert.ToInt32(height * 0.125f)), Color.White);

            spriteBatch.DrawString(m_FontLarge, player.Constitution.ToString(), new Vector2((width * 0.559f), Convert.ToInt32(height * 0.185f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.Constitution.ToString(), new Vector2((width * 0.556f), Convert.ToInt32(height * 0.18f)), Color.White);

            spriteBatch.DrawString(m_FontLarge, player.Agility.ToString(), new Vector2((width * 0.563f), Convert.ToInt32(height * 0.245f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.Agility.ToString(), new Vector2((width * 0.56f), Convert.ToInt32(height * 0.24f)), Color.White);

            spriteBatch.DrawString(m_FontLarge, player.Intelligence.ToString(), new Vector2((width * 0.568f), Convert.ToInt32(height * 0.3f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.Intelligence.ToString(), new Vector2((width * 0.565f), Convert.ToInt32(height * 0.295f)), Color.White);

            spriteBatch.DrawString(m_FontLarge, player.Wisdom.ToString(), new Vector2((width * 0.573f), Convert.ToInt32(height * 0.36f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.Wisdom.ToString(), new Vector2((width * 0.57f), Convert.ToInt32(height * 0.355f)), Color.White);


            spriteBatch.DrawString(m_FontLarge, player.HealthPotions.ToString(), new Vector2((width * 0.599f), Convert.ToInt32(height * 0.525f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.HealthPotions.ToString(), new Vector2((width * 0.596f), Convert.ToInt32(height * 0.52f)), Color.White);

            spriteBatch.DrawString(m_FontLarge, player.ManaPotions.ToString(), new Vector2((width * 0.603f), Convert.ToInt32(height * 0.585f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, player.ManaPotions.ToString(), new Vector2((width * 0.60f), Convert.ToInt32(height * 0.58f)), Color.White);

            spriteBatch.DrawString(m_FontLarge, castle.Upgrade.ToString(), new Vector2((width * 0.608f), Convert.ToInt32(height * 0.645f)), Color.Black);
            spriteBatch.DrawString(m_FontLarge, castle.Upgrade.ToString(), new Vector2((width * 0.605f), Convert.ToInt32(height * 0.64f)), Color.White);


            //spriteBatch.DrawString(m_Font, "Shop", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.35f)), Color.Red);
            //spriteBatch.DrawString(m_Font, "Buy health potions, 100gp. (You got " + player.HealthPotions + ")", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.38f)), Color.WhiteSmoke);
            //spriteBatch.DrawString(m_Font, "Buy mana potions, 100gp. (You got " + player.ManaPotions + ")", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.41f)), Color.WhiteSmoke);

            spellsToBuy.Clear();
            foreach (Spell s in spellHandler.SpellReference)
            {
                if (s.MinLevel <= player.Level)
                {
                    bool alreadyKnow = false;
                    foreach (GameObjects.SpellHandler.Spells spell in spellHandler.LearnedSpells)
                    {
                        if (spell == s.Type)
                        {
                            alreadyKnow = true;
                        }
                    }
                    if (!alreadyKnow)
                    {
                        spellsToBuy.Add(s.Type);
                    }
                }
            }
            if (buySpellIndex >= spellsToBuy.Count)
            {
                buySpellIndex = spellsToBuy.Count - 1;
            }

            if (spellsToBuy.Count > 0)
            {
                if (buySpellIndex <= 0)
                    buySpellIndex = 0;
                //spriteBatch.DrawString(m_Font,spellsToBuy[buySpellIndex].ToString() + " Price: " + spellHandler.GetSpellPrice(spellsToBuy[buySpellIndex]), new Vector2((width * 0.62f), Convert.ToInt32(height * 0.44f)), Color.WhiteSmoke);
                spriteBatch.DrawString(m_FontLarge, spellsToBuy[buySpellIndex].ToString(), new Vector2((width * 0.703f), Convert.ToInt32(height * 0.805f)), Color.Black);
                spriteBatch.DrawString(m_FontLarge, spellsToBuy[buySpellIndex].ToString(), new Vector2((width * 0.7f), Convert.ToInt32(height * 0.8f)), Color.White);
                //spriteBatch.DrawString(m_Font, spellHandler.GetSpellDescription(spellsToBuy[buySpellIndex])+"", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.47f)), Color.WhiteSmoke);
            }
            else
            {
                spriteBatch.DrawString(m_FontLarge, "None for sale", new Vector2((width * 0.703f), Convert.ToInt32(height * 0.805f)), Color.Black);
                spriteBatch.DrawString(m_FontLarge, "None for sale", new Vector2((width * 0.7f), Convert.ToInt32(height * 0.8f)), Color.White);
                //spriteBatch.DrawString(m_Font, "Buy spell: None for sale", new Vector2((width * 0.62f), Convert.ToInt32(height * 0.44f)), Color.WhiteSmoke);
            }

            spriteBatch.DrawString(m_Font, player.Gold + " gp.", new Vector2((width * 0.235f), Convert.ToInt32(height * 0.81f)), Color.Black);
            spriteBatch.DrawString(m_Font, player.Gold + " gp.", new Vector2((width * 0.232f), Convert.ToInt32(height * 0.805f)), Color.White);

            spriteBatch.DrawString(m_Font, priceOnSelectedItem.ToString(), new Vector2((width * 0.385f), Convert.ToInt32(height * 0.65f)), Color.Black);
            spriteBatch.DrawString(m_Font, priceOnSelectedItem.ToString(), new Vector2((width * 0.382f), Convert.ToInt32(height * 0.645f)), Color.White);

            spriteBatch.DrawString(m_Font, desc1, new Vector2((width * 0.12f), Convert.ToInt32(height * 0.65f)), Color.Black);
            spriteBatch.DrawString(m_Font, desc1, new Vector2((width * 0.118f), Convert.ToInt32(height * 0.645f)), Color.White);

            spriteBatch.DrawString(m_Font, desc2, new Vector2((width * 0.125f), Convert.ToInt32(height * 0.695f)), Color.Black);
            spriteBatch.DrawString(m_Font, desc2, new Vector2((width * 0.122f), Convert.ToInt32(height * 0.69f)), Color.White);

            spriteBatch.DrawString(m_Font, selectedItemType, new Vector2((width * 0.385f), Convert.ToInt32(height * 0.705f)), Color.Black);
            spriteBatch.DrawString(m_Font, selectedItemType, new Vector2((width * 0.382f), Convert.ToInt32(height * 0.70f)), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
