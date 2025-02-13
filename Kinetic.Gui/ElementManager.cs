using Kinetic.Math;

namespace Kinetic.Gui;

public class ElementManager : IComponentGroup
{

	public List<Element> Components = new List<Element>();
	public readonly List<Element> ToRemove = new List<Element>();
	public readonly List<Element> ToAdd = new List<Element>();

	public T Join<T>(T component) where T : Element
	{
		ToAdd.Add(component);
		return component;
	}

	public void Remove(Element stru)
	{
		ToRemove.Add(stru);
	}

	//Set the index to the top to let it get displayed firstly to users.
	//Will cause id gap. but it doesn't matter.
	public void Ascend(Element stru)
	{
		ToRemove.Add(stru);
		ToAdd.Add(stru);
	}

	public void UpdateComponents(VaryingVector2 cursor, Vector2 tls)
	{
		foreach(Element comp in Components)
		{
			comp.Bound.Translate(tls.x, tls.y);
			comp.Cursor.Copy(cursor);//do not override it
			comp.Update();
			if(comp.Removed)
				Remove(comp);
			comp.Bound.Translate(-tls.x, -tls.y);
		}

		foreach(Element c in ToRemove)
		{
			Components.Remove(c);
		}

		foreach(Element component in ToAdd)
		{
			Components.Add(component);
			component.Parent = this;
		}

		ToRemove.Clear();
		ToAdd.Clear();
	}

	public List<Element> Values => Components;

}
