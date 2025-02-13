namespace Kinetic.Visual;

public abstract class Texture : IconDimensional
{

	public int Width { get; set; }
	public int Height { get; set; }

	public void Draw(SpriteBatch batch, float x, float y, float w, float h)
	{
		batch.Draw(this, x, y, w, h);
	}

}
