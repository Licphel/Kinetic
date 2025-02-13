namespace Kinetic.App;

public class Coroutine
{

	public bool IsCompleted;
	public bool IsStarted;

	public Response TaskR;
	Timer timer;
	Task _SysTask;
	TimeSpan TimeUsed;

	public Coroutine(Response runnable)
	{
		TaskR = runnable;
	}

	public void Start()
	{
		CurrentTasks.Add(_SysTask = Task.Factory.StartNew(() =>
		{
			var d1 = DateTime.Now;
			IsStarted = true;
			TaskR.Invoke();
			IsCompleted = true;
			Dispose();
			TimeUsed = DateTime.Now - d1;
		}));
	}

	public void Dispose()
	{
		timer?.Dispose();
		CurrentTasks.Remove(_SysTask);
	}

	static HashSet<Task> CurrentTasks = new HashSet<Task>();

	public static void Wait()
	{
		Logger.Info("Executing remaining async tasks...", true);
		Task.WaitAll(CurrentTasks.ToArray());
		Logger.Fix("Done!");
	}

}
