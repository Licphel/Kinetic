namespace Kinetic.Math;

public struct Vector3
{

	public float x, y, z;

	public Vector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3(IReadOnlyList<float> list) : this(list[0], list[1], list[2])
	{
	}

	public static Vector3 operator +(Vector3 vec1, Vector3 vec2)
	{
		return new Vector3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
	}

	public static Vector3 operator -(Vector3 vec1, Vector3 vec2)
	{
		return new Vector3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
	}

	public static Vector3 operator *(Vector3 vec1, Vector3 vec2)
	{
		return new Vector3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
	}

	public static Vector3 operator /(Vector3 vec1, Vector3 vec2)
	{
		return new Vector3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
	}

	public static Vector3 operator +(Vector3 vec1, float v)
	{
		return new Vector3(vec1.x + v, vec1.y + v, vec1.z + v);
	}

	public static Vector3 operator -(Vector3 vec1, float v)
	{
		return new Vector3(vec1.x - v, vec1.y - v, vec1.z - v);
	}

	public static Vector3 operator *(Vector3 vec1, float v)
	{
		return new Vector3(vec1.x * v, vec1.y * v, vec1.z * v);
	}

	public static Vector3 operator /(Vector3 vec1, float v)
	{
		return new Vector3(vec1.x / v, vec1.y / v, vec1.z / v);
	}

	public static Vector3 operator -(Vector3 vec)
	{
		return new Vector3(-vec.x, -vec.y, -vec.z);
	}

	//Dot Operation.
	public static float operator ^(Vector3 vec1, Vector3 vec2)
	{
		return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
	}

}
