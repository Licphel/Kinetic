namespace Kinetic.Math;

public struct Vector2
{

	public static readonly Vector2 Nil = new Vector2(0, 0);
	public static readonly Vector2 One = new Vector2(1, 1);

	public float x, y;

	public int xi => (int) x;
	public int yi => (int) y;

	public float Len => FloatMath.Sqrt(x * x + y * y);

	public Vector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public Vector2(IReadOnlyList<float> vec)
	{
		x = vec[0];
		y = vec[1];
	}

	public Vector2 FromDeg(float ln, float deg)
	{
		x = FloatMath.CosDeg(deg) * ln;
		y = FloatMath.SinDeg(deg) * ln;
		return this;
	}

	public Vector2 FromRad(float ln, float rad)
	{
		x = FloatMath.CosRad(rad) * ln;
		y = FloatMath.SinRad(rad) * ln;
		return this;
	}

	public static Vector2 operator +(Vector2 vec1, Vector2 vector2)
	{
		return new Vector2(vec1.x + vector2.x, vec1.y + vector2.y);
	}

	public static Vector2 operator -(Vector2 vec1, Vector2 vector2)
	{
		return new Vector2(vec1.x - vector2.x, vec1.y - vector2.y);
	}

	public static Vector2 operator *(Vector2 vec1, Vector2 vector2)
	{
		return new Vector2(vec1.x * vector2.x, vec1.y * vector2.y);
	}

	public static Vector2 operator /(Vector2 vec1, Vector2 vector2)
	{
		return new Vector2(vec1.x / vector2.x, vec1.y / vector2.y);
	}

	public static Vector2 operator +(Vector2 vec1, float v)
	{
		return new Vector2(vec1.x + v, vec1.y + v);
	}

	public static Vector2 operator -(Vector2 vec1, float v)
	{
		return new Vector2(vec1.x - v, vec1.y - v);
	}

	public static Vector2 operator *(Vector2 vec1, float v)
	{
		return new Vector2(vec1.x * v, vec1.y * v);
	}

	public static Vector2 operator /(Vector2 vec1, float v)
	{
		return new Vector2(vec1.x / v, vec1.y / v);
	}

	public static Vector2 operator -(Vector2 vec)
	{
		return new Vector2(-vec.x, -vec.y);
	}

	//Dot Operation.
	public static float operator ^(Vector2 vec1, Vector2 vector2)
	{
		return vec1.x * vector2.x + vec1.y * vector2.y;
	}

}
