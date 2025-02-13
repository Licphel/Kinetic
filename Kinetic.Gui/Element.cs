using Kinetic.Input;
using Kinetic.Math;
using Kinetic.Visual;

namespace Kinetic.Gui;

public abstract class Element
{

	public static Element Highlighted;

	public Box Bound = new Box();
	public IComponentGroup Parent;
	public VaryingVector2 Cursor = new VaryingVector2();
	public List<Lore> DefaultAttachedTooltips = new List<Lore>();
	public bool Removed;

	public virtual void Update()
	{
	}

	public virtual void Draw(SpriteBatch batch) { }

	public virtual void CollectTooltips(List<Lore> list)
	{
		list.AddRange(DefaultAttachedTooltips);
	}

	public bool IsExposed()
	{
		if(Parent == null) return true;

		int idx = Parent.Values.IndexOf(this);

		for(int i = 0; i < Parent.Values.Count; i++)
		{
			Element c = Parent.Values[i];
			if(c.Bound.Contains(Cursor) && i > idx)
			{
				return false;
			}
		}

		//if is in a window
		if(Parent is ElementPage win)
		{
			int idx1 = win.Parent.Values.IndexOf(win);
			//check in screen other windows.
			for(int i1 = 0; i1 < win.Parent.Values.Count; i1++)
			{
				Element c1 = win.Parent.Values[i1];
				if(c1.Bound.Contains(Cursor) && i1 < idx1 && c1 is ElementPage)
				{
					return false;
				}
			}
		}

		return true;
	}

	public static void GetHighlight(IComponentGroup group, bool isRoot = true)
	{
		if(!RemoteKey.MouseLeft.Pressed()) return;

		if(isRoot) Element.Highlighted = null;

		foreach(Element c in group.Values)
		{
			if(c.Bound.Contains(RemoteKeyboard.Global.Cursor) && c.IsExposed())
			{
				Element.Highlighted = c;
				if(c is IComponentGroup g1)
				{
					GetHighlight(g1, false);
				}
			}
		}
	}

}
