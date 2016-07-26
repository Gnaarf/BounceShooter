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
    public abstract class CircleBody : Body
    {
        public Vector2 midPoint { get; set; }
        public float radius { get; set; }
        
        public CircleBody(Vector2 midPoint, float radius)
            : base()
        {
            this.midPoint = midPoint;
            this.radius = radius;

            debugDrawShape = new CircleShape(radius);
        }

        public override void DebugDraw(RenderWindow win, View view)
        {
            debugDrawShape.Position = midPoint;
            debugDrawShape.Origin = Vector2.One * radius;

            win.Draw(debugDrawShape);

            CircleShape midPointShape = new CircleShape(1);
            midPointShape.Position = midPoint;
            midPointShape.FillColor = Color.Red;
            win.Draw(midPointShape);
        }
    }
}
