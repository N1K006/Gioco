using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plants_Vs_Zombies
{
    class Salvataggio
    {
        public static void Salva()
        {
            FileStream PianteOttenute = File.Open(@"..\..\..\Salvataggio\PianteOttenute.txt", FileMode.Create);
            FileStream PianteUsate = File.Open(@"..\..\..\Salvataggio\PianteUsate.txt", FileMode.Create);
            FileStream Altro = File.Open(@"..\..\..\Salvataggio\Altro.txt", FileMode.Create);

            BinaryWriter wo = new BinaryWriter(PianteOttenute, Encoding.UTF8, false);

            foreach (Pianta p in Program.all)
                wo.Write(Program.piante_ottenute.Contains(p));

            wo.Close();
            PianteOttenute.Close();


            BinaryWriter wu = new BinaryWriter(PianteUsate, Encoding.UTF8, false);

            foreach (Pianta p in Home.piante)
                if(p != null)
                    wu.Write((Int16)Array.IndexOf(Program.all, p));
                else
                    wu.Write((Int16)(-1));


            BinaryWriter wa = new BinaryWriter(Altro, Encoding.UTF8, false);

            wa.Write((Int32)Program.monete);

            wa.Close();
            Altro.Close();
        }
        public static void Carica()
        {
            FileStream PianteOttenute = File.Open(@"..\..\..\Salvataggio\PianteOttenute.txt", FileMode.Open, FileAccess.Read);
            FileStream PianteUsate = File.Open(@"..\..\..\Salvataggio\PianteUsate.txt", FileMode.Open, FileAccess.Read);
            FileStream Altro = File.Open(@"..\..\..\Salvataggio\Altro.txt", FileMode.Open, FileAccess.Read);

            if (File.Exists(@"..\..\..\Salvataggio\PianteOttenute.txt"))
            {
                BinaryReader ro = new BinaryReader(PianteOttenute, Encoding.UTF8, false);

                foreach (Pianta p in Program.all)
                    if (PianteOttenute.Position < PianteOttenute.Length)
                        if (ro.ReadBoolean())
                            Program.piante_ottenute.Add(p);
                ro.Close();
                PianteOttenute.Close();
            }

            if (File.Exists(@"..\..\..\Salvataggio\PianteUsate.txt"))
            {
                BinaryReader ru = new BinaryReader(PianteUsate, Encoding.UTF8, false);

                for (int i = 0; i < 8; i++)
                {
                    if (PianteUsate.Position < PianteUsate.Length)
                    {
                        int j = ru.ReadInt16();
                        if (j == -1)
                            Home.piante[j] = null;
                        else
                            Program.piante[j] = Program.all[j];
                    }
                }
            }

            if (File.Exists(@"..\..\..\Salvataggio\Altro.txt"))
            {
                BinaryReader ra = new BinaryReader(Altro, Encoding.UTF8, false);
                if (PianteUsate.Position < PianteUsate.Length)
                    Program.monete = ra.ReadInt32();

                ra.Close();
                Altro.Close();
            }
        }

    }
}
