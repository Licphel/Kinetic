using System.Globalization;
using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public delegate string TextConvert(float value);

public class ElementSlideBar : Element
{

	readonly ElementButton decrease;
	readonly ElementButton increase;
	Box decRect, incRect;

	public Lore? Display = null;

	public Icon Icon;
	int scrollBuf;
	public TextConvert TextRelinker = f => f.ToString("%.2f", CultureInfo.InvariantCulture);

	public float Value, MaxValue, MinValue, StepValue;

	public ElementSlideBar(ElementButton dec, ElementButton inc)
	{
		decrease = dec;
		increase = inc;
		decrease.OnLeftFired += () =>
		{
			Value -= StepValue;
			Check();
		};
		increase.OnLeftFired += () =>
		{
			Value += StepValue;
			Check();
		};
		decRect = decrease.Bound;
		incRect = increase.Bound;
	}

	public void Correct()
	{
		decRect.Locate(Bound.x, Bound.y);
		incRect.Locate(Bound.xprom - incRect.w, Bound.y);
	}

	void Check()
	{
		//avoid floating value
		if(Value < MinValue - 0.0001f)
		{
			Value = MaxValue;
		}
		if(Value > MaxValue + 0.0001f)
		{
			Value = MinValue;
		}
	}

	public override void Update()
	{
		base.Update();

		decrease.Update();
		increase.Update();

		RemoteKeyboard input = RemoteKeyboard.Global;

		float scr = input.Scroll;

		if(Bound.Contains(Cursor) && IsExposed() && scrollBuf < 0 && scr != 0)
		{
			if(input.ScrollDirection == ScrollDirection.UP)
			{
				Value += StepValue;
				Check();
			}
			else if(input.ScrollDirection == ScrollDirection.DOWN)
			{
				Value -= StepValue;
				Check();
			}
			scrollBuf = 2;
			input.ConsumeCursorScroll();
		}
		scrollBuf--;

		decrease.Update();
		increase.Update();
	}

	public override void Draw(SpriteBatch batch)
	{
		batch.Draw(Icon, Bound);

		string special = TextRelinker.Invoke(Value);
		Lore? drw = Display == null ? Lore.Literal(special) : Display?.Combine(Lore.Literal(": " + special));

		batch.Draw((Lore) drw, Bound.xcentral, Bound.y + 4, Align.CENTER);

		decrease.Draw(batch);
		increase.Draw(batch);
	}

}
