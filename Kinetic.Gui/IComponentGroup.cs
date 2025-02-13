using Kinetic.Math;

namespace Kinetic.Gui;

public interface IComponentGroup
{

	T Join<T>(T component) where T : Element;

	void Remove(Element stru);

	void Ascend(Element stru);

	void UpdateComponents(VaryingVector2 cursor, Vector2 tls);

	List<Element> Values { get; }

}
