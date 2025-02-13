using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementSelection : Element
{

	public static RemoteKey CL_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.BUTTON_LEFT);
	public static RemoteKey CR_CODE = RemoteKeyboard.Global.Observe(RemoteKeyID.BUTTON_RIGHT);

	public List<Entry> Entries = new List<Entry>();

	public float EntryH;
	protected float TotalSize => Entries.Count * (EntryH + 1);
	public SideScroller Scroller = new SideScroller();

	public void Add(Entry entry)
	{
		Entries.Add(entry);
		entry.Index = Entries.IndexOf(entry);
		entry.Parent = this;
		entry.Cursor = Cursor;
		entry.Activate();
	}

	public override void Update()
	{
		base.Update();

		foreach(Entry t in Entries)
		{
			t.Update();
			if(t.IsHovering() && RemoteKey.MouseLeft.Pressed())
			{
				t.Pressed();
			}
		}

		if(!IsExposed()) return;

		Scroller.TotalSize = TotalSize;
		Scroller.Update(this);
	}

	public override void Draw(SpriteBatch batch)
	{
		renderBasement(batch);

		batch.Scissor(Bound.x - Scroller.Outline, Bound.y - Scroller.Outline,
			Bound.w + Scroller.Outline * 2, Bound.h + Scroller.Outline * 2);
		renderEntries(batch);
		batch.ScissorEnd();

		renderOverlay(batch);
	}

	public override void CollectTooltips(List<Lore> list)
	{
		base.CollectTooltips(list);

		foreach(Entry t in Entries)
		{
			if(t.IsHovering()) t.CollectTooltips(list);
		}
	}

	protected void renderEntries(SpriteBatch batch)
	{
		float per = Bound.h / TotalSize;
		if(per > 1)
		{
			per = 1;
		}

		float pos = Scroller.Pos;

		for(int i = 0; i < Entries.Count; i++)
		{
			Entries[i].Correct(pos);
		}
		for(int i = 0; i < Entries.Count; i++)
		{
			Entries[i].Draw(batch);
		}

		Scroller.Draw(batch, this);
	}

	protected void renderBasement(SpriteBatch batch)
	{
	}

	protected void renderOverlay(SpriteBatch batch)
	{
	}

	public abstract class Entry
	{

		public ElementSelection Parent;

		public float w, w0, h;
		public float x;
		public float y;
		protected float y0;

		public int Index { get; set; }
		public VaryingVector2 Cursor { get; set; }

		public virtual void Activate()
		{
			h = Parent.EntryH;
			w0 = w = Parent.Bound.w;
		}

		public virtual void Correct(float posOffset)
		{
			y0 = y = Parent.Bound.yprom - (Index + 1) * (h + 1);
			x = Parent.Bound.x + 1;
			y = y0 + posOffset;
			w = w0 - Parent.Scroller.Outline;
		}

		public virtual void Update()
		{
		}

		public virtual void CollectTooltips(List<Lore> list)
		{
		}

		public virtual void Draw(SpriteBatch batch)
		{
		}

		public virtual void Pressed()
		{
		}

		public virtual bool IsHovering()
		{
			if(Parent.Scroller.IsDragging)
			{
				return false;//Avoid accidental action
			}
			float mx = Cursor.x;
			float my = Cursor.y;
			return mx >= x && mx <= x + w && my >= y && my <= y + h && Parent.Bound.Contains(mx, my);
		}

	}

}
