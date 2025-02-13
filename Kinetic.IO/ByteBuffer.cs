namespace Kinetic.IO;

public partial class ByteBuffer
{

	public void WriteBytes(byte[] bytes, int length)
	{
		WriteBytes(bytes, 0, length);
	}

	public void WriteBytes(byte[] bytes)
	{
		WriteBytes(bytes, bytes.Length);
	}

	public void Write(ByteBuffer buffer)
	{
		if(buffer.ReadableBytes <= 0) return;
		ByteChunk chk = buffer.Output();
		WriteBytes(chk.Bytes, chk.Offset, chk.Len);
	}

	public void WriteShort(short value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public void WriteInt(int value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public void WriteLong(long value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public void WriteFloat(float value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public void WriteByte(byte value)
	{
		int afterLen = WriteIndex + 1;
		int len = Buf.Length;
		FixSizeAndReset(len, afterLen);
		Buf[WriteIndex] = value;
		WriteIndex = afterLen;
	}

	public void WriteByte(int value)
	{
		byte b = (byte) value;
		WriteByte(b);
	}

	public void WriteDouble(double value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public void WriteChar(char value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public void WriteString(string value)
	{
		WriteInt(value.Length);

		for(int i = 0; i < value.Length; i++)
		{
			WriteChar(value[i]);
		}
	}

	public void WriteBoolean(bool value)
	{
		WriteBytes(Flip(BitConverter.GetBytes(value)));
	}

	public short ReadShort()
	{
		return BitConverter.ToInt16(Read(2), 0);
	}

	public int ReadInt()
	{
		return BitConverter.ToInt32(Read(4), 0);
	}

	public long ReadLong()
	{
		return BitConverter.ToInt64(Read(8), 0);
	}

	public float ReadFloat()
	{
		return BitConverter.ToSingle(Read(4), 0);
	}

	public double ReadDouble()
	{
		return BitConverter.ToDouble(Read(8), 0);
	}

	public char ReadChar()
	{
		return BitConverter.ToChar(Read(2), 0);
	}

	public bool ReadBoolean()
	{
		return BitConverter.ToBoolean(Read(1), 0);
	}

	public string ReadString()
	{
		int len = ReadInt();
		char[] chars = new char[len];

		for(int i = 0; i < len; i++)
		{
			chars[i] = ReadChar();
		}

		return new string(chars);
	}

	public void ReadBytes(byte[] bytes, int len)
	{
		for(int i = 0; i < len; i++)
		{
			bytes[i] = ReadByte();
		}
	}

}
