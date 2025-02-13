namespace Kinetic.Auditory;

public interface AudioClip
{

	protected static List<AudioClip> ClipsPlaying = new List<AudioClip>();
	protected static List<AudioClip> ClipsStopped = new List<AudioClip>();

	public static void CheckClipStates()
	{
		try
		{
			for(int i = 0; i < ClipsPlaying.Count; i++)
			{
				AudioClip? c = ClipsPlaying[i];
				if(!c.IsPlaying && !c.IsPaused)
				{
					ClipsStopped.Add(c);
					c.Dispose();
				}
			}

			ClipsPlaying.RemoveAll(ClipsStopped.Contains);
			ClipsStopped.Clear();
		}
		catch (Exception _) {}
	}

	AudioData GetData();

	void Play();

	void Loop();

	void Pause();

	void Resume();

	void Stop();

	void Set(ClipController controller, object v);

	object Get(ClipController controller);

	bool IsPlaying { get; }
	bool IsPaused { get; }
	bool IsDestroyed { get; }

	void Dispose();

}

public enum ClipController
{

	Gain,
	Pitch,
	FramePosition,
	SourceLocation

}
