using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementTextView : Element
{

	public Lore DisplayedLore;
	public float TotalSize;
	public SideScroller Scroller = new SideScroller();

	public override void Update()
	{
		base.Update();

		if(!IsExposed()) return;
		
		Scroller.TotalSize = TotalSize;
		Scroller.Update(this);
	}

	public override void Draw(SpriteBatch batch)
	{
		TotalSize = batch.Font.GetBounds(DisplayedLore, Bound.w - 2).Height;
		
		float pos = Scroller.Pos;
		float o = Scroller.Outline;
		
		batch.Scissor(Bound.x + o, Bound.y + o - 1, Bound.w - o * 2, Bound.h - o * 2 + 2);
		batch.Draw(DisplayedLore, Bound.x + o, Bound.yprom + pos - batch.Font.YSize, Bound.w - o * 2);
		batch.ScissorEnd();
		
		Scroller.Draw(batch, this);
	}

}
