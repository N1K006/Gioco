using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Timers;
using System.Collections.Generic;

namespace Plants_Vs_Zombies
{
	static class Piante
	{
		static public RenderWindow Finestra;

		static public Pianta[] PianteSelezionate; //piante selezionate
		static public List<Pianta> PianteDisponibili = new();

		static Pianta piantaSelezionata = null; // pianta selezionata
		static public bool stato = false;

		public static void Disegna()
		{
			Home.INDIETRO.Position = new Vector2f(14, 14);
			Home.INDIETRO.Scale = new Vector2f(0.433f, 0.433f);
			Finestra.Draw(Home.HOME);
			Finestra.Draw(Home.INDIETRO);

			RectangleShape cornicetta = new RectangleShape(new Vector2f(1035, 440))
			{
				Position = new Vector2f(5, 142),
				FillColor = new Color(0, 0, 0)
			};
			Finestra.Draw(cornicetta);

			// piante selezionate
			{
				Vector2f scala = new Vector2f(0.45f, 0.45f);
				float larghezza = 340 * scala.X;
				float altezza = 170 * scala.Y;

				RectangleShape rect = new RectangleShape(new Vector2f(2 * larghezza + 40, 4 * altezza + 125))
				{
					Position = new Vector2f(10, 147),
					FillColor = new Color(0, 207, 45)
				};
				Finestra.Draw(rect);

				for (int y = 0; y < 4; y++)
					for (int x = 0; x < 2; x++)
						if (PianteSelezionate[2 * y + x] != null)
							PianteSelezionate[2 * y + x].DisegnaLista(new Vector2f(20  + (larghezza + 20) * x, 157 + (altezza + 35) * y), scala);
						else
						{
							RectangleShape n = new RectangleShape(new Vector2f(larghezza, altezza))
							{
								Position = new Vector2f(20  + (larghezza + 20) * x, 
														157 + (altezza   + 35) * y),
								FillColor = new Color(100, 100, 100, 220)
							};
							Finestra.Draw(n);
						}
			}
			// altre piante
			{
				Vector2f scala = new Vector2f(0.35f, 0.35f);
				float larghezza = 340 * scala.X;
				float altezza = 170 * scala.Y;

				int nx = 5;

				RectangleShape rect = new RectangleShape(new Vector2f(nx * larghezza + (nx - 1) * 10 + 36, 5 * altezza + 133))
				{
					Position = new Vector2f(365, 147),
					FillColor = new Color(140, 0, 160)
				};
				Finestra.Draw(rect);

				for (int x = 0; x < nx; x++)
					for (int y = 0; nx * y + x < PianteDisponibili.Count; y++)
						PianteDisponibili[nx * y + x].DisegnaLista(new Vector2f(383 + (larghezza + 10) * x,
																			 161 + (altezza   + 15) * y), scala);
			}
		}

		public static void SelezionaPiante(ref Pianta[] piante)
		{
			stato = true;
			PianteDisponibili.Clear();
			Piante.PianteSelezionate = piante;

			foreach (Pianta p in Program.piante_ottenute)
				PianteDisponibili.Add(p);
			foreach (Pianta aux in piante)
				PianteDisponibili.Remove(aux);

			Finestra.MouseButtonPressed -= Home.MouseClick;
			Finestra.MouseButtonPressed += MouseClick;

			while (Finestra.IsOpen && Home.schermata == 2)
			{
				Finestra.Clear();
				Disegna();

				Finestra.DispatchEvents();
				Finestra.Display();
			}
			piante = Piante.PianteSelezionate;
		}

		public static void MouseClick(object sender, MouseButtonEventArgs e)
		{
			int x = e.X;
			int y = e.Y;

			Logger.WriteLine("X: " + x.ToString() + " " + "Y: " + y, 6);

			if (x >= 14 && x <= 86 && y >= 14 && y <= 86)  // tasto indietro
			{
				bool completo = true;
				for (int i = 0; i < PianteSelezionate.Length; i++)
				{
					if (PianteSelezionate[i] == null)
						completo = false;
				}
				if (completo || PianteDisponibili.Count == 0)
					Home.schermata = 0;
			}
			else if(x >= 20 && x <= 345) // piante selezionate
			{
				int larghezza = (int)(340 * 0.45);
				int altezza = (int)(170 * 0.45);

				int X = x - 20;
				int auxX = X % (larghezza + 20);
				X /= (larghezza + 20);

				int Y = y - 157;
				int auxY = Y % (altezza + 35);
				Y /= (altezza + 35);

				int n = 8;
				if (auxX <= larghezza && auxY <= altezza && auxX >= 0 && auxY >= 0)
					n = 2 * Y + X;

				if (n < 8)
					if (PianteSelezionate[n] != null)
					{
						PianteDisponibili.Add(PianteSelezionate[n]);
						PianteSelezionate[n] = null;
					}
					else if(piantaSelezionata != null)
					{
						PianteSelezionate[n] = piantaSelezionata;
						PianteDisponibili.Remove(piantaSelezionata);
					}
				piantaSelezionata = null;
			}
			else if (x >= 383 && x <= 1017) // altre piante
			{
				int larghezza = (int)(340 * 0.35);
				int altezza = (int)(170 * 0.35);

				int X = x - 383;
				int auxX = X % (larghezza + 10);
				X /= (larghezza + 10);

				int Y = y - 161;
				int auxY = Y % (altezza + 15);
				Y /= (altezza + 15);

				int n = -1;

				if (auxX <= larghezza && auxY <= altezza && auxX > 0 && auxY > 0)
				{
					n = 5 * Y + X;
				}

				if(n != -1 && n < PianteDisponibili.Count)
					piantaSelezionata = PianteDisponibili[n];
				else 
					piantaSelezionata = null;
			}
		}
	}
}