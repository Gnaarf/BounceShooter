﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace GameProject2D
{
    class Wall : LineSegmentBody
    {
        public Wall(Vector2 start, Vector2 end)
            : base(start, end)
        {
        }

        public Wall(Vector2 start, Vector2 direction, float length)
            : base(start, direction, length)
        {
        }

        protected override void OnCollision(Body other)
        {
            //Make Particles appear and stuff
        }

        public override void debugDraw(RenderWindow win, View view)
        {
            base.debugDraw(win, view);
        }
    }
}
