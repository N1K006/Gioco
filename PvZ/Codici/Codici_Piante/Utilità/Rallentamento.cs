using System;
using System.Timers;

namespace Plants_Vs_Zombies
{
    class Rallentamento
    {
        public int ValRall;
        Zombie z;

        Timer t;

        public Rallentamento(int val, int durata, Zombie z)
        {
            ValRall = val;
            this.z = z;
            z.rallentamenti.Add(this);
            t = new Timer(durata);
            t.Start();
            t.Elapsed += t_Elapsed;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (z != null)
                z.rallentamenti.Remove(this);
        }

        public static implicit operator int(Rallentamento r)
        {
            return r.ValRall;
        }
    }
}