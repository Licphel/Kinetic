using Kinetic.App;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementImageDynamic : Element
{

	public TexturePart texp;
	public float Progress;
	public Style _style;

	public ElementImageDynamic(TexturePart texp, Style style)
	{
		this.texp = texp;
		_style = style;
	}

	public override void Draw(SpriteBatch batch)
	{
		float p = Progress;
		switch(_style)
		{
			case Style.RightShrink:
				batch.Draw(texp.Texture, Bound.x, Bound.y, Bound.w * p, Bound.h, texp.u, texp.v, texp.uw * p, texp.vh);
				break;
			case Style.LeftShrink:
				batch.Draw(texp.Texture, Bound.x + Bound.w * (1 - p), Bound.y, Bound.w * p, Bound.h, texp.u + texp.uw * (1 - p), texp.v, texp.uw * p, texp.vh);
				break;
			case Style.UpShrink:
				batch.Draw(texp.Texture, Bound.x, Bound.y, Bound.w, Bound.h * p, texp.u, texp.v + texp.vh * (1 - p), texp.uw, texp.vh * p);
				break;
			case Style.DownShrink:
				batch.Draw(texp.Texture, Bound.x, Bound.y + Bound.h * (1 - p), Bound.w, Bound.h * p, texp.u, texp.v, texp.uw, texp.vh * p);
				break;
			case Style.Vanish:
				batch.Color4(1, 1, 1, p);
				batch.Draw(texp, Bound);
				batch.NormalizeColor();
				break;
		}
	}

	public enum Style { UpShrink, DownShrink, LeftShrink, RightShrink, Vanish }

}
