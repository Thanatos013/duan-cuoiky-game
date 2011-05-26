﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace MyFirstApp
{
    public class Map : VisibleGameEntity
    {
        #region 1 - Các thuộc tính
        private int _pixelMove;
        private Rectangle _rec;
        private int _DelayTime;
        private int _pixelMoved;
        private char[,] _map;
        private int _mapRows;
        private int _mapCols;
        private Texture2D _background;
        public static int CellSize;
        #endregion

        #region 2 - Các đặc tính
        public int PixelMove
        {
            get { return _pixelMove; }
            set { _pixelMove = value; }
        }
        public Rectangle Rec
        {
            get { return _rec; }
            set { _rec = value; }
        }
        public int DelayTime
        {
            get { return _DelayTime; }
            set { _DelayTime = value; }
        }
        #endregion

        #region 3 - Các phương thức khởi tạo
        public Map()
        {
            _pixelMove = 1;
            _DelayTime = 0;
        }
        #endregion

        #region 4 - Các phương thức xử lý
        public override bool Init(ContentManager Content, int n, string strResource)
        {
            _pixelMoved = 0;
            _pixelMove = 10;
            _DelayTime = 0;
            _rec = new Rectangle(0, 0, Game1.iWidth, Game1.iHeight);            
            _nsprite = n;
            _sprite = new MySprite[_nsprite];

            for (int i = 0; i < _nsprite; ++i)
            {
                Texture2D[] texture2D;
                texture2D = new Texture2D[1];
                texture2D[0] = Content.Load<Texture2D>(@"Maingame/" + strResource);
                _sprite[i] = new MySprite(texture2D, 0.0f, 0.0f, texture2D[0].Width, texture2D[0].Height);
            }
            
            CellSize = _sprite[0].Height;
            _map = null;

            return true;
        }
        public void ReadMap(ContentManager Content, string strTextFile, int rRows, int rCols)
        {
            _map = new char[rRows, rCols];
            _mapRows = rRows;
            _mapCols = rCols;
            System.IO.StreamReader file =
                new System.IO.StreamReader(
                    Content.RootDirectory + @"\Maingame\" + strTextFile);
            string line = string.Empty;
            int i = 0;
            while ((line = file.ReadLine()) != null)
            {
                for (int j = 0; j < line.Length; ++j)
                    _map[i, j] = line[j];
                i++;
            }
        }
        public void InitBackground(ContentManager Content, string strResource)
        {
            _background = Content.Load<Texture2D>(@"Maingame/" + strResource);
        }
        public void UpdateKeyboard(int rWidth, int rHeight)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _pixelMoved++;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                _pixelMoved--;
            }
            else if (keyboardState.IsKeyDown(Keys.Back))
            {
                Game1.bMainGame = false;
            }
            //Xet xem da di den cuoi cua ban do hay chua
            //Neu da den cuoi thi khong the di tiep
            if (_pixelMoved < 0)
                _pixelMoved = 0;
            if (_pixelMoved >= _mapCols - Math.Ceiling((float)Game1.iWidth/_sprite[0].Height))
                _pixelMoved = _mapCols - (int)Math.Ceiling((float)Game1.iWidth / _sprite[0].Height);            

        }
        public override void Update(GameTime gameTime)
        {
            //for (int i = 0; i < _nsprite; ++i)
            //    _sprite[i].Update(gameTime);
        }        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Chua nap duoc ban do thi khong ve
            if (_map == null)
            {
                return;
            }
            //Ve ra anh nen
            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

            int i;
            int j;
            
            int cellIndex = 0;            
            int width = (int)Math.Ceiling((float)Game1.iWidth/(CellSize));
            width = _mapCols < width ? _mapCols : width;
            for (i = 0; i < _mapRows; ++i)
                for (j = 0; j < width; ++j)
                {                    
                    if (_map[i, j + _pixelMoved] == '?')
                        continue;
                    else if (_map[i, j + _pixelMoved] <= ']' && _map[i, j + _pixelMoved] >= 'A')
                        cellIndex = _map[i, j + _pixelMoved] - 65;
                    else
                        continue;
                    _sprite[0].Draw(gameTime, spriteBatch, Color.White,
                        new Vector2(j * CellSize, i * CellSize),
                        new Rectangle(cellIndex * CellSize, 0, CellSize, CellSize));                    
                }
            
            if (_DelayTime++ < 300)
                spriteBatch.DrawString(Game1.gameFont, "Press left or right to move"
                + "\r\nPress Backspace to go back to main menu"
                + "\r\nClick mouse to teleport Megario",
                new Vector2(20, 100), Color.Blue);
        }
        #endregion
    }
}
