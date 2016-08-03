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

        public Body()
        {
            Console.WriteLine("Creating: " + this);
            BodyManager.Add(this);
        }

        public void ResetCollision()
        {
            debugDrawShape.FillColor = Color.Black;
        }

        public bool CheckCollision(Body other)
        {
            Vector2 tmp = Vector2.Zero;
            return CheckCollision(other, ref tmp);
        }

        public bool CheckCollision(Body other, ref Vector2 approximateCollisionPoint)
        {
            bool isColliding = false;

            if (this is CircleBody && other is CircleBody)
            {
                isColliding = CheckCollision(this as CircleBody, other as CircleBody, ref approximateCollisionPoint);
            }
            else if (this is LineSegmentBody && other is CircleBody)
            {
                isColliding = CheckCollision(this as LineSegmentBody, other as CircleBody, ref approximateCollisionPoint);
            }
            else if (this is CircleBody && other is LineSegmentBody)
            {
                isColliding = CheckCollision(this as CircleBody, other as LineSegmentBody, ref approximateCollisionPoint);
            }
            else if (this is LineSegmentBody && other is LineSegmentBody)
            {
                isColliding = CheckCollision(this as LineSegmentBody, other as LineSegmentBody, ref approximateCollisionPoint);
            }

            if (isColliding)
            {
                this.debugDrawShape.FillColor = Color.Red;
                other.debugDrawShape.FillColor = Color.Red;
            }

            return isColliding;
        }

        /// <summary>
        /// checks for collision. If collision occurs, both bodies are informed.
        /// </summary>
        /// <param name="other">body to check collision with</param>
        /// <returns>if a collision has occured</returns>
        public bool CheckAndInformCollision(Body other)
        {
            Vector2 approximateCollisionPoint = Vector2.Zero;

            bool isColliding = CheckCollision(other, ref approximateCollisionPoint);

            if (isColliding)
            {
                OnCollision(other, approximateCollisionPoint);
                other.OnCollision(this, approximateCollisionPoint);
            }

            return isColliding;
        }

        private static bool CheckCollision(CircleBody b1, CircleBody b2, ref Vector2 approximateCollisionPoint)
        {
            float radiusSum = b1.radius + b2.radius;
            if(Vector2.DistanceSqr(b1.midPoint, b2.midPoint) <= radiusSum * radiusSum)
            {
                float distance = Vector2.Distance(b1.midPoint, b2.midPoint);
                float overlapSize = radiusSum - distance;
                Vector2 direction = distance != 0 ? (b2.midPoint - b1.midPoint) / distance : Vector2.Zero;
                approximateCollisionPoint = b1.midPoint + (b1.radius - overlapSize * 0.5F) * direction;

                return true;
            }
            return false;
        }

        private static bool CheckCollision(LineSegmentBody line, CircleBody circle, ref Vector2 approximateCollisionPoint)
        {
            return CheckCollision(circle, line, ref approximateCollisionPoint);
        }

        private static bool CheckCollision(CircleBody circle, LineSegmentBody line, ref Vector2 approximateCollisionPoint)
        {
            // project circleMidpoint on supporting line
            approximateCollisionPoint = line.NearestPositionOnSegment(circle.midPoint);

            float circleRadius = circle.radius;
            float circleRadiusSqr = circleRadius * circleRadius;
            return Vector2.DistanceSqr(circle.midPoint, approximateCollisionPoint) <= circleRadiusSqr;
        }

        private static bool CheckCollision(LineSegmentBody lineSeg1, LineSegmentBody lineSeg2, ref Vector2 approximateCollisionPoint)
        {
            Vector2 lineSeg1Start = lineSeg1.start;
            Vector2 lineSeg1End = lineSeg1.end;
            Vector2 lineSeg2Start = lineSeg2.start;
            Vector2 lineSeg2End = lineSeg2.end;

            // check if parallel
            if(Math.Abs(Vector2.Dot(lineSeg1.direction, lineSeg2.direction)) == 1)
            {
                // check if collinear
                if (Vector2.Dot(lineSeg2Start - lineSeg1Start, lineSeg1.direction.right) == 0)
                {
                    //Projection of lineSeg2 onto lineSeg1's supporting line
                    float projectedStart = Vector2.Dot(lineSeg2Start - lineSeg1Start, lineSeg1.direction);
                    float projectedEnd = Vector2.Dot(lineSeg2End - lineSeg1Start, lineSeg1.direction);

                    // check for intersection
                    if (projectedStart < 0 && projectedEnd < 0 || projectedStart > lineSeg1.length && projectedEnd > lineSeg1.length)
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
                // normal for lineSeg1
                Vector2 n1 = lineSeg1.direction.right;

                // project lineSeg2 on n1's supporting line
                float projectedStart = Vector2.Dot(lineSeg2Start - lineSeg1Start, n1);
                float projectedEnd = Vector2.Dot(lineSeg2End - lineSeg1Start, n1);

                // check if lineSeg2 is partly to lineSeg1's left side and right side
                if ((projectedStart <= 0 && projectedEnd >= 0) || (projectedStart >= 0 && projectedEnd <= 0))
                {
                    // find the position where lineSeg2 intersects lineSeg1's supporting line
                    float t = projectedStart / (projectedStart - projectedEnd);
                    Vector2 intersectingPoint = Vector2.Lerp(lineSeg2Start, lineSeg2End, t);

                    // check if intersectingPoint lies in lineSeg1
                    float projectedIntersectingPoint = Vector2.Dot(intersectingPoint - lineSeg1Start, lineSeg1.direction);
                    if (0F <= projectedIntersectingPoint && projectedIntersectingPoint <= lineSeg1.length)
                        return true;
                }
                return false;
            }
        }

        protected abstract void OnCollision(Body other, Vector2 approximateCollisionPoint);

        public abstract void DebugDraw(RenderWindow win, View view);
    }
}
