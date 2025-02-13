using Kinetic.Math;

namespace Kinetic.App;

//Some essential delegates
public delegate void Response();
public delegate void Response<T>(T t);
public delegate T Factory<out T>();

public class Application
{

	public static readonly SemanticVersion LibVersion = new SemanticVersion("Stable-1.0");

	//{User Configurable Properties}
	public static int MaxFps = -1;//If you want to use synced render, let MaxFps = MaxTps. -1 means no limit.
	public static int MaxTps = 60;//The tps user set. Attempt to reach it.
	public static int MaxLeap = 1;//How many ticks can be leaped(run multi ticks in with one loop) when tps is low.

	//{Automatically Set Properties. DOT NOT TOUCH THEM!}
	public static int Fps;
	public static int Tps;

	public static bool Stopped;//Set to true to stop the main thread.

	static float PartialTicks;
	static float Delta;//Represent a delta-second. Use speed * delta to get a real speed of object.
	static int Ticks;//The total ticks.
	static float Seconds;

	public static Application App;
	public static VaryingVector2 ScaledSize;

	public static event Response Init = () => { };
	public static event Response Update = () => { };
	public static event Response Draw = () => { };
	public static event Response Dispose = () => { };
	public static event Response Resize = () => { };

	public static void _CallResize()
	{
		Resize();
	}

	public static bool PeriodicTask(float seconds)
	{
		int ticks = (int) (seconds * Tps);
		return Ticks % ticks == 0;
	}

	public static void Stop()
	{
		Stopped = true;
	}

	public static void Launch()
	{
		Thread.CurrentThread.Name = "Main";

		Logger.Info($"* Running on Kinetic Framework {LibVersion.FullName}");
		Logger.Info($"* Running on {Environment.MachineName}");

		if(MaxTps < 60)
			Logger.Fatal("Tps cannot be less than 60!");

		bool syncRender = MaxFps != -1;

		double renderPartialTicks = 0f;
		double lastSyncSysClock = Time.Nanosecs;
		double tickLength = 1_000_000_000.0 / MaxTps;
		int framesT = 0, framesR = 0;
		float lastCalcClock = Time.Millisecs;

		Init();

		Tps = MaxTps;
		Fps = MaxFps;

		while(!Stopped)
		{
			double i = Time.Nanosecs;
			double elapsedPartialTicks = (i - lastSyncSysClock) / tickLength;
			lastSyncSysClock = i;
			renderPartialTicks += elapsedPartialTicks;
			int elapsedTicks = (int) renderPartialTicks;
			renderPartialTicks -= elapsedTicks;

			for(int j = 0; j < System.Math.Min(MaxLeap, elapsedTicks); j++)
			{
				framesT++;
				Ticks++;
				Delta = 1f / Tps;
				if(Delta > 1)
				{
					Logger.Info($"A great delay of {Delta}s occurred. Automatically fixed!");
					Delta = 0;
				}

				Seconds += Delta;
				Time.Delta = Delta;
				Time.Ticks = Ticks;
				Time.PartialTicks = PartialTicks;

				Update();

				if(syncRender)
				{
					DrawAndSwap();
				}
			}

			if(!syncRender)
			{
				DrawAndSwap();
			}

			if(Time.Millisecs - lastCalcClock < 500)
			{
				continue;
			}

			lastCalcClock = Time.Millisecs;
			Tps = framesT * 2;
			Fps = framesR * 2;
			framesT = framesR = 0;
		}

		Dispose();

		// It regularly fails, interrupting the upcoming operations. Put it at last.
		Logger.Info("Start finalization...", true);
		NativeManager.I0.Free();
		Logger.Fix("Done!");

		return;

		void DrawAndSwap()
		{
			PartialTicks = (float) renderPartialTicks;
			Draw();
			framesR++;
		}
	}

}
