using System.Collections;

namespace Kinetic.IO;

public interface IBinaryCompound : IEnumerable<KeyValuePair<string, object>>
{

	public static IBinaryCompound New()
	{
		return new BinaryCompoundImpl();
	}

	int Count { get; }

	public IBinaryList GetListSafely(string key)
	{
		return Get<IBinaryList>(key, IBinaryList.New());
	}

	public IBinaryCompound GetCompoundSafely(string key)
	{
		return Get<IBinaryCompound>(key, IBinaryCompound.New());
	}

	public T Search<T>(string treekey)
	{
		string[] keys = treekey.Split('.');

		if(keys == null || keys.Length <= 1)
			return Get<T>(treekey);

		IBinaryCompound c = null;
		for(int i = 0; i < keys.Length; i++)
		{
			string k = keys[i];
			if(i == 0)
				c = GetCompoundSafely(k);
			else if(i == keys.Length - 1)
				return c.Get<T>(k);
			else
				c = c.GetCompoundSafely(k);
		}
		return default;
	}

	T Get<T>(string key);

	public T Get<T>(string key, T defv)
	{
		return Has(key) ? Get<T>(key) : defv;
	}

	bool Has(string key);

	public bool Try<T>(string key, out T valout)
	{
		if(Has(key))
		{
			valout = Get<T>(key);
			return true;
		}
		valout = default;
		return false;
	}

	void Set(string key, object v);

	void Clear();

	IBinaryCompound _Copy();

	public void Merge(IBinaryCompound compound)
	{
		foreach(var kv in compound)
			Set(kv.Key, kv.Value);
	}

	public IBinaryCompound Copy()
	{
		IBinaryCompound compound = _Copy();

		foreach(KeyValuePair<string, object> pair in this)
		{
			switch(pair.Value)
			{
				case IBinaryCompound c1:
					compound.Set(pair.Key, c1.Copy());
					break;
				case IBinaryList l1:
					compound.Set(pair.Key, l1.Copy());
					break;
				default:
					compound.Set(pair.Key, pair.Value);
					break;
			}
		}

		return compound;
	}

	public bool Compare(IBinaryCompound compound)
	{
		if(Count != compound.Count)
			return false;

		foreach(KeyValuePair<string, object> kv in compound)
		{
			object o1 = kv.Value;
			object o2 = Get<object>(kv.Key);

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

}

class BinaryCompoundImpl : IBinaryCompound
{

	public Dictionary<string, object> Map;

	public BinaryCompoundImpl(Dictionary<string, object> MapCpy)
	{
		Map = MapCpy;
	}

	public BinaryCompoundImpl(bool isReadOnly = false)
	{
		Map = new Dictionary<string, object>();
	}

	public int Count => Map.Count;

	public T Get<T>(string key)
	{
		return BinaryDiCall.Cast<T>(Map.GetValueOrDefault(key, default));
	}

	public void Set(string key, object v)
	{
		Map[key] = v;
	}

	public bool Has(string key)
	{
		return Map.ContainsKey(key);
	}

	public void Clear()
	{
		Map.Clear();
	}

	public IBinaryCompound _Copy()
	{
		return IBinaryCompound.New();
	}

	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		return Map.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

}