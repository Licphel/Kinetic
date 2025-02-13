namespace Kinetic.IO;

public struct ByteChunk
{

	public int Offset, Len;
	public byte[] Bytes;

	public ByteChunk(byte[] bytes, int o, int l)
	{
		Offset = o;
		Len = l;
		Bytes = bytes;
	}

	public byte[] PartialArray
	{
		get
		{
			byte[] bytes = new byte[Len];
			Array.Copy(Bytes, 0, bytes, 0, bytes.Length);
			return bytes;
		}
	}

}
