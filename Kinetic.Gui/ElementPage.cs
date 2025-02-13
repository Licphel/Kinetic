using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public class ElementPage : Element, IComponentGroup
{

	public ElementManager Container = new ElementManager();
	public Icon Icon;
	public int LabelH;
	public Lore Title;
	public Vector2 TitleOffset;
	public Vector2 CloserOffset;

	bool dragging;
	float lcx, lcy;
	ElementButton closer;

	public void SetCloseButton(ElementButton cls)
	{
		closer = cls;
		closer.OnLeftFired += () => Removed = true;
		closer.Bound.Locate(Bound.w + CloserOffset.x, Bound.h + CloserOffset.y);
		Join(closer);
	}

	public T Join<T>(T component) where T : Element
	{
		return Container.Join(component);
	}

	public void Remove(Element stru)
	{
		Container.Remove(stru);
	}

	public void Ascend(Element stru)
	{
		Container.Ascend(stru);
	}

	public void UpdateComponents(VaryingVector2 cursor, Vector2 tls)
	{
		Container.UpdateComponents(cursor, tls);
	}

	public List<Element> Values => Container.Values;

	public override void Update()
	{
		base.Update();

		UpdateComponents(Cursor, new Vector2(Bound.x, Bound.y));

		if(!IsExposed())
		{
			return;
		}

		if(!dragging && RemoteKey.MouseLeft.Pressed() && Bound.Contains(Cursor) && Cursor.y >= Bound.yprom - LabelH)
		{
			dragging = true;
			lcx = Cursor.x;
			lcy = Cursor.y;
			Parent.Ascend(this);
		}

		if(!RemoteKey.MouseLeft.Holding())
		{
			dragging = false;
		}
		else if(dragging)
		{
			float nx = Cursor.x, ny = Cursor.y;

			Bound.Translate(nx - lcx, ny - lcy);

			lcx = nx;
			lcy = ny;
		}
	}

	public override void CollectTooltips(List<Lore> list)
	{
		base.CollectTooltips(list);

		foreach(Element c in Container.Components)
		{
			c.Bound.Translate(Bound.x, Bound.y);
			if(c.Bound.Contains(Cursor))
				c.CollectTooltips(list);
			c.Bound.Translate(-Bound.x, -Bound.y);
		}
	}

	public override void Draw(SpriteBatch batch)
	{
		batch.Draw(Icon, Bound);

		base.Draw(batch);

		foreach(Element c in Container.Components)
		{
			c.Bound.Translate(Bound.x, Bound.y);
			c.Draw(batch);
			c.Bound.Translate(-Bound.x, -Bound.y);
		}

		batch.Draw(Title, Bound.x + TitleOffset.x, Bound.yprom - LabelH + TitleOffset.y);
	}

}
