using System.Buffers.Binary;
using Kinetic.App;
using Kinetic.Auditory;
using Kinetic.IO;
using OpenTK.Audio.OpenAL;

namespace Kinetic.OpenAL;

public class OALAudioData : AudioData
{

	public int Id;
	public int Length;
	public int ByteDatSize;
	public int BPSample;
	public int SampleRate;
	public int NChannel;

	OALAudioData() { }

	public AudioClip CreateClip()
	{
		return new OALAudioClip(this);
	}

	public float GetLengthSeconds()
	{
		return (float) ByteDatSize / (SampleRate * BPSample / 8) / NChannel;
	}

	public static void InitALDevice()
	{
		ALDevice device = ALC.OpenDevice(null);
		ALContext context = ALC.CreateContext(device, (int[]) null);
		ALC.MakeContextCurrent(context);

		NativeManager.I0.Remind(() =>
		{
			ALC.DestroyContext(context);
			ALC.CloseDevice(device);
		});

		Application.Update += AudioClip.CheckClipStates;
	}

	public static unsafe OALAudioData Read(FileHandle handler)
	{
		ReadOnlySpan<byte> file = File.ReadAllBytes(handler.Path);

		int index = 0;

		if(file[index++] != 'R'
		   || file[index++] != 'I'
		   || file[index++] != 'F'
		   || file[index++] != 'F')
		{
			throw new Exception("Only support Wave file!");
		}

		BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
		index += 4;

		if(file[index++] != 'W'
		   || file[index++] != 'A'
		   || file[index++] != 'V'
		   || file[index++] != 'E')
		{
			throw new Exception("Only support Wave file!");
		}

		int sampleRate = 0;
		short bitsPerSample = 0;
		short numChannels = 0;
		ALFormat format = 0;

		int buffer = AL.GenBuffer();
		OALAudioData aad = new OALAudioData();
		aad.Id = buffer;

		while(index + 4 < file.Length)
		{
			string identifier = "" + (char) file[index++] + (char) file[index++] + (char) file[index++] +
			                    (char) file[index++];
			int size = BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
			index += 4;

			switch(identifier)
			{
				case "fmt " when size != 16:
					throw new Exception($"Unknown Audio Format with subchunk1 size {size}");
				case "fmt ":
					{
						short audioFormat = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
						index += 2;
						if(audioFormat != 1)
						{
							throw new Exception($"Unknown Audio Format with ID {audioFormat}");
						}
						aad.NChannel = numChannels = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
						index += 2;
						aad.SampleRate = sampleRate = BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
						index += 4;
						BinaryPrimitives.ReadInt32LittleEndian(file.Slice(index, 4));
						index += 4;
						BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
						index += 2;
						aad.BPSample = bitsPerSample = BinaryPrimitives.ReadInt16LittleEndian(file.Slice(index, 2));
						index += 2;

						if(numChannels == 1)
						{
							if(bitsPerSample == 8)
							{
								format = ALFormat.Mono8;
							}
							else if(bitsPerSample == 16)
							{
								format = ALFormat.Mono16;
							}
							else
							{
								throw new Exception($"Can't Play mono {bitsPerSample} sound.");
							}
						}
						else if(numChannels == 2)
						{
							if(bitsPerSample == 8)
							{
								format = ALFormat.Stereo8;
							}
							else if(bitsPerSample == 16)
							{
								format = ALFormat.Stereo16;
							}
							else
							{
								throw new Exception($"Can't Play stereo {bitsPerSample} sound.");
							}
						}
						else
						{
							throw new Exception($"Can't play audio with {numChannels} sound");
						}

						break;
					}
				case "data":
					{
						ReadOnlySpan<byte> data = file.Slice(index, size);
						index += size;
						aad.ByteDatSize = size;

						fixed(byte* pData = data)
						{
							AL.BufferData(buffer, format, pData, size, sampleRate);
						}

						break;
					}
				case "JUNK":
				case "iXML":
					// this exists to align things
					index += size;
					break;
				default:
					index += size;
					break;
			}
		}

		NativeManager.I0.Remind(() =>
		{
			AL.DeleteBuffer(buffer);
		});

		return aad;
	}

}
