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
    public abstract class Body
    {
        protected Shape debugDrawShape;

        public void resetCollision()
        {
            debugDrawShape.FillColor = Color.Black;
        }

        public bool checkCollision(Body other)
        {
            bool isColliding = false;

            if (this is CircleBody && other is CircleBody)
            {
                isColliding = checkCollision(this as CircleBody, other as CircleBody);
            }
            else if (this is LineBody && other is CircleBody)
            {
                isColliding = checkCollision(this as LineBody, other as CircleBody);
            }
            else if (this is CircleBody && other is LineBody)
            {
                isColliding = checkCollision(this as CircleBody, other as LineBody);
            }
            else if (this is LineBody && other is LineBody)
            {
                isColliding = checkCollision(this as LineBody, other as LineBody);
            }

            if (isColliding)
            {
                this.debugDrawShape.FillColor = Color.Red;
                other.debugDrawShape.FillColor = Color.Red;
            }

            return isColliding;
        }

        private static bool checkCollision(CircleBody b1, CircleBody b2)
        {
            float radiusSum = b1.radius + b2.radius;
            return Vector2.distanceSqr(b1.midPoint, b2.midPoint) < radiusSum * radiusSum;
        }

        private static bool checkCollision(LineBody line, CircleBody circle)
        {
            return checkCollision(circle, line);
        }

        private static bool checkCollision(CircleBody circle, LineBody line)
        {
            // project circleMidpoint on supporting line
            float projectedLength = Vector2.dot(circle.midPoint - line.start, line.direction);

            float circleRadius = circle.radius;
            float circleRadiusSqr = circleRadius * circleRadius;
            if(projectedLength < 0)
            {
                return Vector2.distanceSqr(circle.midPoint, line.start) < circleRadiusSqr;
            }
            else if (projectedLength > line.length)
            {
                return Vector2.distanceSqr(circle.midPoint, line.end) < circleRadiusSqr;
            }
            else
            {
                Vector2 projectedPoint = line.start + (line.direction * projectedLength);
                return Vector2.distanceSqr(projectedPoint, circle.midPoint) < circleRadiusSqr;
            }
        }

        private static bool checkCollision(LineBody line1, LineBody line2)
        {
            // check if parallel
            if(Vector2.dot(line1.direction, line2.direction) == 0)
            {
                // check if collinear
                if (Vector2.dot(line2.start - line1.start, line2.direction) == 0)
                {
                    float l2StartProjectedOnl1Length = Vector2.dot(line2.start - line1.start, line1.direction);
                    float l2EndProjectedOnl1Length = Vector2.dot(line2.end - line1.start, line1.direction);

                    // check for intersection
                    if (l2StartProjectedOnl1Length < 0 && l2EndProjectedOnl1Length < 0 || l2StartProjectedOnl1Length > line1.length && l2EndProjectedOnl1Length > line1.length)
                    {
                        return true;
                    }
                    else // not intersecting
                        return false;
                }
                else// not collinear
                    return false;
            }
            else// not parallel
            {
                // normal for l1
                Vector2 n1 = line1.direction.right;

                // TODO: check non-parallel case
                //
                return false;
            }
        }

        public abstract void debugDraw(RenderWindow win, View view);
    }
}
