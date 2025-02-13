namespace Kinetic.Math;

public class SeedLoot<E>
{

	public readonly List<E> Objects = new List<E>();

	public int Count => Objects.Count;

	public SeedLoot<E> Put(E obj, int weight)
	{
		for(int i = 0; i < weight; i++)
		{
			Objects.Add(obj);
		}
		return this;
	}

	public E Get(int index)
	{
		return Objects[index];
	}

}
