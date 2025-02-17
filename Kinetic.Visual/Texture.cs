namespace Kinetic.Visual;

public abstract class Texture : TexturePart
{

	public override Texture Src => this;

	public override void Draw(SpriteBatch batch, float x, float y, float w, float h)
	{
		batch.Draw((Texture) this, x, y, w, h);
	}

}
