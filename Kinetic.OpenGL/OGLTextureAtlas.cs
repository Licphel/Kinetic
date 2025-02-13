using System.Drawing;
using System.Drawing.Drawing2D;
using Kinetic.IO;
using Kinetic.Visual;

namespace Kinetic.OpenGL;

public class OGLTextureAtlas : TextureAtlas
{

	readonly int powerx = 12;
	readonly int powery = 12;
	Bitmap bmap;
	Graphics g;
	int cx, cy;
	int mh;

	OGLTexture glt;

	public TexturePart Accept(Texture tex)
	{
		if(cx + tex.Width >= bmap.Width)
		{
			cy += mh;
			mh = 0;
			cx = 0;
		}
		OGLTexture gltin = (OGLTexture) tex;
		mh = System.Math.Max(tex.Height, mh);
		TexturePart part = TexturePart.BySize(glt, cx, cy, tex.Width, tex.Height);
		g.DrawImageUnscaled(Bitmap.FromFile(gltin.FileSrc), cx, cy);
		cx += tex.Width;
		return part;
	}

	public void Begin()
	{
		if(glt == null) glt = new OGLTexture();
		int p0 = (int) System.Math.Pow(2, powerx);
		int p1 = (int) System.Math.Pow(2, powery);
		bmap = new Bitmap(p0, p1);
		g = Graphics.FromImage(bmap);
		g.CompositingQuality = CompositingQuality.Invalid;
		g.SmoothingMode = SmoothingMode.None;
		g.InterpolationMode = InterpolationMode.NearestNeighbor;
		g.PixelOffsetMode = PixelOffsetMode.None;
	}

	public void End()
	{
		bmap.Save(FileSystem.GetLocal("generated_atlas_gb.png").Path);
		glt.Upload(bmap);
	}

}
