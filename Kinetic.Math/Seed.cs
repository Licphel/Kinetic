namespace Kinetic.Math;

public abstract class Seed
{

	public static readonly Seed Global = new SeedLCG();

	public bool Next()
	{
		return NextFloat() <= 0.5f;
	}

	public abstract float NextFloat();

	public abstract int NextInt(int bound);

	public float NextFloat(float min, float max)
	{
		return NextFloat() * (max - min) + min;
	}

	public int NextInt(int min, int max)
	{
		max++;
		return NextInt(max - min) + min;
	}

	public abstract void SetSeed(long seed);

	public abstract long GetISeed();

	public abstract long GetCSeed();

	public abstract Seed Copy();

	public abstract Seed Copyx(int offset);

	public T Select<T>(List<T> col)
	{
		if(col == null)
		{
			return default;
		}

		if(col.Count == 0)
		{
			return default;
		}

		return col[NextInt(col.Count)];
	}

	public T Select<T>(params T[] arr)
	{
		if(arr == null)
		{
			return default;
		}

		if(arr.Length == 0)
		{
			return default;
		}

		return arr[NextInt(arr.Length)];
	}

	public float NextGaussian()
	{
		float x, y, w;
		do
		{
			x = NextFloat() * 2 - 1;
			y = NextFloat() * 2 - 1;
			w = x * x + y * y;
		}
		while(w >= 1 || w == 0);

		double c = FloatMath.Sqrt(-2 * FloatMath.Log(w) / w);
		return (float) (y * c);//Use a temp is good but this is fast enough.
	}

	public int NextGaussianInt()
	{
		return FloatMath.Round(NextGaussian());
	}

	public float NextGaussian(float min, float max)
	{
		return NextGaussian() * (max - min) + min;
	}

	public int NextGaussianInt(int min, int max)
	{
		max++;
		return FloatMath.Round(NextGaussian() * (max - min)) + min;
	}

}