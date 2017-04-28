using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.util
{
    public class VectorMath
    {
        private VectorMath()
        {

        }

        /**
            * Abstand zwischen zwei Vector2f Vectoren
            * @param p1 Vector 1
            * @param p2 Vector 2
            * @return der Abstand
            */
        public static float Distance(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        /**
            * Abstand zwischen zwei Vector3f Vectoren
            * @param p1 Vector 1
            * @param p2 Vector 2
            * @return der Abstand
            */
        public static float Distance(Vector3 p1, Vector3 p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            float dz = p1.Z - p2.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static Vector2 RotateAroundOrigin(Vector2 vector, float angle, bool cw)
        {
            Vector2 v = new Vector2(vector.X, vector.Y);
            if (cw)
            {
                angle = -angle;
            }

            float newX = (float)(Math.Cos(angle) * v.X - Math.Sin(angle) * v.Y);
            float newY = (float)(Math.Sin(angle) * v.X + Math.Cos(angle) * v.Y);
            v.X = newX;
            v.Y = newY;

            return v;
        }

        static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, float angleInRadians)
        {
            //   double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Vector2(
                (float)
                (cosTheta * (pointToRotate.X - centerPoint.X) -
                sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),

                (float)
                (sinTheta * (pointToRotate.X - centerPoint.X) +
                cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            );
        }



        /**
            * Bewegt einen Punkt in eine Richtung
            * @param origin Startpunkt
            * @param dir Verschiebungsrichtung
            * @param distance Verschiebungsdistanz
            * @param n Vorwärts oder rückwärs
            * @return Neuer, verschobener Punkt
            */
        public static Vector2 Move(Vector2 origin, Vector2 dir, float distance, bool n)
        {
            int d = n ? 1 : -1;
            return new Vector2((dir.X + origin.X * distance * d), (dir.Y + origin.Y * distance * d));


            /*	Vector2f actualMovement = new Vector2f(
                        this.wiggleDirection.getX()* speed*delta,
                        this.wiggleDirection.getY() * speed*delta
                    );
		
                this.wigglePosition.translate(-actualMovement.x, -actualMovement.y);*/
        }

        /**
    * Bewegt einen Punkt in eine Richtung
    * @param origin Startpunkt
    * @param dir Verschiebungsrichtung
    * @param distance Verschiebungsdistanz
    * @param n Vorwärts oder rückwärs
    * @return Neuer, verschobener Punkt
    */
        public static Vector3 Move(Vector3 origin, Vector3 dir, float distance, bool n)
        {
            int d = n ? 1 : -1;
            return new Vector3((dir.X + origin.X * distance * d), (dir.Y + origin.Y * distance * d), (dir.Z + origin.Z * distance * d));


            /*	Vector2f actualMovement = new Vector2f(
                        this.wiggleDirection.getX()* speed*delta,
                        this.wiggleDirection.getY() * speed*delta
                    );
		
                this.wigglePosition.translate(-actualMovement.x, -actualMovement.y);*/
        }

        /**
            * Berechnet neuen Richtungsvector
            * @param start	Urpsrung der Geraden
            * @param end	Zweiter Punkt
            * @param negate	Soll neuer Richtungsvektor negiert werden?
            * @return	Neuer Richtungsvektor
            */
        public static Vector2 CalculateDirection(Vector2 start, Vector2 end, bool negate)
        {
            Vector2 direction;
            Vector2.Subtract(ref start, ref end, out direction);

            if (negate)
            {
                Vector2.Negate(direction);
            }
            direction.Normalize();
            return direction;
        }

        /**
            * <code>angleBetween</code> returns (in radians) the angle required to
            * rotate a ray represented by this vector to lie colinear to a ray
            * described by the given vector. It is assumed that both this vector and
            * the given vector are unit vectors (iow, normalized).
            * 
            * @param otherVector
            *            the "destination" unit vector
            * @return the angle in radians.
            */
        public static float AngleBetween(Vector2 vector, Vector2 otherVector)
        {
            /* float angle = (float)(Math.Atan2(otherVector.Y, otherVector.X)
                        - Math.Atan2(vector.Y, vector.X));
                return angle;*/

            // return (float)Math.Atan2(otherVector.Y - vector.Y, otherVector.X - vector.X);


            float dot = vector.X * otherVector.X + vector.Y * otherVector.Y;         // dot product
            float det = vector.X * otherVector.Y - vector.Y * otherVector.X;         // determinant
            return (float)Math.Atan2(det, dot);       // atan2(y, x) or atan2(sin, cos)

        }

        /// <summary>
        /// Angel in radians
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float GetAngle(Vector2 v)
        {
            return (float)Math.Atan2(v.X, v.Y);
            // return Vector2f.angle(Cursor.DEFAULT_DIRECTION, this.direction);
        }

        Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /*   float VectorToAngle(Vector2 vector)
            {
                return (float)Math.Atan2(vector.Y, vector.X);
            }*/


        // Find the point of intersection between
        // the lines p1 --> p2 and p3 --> p4.
        public static Vector2 FindIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);
            float t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;

            if (float.IsInfinity(t1)) // The lines are parallel (or close enough to it).
            {
                return new Vector2(float.NaN, float.NaN);
            }

            // Find the point of intersection.
            return new Vector2(p1.X + dx12 * t1, p1.Y + dy12 * t1);
        }

        public static float DistanceFromPointToLineSegment(Vector2 point, Vector2 anchor, Vector2 end)
        {
            Vector2 d = end - anchor;
            float length = d.Length();
            if (d == Vector2.Zero) return (point - anchor).Length();
            d.Normalize();
            float intersect = Vector2.Dot((point - anchor), d);
            if (intersect < 0) return (point - anchor).Length();
            if (intersect > length) return (point - end).Length();
            return (point - (anchor + d * intersect)).Length();
        }

        public static Vector2 Random(float maxLength)
        {
            Random r = new Random();

            int x = r.NextDouble() > 0.5f ? 1 : -1;
            int y = r.NextDouble() > 0.5f ? 1 : -1;

            return new Vector2((float)r.NextDouble() * maxLength * x, (float)r.NextDouble() * maxLength * y);
        }
    }
   
}
