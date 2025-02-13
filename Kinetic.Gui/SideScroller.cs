using Kinetic.App;
using Kinetic.Input;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class SideScroller
{

	float bx, by, bw, bh;
	float lcy, sp0;
	float prevPos;
	float startPos;

	public bool IsDragging;
	public float Speed = 250;
	float accels;
	public float TotalSize;
	public float Outline;
	public float Pos => Time.PartialTicks * (startPos - prevPos) + prevPos;

	public SideScroller(float otl = 0)
	{
		Outline = otl;
		startPos = -Outline;
	}

	public void UpToTop(Element c)
	{
		startPos = -Outline;
		ClampPos(c);
		prevPos = startPos;
	}

	public void DownToGround(Element c)
	{
		startPos = TotalSize - c.Bound.h + Outline * 2;
		ClampPos(c);
		prevPos = startPos;
	}

	protected void ClampPos(Element c)
	{
		//A minimum offset.
		if(startPos <= -Outline)
		{
			startPos = -Outline;
		}

		if(startPos - TotalSize - Outline >= -c.Bound.h)
		{
			startPos = TotalSize - c.Bound.h + Outline;
		}

		if(TotalSize + Outline * 2 < c.Bound.h)
		{
			startPos = -Outline;
		}
	}

	public void Update(Element c)
	{
		RemoteKeyboard input = RemoteKeyboard.Global;

		float scr = input.Scroll;

		prevPos = startPos;

		if(c.Bound.Contains(c.Cursor) && scr != 0)
		{
			switch(input.ScrollDirection)
			{
				case ScrollDirection.UP:
					accels -= Speed;
					break;
				case ScrollDirection.DOWN:
					accels += Speed;
					break;
			}
			input.ConsumeCursorScroll();
		}

		startPos += accels * Time.Delta;
		accels *= 0.75f;
		ClampPos(c);

		if(RemoteKey.MouseLeft.Holding())
		{
			if(!IsDragging)
			{
				float mx = c.Cursor.x;
				float my = c.Cursor.y;

				if(mx >= bx - 1 && mx <= bx + bw + 1 && my >= by - 1 && my <= by + bh + 1)
				{
					IsDragging = true;
					lcy = c.Cursor.y;
					sp0 = startPos;
				}
			}
		}
		else
		{
			IsDragging = false;
		}

		if(IsDragging)
		{
			startPos = sp0 - (c.Cursor.y - lcy) / c.Bound.h * TotalSize;
			ClampPos(c);
		}
	}

	public void Draw(SpriteBatch batch, Element c)
	{
		float per = (c.Bound.h - Outline * 2) / TotalSize;
		if(per > 1)
		{
			per = 1;
		}

		float scrollPer = System.Math.Abs(Pos) / TotalSize;
		float h = c.Bound.h * per;
		float oh = scrollPer * c.Bound.h;

		bh = h;
		bx = c.Bound.xprom - 5;
		by = c.Bound.yprom - oh - h;
		bw = 2;

		if(per < 1)
		{
			batch.Fill(bx, by, bw, bh);
		}
	}

}
