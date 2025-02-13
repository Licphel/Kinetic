using Kinetic.Math;

namespace Kinetic.Visual;

public abstract class FontCarver
{

	public int MaxDrawableChars = int.MaxValue;

	public abstract void Draw(SpriteBatch batch, Font font, string text, float x, float y, float maxw);

	public abstract void Draw(SpriteBatch batch, Font font, Lore text, float x, float y, float maxw);

}

public class DefaultFontCarver : FontCarver
{

	public override void Draw(SpriteBatch batch, Font font, string text, float x, float y, float maxw)
	{
		if(string.IsNullOrWhiteSpace(text))
		{
			return;
		}

		int fontHeight = (int) ((font.YSize + 2) * font.Scale);

		float drawX = x;
		float drawY = y;

		bool newLine = false;

		for(int i = 0; i < text.Length && i < MaxDrawableChars; i++)
		{
			char ch = text[i];

			if(ch == '\n' || newLine)
			{
				drawY -= fontHeight;
				drawX = x;
				newLine = false;
				continue;
			}

			float w = font.GlyphWidth[ch] * font.Scale;

			if(drawX - x + w >= maxw)
			{
				newLine = true;
				i -= 2;
				continue;
			}

			Texture map = font.texture[font.Locate(ch)];

			batch.Draw(map, drawX, drawY, w, font.YSize * font.Scale, font.GlyphX[ch], font.GlyphY[ch], font.GlyphWidth[ch], font.YSize);
			drawX += w;
		}
	}

	public override void Draw(SpriteBatch batch, Font font, Lore text, float x, float y, float maxw)
	{
		batch.PushColor();

		int fontHeight = (int) ((font.YSize + 2) * font.Scale);
		float frh = font.YSize * font.Scale;

		float drawX = x;
		float drawY = y;

		bool newLine = false;

		foreach(var v in text.Content)
		{
			LoreStyle sp = v.Item1;
			string text0 = v.Item2();
			batch.Color4(sp.Color);

			for(int i = 0; i < text0.Length && i < MaxDrawableChars; i++)
			{
				char ch = text0[i];
				float w = font.GlyphWidth[ch] * font.Scale;
				char ch1 = ch;

				if(sp.Messy)
				{
					ch1 = (char) Seed.Global.NextInt(0, 1024);
				}

				if(ch == '\n' || newLine)
				{
					drawY -= fontHeight;
					drawX = x;
					newLine = false;
					continue;
				}

				if(drawX - x + w >= maxw)
				{
					newLine = true;
					i -= 2;
					continue;
				}

				Texture map = font.texture[font.Locate(ch1)];
				float gx = font.GlyphX[ch1], gy = font.GlyphY[ch1], gw = font.GlyphWidth[ch];

				if(!sp.Bold)
				{
					DrawOnce(batch, map, sp, drawX, drawY, w, frh, gx, gy, gw, font);
				}
				else
				{
					batch.PushColor();
					if(sp.Darkoutline)
					{
						batch.Merge4(0.4f, 0.4f, 0.4f);
					}
					DrawOnce(batch, map, sp, drawX + 0.5f, drawY + 0.25f, w, frh, gx, gy, gw, font);
					DrawOnce(batch, map, sp, drawX - 0.5f, drawY + 0.25f, w, frh, gx, gy, gw, font);
					DrawOnce(batch, map, sp, drawX + 0.5f, drawY - 0.25f, w, frh, gx, gy, gw, font);
					DrawOnce(batch, map, sp, drawX - 0.5f, drawY - 0.25f, w, frh, gx, gy, gw, font);

					batch.PopColor();
					DrawOnce(batch, map, sp, drawX, drawY, w, frh, gx, gy, gw, font);
				}

				if(sp.Underlined)
				{
					batch.Fill(drawX, drawY, w, 1);
				}
				if(sp.Deleted)
				{
					batch.Fill(drawX, drawY + fontHeight / 2f - 2 * font.Scale, w, 1);
				}

				drawX += w;
			}
		}

		batch.PopColor();

		return;

		static void DrawOnce(SpriteBatch batch, Texture map, LoreStyle sp, float drawX, float drawY, float w, float frh, float gx, float gy, float gw, Font font)
		{
			if(sp.Italic)
			{
				batch.Draw(map, drawX, drawY, w, frh / 3f, gx, gy + font.YSize / 3f * 2f, gw, font.YSize / 3f);
				batch.Draw(map, drawX + 0.75f, drawY + frh / 3f, w, frh / 3f, gx, gy + font.YSize / 3f, gw, font.YSize / 3f);
				batch.Draw(map, drawX + 1.5f, drawY + frh / 3f * 2, w, frh / 3f, gx, gy, gw, font.YSize / 3f);
			}
			else
			{
				batch.Draw(map, drawX, drawY, w, frh, gx, gy, gw, font.YSize);
			}
		}
	}

}
