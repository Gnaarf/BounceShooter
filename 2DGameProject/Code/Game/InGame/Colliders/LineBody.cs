using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    public class LineBody : Body
    {
        public Vector2 start { get; set; }
        private Vector2 _direction;
        public Vector2 direction { get { return _direction; } set { _direction = value.normalized; } }
        public float length { get; set; }

        public Vector2 end { get { return start + (length * direction); } }

        public LineBody(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.length = Vector2.distance(start, end);
            this.direction = (end - start) / length;

            debugDrawShape = new RectangleShape(new Vector2(1, 1));
        }

        public LineBody(Vector2 start, Vector2 direction, float length)
            : this(start, start + length * direction)
        {
        }

        public override void debugDraw(RenderWindow win, View view)
        {
            debugDrawShape.Position = start;
            debugDrawShape.Scale = new Vector2f(length, 1);
            debugDrawShape.Rotation = direction.rotation * Helper.RadianToDegree;
            win.Draw(debugDrawShape);
        }
    }
}
