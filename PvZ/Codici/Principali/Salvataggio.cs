using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Plants_Vs_Zombies
{
    class Salvataggio
    {
        public static object Lock = new object();
        public static void Salva()
        {
            lock (Lock)
            {
                string pianteOttenutePath = @"..\..\..\Salvataggio\PianteOttenute.txt";
                string pianteUsatePath = @"..\..\..\Salvataggio\PianteUsate.txt";
                string altroPath = @"..\..\..\Salvataggio\Altro.txt";

                using (FileStream PianteOttenute = File.Open(pianteOttenutePath, FileMode.Create))
                using (BinaryWriter wo = new BinaryWriter(PianteOttenute, Encoding.UTF8, false))
                {
                    foreach (Pianta p in Program.all)
                        wo.Write(Program.piante_ottenute.Contains(p));
                }

                using (FileStream PianteUsate = File.Open(pianteUsatePath, FileMode.Create))
                using (BinaryWriter wu = new BinaryWriter(PianteUsate, Encoding.UTF8, false))
                {
                    foreach (Pianta p in Program.piante)
                        if (p != null)
                        {
                            wu.Write((Int16)Array.IndexOf(Program.all, p));
                            Logger.WriteLine(Array.IndexOf(Program.all, p).ToString(), 5);
                        }
                        else
                            wu.Write((Int16)(-1));
                }

                using (FileStream Altro = File.Open(altroPath, FileMode.Create))
                using (BinaryWriter wa = new BinaryWriter(Altro, Encoding.UTF8, false))
                {
                    wa.Write((Int32)Program.monete);
                }
            }
        }

        public static void Carica()
        {
            lock (Lock)
            {
                string pianteOttenutePath = @"..\..\..\Salvataggio\PianteOttenute.txt";
                string pianteUsatePath = @"..\..\..\Salvataggio\PianteUsate.txt";
                string altroPath = @"..\..\..\Salvataggio\Altro.txt";

                if (File.Exists(pianteOttenutePath))
                {
                    using (FileStream PianteOttenute = File.Open(pianteOttenutePath, FileMode.Open, FileAccess.Read))
                    using (BinaryReader ro = new BinaryReader(PianteOttenute, Encoding.UTF8, false))
                    {
                        Program.piante_ottenute.Clear();
                        foreach (Pianta p in Program.all)
                            if (PianteOttenute.Position < PianteOttenute.Length)
                                if (ro.ReadBoolean())
                                {
                                    Program.piante_ottenute.Add(p);
                                }
                    }
                }

                if (File.Exists(pianteUsatePath))
                {
                    using (FileStream PianteUsate = File.Open(pianteUsatePath, FileMode.Open, FileAccess.Read))
                    using (BinaryReader ru = new BinaryReader(PianteUsate, Encoding.UTF8, false))
                    {
                        for (int i = 0; i < 8; i++)
                            if (PianteUsate.Position < PianteUsate.Length)
                            {
                                int j = ru.ReadInt16();
                                if (j == -1)
                                    Program.piante[i] = null;
                                else if (j >= 0 && j < Program.all.Length)
                                    Program.piante[i] = Program.all[j];
                            }
                    }
                }

                if (File.Exists(altroPath))
                {
                    using (FileStream Altro = File.Open(altroPath, FileMode.Open, FileAccess.Read))
                    using (BinaryReader ra = new BinaryReader(Altro, Encoding.UTF8, false))
                    {
                        if (Altro.Position < Altro.Length)
                        {
                            Program.monete = ra.ReadInt32();
                        }
                    }
                }
            }
        }
    }
}
