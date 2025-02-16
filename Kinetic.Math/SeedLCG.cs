namespace Kinetic.Math;

public class SeedLCG : Seed
{

    readonly static int A = 4097;
    readonly static int C = 123435311;
    readonly static int M = (int) System.Math.Pow(2, 32);

    public long InitialSeed;
    public long NowSeed;

    public SeedLCG(long initialSeed)
    {
        SetSeed(initialSeed);
    }

    public SeedLCG()
    {
        SetSeed(new Random().Next());
    }

    long NewSeed()
    {
        NowSeed = (NowSeed * A + C) % M;
        return NowSeed;
    }

    public override float NextFloat()
    {
        return (float) NewSeed() / (M - 1);
    }

    public override int NextInt(int bound)
    {
        return (int) (bound * NextFloat());
    }

    public override void SetSeed(long seed)
    {
        InitialSeed = seed;
        NowSeed = seed;
    }

    public override long GetISeed()
    {
        return InitialSeed;
    }

    public override long GetCSeed()
    {
        return NowSeed;
    }

    public override Seed Copy()
    {
        return new SeedLCG(InitialSeed);
    }

    public override Seed Copyx(int offset)
    {
        return new SeedLCG(InitialSeed + offset);
    }

}