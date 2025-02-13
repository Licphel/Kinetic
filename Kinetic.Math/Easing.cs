namespace Kinetic.Math;

public class Easing
{

	public static float Linear(float v, float d, float step)
	{
		if(FloatMath.Abs(v - d) < step)
		{
			return d;
		}
		return v > d ? step : -step;
	}

	public static float Perc(float v, float d, float ps)
	{
		return v + (d - v) * ps;
	}

}
