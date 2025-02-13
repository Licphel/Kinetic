using System.Collections;

namespace Kinetic.IO;

public interface IBinaryList : IEnumerable<object>
{

	public static IBinaryList New()
	{
		return new BinaryList();
	}

	byte Type => Count > 0 ? BinaryIO.GetId(Get<object>(0)) : (byte) 0;
	int Count { get; }

	T Get<T>(int i);

	void Insert(object v);

	void Set(int i, object v);

	IBinaryList _Copy();

	public IBinaryList Copy()
	{
		IBinaryList lst = _Copy();

		foreach(object o in this)
		{
			switch(o)
			{
				case IBinaryCompound c1:
					lst.Insert(c1.Copy());
					break;
				case IBinaryList l1:
					lst.Insert(l1.Copy());
					break;
				default:
					lst.Insert(o);
					break;
			}
		}

		return lst;
	}

	public bool Compare(IBinaryList list)
	{
		if(Count != list.Count)
		{
			return false;
		}

		for(int i = 0; i < list.Count; i++)
		{
			object o1 = Get<object>(i);
			object o2 = list.Get<object>(i);

			if(o1.GetType() != o2.GetType())
			{
				return false;
			}

			bool eq;
			switch(o2)
			{
				case IBinaryCompound c1:
					eq = c1.Compare((IBinaryCompound) o1);
					break;
				case IBinaryList l1:
					eq = l1.Compare((IBinaryList) o1);
					break;
				default:
					eq = o1.Equals(o2);
					break;
			}

			if(!eq) return false;
		}

		return true;
	}

	class BinaryList : IBinaryList
	{

		public List<object> Values;

		public BinaryList(List<object> ListCpy)
		{
			Values = ListCpy;
		}

		public BinaryList()
		{
			Values = new List<object>();
		}

		public int Count => Values.Count;

		public T Get<T>(int i)
		{
			return BinaryDiCall.Cast<T>(Values[i]);
		}

		public void Insert(object v)
		{
			Values.Add(v);
		}

		public void Set(int i, object v)
		{
			Values[i] = v;
		}

		public IBinaryList _Copy()
		{
			return IBinaryList.New();
		}

		public IEnumerator<object> GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}

}