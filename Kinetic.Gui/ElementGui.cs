using Kinetic.App;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementGui : ElementManager
{

	public static List<ElementGui> Viewings = new();
	public static NinePatch DefaultTooltipPatches;

	public int OpenTicks;
	public ElementGui Parent;
	public float ScaleFactor;

	public Vector2 Size;
	public VaryingVector2 TempCursor = new VaryingVector2();

	public Resolution Resolution;
	public virtual float FontScmul => 1;
	public virtual float ScaleMul => 1;
	public virtual float ScaleLocked => -1;
	public virtual bool ForceResolution => false;

	bool IsInited;

	public virtual void Reflush()
	{
		if(!IsInited)
		{
			IsInited = true;
			InitComponents();
		}
		RelocateComponents();

		OpenTicks = 0;
	}

	public void Resolve(Resolution res)
	{
		Resolution = res;
		Size = new Vector2(res.Xsize, res.Ysize);
		ScaleFactor = res.Factor;

		Reflush();
	}

	public virtual void InitComponents() { }

	public virtual void RelocateComponents() {}

	public ElementGui Extend(ElementGui scrp)
	{
		Parent = scrp;
		return this;
	}

	public void Display()
	{
		Viewings.Add(this);
		new Resolution(this);
		Reflush();
	}

	public void Close()
	{
		OnClosed();
		Viewings.Remove(this);
	}

	public virtual void OnClosed() { }

	public virtual void Update(VaryingVector2 cursor)
	{
		UpdateComponents(cursor, new Vector2());

		CollectTooltips();

		OpenTicks++;
		TempCursor.Copy(cursor);

		Element.GetHighlight(this);
	}

	public virtual void Draw(SpriteBatch batch)
	{
		foreach(Element comp in Components)
		{
			comp.Draw(batch);
		}

		DrawTooltips(batch);
	}

	public Vector4 GetScaledLargestCanvas()
	{
		float w = Size.x, h = Size.y;
		float rt = Application.ScaledSize.x / Application.ScaledSize.y;
		float fw, fh;

		if(w / h > rt)
		{
			fw = w;
			fh = w / rt;
		}
		else
		{
			fh = h;
			fw = h * rt;
		}

		float x0 = (w - fw) / 2, y0 = (h - fh) / 2;

		return new Vector4(x0, y0, fw, fh);
	}

	//Tooltips

	List<Lore> TooltipList = new List<Lore>();

	public virtual List<Lore> CollectTooltips()
	{
		List<Lore> lst = TooltipList;
		lst.Clear();

		foreach(Element component in Components)
		{
			if(component.Bound.Contains(TempCursor))
				component.CollectTooltips(lst);
		}

		lst.Reverse();//we need to reverse it because the opengl origin.

		return lst;
	}

	public virtual NinePatch TooltipBackground => DefaultTooltipPatches;

	public virtual void DrawTooltips(SpriteBatch batch)
	{
		if(TooltipList.Count <= 0) return;

		float maxw0 = Size.x / 1.5f;

		float eh = batch.Font.Scale * (batch.Font.YSize + 1);
		float h = 0;
		float mw = 0;

		foreach(Lore o in TooltipList)
		{
			var bd = batch.Font.GetBounds(o, maxw0);
			float w = bd.Width;
			mw = System.Math.Max(mw, w);
			h += bd.Height;
			if(w > maxw0) mw = maxw0;
		}

		float x = TempCursor.x + 10, y = TempCursor.y - 10 - h;

		if(x + 12 + mw >= Size.x)
		{
			x = System.Math.Max(Size.x - 12 - mw, 0);
		}

		if(y - 12 <= 0)
		{
			y = System.Math.Min(Size.y, 12);
		}

		DefaultTooltipPatches.Draw(batch, x - 4, y - 4, mw + 8, h + 8);

		float dy = 0;
		foreach(Lore o in TooltipList)
		{
			float hin = batch.Font.GetBounds(o, maxw0).Height;
			batch.Draw(o, x, y + dy + hin - eh, maxw0);
			dy += hin;
		}
	}

}