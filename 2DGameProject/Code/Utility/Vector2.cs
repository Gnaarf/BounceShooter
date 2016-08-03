using System;

public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    //------------------------------------------//
    //                  Casts                   //
    //------------------------------------------//
    // SFML-Vectors
    public static implicit operator Vector2(SFML.Window.Vector2f v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator SFML.Window.Vector2f(Vector2 v)
    {
        return new SFML.Window.Vector2f(v.x, v.y);
    }

    public static implicit operator Vector2(SFML.Window.Vector2u v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator SFML.Window.Vector2u(Vector2 v)
    {
        return new SFML.Window.Vector2u((uint)v.x, (uint)v.y);
    }

    public static implicit operator Vector2(SFML.Window.Vector2i v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator SFML.Window.Vector2i(Vector2 v)
    {
        return new SFML.Window.Vector2i((int)v.x, (int)v.y);
    }


    //------------------------------------------//
    //                 Constants                //
    //------------------------------------------//
    /// <summary>(0, 0)</summary>
    public static Vector2 Zero { get { return new Vector2(0F, 0F); } }
    /// <summary>(1, 1)</summary>
    public static Vector2 One { get { return new Vector2(1F, 1F); } }
    /// <summary>(0, -1)</summary>
    public static Vector2 Up { get { return new Vector2(0F, -1F); } }
    /// <summary>(1, 0)</summary>
    public static Vector2 Right { get { return new Vector2(1F, 0F); } }
    /// <summary>(0, 1)</summary>
    public static Vector2 Down { get { return new Vector2(0F, 1F); } }
    /// <summary>(-1, 0)</summary>
    public static Vector2 Left { get { return new Vector2(-1F, 0F); } }

    //------------------------------------------//
    //             Instance Functions           //
    //------------------------------------------//
    public float length { get { return (float)System.Math.Sqrt(x * x + y * y); } }
    public float lengthSqr { get { return x * x + y * y; } }

    public float rotation 
    { 
        get    
        {
            float cos = (float)Math.Acos(this.normalized.x);
            return y > 0F ? cos : 2F * Helper.PI - cos;
        } 
    }

    public Vector2 normalized 
    { 
        get 
        {
            float l = length;
            return this / l; 
        } 
    }

    /// <summary>returs a vector rotated around the given angle</summary>
    public Vector2 Rotate(float angle)
    {
        Vector2 ret = new Vector2();
        float cosA = (float)System.Math.Cos(angle);
        float sinA = (float)System.Math.Sin(angle);

        ret.x = x * cosA - y * sinA;
        ret.y = y * cosA + x * sinA;

        return ret;
    }

    public Vector2 right { get { return new Vector2(y, -x); } }
    public Vector2 rightNormalized { get { return new Vector2(y, -x) / length; } }

    //------------------------------------------//
    //           Static Functions               //
    //------------------------------------------//
    /// <summary>linear interpolation by t=[0,1]</summary>
    public static Vector2 Lerp(Vector2 from, Vector2 to, float t)
    {
        return (1F - t) * from + t * to;
    }

    public static Vector2 Average(params Vector2[] values)
    {
        return Sum(values) / (float)values.Length;
    }

    public static Vector2 Sum(params Vector2[] values)
    {
        Vector2 sum = new Vector2(0F, 0F);
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }
        return sum;
    }

    public static Vector2 Project(Vector2 projectThis, Vector2 onThat)
    {
        return Dot(projectThis, onThat.normalized) * onThat;
    }

    public static Vector2 Reflect(Vector2 reflectThis, Vector2 normal)
    {
        return Vector2.Project(-reflectThis, normal) * 2 + reflectThis;
    }

    public static float Distance(Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).length;
    }

    public static float DistanceSqr(Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).lengthSqr;
    }

    public static float Dot(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    //------------------------------------------//
    //           Arithmetic Operators           //
    //------------------------------------------//
    // Addition
    /// <summary>add component-wise</summary>
    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }

    // Subtraction
    /// <summary>subtract component-wise</summary>
    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x - v2.x, v1.y - v2.y);
    }
    /// <summary>negate every component</summary>
    public static Vector2 operator -(Vector2 v)
    {
        return new Vector2(-v.x, -v.y);
    }

    // Multiplication
    /// <summary>multiply component-wise</summary>
    public static Vector2 operator *(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2 operator *(float f, Vector2 v)
    {
        return new Vector2(f * v.x, f * v.y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2 operator *(Vector2 v, float f)
    {
        return new Vector2(f * v.x, f * v.y);
    }

    // Division
    /// <summary>divide component-wise</summary>
    public static Vector2 operator /(Vector2 v1, Vector2 v2)
    {
        if (v2.x == 0F || v2.y == 0F) { throw new Exception("Devide by Zero"); }
        return new Vector2(v1.x / v2.x, v1.y / v2.y);
    }
    /// <summary>divide both components by factor</summary>
    public static Vector2 operator /(Vector2 v, float f)
    {
        if (f == 0F) { throw new Exception("Devide by Zero"); }
        return new Vector2(v.x / f, v.y / f);
    }

    // Equality
    /// <summary>check component-wise</summary>
    public static bool operator ==(Vector2 v1, Vector2 v2)
    {
        return (v1.x == v2.x) && (v1.y == v2.y);
    }
    /// <summary>check component-wise</summary>
    public static bool operator !=(Vector2 v1, Vector2 v2)
    {
        return (v1.x != v2.x) || (v1.y != v2.y);
    }
}

