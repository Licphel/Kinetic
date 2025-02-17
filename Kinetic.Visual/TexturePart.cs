namespace Kinetic.Visual;

public class TexturePart : IconDimensional
{

	public virtual Texture Src { get; set; }
	public float u, v, uw, vh;

	public int Width => (int) uw;
	public int Height => (int) vh;

	public virtual void Draw(SpriteBatch batch, float x, float y, float w, float h)
	{
		batch.Draw(Src, x, y, w, h, u, v, uw, vh);
	}

	public static TexturePart BySize(TexturePart tex, float u, float v, float uw, float vh)
	{
		TexturePart part = new TexturePart();
		part.Src = tex.Src;
		part.u = u + tex.u;
		part.v = v + tex.v;
		part.uw = uw;
		part.vh = vh;
		return part;
	}

	public static TexturePart ByVerts(TexturePart tex, float u, float v, float u2, float v2)
	{
		return BySize(tex, u, v, u2 - u, v2 - v);
	}

	public static TexturePart ByPercentSize(TexturePart tex, float u, float v, float w, float h)
	{
		return BySize(tex, tex.Width * u, tex.Height * v, tex.Width * w, tex.Width * h);
	}

	public static TexturePart ByPercentVerts(TexturePart tex, float u, float v, float u2, float v2)
	{
		return ByVerts(tex, tex.Width * u, tex.Height * v, tex.Width * u2, tex.Width * v2);
	}

}
