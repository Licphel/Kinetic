using Kinetic.App;
using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementButton : Element
{

	bool cursorOn;

	//if it is true, this is a state-switching button.
	public bool IsSwitcher;
	public bool IsOn;

	public Icon[] Icons = new Icon[3];
	public Texture Texture3Line;

	public Response OnLeftFired = () => { };
	public Response OnRightFired = () => { };

	int pressDelay;
	public Lore Text;
	public Vector2 TextOffset = new Vector2(0, 0);

	public static int DEFAULT_PRESS_DELAY => Application.Tps / 8;

	public override void Update()
	{
		base.Update();

		pressDelay--;

		cursorOn = false;

		if(Bound.Contains(Cursor) && IsExposed())
		{
			cursorOn = true;

			if(RemoteKey.MouseLeft.Pressed())
			{
				OnLeftFired.Invoke();
				pressDelay = DEFAULT_PRESS_DELAY;
				IsOn = !IsOn;
			}

			if(RemoteKey.MouseRight.Pressed())
			{
				OnRightFired.Invoke();
				pressDelay = DEFAULT_PRESS_DELAY;
			}
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		if(Texture3Line == null)
		{
			if(pressDelay > 0 || (IsOn && IsSwitcher))
			{
				batch.Draw(Icons[2], Bound);
			}
			else if(cursorOn)
			{
				batch.Draw(Icons[1], Bound);
			}
			else
			{
				batch.Draw(Icons[0], Bound);
			}

			batch.Draw(Text, Bound.xcentral, Bound.y + 4, Align.CENTER);
		}
		else
		{
			float sy;
			if(pressDelay > 0|| (IsOn && IsSwitcher))
			{
				sy = Texture3Line.Height / 3f * 2;
			}
			else if(cursorOn)
			{
				sy = Texture3Line.Height / 3f;
			}
			else
			{
				sy = 0;
			}

			batch.Draw(Texture3Line, Bound, 0, sy, Texture3Line.Width, Texture3Line.Height / 3f);
			batch.Draw(Text, Bound.xcentral + TextOffset.x, Bound.y + TextOffset.y, Align.CENTER);
		}
	}

}
