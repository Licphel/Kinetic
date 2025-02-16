using Kinetic.Math;

namespace Morinia.World.TheGen;

public class NoiseOctave : Noise
{

    readonly NoisePerlin[] noises;

    public NoiseOctave(Seed seed, int octave)
    {
        noises = new NoisePerlin[octave];

        for(int i = 0; i < octave; i++)
        {
            noises[i] = new NoisePerlin(seed.Copyx(i * i - i * 3));
        }
    }

    public float Generate(float x, float y, float z)
    {
        float v = 0;
        float freq = 1, ampl = 1f / noises.Length;

        for(int i = 0; i < noises.Length; i++)
        {
            v += noises[i].Generate(x * freq, y * freq, z * freq) * ampl;
            freq *= 2;
        }
        return v;
    }

}