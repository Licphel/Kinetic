using Kinetic.App;

namespace Kinetic.Gui;

public class Resolution
{

	public static bool AllowResolution = false;
	public static float GlobalLocked = 1;
	public static bool LimInt = true;
	public float Factor;

	public float Xsize;
	public float Ysize;

	public Resolution(ElementGui curGui)
	{
		Xsize = GraphicsDevice.Global.Size.x;
		Ysize = GraphicsDevice.Global.Size.y;

		if(AllowResolution)
		{
			if(AllowResolution || curGui.ForceResolution)
			{
				Factor = 0.5f;

				while(Xsize / (Factor + 0.5f) >= Application.ScaledSize.x && Ysize / (Factor + 0.5f) >=  Application.ScaledSize.y)
				{
					Factor += 0.5f;
				}

				if(LimInt && Factor * 2 % 2 != 0 && Factor - 0.5f > 0)
				{
					Factor -= 0.5f;
				}
			}
			else
			{
				Factor = 1;
			}
		}
		else
		{
			Factor = GlobalLocked;
		}

		if(curGui != null)
		{
			Factor *= curGui.ScaleMul;

			float locked = curGui.ScaleLocked;

			if(locked > 0)
			{
				Factor = locked;
			}
		}

		Xsize /= Factor;
		Ysize /= Factor;

		curGui?.Resolve(this);
	}

}
