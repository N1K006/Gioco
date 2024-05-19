using System;
using System.Timers;
using SFML.Graphics;
using SFML.System;

namespace Plants_Vs_Zombies
{
	abstract class Pianta
	{
		public static Gioco gioco;
		public object LockVita = new object();
		protected int vita;
		public int costo_soli;
		public int costo_monete;
		public static IntRect rect;
		public static Texture texture;
		public Sprite pianta = new Sprite();
		public Timer attesa;
		int X, Y;

		protected Pianta(int x, int y)
		{
			X = x; Y = y;
			gioco.Mappa_piante[x, y] = this;
			pianta.Position = new Vector2f(254 + x * 81, 72 + y * 100);
		}

		protected Pianta() { }

		virtual public int Vita
        {
            get
            {
				lock (LockVita)
					return vita;
            }
			set 
			{
				if (value <= 0)
				{
					try
					{
						if (Program.fase != 0)
							gioco.Mappa_piante[X, Y] = null;
					}
					catch (NullReferenceException){ }
					GC.Collect();
				}
			} 
		}

		abstract public void GetInstace(int x, int y);
		abstract public Pianta GetInstace();
		abstract public bool Disponibile { get; set; }
		abstract public void DisegnaLista(Vector2f posizione, Vector2f scala);
        abstract public void Stop();
        //abstract public void Riprendi();
    }
}
