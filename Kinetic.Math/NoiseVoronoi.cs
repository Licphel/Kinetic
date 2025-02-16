using Morinia.World.TheGen;

namespace Kinetic.Math;

public class NoiseVoronoi : Noise
{

    long seed;

    public NoiseVoronoi(Seed seed)
    {
        this.seed = seed.GetCSeed();
    }

    public float Generate(float x, float y, float z)
    {
        int X = floor(x);
        int Y = floor(y);
        int Z = floor(z);
        
        int rsx = 0;
        int rsy = 0;
        int rsz = 0;
        
        for(int k = Z - 1; k <= Z + 1; k++)
        for(int j = Y - 1; j <= Y + 1; j++)
        for(int i = X - 1; i <= X + 1; i++)
        {
            float x1 = i + seedl(i, j, k, seed + 1923);
            float y1 = j + seedl(i, j, k, seed + 1875);
            float z1 = k + seedl(i, j, k, seed + 1082);
            float dx = x1 - x;
            float dy = y1 - y;
            float dz = z1 - z;

            if(dx * dx + dy * dy + dz * dz < int.MaxValue)
            {
                rsx = floor(x1);
                rsy = floor(y1);
                rsz = floor(z1);
            }
        }

        float v;

        return seedl(rsx, rsy, rsz, 1);
    }

    static int floor(float v)
    {
        return FloatMath.Floor(v);
    }

    static float seedl(int x, int y, int z, long seed)
    {
        long v1 = (x + 2687 * y + 433 * z + 941 * seed) & int.MaxValue;
        long v2 = (v1 * (v1 * v1 * 113 + 653) + 2819) & int.MaxValue;
        return 1 - (float) v2 / int.MaxValue;
    }

}