namespace Kinetic.App;

public class Time
{

	public static Func<double> SysNanotimer = () => throw new NotImplementedException("Application has not implemented nano timer.");

	public static float PartialTicks;
	public static float Delta;
	public static int Ticks;//Tick's unstable. You'd better not use it.

	public static double Nanosecs => SysNanotimer();
	public static float Millisecs => (float) (Nanosecs / 1000_000f);
	public static float Seconds => Millisecs / 1000f;
	public static float Minutes => Seconds / 60f;
	public static float Hours => Minutes / 60f;

	public static float Lerp(float p, float v)
	{
		return PartialTicks * (v - p) + p;
	}

}
