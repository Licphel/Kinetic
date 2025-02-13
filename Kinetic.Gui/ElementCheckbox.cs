using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

//a special bounded struct: the Bound refers to its part "box", not including the text.
public class ElementCheckbox : Element
{

	public bool IsOn;
	public bool ShouldShowCross;

	public Icon[] Icons = new Icon[3];
	public Lore DisplayedLore;
	public Vector2 TextOffset = new Vector2();

	public override void Update()
	{
		base.Update();

		if(RemoteKey.MouseLeft.Pressed() && Bound.Contains(Cursor))
		{
			IsOn = !IsOn;
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		base.Draw(batch);

		if(IsOn)
		{
			batch.Draw(Icons[1], Bound);
		}
		else
		{
			if(ShouldShowCross) 
				batch.Draw(Icons[2], Bound);
			else
				batch.Draw(Icons[0], Bound);
		}
		
		batch.Draw(DisplayedLore, Bound.xprom + TextOffset.x, Bound.y + TextOffset.y);
	}

}
