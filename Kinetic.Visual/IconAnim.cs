using Kinetic.App;
using Kinetic.Math;

namespace Kinetic.Visual;

public class IconAnim : Icon
{

	readonly int maxIndex;

	readonly TexturePart[] stream;
	int index;
	float frameLen;
	TimeSchedule schedule = new TimeSchedule();

	public IconAnim(TexturePart tex, int count, int w, int h, int u, int v)
	{
		maxIndex = count;
		stream = new TexturePart[maxIndex];

		for(int i = 0; i < maxIndex; i++)
			stream[i] = TexturePart.BySize(tex, i * w + u, v, w, h);
	}

	public IconAnim(Texture tex, int count, int w, int h, int u, int v)
	{
		maxIndex = count;
		stream = new TexturePart[maxIndex];

		for(int i = 0; i < maxIndex; i++)
			stream[i] = TexturePart.BySize(tex, i * w + u, v, w, h);
	}

	public IconAnim(params TexturePart[] parts)
	{
		stream = parts;
	}

	public void Draw(SpriteBatch batch, float x, float y, float w, float h)
	{
		if(schedule.PeriodicTaskChecked(frameLen))
		{
			index++;
			if(index >= maxIndex)
				index = 0;
		}

		batch.Draw(stream[index], x, y, w, h);
	}

	public IconAnim Seconds(float time)
	{
		frameLen = time;
		return this;
	}

	public void Reset()
	{
		index = 0;
	}

	public float GetTimePerCycle()
	{
		return frameLen * (maxIndex - 1);
	}

}
