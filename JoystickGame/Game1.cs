using JoystickGame.Menus;
using JoystickGame.PickUps;
using JoystickGame.Models;
using JoystickGame.Managers;
using System.Diagnostics;
using JoystickGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using JoystickGame.Map;
using System;
using JoystickGame.Bullet;
using System.Threading;
using System.Timers;

namespace JoystickGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D dummy;

        Player player;
        Texture2D playerTex;

        //Map 
        Maps map;
        int[,] mapGrid = new int[,]
        {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,},
                {1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,3,1,},
                {1,1,2,0,0,0,0,1,1,0,0,0,1,1,1,1,0,0,0,1,},
                {1,1,3,0,0,0,1,1,0,0,0,1,1,1,0,1,2,0,0,1,},
                {1,1,2,0,0,0,1,1,0,0,2,1,1,0,0,0,3,0,0,1,},
                {1,1,3,0,0,0,0,1,1,0,3,1,1,0,0,0,1,1,0,1,},
                {1,1,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,1,2,},
                {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,3,},
                {2,0,0,0,1,1,1,0,0,2,0,0,0,0,0,0,0,0,0,2,},
                {3,0,0,1,1,1,1,1,1,3,0,0,0,0,0,0,0,0,0,3,},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,}
        };

        int[,] mapGrid2 = new int[,]
        {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,2,1,},
                {1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,3,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,},
                {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,},
                {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,}
        };
        int size = 64;


        //Sprite player
        Vector2 startPos = new Vector2(100, 100);    
        
        //Buttons
        cButton BtnPlay;
        cButton BtnQuiting;
        cButton BtnConfirmQuiting;

        //Enemy
        List<Enemy> enemies = new List<Enemy>();
        Texture2D ememyTex;

        //Bullet
        List<BulletM> bullets = new List<BulletM>();
        Texture2D bulletTex;
        float timer;

        //Pick Ups
        Texture2D aArmourBar;
        SpriteBatch aBatch;
        List<PickUp> pickUps = new List<PickUp>();
        private int aCurrentArmour = 100;
        Texture2D armTex;

        //arrow
        List<Level> levels = new List<Level>();
        Texture2D arrowTex;

        //Collision Detection
        bool isIntersecting;
        bool isDead;
        bool touchBuff;
        int playerhits;
        public static Stopwatch stopWatch = new Stopwatch();

        //HUD Elements
        Texture2D weaponIcon;
        Rectangle wpnIconRect;
        Vector2 wpnIconPos;

        //Armour Icon Status
        List<Texture2D> armourIcons = new List<Texture2D>();
        Rectangle armIconRect;
        Vector2 armIconPos;

        //Button Lists
        List<cButton> mainMenuButtons = new List<cButton>();
        List<cButton> quitButtons = new List<cButton>();
        List<cButton> confirmquitButton = new List<cButton>();

        int screenWidth = 1280, screenHeight = 720;

        int clickTimer;
        bool canTick;

        GameState CurrentGameState = GameState.MainMenu;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            map = new Maps();

            dummy = new Texture2D(GraphicsDevice, 1, 1);
            dummy.SetData(new[] { Color.White });

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            

            IsMouseVisible = true;

            Content.Load<Texture2D>("Background/MapBackground");
            
            BtnPlay = new cButton(Content.Load<Texture2D>("Buttons/PlayButtonAgile"), screenWidth, screenHeight, 120, GameState.Playing);
            BtnQuiting = new cButton(Content.Load<Texture2D>("Buttons/QuitButton"), screenWidth, screenHeight, 80, GameState.Quiting);
            BtnConfirmQuiting = new cButton(Content.Load<Texture2D>("Buttons/QuitButton"), screenWidth, screenHeight, 40, GameState.ConfirmQuiting);

            mainMenuButtons.Add(BtnPlay);
            mainMenuButtons.Add(BtnQuiting);

            quitButtons.Add(new cButton(Content.Load<Texture2D>("Buttons/QuitButton"), screenWidth, screenHeight, 120, GameState.ConfirmQuiting));
            quitButtons.Add(new cButton(Content.Load<Texture2D>("Buttons/BackToMenu"), screenWidth, screenHeight, 40, GameState.MainMenu));

            MapTiles.Content = Content;

            map.Generate(mapGrid, size);

            for (int x = 0; x < mapGrid.GetLength(1); x++)
            {
                for (int y = 0; y < mapGrid.GetLength(0); y++)
                {
                    int current = mapGrid[y, x];
                }
            }

            //Player
            playerTex = Content.Load<Texture2D>("Player/CharacterSpriteSheetp");
            player = new Player(Content, new Vector2(1000, 700), 1, 12, 100);

            //Bullet
            bulletTex = Content.Load<Texture2D>("Bullet/BulletM");

            //Armour Bar
            aBatch = new SpriteBatch(this.GraphicsDevice);

           //Arrow
           arrowTex = Content.Load<Texture2D>("MapAssets/Arrow");
           levels.Add(new Level(new Vector2 (1150, 600),50, Content));

            ContentManager aLoader = new ContentManager(this.Services);

            //HUD Elements
            weaponIcon = Content.Load<Texture2D>("PickUps/WeaponPickUpSingle5");
            wpnIconPos = new Vector2(GraphicsDevice.Viewport.X + 20, GraphicsDevice.Viewport.Height - 120);
            wpnIconRect = new Rectangle((int)wpnIconPos.X, (int)wpnIconPos.Y, 100, 100);
            
            for (int i = 0; i < 5; i++)
            {
                armourIcons.Add(Content.Load<Texture2D>("PickUps/armourIcon" + (i * 25)));                
            };

            armIconPos = new Vector2(GraphicsDevice.Viewport.X + 330, GraphicsDevice.Viewport.Y + 5);
            armIconRect = new Rectangle((int)armIconPos.X, (int)armIconPos.Y, 100, 100);
            

            aArmourBar = Content.Load<Texture2D>("PickUps/HealthBar") as Texture2D;

            //Pick Ups
            pickUps.Add(new PickUp(1, 8, Content, new Vector2(500, 600), 0));
            pickUps.Add(new PickUp(1, 8, Content, new Vector2(500, 80), 1));
            pickUps.Add(new PickUp(1, 8, Content, new Vector2(600, 400), 0));

            //Enemy
            enemies.Add(new Enemy(Content, new Vector2(300, 450), false, 100));
            enemies.Add(new Enemy(Content, new Vector2(575, 200), true, 150));
            enemies.Add(new Enemy(Content, new Vector2(1025, 650), false, 200));
            #region OldCode
            /*
            _sprites = new List<Sprite>()
            {
                new Sprite(animations)
                {
                    Position = new Vector2(100,100),
                    input = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D

                    },
                },
            };
            */
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
       
        //Reset
        private void resetPlayer()
        {
          enemies.Clear();
          enemies.Add(new Enemy(Content, new Vector2(300, 450), false, 100));
          enemies.Add(new Enemy(Content, new Vector2(575, 200), true, 150));
          enemies.Add(new Enemy(Content, new Vector2(1025, 650), false, 200));
          aCurrentArmour = 100;
          player.pos = startPos;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    foreach (cButton b in mainMenuButtons)
                    {
                        b.Update(ms);

                        if (b.isClicked)
                            OnClick(gameTime, b);
                    }

                    break;

                case GameState.Playing:


                        player.Update(gameTime);

                        //Player Collision Checks
                        foreach(CollisionTiles t in map.CollisionTiles)
                        {
                            if(player.UpRect.Intersects(t.Rectangle))
                            {
                                player.CanMoveUp = false;
                                //player.pos = new Vector2(player.pos.X, t.Rectangle.Y + t.Rectangle.Height + 1);
                                break;
                            }
                            else
                            {
                                player.CanMoveUp = true;
                            }                            
                        }

                        foreach (CollisionTiles t in map.CollisionTiles)
                        {
                            if (player.LeftRect.Intersects(t.Rectangle))
                            {
                                player.CanMoveLeft = false;
                                //player.pos = new Vector2(t.Rectangle.X + t.Rectangle.Width + 1, player.pos.Y);
                                break;
                            }
                            else
                            {
                                player.CanMoveLeft = true;
                            }
                        }

                        foreach (CollisionTiles t in map.CollisionTiles)
                        {
                            if (player.DownRect.Intersects(t.Rectangle))
                            {
                                player.CanMoveDown = false;
                                //player.pos = new Vector2(player.pos.X, t.Rectangle.Y - player.destRect.Height);
                                break;
                            }
                            else
                            {
                                player.CanMoveDown = true;
                            }
                        }

                        foreach (CollisionTiles t in map.CollisionTiles)
                        {
                            if (player.RightRect.Intersects(t.Rectangle))
                            {
                                player.CanMoveRight = false;
                                //player.pos = new Vector2(t.Rectangle.X - player.destRect.Width, player.pos.Y);
                                break;
                            }
                            else
                            {
                                player.CanMoveRight = true;
                            }
                        }

                        foreach (Enemy e in enemies)
                        {
                            e.Update(gameTime);
                            e.GetDistance(player);
                            Console.WriteLine("Updating...");
                        }

                        //Shooting
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            if (player.WEquipped)
                            {
                                timer += gameTime.ElapsedGameTime.Milliseconds;

                                if (timer > 250)
                                {
                                    Shoot();
                                    timer = 0;
                                }
                            }
                        }
                        else if (ms.LeftButton == ButtonState.Released)
                        {
                            timer = 0;
                        }
                                       
                    //Active bullets - Check lifetime
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Update(gameTime);

                        if (bullets[i].Timer > bullets[i].LifeTime)
                        {
                            bullets.Remove(bullets[i]);
                            i--;
                        }
                    }

                    //Active bullets - Check enemy collision
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        for (int i = 0; i < bullets.Count; i++)
                        {
                            if (bullets[i].Rect.Intersects(enemies[j].enemyRect))
                            {
                                bullets.Remove(bullets[i]);
                                i--;

                                enemies.Remove(enemies[j]);
                            }
                        }
                    }

                    foreach (Level l in levels)
                    {
                        if (player.destRect.Intersects(l.arrowRect))
                        {
                            Debug.WriteLine("Next level");
                            map.Generate(mapGrid, 0);
                            map.Generate(mapGrid2, size);
                            for (int x = 0; x < mapGrid2.GetLength(1); x++)
                            {
                                for (int y = 0; y < mapGrid2.GetLength(0); y++)
                                {
                                    int current = mapGrid2[y, x];
                                }
                            }
                            resetPlayer();
                            enemies.Clear();
                            pickUps.Clear();
                        }
                    }



                    //enemy collision
                    foreach(Enemy f in enemies)
                    {

                        if(player.destRect.Intersects(f.enemyRect))
                        {
                            isIntersecting = true;
                            stopWatch.Start();
                            if(touchBuff == false)
                            {
                                aCurrentArmour -= 25;
                                touchBuff = true;
                            }
                            if(stopWatch.ElapsedMilliseconds > 3000)
                            {
                                stopWatch.Reset();
                                stopWatch.Stop();
                                touchBuff = false;
                            }
                            if(aCurrentArmour == 0)
                            {
                                resetPlayer();
                                CurrentGameState = GameState.MainMenu;
                            }
                            break;
                        }

                        else
                        {
                            isIntersecting = false;
                        }
                    }


                    foreach (PickUp p in pickUps)
                    {
                        p.Update(gameTime);
                    }

                    for(int x = 0; x < pickUps.Count; x++)
                    {
                        if(player.destRect.Intersects(pickUps[x].rectangle))
                        {
                            switch(pickUps[x].ItemID)
                            {
                                case 0:
                                    Console.WriteLine("Picked up some armour!");
                                    if (aCurrentArmour == 100)
                                    {
                                        aCurrentArmour += 0;
                                    }
                                    else
                                    {
                                        aCurrentArmour += 25;
                                    }


                                    break;
                                case 1:
                                    Console.WriteLine("Picked up an awesome sword!");
                                    player.HasPickedUp = true;
                                    break;
                               
                                
                            }

                            pickUps.Remove(pickUps[x]);
                        }
                    }
                    #region old code

                    /*
                    for (int i = 0; i < pickUps.Count; i++)
                    {
                        if (player.rectangle.Intersects(pickUps[i].rectangle))
                        {
                            aCurrentArmour += 5;
                            pickUps.Remove(pickUps[i]);
                            i--;
                        }
                    }
                    */

                    /*
                    foreach (var sprite in _sprites)
                        sprite.Update(gameTime, _sprites);
                        */
                    #endregion
            
                    break;


                case GameState.Playing2:


                    player.Update(gameTime);

                    //Player Collision Checks
                    foreach (CollisionTiles t in map.CollisionTiles)
                    {
                        if (player.UpRect.Intersects(t.Rectangle))
                        {
                            player.CanMoveUp = false;
                            //player.pos = new Vector2(player.pos.X, t.Rectangle.Y + t.Rectangle.Height + 1);
                            break;
                        }
                        else
                        {
                            player.CanMoveUp = true;
                        }
                    }

                    foreach (CollisionTiles t in map.CollisionTiles)
                    {
                        if (player.LeftRect.Intersects(t.Rectangle))
                        {
                            player.CanMoveLeft = false;
                            //player.pos = new Vector2(t.Rectangle.X + t.Rectangle.Width + 1, player.pos.Y);
                            break;
                        }
                        else
                        {
                            player.CanMoveLeft = true;
                        }
                    }

                    foreach (CollisionTiles t in map.CollisionTiles)
                    {
                        if (player.DownRect.Intersects(t.Rectangle))
                        {
                            player.CanMoveDown = false;
                            //player.pos = new Vector2(player.pos.X, t.Rectangle.Y - player.destRect.Height);
                            break;
                        }
                        else
                        {
                            player.CanMoveDown = true;
                        }
                    }

                    foreach (CollisionTiles t in map.CollisionTiles)
                    {
                        if (player.RightRect.Intersects(t.Rectangle))
                        {
                            player.CanMoveRight = false;
                            //player.pos = new Vector2(t.Rectangle.X - player.destRect.Width, player.pos.Y);
                            break;
                        }
                        else
                        {
                            player.CanMoveRight = true;
                        }
                    }

                    foreach (Enemy e in enemies)
                    {
                        e.Update(gameTime);
                        e.GetDistance(player);
                        Console.WriteLine("Updating...");
                    }

                    //Shooting
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        if (player.WEquipped)
                        {
                            timer += gameTime.ElapsedGameTime.Milliseconds;

                            if (timer > 250)
                            {
                                Shoot();
                                timer = 0;
                            }
                        }
                    }
                    else if (ms.LeftButton == ButtonState.Released)
                    {
                        timer = 0;
                    }

                    //Active bullets - Check lifetime
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Update(gameTime);

                        if (bullets[i].Timer > bullets[i].LifeTime)
                        {
                            bullets.Remove(bullets[i]);
                            i--;
                        }
                    }

                    //Active bullets - Check enemy collision
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        for (int i = 0; i < bullets.Count; i++)
                        {
                            if (bullets[i].Rect.Intersects(enemies[j].enemyRect))
                            {
                                bullets.Remove(bullets[i]);
                                i--;

                                enemies.Remove(enemies[j]);
                            }
                        }
                    }

                    foreach (Level l in levels)
                    {
                        if (player.destRect.Intersects(l.arrowRect))
                        {
                            CurrentGameState = GameState.Playing2;
                        }
                    }



                    //enemy collision
                    foreach (Enemy f in enemies)
                    {

                        if (player.destRect.Intersects(f.enemyRect))
                        {
                            isIntersecting = true;
                            stopWatch.Start();
                            if (touchBuff == false)
                            {
                                aCurrentArmour -= 25;
                                touchBuff = true;
                            }
                            if (stopWatch.ElapsedMilliseconds > 3000)
                            {
                                stopWatch.Reset();
                                stopWatch.Stop();
                                touchBuff = false;
                            }
                            if (aCurrentArmour == 0)
                            {
                                resetPlayer();
                                CurrentGameState = GameState.MainMenu;
                            }
                            break;
                        }

                        else
                        {
                            isIntersecting = false;
                        }
                    }


                    foreach (PickUp p in pickUps)
                    {
                        p.Update(gameTime);
                    }

                    for (int x = 0; x < pickUps.Count; x++)
                    {
                        if (player.destRect.Intersects(pickUps[x].rectangle))
                        {
                            switch (pickUps[x].ItemID)
                            {
                                case 0:
                                    Console.WriteLine("Picked up some armour!");
                                    if (aCurrentArmour == 100)
                                    {
                                        aCurrentArmour += 0;
                                    }
                                    else
                                    {
                                        aCurrentArmour += 25;
                                    }


                                    break;
                                case 1:
                                    Console.WriteLine("Picked up an awesome sword!");
                                    player.HasPickedUp = true;
                                    break;


                            }

                            pickUps.Remove(pickUps[x]);
                        }
                    }
                    #region old code

                    /*
                    for (int i = 0; i < pickUps.Count; i++)
                    {
                        if (player.rectangle.Intersects(pickUps[i].rectangle))
                        {
                            aCurrentArmour += 5;
                            pickUps.Remove(pickUps[i]);
                            i--;
                        }
                    }
                    */

                    /*
                    foreach (var sprite in _sprites)
                        sprite.Update(gameTime, _sprites);
                        */
                    #endregion

                    break;

                case GameState.Quiting:

                    foreach (cButton b in quitButtons)
                    {
                        b.Update(ms);

                        if (b.isClicked)
                            OnClick(gameTime, b);
                    }

                    break;

                case GameState.ConfirmQuiting:

                    foreach (cButton b in quitButtons)
                    {
                        b.Update(ms);

                        if (b.isClicked)
                            OnClick(gameTime, b);
                    }

                    break;

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Background/MainMenuBKAgile"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    foreach (cButton b in mainMenuButtons)
                    {
                        b.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    break;

                case GameState.Playing:
                    
                    //Main Game Draw Pass
                    spriteBatch.Begin();

                    spriteBatch.Draw(Content.Load<Texture2D>("Background/MapBackground"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

                    #region Draw Map
                    //Map
                    map.Draw(spriteBatch);
                    #endregion
                    #region Draw Test
                    /* foreach(CollisionTiles t in map.CollisionTiles)
                     {
                         DrawBorders(t.Rectangle, 1, Color.Cyan);
                     }
                     */
                    #endregion
                    #region Draw Player
                    player.Draw(spriteBatch);
                    #endregion
                    foreach (Enemy j in enemies)
                    {
                        j.Draw(spriteBatch);
                    }

                    foreach (Level l in levels)
                    {
                        l.Draw(spriteBatch);
                    }

                    #region Bullet Draw 
                    foreach (BulletM b in bullets)
                    {
                        b.Draw(spriteBatch);
                    }
                    #endregion
                    #region Draw Pickups
                    foreach (PickUp a in pickUps)
                    {
                        a.Draw(spriteBatch);
                    }                   
                    spriteBatch.End();
                    #endregion
                    #region Armour Bar
                    //This is your armour pass
                    aBatch.Begin();

                    aBatch.Draw(aArmourBar, new Rectangle(this.Window.ClientBounds.Width / 2 - aArmourBar.Width / 2,
                        30, aArmourBar.Width, 44), new Rectangle(0, 45, aArmourBar.Width, 44), Color.Gray);

                    aBatch.Draw(aArmourBar, new Rectangle(this.Window.ClientBounds.Width / 2 - aArmourBar.Width / 2,
                         30, (int)(aArmourBar.Width * ((double)aCurrentArmour / 100)), 44),
                         new Rectangle(0, 45, aArmourBar.Width, 44), Color.LightBlue);

                    aBatch.Draw(aArmourBar, new Rectangle(this.Window.ClientBounds.Width / 2 - aArmourBar.Width / 2,
                        30, aArmourBar.Width, 44), new Rectangle(0, 0, aArmourBar.Width, 44), Color.White);

                   aBatch.End();
                    #endregion
                    #region HUD Elements 
                    //This is your HUD pass
                    spriteBatch.Begin();
                   
                    if (player.HasPickedUp)
                    {
                        if(player.WEquipped)
                            spriteBatch.Draw(weaponIcon, wpnIconRect, Color.White);
                        else
                            spriteBatch.Draw(weaponIcon, wpnIconRect, Color.Black);
                    }

                    switch (aCurrentArmour)
                    {
                        case 0:
                            spriteBatch.Draw(armourIcons[0], armIconRect, Color.Black);
                            break;
                        case 25:
                            spriteBatch.Draw(armourIcons[0], armIconRect, Color.White);
                            break;
                        case 50:
                            spriteBatch.Draw(armourIcons[2], armIconRect, Color.White);
                            break;
                        case 75:
                            spriteBatch.Draw(armourIcons[3], armIconRect, Color.White);
                            break;
                        case 100:
                            spriteBatch.Draw(armourIcons[4], armIconRect, Color.White);
                            break;
                    }
       
                    spriteBatch.End();
                    
                    base.Draw(gameTime);
                    break;
                #endregion


                case GameState.Playing2:

                    //Main Game Draw Pass
                    spriteBatch.Begin();

                    spriteBatch.Draw(Content.Load<Texture2D>("Background/MapBackground"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

                    #region Draw Map
                    //Map
                    map.Draw(spriteBatch);
                    #endregion
                    #region Draw Test
                    /* foreach(CollisionTiles t in map.CollisionTiles)
                     {
                         DrawBorders(t.Rectangle, 1, Color.Cyan);
                     }
                     */
                    #endregion
                    #region Draw Player
                    player.Draw(spriteBatch);
                    #endregion
                    foreach (Enemy j in enemies)
                    {
                        j.Draw(spriteBatch);
                    }

                    foreach (Level l in levels)
                    {
                        l.Draw(spriteBatch);
                    }

                    #region Bullet Draw 
                    foreach (BulletM b in bullets)
                    {
                        b.Draw(spriteBatch);
                    }
                    #endregion
                    #region Draw Pickups
                    foreach (PickUp a in pickUps)
                    {
                        a.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    #endregion
                    #region Armour Bar
                    //This is your armour pass
                    aBatch.Begin();

                    aBatch.Draw(aArmourBar, new Rectangle(this.Window.ClientBounds.Width / 2 - aArmourBar.Width / 2,
                        30, aArmourBar.Width, 44), new Rectangle(0, 45, aArmourBar.Width, 44), Color.Gray);

                    aBatch.Draw(aArmourBar, new Rectangle(this.Window.ClientBounds.Width / 2 - aArmourBar.Width / 2,
                         30, (int)(aArmourBar.Width * ((double)aCurrentArmour / 100)), 44),
                         new Rectangle(0, 45, aArmourBar.Width, 44), Color.LightBlue);

                    aBatch.Draw(aArmourBar, new Rectangle(this.Window.ClientBounds.Width / 2 - aArmourBar.Width / 2,
                        30, aArmourBar.Width, 44), new Rectangle(0, 0, aArmourBar.Width, 44), Color.White);

                    aBatch.End();
                    #endregion
                    #region HUD Elements 
                    //This is your HUD pass
                    spriteBatch.Begin();

                    if (player.HasPickedUp)
                    {
                        if (player.WEquipped)
                            spriteBatch.Draw(weaponIcon, wpnIconRect, Color.White);
                        else
                            spriteBatch.Draw(weaponIcon, wpnIconRect, Color.Black);
                    }

                    switch (aCurrentArmour)
                    {
                        case 0:
                            spriteBatch.Draw(armourIcons[0], armIconRect, Color.Black);
                            break;
                        case 25:
                            spriteBatch.Draw(armourIcons[0], armIconRect, Color.White);
                            break;
                        case 50:
                            spriteBatch.Draw(armourIcons[2], armIconRect, Color.White);
                            break;
                        case 75:
                            spriteBatch.Draw(armourIcons[3], armIconRect, Color.White);
                            break;
                        case 100:
                            spriteBatch.Draw(armourIcons[4], armIconRect, Color.White);
                            break;
                    }

                    spriteBatch.End();

                    base.Draw(gameTime);
                    break;
                #endregion



                case GameState.Quiting:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Background/MainMenuBKAgile"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    foreach (cButton b in quitButtons)
                    {
                        b.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    break;

                case GameState.ConfirmQuiting:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Background/MainMenuBKAgile"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    foreach (cButton b in quitButtons)
                    {
                        b.Draw(spriteBatch);
                    }
                    spriteBatch.End();
                    break;
            }
            
        }

        private void Shoot()
        {
            switch(player.CurrentDirection)
            {
                case Direction.Up:
                    bullets.Add(new BulletM(bulletTex, new Vector2(player.pos.X + player.destRect.Width, player.pos.Y + ((player.destRect.Height / 2) - 5)), player.CurrentDirection));
                    break;
                case Direction.Down:
                    bullets.Add(new BulletM(bulletTex, new Vector2(player.pos.X - 10, player.pos.Y + ((player.destRect.Height / 2) - 5)), player.CurrentDirection));
                    break;
                case Direction.Left:
                    bullets.Add(new BulletM(bulletTex, new Vector2(player.pos.X - 10, player.pos.Y + ((player.destRect.Height / 2) - 5)), player.CurrentDirection));
                    break;
                case Direction.Right:
                    bullets.Add(new BulletM(bulletTex, new Vector2(player.pos.X + player.destRect.Width, player.pos.Y + ((player.destRect.Height / 2) - 5)), player.CurrentDirection));
                    break;
            }
        }

        private void OnClick(GameTime gt, cButton button)
        {
            canTick = true;

            if (canTick)
                clickTimer += gt.ElapsedGameTime.Milliseconds;

            if (clickTimer > 100)
            {
                CurrentGameState = button.Destination;
                button.isClicked = false;
                canTick = false;
                clickTimer = 0;
            }

            if (button.Destination == GameState.ConfirmQuiting)
                Exit();
            else
                CurrentGameState = button.Destination;

        }

        void DrawBorders(Rectangle toDraw, int t, Color c)
        {
            spriteBatch.Draw(dummy, new Rectangle(toDraw.X, toDraw.Y, toDraw.Width, t), c);
            spriteBatch.Draw(dummy, new Rectangle(toDraw.X + toDraw.Width, toDraw.Y, t, toDraw.Height), c);
            spriteBatch.Draw(dummy, new Rectangle(toDraw.X, toDraw.Y + toDraw.Height, toDraw.Width, t), c);
            spriteBatch.Draw(dummy, new Rectangle(toDraw.X, toDraw.Y, 1, toDraw.Height), c);
        }
        
    }
    public enum GameState { MainMenu, Playing, ConfirmQuiting, Quiting, Playing2 }

    public enum Direction { Up, Down, Left, Right }
}
