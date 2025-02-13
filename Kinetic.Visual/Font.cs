namespace Kinetic.Visual;

public delegate int LocatePage(char ch);

public class Font
{

	public static string ASCII = "!@#$%^&*()_+-=[]{}|\\;':\"<>,./?~`ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

	public float[] GlyphWidth = new float[65536];
	public int[] GlyphX = new int[65536];
	public int[] GlyphY = new int[65536];

	public LocatePage Locate;
	public float Scale = 1f;

	public Texture[] texture;
	public int YSize;

	public static Font Load(Texture[] textures, LocatePage locator, int[] ghx, int[] ghy, float[] ghw, int h)
	{
		Font font = new Font();
		font.texture = textures;
		font.Locate = locator;
		font.GlyphX = ghx;
		font.GlyphY = ghy;
		font.GlyphWidth = ghw;
		font.YSize = h;

		return font;
	}

	public GlyphBounds GetBounds(string text, float maxWidth = int.MaxValue)
	{
		float width = 0;
		float lineWidth = 0;
		int height = 0;
		int lineHeight = (int) ((YSize + 2) * Scale);
		bool needNewLine = false;

		for(int i = 0; i < text.Length; i++)
		{
			char c = text[i];

			if(c == '\n' || needNewLine)
			{
				height += lineHeight;
				width = System.Math.Max(lineWidth, width);
				lineWidth = 0;
				needNewLine = false;
				continue;
			}

			float w = GlyphWidth[c] * Scale;

			if(lineWidth + w >= maxWidth)
			{
				needNewLine = true;
				i -= 2;
				continue;
			}

			lineWidth += w;
		}

		height += lineHeight;
		width = System.Math.Max(lineWidth, width);

		return new GlyphBounds(text, width, height, lineWidth);
	}

	public GlyphBounds GetBounds(Lore text, float maxWidth = int.MaxValue)
	{
		return GetBounds(text.Summary, maxWidth);
	}

}

public struct GlyphBounds
{

	public string Sequence;
	public float Width;
	public float Height;
	public float LastWidth;

	public GlyphBounds(string sequence, float w, float h, float lw)
	{
		Sequence = sequence;
		Width = w;
		Height = h;
		LastWidth = lw;
	}

}

public enum Align
{

	LEFT,
	CENTER,
	RIGHT

}
