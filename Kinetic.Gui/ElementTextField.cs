using System.Text;
using Kinetic.App;
using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementTextField : Element
{

	public static RemoteKey A_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.KEY_A);
	public static RemoteKey C_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.KEY_C);
	public static RemoteKey V_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.KEY_V);
	public static RemoteKey LD_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.KEY_LEFT);
	public static RemoteKey RD_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.KEY_RIGHT);

	public static int CURSOR_SHINE_TIME = Application.MaxTps / 3;
	readonly StringBuilder text = new StringBuilder();

	int clock;
	bool cursorOn;
	int pointer;
	bool selectAll;
	bool uppos;

	public string InputHint = "";
	public Icon[] Icons = new Icon[3];
	public float TotalSize;
	public SideScroller Scroller = new SideScroller();

	public string Text
	{
		get => text.ToString();
		set
		{
			text.Clear();
			text.Append(value);
			pointer = text.Length;
		}
	}

	public override void Update()
	{
		base.Update();

		clock++;

		cursorOn = false;

		if(!IsExposed()) return;

		Scroller.TotalSize = TotalSize;
		Scroller.Update(this);

		bool boundIn = Bound.Contains(Cursor);
		bool pressed = RemoteKey.MouseLeft.Pressed();

		RemoteKeyboard input = RemoteKeyboard.Global;

		if(Element.Highlighted == this)
		{
			//Common Input Operations
			string txt = input.Text;
			if(!string.IsNullOrEmpty(txt))
			{
				insert(txt);
				input.ConsumeTextInput();
			}
			//Process Clipboard Operations
			if(RemoteKey.KeyCtrl.Holding())
			{
				if(V_CODE.Pressed())
				{
					insert(input.ClippedText);
				}
				if(A_CODE.Pressed())
				{
					selectAll = !selectAll;
				}
				if(C_CODE.Pressed() && selectAll)
				{
					input.ClippedText = text.ToString();
					selectAll = false;
				}
			}
			if(RemoteKey.KeyEnter.Pressed())
			{
				insert("\n");
			}
			//END

			//Pointer Move Operations
			if(LD_CODE.Pressed())
			{
				pointer = System.Math.Max(0, pointer - 1);
			}
			if(RD_CODE.Pressed())
			{
				pointer = System.Math.Min(text.Length, pointer + 1);
			}
			//Backspace Operations
			if(RemoteKey.KeyBackspace.Pressed())
			{
				if(text.Length > 0 && pointer != 0)
				{
					pointer = System.Math.Max(0, pointer - 1);
					text.Remove(pointer, 1);
				}
				if(selectAll)
				{
					pointer = 0;
					text.Clear();
					selectAll = false;
				}
			}
		}
	}

	void insert(string txt)
	{
		text.Insert(pointer, txt);
		pointer += txt.Length;
		uppos = true;
	}

	public override void Draw(SpriteBatch batch)
	{
		if(Element.Highlighted == this)
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

		if(text.Length == 0)
		{
			renderInBox(batch, InputHint, new Vector4(1, 1, 1, 0.2f));
		}
		else
		{
			renderInBox(batch, Text, selectAll ? new Vector4(0.2f, 0.6f, 1f, 1f) : new Vector4(1, 1, 1, 1));
		}
	}

	void renderInBox(SpriteBatch batch, string textIn, Vector4 color)
	{
		float x = Bound.x + Scroller.Outline;
		float y = Bound.yprom - batch.Font.YSize - Scroller.Outline;

		batch.Color4(color);
		TotalSize = batch.Font.GetBounds(textIn, Bound.w - Scroller.Outline * 2).Height;

		if(uppos)
		{
			uppos = false;
			Scroller.TotalSize = TotalSize;
			Scroller.DownToGround(this);
		}

		float pos = Scroller.Pos;
		float o = Scroller.Outline;

		batch.Scissor(Bound.x + o, Bound.y + o - 1, Bound.w - o * 2, Bound.h - o * 2 + 2);
		batch.Draw(textIn, Bound.x + o, Bound.yprom + pos - batch.Font.YSize, Bound.w - o * 2);
		batch.ScissorEnd();

		Scroller.Draw(batch, this);

		if(Element.Highlighted == this && clock % CURSOR_SHINE_TIME > CURSOR_SHINE_TIME / 2)
		{
			GlyphBounds bounds = batch.Font.GetBounds(textIn.Substring(0, pointer), Bound.w - Scroller.Outline * 2);
			//do not use emptyDisplay
			x += bounds.LastWidth;
			y -= bounds.Height - o - batch.Font.YSize - pos;

			batch.NormalizeColor();
			if(Bound.Contains(x, y)) batch.Fill(x, y, 1, batch.Font.YSize);
		}

		batch.NormalizeColor();
	}

}
