﻿using System;
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
            Vector2 approximatedCollisionPoint = Vector2.Zero;

            if (this is CircleBody && other is CircleBody)
            {
                isColliding = checkCollision(this as CircleBody, other as CircleBody, ref approximatedCollisionPoint);
            }
            else if (this is LineSegmentBody && other is CircleBody)
            {
                isColliding = checkCollision(this as LineSegmentBody, other as CircleBody, ref approximatedCollisionPoint);
            }
            else if (this is CircleBody && other is LineSegmentBody)
            {
                isColliding = checkCollision(this as CircleBody, other as LineSegmentBody, ref approximatedCollisionPoint);
            }
            else if (this is LineSegmentBody && other is LineSegmentBody)
            {
                isColliding = checkCollision(this as LineSegmentBody, other as LineSegmentBody, ref approximatedCollisionPoint);
            }

            if (isColliding)
            {
                this.debugDrawShape.FillColor = Color.Red;
                other.debugDrawShape.FillColor = Color.Red;
            }

            return isColliding;
        }

        public bool checkAndInformCollision(Body other)
        {
            bool isColliding = checkCollision(other);

            if (isColliding)
            {
                OnCollision(other);
            }

            return isColliding;
        }

        private static bool checkCollision(CircleBody b1, CircleBody b2, ref Vector2 approximateCollisionPoint)
        {
            float radiusSum = b1.radius + b2.radius;
            if(Vector2.distanceSqr(b1.midPoint, b2.midPoint) <= radiusSum * radiusSum)
            {
                float distance = Vector2.distance(b1.midPoint, b2.midPoint);
                float overlapSize = radiusSum - distance;
                Vector2 direction = (b2.midPoint - b1.midPoint) / distance;
                approximateCollisionPoint = b1.midPoint + (b1.radius - overlapSize * 0.5F) * direction;

                return true;
            }
            return false;
        }

        private static bool checkCollision(LineSegmentBody line, CircleBody circle, ref Vector2 approximateCollisionPoint)
        {
            return checkCollision(circle, line, ref approximateCollisionPoint);
        }

        private static bool checkCollision(CircleBody circle, LineSegmentBody line, ref Vector2 approximateCollisionPoint)
        {
            // project circleMidpoint on supporting line
            float projectedLength = Vector2.dot(circle.midPoint - line.start, line.direction);

            float circleRadius = circle.radius;
            float circleRadiusSqr = circleRadius * circleRadius;
            if(projectedLength < 0)
            {
                approximateCollisionPoint = line.start;
                return Vector2.distanceSqr(circle.midPoint, line.start) <= circleRadiusSqr;
            }
            else if (projectedLength > line.length)
            {
                approximateCollisionPoint = line.end;
                return Vector2.distanceSqr(circle.midPoint, line.end) <= circleRadiusSqr;
            }
            else
            {
                Vector2 projectedPoint = line.start + (line.direction * projectedLength);
                approximateCollisionPoint = projectedPoint;
                return Vector2.distanceSqr(projectedPoint, circle.midPoint) <= circleRadiusSqr;
            }
        }

        private static bool checkCollision(LineSegmentBody lineSeg1, LineSegmentBody lineSeg2, ref Vector2 approximatedCollisionPoint)
        {
            Vector2 lineSeg1Start = lineSeg1.start;
            Vector2 lineSeg1End = lineSeg1.end;
            Vector2 lineSeg2Start = lineSeg2.start;
            Vector2 lineSeg2End = lineSeg2.end;

            // check if parallel
            if(Math.Abs(Vector2.dot(lineSeg1.direction, lineSeg2.direction)) == 1)
            {
                // check if collinear
                if (Vector2.dot(lineSeg2Start - lineSeg1Start, lineSeg2.direction) == 0)
                {
                    //Projection of lineSeg2 onto lineSeg1's supporting line
                    float projectedStart = Vector2.dot(lineSeg2Start - lineSeg1Start, lineSeg1.direction);
                    float projectedEnd = Vector2.dot(lineSeg2End - lineSeg1Start, lineSeg1.direction);

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
                float projectedStart = Vector2.dot(lineSeg2Start - lineSeg1Start, n1);
                float projectedEnd = Vector2.dot(lineSeg2End - lineSeg1Start, n1);

                // check if lineSeg2 is partly to lineSeg1's left side and right side
                if ((projectedStart <= 0 && projectedEnd >= 0) || (projectedStart >= 0 && projectedEnd <= 0))
                {
                    // find the position where lineSeg2 intersects lineSeg1's supporting line
                    float t = projectedStart / (projectedStart - projectedEnd);
                    Vector2 intersectingPoint = Vector2.lerp(lineSeg2Start, lineSeg2End, t);

                    // check if intersectingPoint lies in lineSeg1
                    float projectedIntersectingPoint = Vector2.dot(intersectingPoint - lineSeg1Start, lineSeg1.direction);
                    if (0F <= projectedIntersectingPoint && projectedIntersectingPoint <= lineSeg1.length)
                        return true;
                }
                return false;
            }
        }

        protected abstract void OnCollision(Body other);

        public abstract void debugDraw(RenderWindow win, View view);
    }
}
