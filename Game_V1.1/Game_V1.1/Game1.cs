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
using System.Configuration;
using System.Threading;
using Game_V1._2.Data_Items;

namespace Game_V1._2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Communicator cmn;

        int serverPortNumber;
        String serverIP;
        String clientIP;
        int clientPortNumber;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        int screenWidth;
        int screenHeight;
        int mapSize;
        int offset;
        int myNumber;
        float scale;
        int numberOfPlayers;
        Player[] players;
        bool playersSet;
        bool gameInitializing;
        bool gameFinished;
        bool playerDead;

        bool sendOK;

        Texture2D backTexture;
        Texture2D whiteTexture;
        Texture2D squareTexture;
        Texture2D brick100Texture;
        Texture2D brick75Texture;
        Texture2D brick50Texture;
        Texture2D brick25Texture;
        Texture2D stoneTexture;
        Texture2D waterTexture;
        Texture2D tankTexture;
        Texture2D starTexture;
        Texture2D coinTexture;
        Texture2D lifeTexture;
        Texture2D initTexture;
        Texture2D finishTexture;
        Texture2D bulletTexture;
        Texture2D arenaTexture;
        Texture2D logoTexture;
        SpriteFont font;
        SpriteFont heading;
        Texture2D tableBack;
        Texture2D rect;

        Data_Items.Square[] squares;
        List<Data_Items.Coin> coinPiles = new List<Data_Items.Coin>();
        List<Data_Items.LifePack> lifePacks = new List<Data_Items.LifePack>();
        List<Data_Items.Bullet> bullets = new List<Data_Items.Bullet>();
        int imageSize;

        List<int> commands;
        List<Square> sqrPath;
        Data_Items.Coin targetCoin;
        Data_Items.LifePack targetPack;
        int lastTime;
        String opMode;
        int lastCommand;
        Square lastSquare;

        bool isInRange;
        int rangeDir;
        bool skip;

        float shootcount;
        Thread com;

        public Game1(String serverIP, String clientIP, int serverPort, int clientport, int map)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.serverIP = serverIP;
            this.clientIP = clientIP;
            this.serverPortNumber = serverPort;
            this.clientPortNumber = clientport;
            this.mapSize = map;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Util.Constants.ScreenWidth;
            graphics.PreferredBackBufferHeight = Util.Constants.ScreenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Battle Tanks";
            //mapSize = Util.Constants.MapSize;         
            scale = Util.Constants.Scale;   
            offset = Util.Constants.Offset;
            imageSize = Util.Constants.ImageSize;
            //Console.WriteLine(imageSize);
            cmn = new Communicator(serverIP,clientIP, serverPortNumber, clientPortNumber);
            playersSet = false;
            commands = new List<int>();
            sqrPath = new List<Square>();
            
            targetCoin = new Data_Items.Coin(0,0,0,0,0);
            targetPack = new Data_Items.LifePack(0, 0, 0, 0);
            lastTime = 0;
            shootcount = (float)0.1;
            sendOK = false;
            opMode = "";
            lastCommand = -1;
            lastSquare = null;

            isInRange = false;
            rangeDir = 0;
            skip = false;

            playerDead = false;
            gameInitializing = false;
            gameFinished = false;

            rect = new Texture2D(graphics.GraphicsDevice, 90, 30);
            Color[] data = new Color[90 * 30];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            rect.SetData(data);

            tableBack = new Texture2D(graphics.GraphicsDevice, 385, 215);
            data = new Color[385 * 215];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;
            tableBack.SetData(data);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            squareTexture = Content.Load<Texture2D>("grass1");
            brick100Texture = Content.Load<Texture2D>("brick100");
            brick75Texture = Content.Load<Texture2D>("brick75");
            brick50Texture = Content.Load<Texture2D>("brick50");
            brick25Texture = Content.Load<Texture2D>("brick25");
            stoneTexture = Content.Load<Texture2D>("stone");
            waterTexture = Content.Load<Texture2D>("water");
            backTexture = Content.Load<Texture2D>("background");
            whiteTexture = Content.Load<Texture2D>("white");
            tankTexture = Content.Load<Texture2D>("tank");
            starTexture = Content.Load<Texture2D>("star");
            coinTexture = Content.Load<Texture2D>("coin");
            lifeTexture = Content.Load<Texture2D>("life");
            bulletTexture = Content.Load<Texture2D>("bullet");
            logoTexture = Content.Load<Texture2D>("tanklogo");
            arenaTexture = Content.Load<Texture2D>("arena");
            font = Content.Load<SpriteFont>("myFont");
            heading = Content.Load<SpriteFont>("Texture");
            heading.Spacing = 8;
            initTexture = Content.Load<Texture2D>("init");
            finishTexture = Content.Load<Texture2D>("finished");

            squares = new Data_Items.Square[mapSize * mapSize];
            SetUpArena();            
            com = new Thread(new ThreadStart(cmn.joinGame));
            com.Start();
            if (!cmn.launched)
                this.Exit();

            
        }

        private void SetUpArena()   //Fill the square array according to map size
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    int index = i + (j * mapSize);
                    squares[index] = new Data_Items.Square(i, j, 0);
                }
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            cmn.ended = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            if (gameFinished)   //if the game is finished, abort the communication thread
            {
                com.Abort();
            }

            if (!skip)          //if too quick hasn't received earlier
            {
                if ((playersSet) && (players[myNumber].isAlive))    //if players are set and my player is alive
                {
                    if (opMode.Equals("coin") && !coinPiles.Contains(targetCoin))
                    {
                        //if the targeting coin pile has disappeared
                        ClearPath();
                    }
                    else if (opMode.Equals("life") && !lifePacks.Contains(targetPack))
                    {
                        //if the targeting lifepack pile has disappeared
                        ClearPath();
                    }
                    else if (opMode.Equals("center") && ((coinPiles.Count > 0)||(lifePacks.Count > 0)))
                    {
                        //if coin piles or lifepacks appeared
                        ClearPath();
                    }

                    if ((targetCoin == null) && (targetPack == null))
                    {
                        //if no coin piles or lifepacks
                        MoveToCenter();
                    }

                    if ((commands.Count == 0))  //if commands are available
                    {
                        if ((coinPiles.Count == 0) && (lifePacks.Count == 0))
                        {
                            //if no coin piles or no lifepacks, move to center
                            MoveToCenter();
                        }

                        else if (((players[myNumber].health >= 80) || (lifePacks.Count == 0)) && (coinPiles.Count != 0))
                        {
                            //if the health is more than 80 and coin piles are present, get coins
                            GetCoins(gameTime.TotalGameTime);
                        }

                        else
                        {
                            //if the health is less than 80 or only lifepacks are present, get lifepacks
                            GetHealth(gameTime.TotalGameTime);
                        }
                        lastTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    }

                    if (sendOK)
                    {
                        if (isInRange && InRange()) //is another player in shooting range
                        {
                            if (players[myNumber].direction == rangeDir)
                            {
                                cmn.shoot();
                            }
                            else
                            {
                                cmn.move(rangeDir);
                            }
                            isInRange = false;
                            sendOK = false;
                            ClearPath();
                        }
                        else if (commands.Count > 0) //if no player is in shooting range
                        {
                            ExecuteCommands();
                        }
                    }
                }
            }
            else
            {
                skip = false;
                sendOK = false;
            }

            //ProcessKeyBoard();
            DecodeMessage(gameTime);
            UpdateCoinPiles(gameTime.TotalGameTime);
            UpdateLifePacks(gameTime.TotalGameTime);
            base.Update(gameTime);
        }

        private void DecodeMessage(GameTime gt)     //decodes the messages received from the server
        {
            if (cmn.isNew)  //if new message is received
            {
                String output = cmn.output;
                cmn.isNew = false;

                if (output.ElementAt(1) == ':') //update messages in the format X:xxxxxxxxxxx
                {
                    char messageType = output.ElementAt(0);
                    output = output.Substring(2, output.Length - 3);

                    switch (messageType)
                    {
                        case ('I'):      //Initialize arena
                            InitArena(output);
                            gameInitializing = true;
                            break;
                        case ('S'):     //Setup players
                            SetPlayers(output);
                            gameInitializing = false;
                            break;
                        case ('G'):     //Global Update
                            UpdatePlayers(gt.TotalGameTime, output);
                            sendOK = true;
                            break;
                        case ('C'):     //Coin Pile
                            AddCoins(gt.TotalGameTime, output);
                            break;
                        case ('L'):      //Life pack
                            AddLifePack(gt.TotalGameTime, output);
                            break;                        
                    }
                }

                else //Special messages
                {
                    output = output.Substring(0, output.Length - 1);
                    
                    if (output == "OBSTACLE")
                    {
                        //ClearPath();
                    }
                    else if (output == "CELL_OCCUPIED")
                    {
                        ClearPath();
                    }
                    else if (output == "DEAD")
                    {
                        playerDead = true;
                        players[myNumber].isAlive = false;                        
                    }
                    else if (output == "TOO_QUICK")
                    {                        
                        skip = true;
                        ClearPath();
                    }
                    else if ((output == "INVALID_CELL")||(output.Contains("OBSTACLE")))
                    {
                        ClearPath();
                    }
                    else if (output == "GAME_FINISHED")
                    {
                        gameFinished = true;                        
                    }
                    else if (output == "GAME_HAS_FINISHED")
                    {
                        if(!gameFinished)
                            gameFinished = true;                        
                    }
                    else if (output == "GAME_NOT_STARTED_YET")
                    {

                    }
                    else if (output == "NOT_A_VALID_CONTESTANT")
                    {

                    }
                    else if (output == "PLAYERS_FULL")
                    {
                        
                    }
                    else if (output == "ALREADY_ADDED")
                    {

                    }
                    else if (output == "GAME_ALREADY_STARTED")
                    {

                    }
                    else
                    {
                       
                    }
                }

            }
        }

        private void InitArena(String str)      //initialize the arena
        {
            String[] mainParts = str.Split(':');
            this.myNumber = int.Parse(mainParts[0].ElementAt(1).ToString());

            //setting bricks
            String[] subParts = mainParts[1].Split(';');
            foreach (String st in subParts)
            {
                String[] coor = st.Split(',');
                int i = int.Parse(coor[0]);
                int j = int.Parse(coor[1]);
                int index = i + (j * mapSize);

                squares[index] = new Data_Items.Brick(i, j, 100);
            }

            //setting stones
            subParts = mainParts[2].Split(';');
            foreach (String st in subParts)
            {
                String[] coor = st.Split(',');
                int i = int.Parse(coor[0]);
                int j = int.Parse(coor[1]);
                int index = i + (j * mapSize);

                squares[index] = new Data_Items.Stone(i, j);
            }

            //setting water
            subParts = mainParts[3].Split(';');
            foreach (String st in subParts)
            {
                String[] coor = st.Split(',');
                int i = int.Parse(coor[0]);
                int j = int.Parse(coor[1]);
                int index = i + (j * mapSize);

                squares[index] = new Data_Items.Water(i, j);
            }
        }

        private void UpdateArena(String str)    //update the squares array
        {
            //< x>,<y>,<damage-level>;< x>,<y>,<damage-level>;< x>,<y>,<damage-level>;< x>,<y>,<damage-level>…..< x>,<y>,<damage-level>
            String[] mainParts = str.Split(';');

            foreach (String st in mainParts)
            {
                String[] subParts = st.Split(',');
                int x = int.Parse(subParts[0]);
                int y = int.Parse(subParts[1]);
                int damage = int.Parse(subParts[2]);

                int index = x + (y * mapSize);

                if (damage == 4)
                {
                    squares[index] = new Data_Items.Square(x, y, 0);
                }
                else
                {
                    squares[index] = new Data_Items.Brick(x, y, ((4 - damage) * 25));
                }
            }
        }

        private void SetPlayers(String str)     //initialize the players
        {
            Color[] playerColors = new Color[5];
            playerColors[0] = Color.Red;
            playerColors[1] = Color.Green;
            playerColors[2] = Color.Blue;
            playerColors[3] = Color.Purple;
            playerColors[4] = Color.DeepPink;

            String[] mainParts = str.Split(':');
            players = new Player[mainParts.Length];

            foreach (String s in mainParts)
            {
                String[] subParts = s.Split(';');
                int playerN = int.Parse(subParts[0].ElementAt(1).ToString());
                int x = int.Parse(subParts[1].Split(',')[0]);
                int y = int.Parse(subParts[1].Split(',')[1]);
                int d = int.Parse(subParts[2]);

                int index = x + (y * mapSize);

                players[playerN] = new Player(playerN, x, y, d, playerColors[playerN], squares[index]);
            }
            numberOfPlayers = mainParts.Length;
            playersSet = true;
        }

        private void UpdatePlayers(TimeSpan time, String str)   //update the players
        {
            String[] mainParts = str.Split(':');
            bool evade = false;
            String[] subParts = mainParts[myNumber].Split(';');
            int xMe = int.Parse(subParts[1].Split(',')[0]);
            int yMe = int.Parse(subParts[1].Split(',')[1]);
            int dirMe = int.Parse(subParts[2]);

            for (int i = 0; i < numberOfPlayers; i++)
            {
                //P1;< player location  x>,< player location  y>;<Direction>;< whether shot>;<health>;< coins>;< points>
                subParts = mainParts[i].Split(';');
                int playerN = int.Parse(subParts[0].ElementAt(1).ToString());
                int x = int.Parse(subParts[1].Split(',')[0]);
                int y = int.Parse(subParts[1].Split(',')[1]);
                int dir = int.Parse(subParts[2]);
                int s = int.Parse(subParts[3]);
                int health = int.Parse(subParts[4]);
                int coins = int.Parse(subParts[5]);
                int points = int.Parse(subParts[6]);

                int index = x + (y * mapSize);

                bool shot = false;
                if (s == 1)
                {
                    bullets.Add(new Data_Items.Bullet(new Vector2(x, y), dir,playerN));
                    //Console.WriteLine("Bullet added:" + playerN);
                }

                if ((i == myNumber) && (health < players[myNumber].health))
                {
                    evade = true;
                }

                if(i != myNumber)
                {
                    if (x == xMe)
                    {
                        isInRange = true;
                        if (y < yMe)
                        {
                            rangeDir = 0;
                        }
                        else
                        {
                            rangeDir = 2;
                        }
                    }
                    if (y == yMe)
                    {
                        isInRange = true;
                        if (x < xMe)
                        {
                            rangeDir = 3;
                        }
                        else
                        {
                            rangeDir = 1;
                        }
                    }   
                }


                if (((players[playerN].isAlive) && health <= 0) && (players[playerN].coins > 0))
                {
                    int currentTime = (int)time.TotalMilliseconds;
                    coinPiles.Add(new Data_Items.Coin(x, y, players[playerN].coins, (currentTime + 5000), 5000));
                }
                players[playerN].updatePlayer(x, y, dir, shot, health, coins, points, squares[index]);
            }

            UpdateArena(mainParts[numberOfPlayers]);

            if (evade)
            {
                Evade();
            }
        }

        private void AddCoins(TimeSpan current, String str)     //add new coin piles
        {
            //2,2:16548:1749#
            int currentTime = (int)current.TotalMilliseconds;
            String[] mainParts = str.Split(':');
            int x = int.Parse(mainParts[0].Split(',')[0]);
            int y = int.Parse(mainParts[0].Split(',')[1]);

            int duration = int.Parse(mainParts[1]);
            int value = int.Parse(mainParts[2]);

            coinPiles.Add(new Data_Items.Coin(x, y, value, (currentTime + duration), duration));
            ClearPath();
        }

        private void UpdateCoinPiles(TimeSpan t)    //update coin piles
        {
            int time = (int)t.TotalMilliseconds;

            for (int i = 0; i < coinPiles.Count; )
            {
                Data_Items.Coin tempCoin = coinPiles.ElementAt(i);
                int index = (int)(tempCoin.position.X + (tempCoin.position.Y * mapSize));

                if ((time >= tempCoin.endTime) || (squares[index].hasPlayer()))
                {
                    coinPiles.RemoveAt(i);
                }
                else
                {
                    coinPiles[i].UpdateTime(time);
                    //int remainT = tempCoin.endTime - time;
                    //coinPiles.RemoveAt(i);
                    //coinPiles.Insert(i, new Data_Items.Coin((int)tempCoin.position.X, (int)tempCoin.position.Y, tempCoin.value, tempCoin.endTime, remainT));
                    i++;
                }
            }
        }

        private void AddLifePack(TimeSpan current, String str)  //add new lifepacks
        {
            //5,6:18153#
            int currentTime = (int)current.TotalMilliseconds;
            String[] mainParts = str.Split(':');
            int x = int.Parse(mainParts[0].Split(',')[0]);
            int y = int.Parse(mainParts[0].Split(',')[1]);

            int duration = int.Parse(mainParts[1]);

            lifePacks.Add(new Data_Items.LifePack(x, y, (currentTime + duration), duration));
        }

        private void UpdateLifePacks(TimeSpan t)    //update the life packs
        {
            int time = (int)t.TotalMilliseconds;

            for (int i = 0; i < lifePacks.Count; )
            {
                Data_Items.LifePack tempLife = lifePacks.ElementAt(i);
                int index = (int)(tempLife.position.X + (tempLife.position.Y * mapSize));

                if ((time >= tempLife.endTime) || (squares[index].hasPlayer()))
                {
                    lifePacks.RemoveAt(i);
                }
                else
                {
                    lifePacks[i].UpdateTime(time);
                    //int remainT = tempLife.endTime - time;
                    //lifePacks.RemoveAt(i);
                    //lifePacks.Insert(i, new Data_Items.LifePack((int)tempLife.position.X, (int)tempLife.position.Y, tempLife.endTime, remainT));
                    i++;
                }
            }
        }

        private Vector2 GetScreenCoord(Vector2 coord)
        {
            int scaledSize = (int)(imageSize * scale);
            float x = (offset/2) + (scaledSize / 2) + (coord.X * scaledSize);
            float y = (offset/2) + (scaledSize / 2) + (coord.Y * scaledSize);

            return (new Vector2(x, y));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            
            if (gameFinished)
            {
                DrawBackground(Color.Brown * 0.7f);
                DrawFinished();
            }

            else
            {
                DrawBackground(Color.White);
                DrawArena();
                if (gameInitializing)
                {
                    DrawInit();
                }
                else
                {
                    DrawLogo();
                }
                DrawPlayers();
                DrawCoinPiles();
                DrawLifePacks();
                DrawTable();
                //DrawShooting();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBackground(Color clr)  //draw background texture
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backTexture, screenRectangle, clr);
        }

        private void DrawArena()    //draw arena
        {
            Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);

            foreach (Data_Items.Square square in squares)
            {
                Vector2 screenCood = GetScreenCoord(square.getCoord());

                Texture2D tempTexture = squareTexture;
                
                switch (square.getSType())
                {
                    case (0):
                        tempTexture = squareTexture;                        
                        break;
                    case (1):
                        Data_Items.Brick tempBrick = (Data_Items.Brick)square;
                        switch (tempBrick.getStrength())
                        {
                            case (100):
                                tempTexture = brick100Texture;
                                break;
                            case (75):
                                tempTexture = brick75Texture;
                                break;
                            case (50):
                                tempTexture = brick50Texture;
                                break;
                            case (25):
                                tempTexture = brick25Texture;
                                break;
                        }
                        break;
                    case (2):
                        tempTexture = stoneTexture;
                        break;
                    case (3):
                        tempTexture = waterTexture;
                        break;
                }

                spriteBatch.Draw(tempTexture, screenCood, null, Color.White, 0.0f, originS, scale, SpriteEffects.None, 0);
            }
        }

        private void DrawPlayers()  //draw players
        {
            if (gameFinished)
            {
                return;
            }

            if (playersSet)
            {
                Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);
                //Vector2 originS = new Vector2(10, 10);

                foreach (Player player in players)
                {
                    if (player.isAlive)
                    {
                        Vector2 screenCoord = GetScreenCoord(player.position);
                        float angle = 0.0f;
                        switch (player.direction)
                        {
                            case (0):
                                angle = MathHelper.ToRadians(0);
                                break;
                            case (1):
                                angle = MathHelper.ToRadians(90);
                                break;
                            case (2):
                                angle = MathHelper.ToRadians(180);
                                break;
                            case (3):
                                angle = MathHelper.ToRadians(270);
                                break;
                        }
                        //Console.WriteLine("player::"+player.playerNumber+":" + screenCoord.X + "," + screenCoord.Y);
                        spriteBatch.Draw(tankTexture, screenCoord, null, player.color, angle, originS, scale, SpriteEffects.None, 0);
                        if (player.playerNumber == this.myNumber)
                        {
                            spriteBatch.Draw(starTexture, screenCoord, null, Color.White, angle, originS, scale, SpriteEffects.None, 0);
                        }
                    }
                }
            }
        }

        private void DrawCoinPiles()    //draw coin piles
        {
            Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);

            foreach (Data_Items.Coin coinPile in coinPiles)
            {
                Vector2 screenCoord = GetScreenCoord(coinPile.position);
                spriteBatch.Draw(coinTexture, screenCoord, null, Color.White, 0.0f, originS, scale, SpriteEffects.None, 0);
            }
        }

        private void DrawLifePacks()    //draw life packs
        {
            Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);

            foreach (Data_Items.LifePack lifePack in lifePacks)
            {
                Vector2 screenCoord = GetScreenCoord(lifePack.position);
                spriteBatch.Draw(lifeTexture, screenCoord, null, Color.White, 0.0f, originS, scale, SpriteEffects.None, 0);
            }
        }

        private void DrawTable()       //draw points table
        {
            int leftMargin = (int)(mapSize * imageSize * scale) + (2 * offset);            

            spriteBatch.Draw(tableBack, new Vector2(leftMargin - 5 , 45), Color.White*0.5f);

            spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 0), 50), Color.Black);
            spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 1), 50), Color.Black);
            spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 2), 50), Color.Black);
            spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 3), 50), Color.Black);
            spriteBatch.DrawString(font, "  Player ", new Vector2(leftMargin + (95 * 0), 55), Color.Red);
            spriteBatch.DrawString(font, "  Points ", new Vector2(leftMargin + (95 * 1), 55), Color.Red);
            spriteBatch.DrawString(font, "  Coins ", new Vector2(leftMargin + (95 * 2), 55), Color.Red);
            spriteBatch.DrawString(font, "  Health ", new Vector2(leftMargin + (95 * 3), 55), Color.Red);


            for (int i = 0; i < numberOfPlayers; i++)
            {
                spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 0), 50 + 35 * (i + 1)), players[i].color);
                spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 1), 50 + 35 * (i + 1)), players[i].color);
                spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 2), 50 + 35 * (i + 1)), players[i].color);
                spriteBatch.Draw(rect, new Vector2(leftMargin + (95 * 3), 50 + 35 * (i + 1)), players[i].color);
                spriteBatch.DrawString(font, "  P" + players[i].playerNumber, new Vector2(leftMargin + (95 * 0), 55 + 35 * (i + 1)), Color.White);
                spriteBatch.DrawString(font, "  " + players[i].points, new Vector2(leftMargin + (95 * 1), 55 + 35 * (i + 1)), Color.White);
                spriteBatch.DrawString(font, "  " + players[i].coins, new Vector2(leftMargin + (95 * 2), 55 + 35 * (i + 1)), Color.White);
                spriteBatch.DrawString(font, "  " + players[i].health + " %", new Vector2(leftMargin + (95 * 3), 55 + 35 * (i + 1)), Color.White);

                if (players[i].playerNumber == this.myNumber)
                {
                    Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);
                    spriteBatch.Draw(starTexture, new Vector2(leftMargin - offset, 55 + 35 * (i + 1) + 10), null, Color.White, 0.0f, originS, scale, SpriteEffects.None, 0);
                }
            }

        }

        private void DrawInit()
        {
            int leftMargin = (int)(mapSize * imageSize * scale) + (2 * offset);
            DrawOverlay(initTexture, 385, 200, new Vector2(leftMargin - 5 + 190, screenHeight - 200));
        }

        private void DrawLogo()
        {
            int leftMargin = (int)(mapSize * imageSize * scale) + (2 * offset);
            DrawOverlay(logoTexture, 385, 300, new Vector2(leftMargin - 5 + 190, screenHeight - 200));
        }

        private void DrawFinished()
        {
            Player winner = players[0];
            //Player winner = new Player(1, 0, 0, 0, Color.Red, squares[0]);
            foreach (Player player in players)
            {
                if (player.points > winner.points)
                {
                    winner = player;
                }
            }
            Vector2 position = new Vector2(screenWidth / 2, 125 + (offset * 2));
            this.DrawOverlay(finishTexture, 400, 250, position);

            String[] texts = new String[] { "Player " + winner.playerNumber, "WON THE BATTLE", "with " + winner.points + " points" };
            int vPos = 250 + (offset * 6);

            for (int i = 0; i < texts.Length; i++)
            {
                String str = texts[i];
                Vector2 size = heading.MeasureString(str);
                Vector2 origin = size * 0.5f;

                Color txtColor = Color.GreenYellow;
                if (i == 0)
                    txtColor = Color.Yellow;

                spriteBatch.DrawString(heading, str, new Vector2(screenWidth / 2, vPos + (offset * 4 * i)), txtColor, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }


        }

        private void DrawOverlay(Texture2D texture, int width, int height, Vector2 position)
        {
            int offset = Util.Constants.Offset;
            spriteBatch.Draw(texture, position, null, Color.White, 0.0f, new Vector2(width / 2, height / 2), 1.0f, SpriteEffects.None, 0);
        }

        private void GetCoins(TimeSpan time)    //get coin piles
        {
            if ((coinPiles.Count == 0))
                return;
                        
            Navigator navg = new Navigator(mapSize);
            List<StarNode> bestPath;
            navg.GetBestCoin(squares, coinPiles.ToArray(), players[myNumber].currentSquare, players[myNumber].direction, out bestPath, out targetCoin);
            
            commands = navg.generateCommandList(bestPath, players[myNumber].direction);
            if ((commands.Count > 0)&&(bestPath.Count >0))
            {
                foreach (StarNode node in bestPath)
                {
                    sqrPath.Add(node.mySqr);
                }
            }
            opMode = "coin";
            //Console.WriteLine("Got command");            
        }

        private void GetHealth(TimeSpan time)   //get health packs
        {
            if (lifePacks.Count == 0)
                return;

            Navigator navg = new Navigator(mapSize);
            List<StarNode> bestPath; 
            navg.GetBestHealth(squares, lifePacks.ToArray(), players[myNumber].currentSquare, players[myNumber].direction, out bestPath, out targetPack);
              
            commands = navg.generateCommandList(bestPath, players[myNumber].direction);
            if ((commands.Count > 0) && (bestPath.Count > 0))
            {
                foreach (StarNode node in bestPath)
                {
                    sqrPath.Add(node.mySqr);
                }
            }
            opMode = "life";
        }

        private void ExecuteCommands()  //execute a list of commands
        {
            if ((players[myNumber].currentSquare.getCoord().X != sqrPath[0].getCoord().X) || (players[myNumber].currentSquare.getCoord().Y != sqrPath[0].getCoord().Y))
            {
                ClearPath();
                return;
            }
            
            
            int command = commands[0];

            if (command < 4)
            {
                if (this.VacantSquare(command))
                {
                    if (players[myNumber].direction == command)
                    {
                        lastSquare = sqrPath[0];
                        sqrPath.RemoveAt(0);
                    }
                    cmn.move(command);
                    lastCommand = commands[0];
                    commands.RemoveAt(0);                   
                }
                else
                {
                    cmn.shoot();
                }
            }
            else if (command == 5)
            {
                cmn.shoot();
                lastCommand = commands[0];
                commands.RemoveAt(0);
            }

            sendOK = false;
        }

        private bool VacantSquare(int command)  //checks whether the next square is vacant
        {
            int x = (int)players[myNumber].position.X;
            int y = (int)players[myNumber].position.Y;
            int index = x + (y * mapSize);

            if (command != players[myNumber].direction)
            {
                return true;
            }
            else if (command == 0)
            {
                index = x + ((y - 1) * mapSize);
            }
            else if (command == 1)
            {
                index = (x + 1) + (y * mapSize);
            }
            else if (command == 2)
            {
                index = x + ((y + 1) * mapSize);
            }
            else if (command == 3)
            {
                index = (x - 1) + (y * mapSize);
            }
            
            if((index < 0) || (index > (mapSize*mapSize - 1 ))){
                ClearPath();
                return false;
            }

            if (squares[index].hasPlayer())
            {
                return false;
            }
            return true;
        }

        private bool InRange()   //check whether another player is in shooting range
        {
            if (!isInRange)
            {
                return false;
            }

            int x = (int)players[myNumber].position.X;
            int y = (int)players[myNumber].position.Y;
            int dir = (int)players[myNumber].direction;

            switch (rangeDir)
            {
                case 0:
                    {
                        for (int j = 1; j <= y; j++)
                        {
                            int index = x + ((y - j) * mapSize);
                            Data_Items.Square tempSquare = squares[index];
                            if (/*(tempSquare.getSType() == 1) ||*/ (tempSquare.getSType() == 2))
                            {
                                return false;
                            }
                            else if (tempSquare.hasPlayer())
                            {
                                return true;
                            }
                        }
                        break;
                    }
                    
                case 1:
                    {
                        for (int i = 1; i <= (mapSize-1-x); i++)
                        {
                            int index = (x + i) + (y * mapSize);
                            Data_Items.Square tempSquare = squares[index];
                            if (/*(tempSquare.getSType() == 1) ||*/ (tempSquare.getSType() == 2))
                            {
                                return false;
                            }
                            else if (tempSquare.hasPlayer())
                            {
                                return true;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        for (int j = 1; j <= (mapSize - 1 - y); j++)
                        {
                            int index = x + ((y + j) * mapSize);
                            Data_Items.Square tempSquare = squares[index];
                            if (/*(tempSquare.getSType() == 1) ||*/ (tempSquare.getSType() == 2))
                            {
                                return false;
                            }
                            else if (tempSquare.hasPlayer())
                            {
                                return true;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        for (int i = 1; i <= x; i++)
                        {
                            int index = (x - i) + (y * mapSize);
                            Data_Items.Square tempSquare = squares[index];
                            if (/*(tempSquare.getSType() == 1) ||*/  (tempSquare.getSType() == 2))
                            {
                                return false;
                            }
                            else if (tempSquare.hasPlayer())
                            {
                                return true;
                            }
                        }
                        break;
                    }
            }

            return false;
        }

        private void DrawShooting()      //draw the bullets
        {
            Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);

            foreach (Data_Items.Bullet bullet in bullets)
            {
                Vector2 defV;
                bool hitObject = false;

                if (bullet.direction == 0) //up
                {
                    bullet.vector.Y = bullet.vector.Y - shootcount;
                }
                else if (bullet.direction == 2) // down
                {
                    bullet.vector.Y = bullet.vector.Y + shootcount;
                    //Console.WriteLine("bullet down y - " + bullet.vector.Y);

                }
                else if (bullet.direction == 1) // right
                {
                    bullet.vector.X = bullet.vector.X + shootcount;

                }
                else if (bullet.direction == 3) // left
                {
                    bullet.vector.X = bullet.vector.X - shootcount;

                }

                // *******new lines addded .............
                if (bullet.vector.X < 0 || bullet.vector.X > 9 || bullet.vector.Y < 0 || bullet.vector.Y > 9)
                {
                    hitObject = true;
                }
                else
                {

                    hitObject = CollisionDetected(new Vector2(bullet.vector.X, bullet.vector.Y), bullet);
                }
                //******upto here........

                if (bullet.vector.X < 0 || bullet.vector.X > mapSize || bullet.vector.Y < 0 || bullet.vector.Y > mapSize || hitObject == true)
                {
                    bullets.Remove(bullet);
                    break;
                }
                else
                {
                    float angle = 0.0f;
                    switch (bullet.direction)
                    {
                        case (0):
                            angle = MathHelper.ToRadians(0);
                            break;
                        case (1):
                            angle = MathHelper.ToRadians(90);
                            break;
                        case (2):
                            angle = MathHelper.ToRadians(180);
                            break;
                        case (3):
                            angle = MathHelper.ToRadians(270);
                            break;
                    }
                    defV = new Vector2(bullet.vector.X, bullet.vector.Y);
                    Vector2 vec = GetScreenCoord(defV);
                    spriteBatch.Draw(bulletTexture, vec, null, Color.White, angle, originS, scale, SpriteEffects.None, 0);
                }
            }

        }

        private bool CollisionDetected(Vector2 Newvec, Bullet bullet)   //check for collisions between bullets and other objects
        {
            Vector2 bulletNewCoor = Newvec;
            int bulletX = Convert.ToInt32(Math.Ceiling(bulletNewCoor.X));  //****** update line
            int bulletY = Convert.ToInt32(Math.Ceiling(bulletNewCoor.Y));   //**** update line
            Vector2 originS = new Vector2(imageSize / 2, imageSize / 2);
            bool hit = false;

            // hit on players
            for (int j = 0; j < numberOfPlayers; j++)
            {

                if (j == bullet.playerNo)
                {
                    continue;
                }

                if (new Vector2(bulletX, bulletY) == players[j].position)
                {
                    hit = true;
                    Vector2 vec = GetScreenCoord(bulletNewCoor);
                    spriteBatch.Draw(coinTexture, vec, null, Color.White, 0.0f, originS, 1.0f, SpriteEffects.None, 0);
                }
            }

            //hit on stones
            int index = bulletX + (bulletY * mapSize);
            //      Console.WriteLine("index Value - " + index + " y value - " + bulletY + " x val- " + bulletNewCoor.X);

            if (index < 0 || index > (mapSize*mapSize)-1)
            {
                hit = true;
            }
            else
            {
                int type = squares[index].getSType();

                if (type == 1 || type == 2)
                {
                    hit = true;
                }
            }
            return hit;
        }

        private void MoveToCenter() //move to the centermost cell
        {
            int x = mapSize/2;
            int y = mapSize/2;
            int index = x + ( y * mapSize );
            Square center = squares[index];

            int add = 1;
            bool succ = true;

            while (center.getSType() != 0)
            {
                x = x + add;
                add = -(add+1);

                if((x<0)||(x>mapSize-1)){
                    succ = false;
                    break;
                }
                index = x + (y * mapSize);
                center = squares[index];
            }

            if (!succ)
            {
                succ = true;
                while (center.getSType() != 0)
                {
                    y = y + add;
                    add = -(add + 1);

                    if ((y < 0) || (y > mapSize - 1))
                    {
                        succ = false;
                        break;
                    }
                    index = x + (y * mapSize);
                    center = squares[index];
                }
            }

            if (succ)
            {
                AStar ast = new AStar(squares, mapSize);
                List<StarNode> path = ast.calculatePath(players[myNumber].currentSquare, center, players[myNumber].direction);
                foreach (StarNode node in path)
                {
                    sqrPath.Add(node.mySqr);
                }
                Navigator navg = new Navigator(mapSize);
                commands = navg.generateCommandList(path, players[myNumber].direction);
                opMode = "center";
            }            
        }   

        private void ClearPath()    //clears the commands list
        {
            commands.Clear();
            sqrPath.Clear();
            targetPack = null;
            targetCoin = null;
        }

        private void Evade()
        {
            bool hor = false;
            bool ver = false;
            int vDir = 1;
            int hDir = 1;
            bool success = true;
            Square nextSquare = null;

            foreach (Player player in players)
            {
                if (player.playerNumber == myNumber)
                    continue;

                if (player.position.X == players[myNumber].position.X)
                {
                    if ((player.position.Y < players[myNumber].position.Y) && (player.direction == 2))
                    {
                        ver = true;
                        vDir = 1;

                    }
                    else if ((player.position.Y > players[myNumber].position.Y) && (player.direction == 0))
                    {
                        ver = true;
                        vDir = -1;
                    }
                }

                if (player.position.Y == players[myNumber].position.Y)
                {
                    if ((player.position.X < players[myNumber].position.X) && (player.direction == 1))
                    {
                        hor = true;
                        hDir = 1;
                    }
                    else if ((player.position.X > players[myNumber].position.X) && (player.direction == 3))
                    {
                        hor = true;
                        hDir = -1;
                    }
                }
            }

            //how to evade
            if (ver)
            {
                int x = (int)players[myNumber].position.X;
                int y = (int)players[myNumber].position.Y;

                if(x == 0){
                    while ((squares[GetIndex(x+1, y)].getSType() != 0) || (squares[GetIndex(x+1, y)].hasPlayer()))
                    {
                        if ((y == 0) || (y == mapSize - 1))
                        {
                            success = false;
                            break;
                        }
                        y = y + vDir;
                    }
                    if (success)
                    {
                        nextSquare = squares[GetIndex(x + 1, y)];
                    }
                }
                else if (x == mapSize - 1)
                {
                    while ((squares[GetIndex(x-1, y)].getSType() != 0) || (squares[GetIndex(x-1, y)].hasPlayer()))
                    {
                        if ((y == 0) || (y == mapSize - 1))
                        {
                            success = false;
                            break;
                        }
                        y = y + vDir;
                    }
                    if (success)
                    {
                        nextSquare = squares[GetIndex(x - 1, y)];
                    }
                }
                else
                {
                    while (((squares[GetIndex(x - 1, y)].getSType() != 0) || (squares[GetIndex(x - 1, y)].hasPlayer())) && ((squares[GetIndex(x + 1, y)].getSType() != 0) || (squares[GetIndex(x + 1, y)].hasPlayer())))
                    {
                        if ((y == 0) || (y == mapSize - 1))
                        {
                            success = false;
                            break;
                        }
                        y = y + vDir;
                    }
                    if (success)
                    {
                        if ((squares[GetIndex(x - 1, y)].getSType() == 0) && (!squares[GetIndex(x - 1, y)].hasPlayer()))
                        {
                            nextSquare = squares[GetIndex(x - 1, y)];
                        }
                        else if ((squares[GetIndex(x + 1, y)].getSType() == 0) && (!squares[GetIndex(x + 1, y)].hasPlayer()))
                        {
                            nextSquare = squares[GetIndex(x + 1, y)];
                        }                        
                    }
                }
                
            }
            //////////////////
            success = true;
            if (hor)
            {
                int x = (int)players[myNumber].position.X;
                int y = (int)players[myNumber].position.Y;

                if (y == 0)
                {
                    while ((squares[GetIndex(x, y+1)].getSType() != 0) || (squares[GetIndex(x, y+1)].hasPlayer()))
                    {
                        if ((x == 0) || (x == mapSize - 1))
                        {
                            success = false;
                            break;
                        }
                        x = x + hDir;
                    }
                    if (success)
                    {
                        nextSquare = squares[GetIndex(x, y+1)];
                    }
                }
                else if (y == mapSize - 1)
                {
                    while ((squares[GetIndex(x, y-1)].getSType() != 0) || (squares[GetIndex(x, y-1)].hasPlayer()))
                    {
                        if ((x == 0) || (x == mapSize - 1))
                        {
                            success = false;
                            break;
                        }
                        x = x + hDir;
                    }
                    if (success)
                    {
                        nextSquare = squares[GetIndex(x, y-1)];
                    }
                }
                else
                {
                    while (((squares[GetIndex(x , y-1)].getSType() != 0) || (squares[GetIndex(x, y-1)].hasPlayer())) && ((squares[GetIndex(x, y+1)].getSType() != 0) || (squares[GetIndex(x, y+1)].hasPlayer())))
                    {
                        if ((x == 0) || (x == mapSize - 1))
                        {
                            success = false;
                            break;
                        }
                        x = x + hDir;
                    }
                    if (success)
                    {
                        if ((squares[GetIndex(x, y - 1)].getSType() == 0) && (!squares[GetIndex(x, y - 1)].hasPlayer()))
                        {
                            nextSquare = squares[GetIndex(x, y - 1)];
                        }
                        else if ((squares[GetIndex(x, y + 1)].getSType() == 0) && (!squares[GetIndex(x, y + 1)].hasPlayer()))
                        {
                            nextSquare = squares[GetIndex(x, y + 1)];
                        }
                    }
                }
            }

            if (nextSquare != null)
            {
                Navigator navg = new Navigator(mapSize);
                AStar ast = new AStar(squares, mapSize);

                List<StarNode> path = ast.calculatePath(players[myNumber].currentSquare, nextSquare, players[myNumber].direction);
                foreach (StarNode node in path)
                {
                    sqrPath.Add(node.mySqr);
                }
                commands = navg.generateCommandList(path, players[myNumber].direction);
                opMode = "evade";
            }
            else
            {
                //call shoot method
            }
        }   //if shot, evade from that position

        private int GetIndex(int x, int y)
        {
            int index = x + (y * mapSize);
            return index;
        }
    }
}
