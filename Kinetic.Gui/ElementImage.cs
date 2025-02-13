using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementImage : Element
{

	public Icon Icon;

	public override void Draw(SpriteBatch batch)
	{
		batch.Draw(Icon, Bound);
	}

}
