using System.Runtime.InteropServices;

namespace Kinetic.App;

public unsafe class NativeMem
{

	static HashSet<IntPtr> Disposes = new HashSet<IntPtr>();

	static NativeMem()
	{
		NativeManager.I0.Remind(() =>
		{
			foreach(IntPtr ip in Disposes)
			{
				MemFree((void*) ip);
			}
		});
	}

	public static int MemGetNextCap(int old)
	{
		return old < 64 ? 64 : old * 2;
	}

	public static T[] MemReallocate<T>(T[] ptr, int newcap)
	{
		if(newcap == 0)
			return new T[0];
		if(ptr == null)
			return new T[newcap];

		T[] newarr = new T[newcap];
		Array.Copy(ptr, 0, newarr, 0, ptr.Length);
		return newarr;
	}

	public static T* MemReallocate<T>(T* ptr, int newcap, int oldcap, bool autorelease = true)
	where T : unmanaged
	{
		newcap *= sizeof(T);
		if(ptr == null)
		{
			T* p = (T*) NativeMemory.Alloc((UIntPtr) newcap);
			if(autorelease)
				Disposes.Add((IntPtr) p);
			return p;
		}
		return (T*) NativeMemory.Realloc(ptr, (UIntPtr) newcap);
	}

	public static void MemFree(void* ptr)
	{
		if(ptr != null)
			NativeMemory.Free(ptr);
	}

}
