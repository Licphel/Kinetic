using Kinetic.Auditory;
using Kinetic.Math;
using OpenTK.Audio.OpenAL;

namespace Kinetic.OpenAL;

public class OALAudioClip : AudioClip
{

	public OALAudioData Data;

	public int Id;

	public OALAudioClip(OALAudioData data)
	{
		Id = AL.GenSource();
		Data = data;

		AL.Source(Id, ALSourcei.Buffer, data.Id);
	}

	public AudioData GetData()
	{
		return Data;
	}

	public void Play()
	{
		if(AudioClip.ClipsPlaying.Count > 128)
			return;
		AL.SourcePlay(Id);

		AudioClip.ClipsPlaying.Add(this);
	}

	public void Loop()
	{
		if(AudioClip.ClipsPlaying.Count > 128)
			return;
		AL.Source(Id, ALSourceb.Looping, true);
		AL.SourcePlay(Id);

		AudioClip.ClipsPlaying.Add(this);
	}

	public void Pause()
	{
		AL.SourcePause(Id);
	}

	public void Resume()
	{
		AL.SourcePlay(Id);
	}

	public void Stop()
	{
		AL.SourceStop(Id);
	}

	public void Set(ClipController controller, object v)
	{
		switch(controller)
		{
			case ClipController.Gain:
				AL.Source(Id, ALSourcef.Gain, (float) v);
				break;
			case ClipController.Pitch:
				AL.Source(Id, ALSourcef.Pitch, (float) v);
				break;
			case ClipController.SourceLocation:
				Vector3 vec = (Vector3) v;
				AL.Source(Id, ALSource3f.Position, vec.x, vec.y, vec.z);
				break;
		}
	}

	public object Get(ClipController controller)
	{
		switch(controller)
		{
			case ClipController.Gain:
				AL.GetSource(Id, ALSourcef.Gain, out float v1);
				return v1;
			case ClipController.Pitch:
				AL.GetSource(Id, ALSourcef.Pitch, out float v2);
				return v2;
			case ClipController.SourceLocation:
				AL.GetSource(Id, ALSource3f.Position, out float v3, out float v4, out float v5);
				return new Vector3(v3, v4, v5);
		}

		return null;
	}

	public bool IsPlaying
	{
		get
		{
			AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
			return v == (int) ALSourceState.Playing;
		}
	}

	public bool IsPaused
	{
		get
		{
			AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
			return v == (int) ALSourceState.Paused;
		}
	}

	public bool IsDestroyed
	{
		get
		{
			AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
			return v == (int) ALSourceState.Stopped;
		}
	}

	public void Dispose()
	{
		AL.DeleteSource(Id);
	}

}
