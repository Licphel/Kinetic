namespace Kinetic.Math;

public class FloatMath
{

	public const float DTR = (float) (System.Math.PI / 180F);
	public const float RTD = 1 / DTR;

	static readonly float[] SinTable = new float[0x10000];

	static FloatMath()
	{
		for(int i = 0; i < 65535; i++)
		{
			SinTable[i] = (float) System.Math.Sin(i * System.Math.PI * 2D / 65536D);
		}
	}

	public static float SafeDiv(float v1, float v2)
	{
		return v2 == 0 ? 0 : v1 / v2;
	}

	public static float Pow(float v, float d)
	{
		return (float) System.Math.Pow(v, d);
	}

	public static float Sqrt(float v)
	{
		return (float) System.Math.Sqrt(v);
	}

	public static float Log(float v)
	{
		return (float) System.Math.Log(v);
	}

	public static float Abs(float v)
	{
		return v > 0 ? v : -v;
	}

	public static long Abs(long v)
	{
		return v > 0 ? v : -v;
	}

	public static float AtanRad(float y, float x)
	{
		return (float) System.Math.Atan2(y, x);
	}

	public static float AtanDeg(float y, float x)
	{
		return AtanRad(y, x) * RTD;
	}

	public static float SinRad(float v)
	{
		return SinTable[(int) (v * 10430.38f) & 0xffff];
	}

	public static float CosRad(float v)
	{
		return SinTable[(int) (v * 10430.38f + 16384f) & 0xffff];
	}

	public static float SinDeg(float v)
	{
		return SinRad(v * DTR);
	}

	public static float CosDeg(float v)
	{
		return CosRad(v * DTR);
	}

	public static float Rad(float v)
	{
		return v * DTR;
	}

	public static float Deg(float v)
	{
		return v * RTD;
	}

	public static float Clamp(float v, float min, float max)
	{
		return v < min ? min : System.Math.Min(v, max);
	}

	public static int Clamp(int v, int min, int max)
	{
		return v < min ? min : System.Math.Min(v, max);
	}

	public static int Factorial(int n)
	{
		return n > 1 ? n * Factorial(n - 1) : 1;
	}

	public static int Permutations(int n, int m)
	{
		return n >= m ? Factorial(n) / Factorial(n - m) / Factorial(m) : 0;
	}

	public static int Ceiling(float v)
	{
		return (int) System.Math.Ceiling(v);
	}

	public static int Floor(float v)
	{
		int i = (int) v;
		return v >= i ? i : i - 1;
	}

	public static int Round(float v)
	{
		return (int) System.Math.Round(v);
	}

}
