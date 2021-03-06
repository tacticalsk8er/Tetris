﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    //Created by Nick Peterson and Daric Sage
    //Features for Possible Extra Credit Include:
    //Dynamically resizable
    //Ghost Game Piece
    [Serializable]
    public class GameInfoView {

        private delegate void TimerDel(int level);

        [NonSerialized]
        private MainForm _mainForm;
        private Rectangle _view;

        private TimerDel timerNotifier;
        
        private TComponents<GamePieces> _nextBlock;
        private TComponents<int> _score;
        private TComponents<int> _lines;
        private TComponents<int> _level;
        
        public GameInfoView(Game game, Rectangle view = new Rectangle()) {
            _view = view;
            
            _nextBlock = new TNextBlockComponent();
            _score = new TNumberComponent("Score");
            _lines = new TNumberComponent("Lines");
            _level = new TNumberComponent("Level");

            _level.detail = 1;

            timerNotifier = game.updateTimer;
            _mainForm = game.MainForm;
        }


        public Rectangle view {
            get {
                return _view;
            }

            set {
                _view = value;
            }
        }

        public MainForm MainForm {
            get {
                return _mainForm;
            }

            set {
                _mainForm = value;
            }
        }

        public int getScore()
        {
            return _score.detail;
        }

        public int getLines()
        {
            return _lines.detail;
        }

        public int getLevel()
        {
            return _level.detail;
        }

        public void addNextBlock(GamePieces nextPiece)
        {
            _nextBlock.detail = nextPiece;
        }

        public void addToScore(int scoreToAdd)
        {
            if(scoreToAdd > 0)
            {
              _score.detail += scoreToAdd * _level.detail;
            }
        }

        public void addToLine(int linesToAdd)
        {
            if(linesToAdd > 0)
            {
                int combo = linesToAdd - Constants.GAME_COMBO_LINES;
                int score = (linesToAdd * Constants.GAME_LINE_SCORE + combo * Constants.GAME_COMBO_SCORE_BONUS);

                if (_lines.detail + linesToAdd >= Constants.GAME_LINES_PER_LEVEL)
                {
                    addToLevel();
                }
                _lines.detail = (_lines.detail + linesToAdd) % 10;

                addToScore(score);
                _mainForm.PlayLineSound();
            }
        }

        public void addToLevel()
        {
            _level.detail += (Constants.GAME_LEVEL_INCREMENT);
            timerNotifier(_level.detail);
            _mainForm.PlayLeveUpSound();
        }

        public void draw(Graphics g) {

            Brush brush = new SolidBrush(Constants.INFO_BACKGROUND_COLOR);
            
            g.FillRectangle(brush, _view);

            Rectangle[] components = _view.splitVertically(Constants.GAME_INFO_RECTS);

            setComponents(components);

            _nextBlock.draw(g);
            _score.draw(g);
            _lines.draw(g);
            _level.draw(g);

            brush.Dispose();
        }

        private void setComponents(Rectangle[] components)
        {
            _nextBlock.box = components[0];
            _score.box = components[1];
            _lines.box = components[2];
            _level.box = components[3];
        }
    }
}
