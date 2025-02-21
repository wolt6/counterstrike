﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using FarseerGames.FarseerPhysics.Mathematics;

namespace CounterStrikeLive
{
    public class Explosion : GameObj
    {
        protected Vector2 _OldPosition;
        RotateTransform _RotateTransform = new RotateTransform();
        protected void UpdateTranslations()
        {
            Canvas.SetLeft(_Canvas, _x);
            Canvas.SetTop(_Canvas, _y);
            _RotateTransform.Angle = _Angle;
        }
        private float angle;
        public float _Angle
        {
            get
            {
                if (angle == float.NaN) return 0;
                return angle;
            }
            set
            {
                if (value == float.NaN) return;
                angle = CorrectAngle(value);
            }
        }

        public static float CorrectAngle(float value)
        {
            float angle;
            angle = value % 360;
            if (angle < 0) angle = 360 + angle;
            return angle;
        }

        public float _width { get { return _AnimatedBitmap._Width; } set { _AnimatedBitmap._Width = value; } }
        public float _height { get { return _AnimatedBitmap._Height; } set { _AnimatedBitmap._Height = value; } }



        public float _x
        {
            get { return _Position.X; }
            set
            {
                if (value == float.NaN) throw new Exception("Break");
                _OldPosition.X = position.X = value;
            }
        }
        public float _y { get { return _Position.Y; } set { _OldPosition.Y = position.Y = value; } }
        public Database.AnimatedBitmap _AnimatedBitmap;
        public float _frame;
        public bool _Remove = true;

        public Canvas _Canvas2 = new Canvas();
        public virtual void Load()
        {
            if (!_VisibleToAll) this._Visibility = Visibility.Collapsed;

            _Canvas2.RenderTransform = _RotateTransform;

            _Image = _AnimatedBitmap.GetImage();
            _Canvas2.Children.Add(_Image);
            _Canvas.Children.Add(_Canvas2);

            UpdateTranslations();
            Add();
        }
        public virtual void Add()
        {
            _Explosions.Add(this);
            _PaternCanvas.Children.Add(_Canvas);
        }
        public virtual List<Explosion> _Explosions { get { return _Game._Explosions; } set { } }
        public virtual Canvas _PaternCanvas { get { return _Map._Canvas; } set { } }
        public virtual void Remove()
        {
            _Explosions.Remove(this);
            _PaternCanvas.Children.Remove(_Canvas);
        }

        public Image _Image;
        public bool _isPlaying = true;
        public virtual void Update()
        {
            if (_isPlaying)
            {
                _frame += (float)(30 * STimer._SecodsElapsed);
                if (_frame < _AnimatedBitmap._BitmapImages.Count)
                {
                    _Image.Source = _AnimatedBitmap._BitmapImages[(int)_frame];
                }
                else
                {
                    if (_Remove == true) Remove();
                    _isPlaying = false;
                    _frame = 0;
                }
            }
        }
    }
}
