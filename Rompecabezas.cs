using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace RompecabezasCCA
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Rompecabezas : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D texture;
		Texture2D imgGanador;
		Texture2D minion;
		Pieza[,] pieza;
		int cortes;
		int anchoCorte;
		int altoCorte;
		int xx;
		int yy;

		bool ganador;
		int nPiezas;

		int movimiento = 0;

		float posIniX = 0;
		float posIniY = 0;
		char accion;

		bool fueMovido = false;

		public Rompecabezas (int nPiezas)
		{
			this.nPiezas = nPiezas;

			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			Window.Title = "Rompecabezas";
			graphics.PreferredBackBufferWidth = 812;
			graphics.PreferredBackBufferHeight = 456;
			graphics.ApplyChanges ();

			xx = nPiezas - 1;
			yy = nPiezas - 1;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
			texture = this.Content.Load<Texture2D> ("minion");
			imgGanador = this.Content.Load<Texture2D> ("ganaste");
			minion = this.Content.Load<Texture2D> ("minion");
			pieza = new Pieza[nPiezas, nPiezas];
			cortes = nPiezas;
			ganador = false;
			anchoCorte = (int)(texture.Width / cortes);
			altoCorte = (int)(texture.Height / cortes);
			generarRompecabezas ();
			mesclarPiezas ();	
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent ()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape)) {                      
				this.Exit ();

			}

			TouchCollection touchCollection = TouchPanel.GetState ();
			foreach (TouchLocation tl in touchCollection) {
				if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {					
					if (movimiento == 0) {
						posIniX = tl.Position.X;
						posIniY = tl.Position.Y;
					}
					movimiento++;
					if (movimiento == 2) {								
						if (tl.Position.X - posIniX > 10) {					
							accion = 'R';
						}

						if (tl.Position.X - posIniX < -10) {					
							accion = 'L';
						}

						if (tl.Position.Y - posIniY > 10) {					
							accion = 'D';
						} 

						if (tl.Position.Y - posIniY < -10) {					
							accion = 'U';
						}

						movimiento = 0;
					}
				} else {
					Console.WriteLine (accion);
					fueMovido = true;
				}
			}


			Pieza piezaAux = new Pieza ();

			if (!ganador && fueMovido) {

				fueMovido = false;

				if (accion =='U' && yy + 1 < cortes) {
					piezaAux = pieza [xx, yy];
					pieza [xx, yy] = pieza [xx, yy + 1];
					pieza [xx, yy + 1] = piezaAux;
					yy++;
				}

				if (accion =='D' && yy - 1 >= 0) {
					piezaAux = pieza [xx, yy];
					pieza [xx, yy] = pieza [xx, yy - 1];
					pieza [xx, yy - 1] = piezaAux;
					yy--;
				}


				if (accion =='R' && xx - 1 >= 0) {
					piezaAux = pieza [xx, yy];
					pieza [xx, yy] = pieza [xx - 1, yy];
					pieza [xx - 1, yy] = piezaAux;
					xx--;
				}


				if (accion =='L' && xx + 1 < cortes) {
					piezaAux = pieza [xx, yy];
					pieza [xx, yy] = pieza [xx + 1, yy];
					pieza [xx + 1, yy] = piezaAux;
					xx++;
				}

			}

			base.Update (gameTime);
		}


		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{		
			GraphicsDevice.Clear (Color.Blue);
			spriteBatch.Begin ();
			spriteBatch.Draw (minion, new Vector2(1000,0), null, Color.White, 0f, Vector2.Zero, new Vector2 (1f, 1f), SpriteEffects.None, 0f);
			if (!ganador) {
				int verifOrden = 0;
				for (int y = 0; y < cortes; y++) {	
					for (int x = 0; x < cortes; x++) {			
						spriteBatch.Draw (texture, obtenerUbicacion (x, y), pieza [x, y].getSourceRectangle (), Color.White, 0f, Vector2.Zero, new Vector2 (1f, 1f), SpriteEffects.None, 0f);

						if (pieza [x, y].esOrdenCorrecto (x, y))
							verifOrden++;
					}  
					if (verifOrden == nPiezas * nPiezas) {						
						ganador = true;
					}
				}
			} else {
					
			}
			spriteBatch.End ();


			base.Draw (gameTime);
		}

		protected void generarRompecabezas ()
		{	
			for (int y = 0; y < cortes; y++) {	
				for (int x = 0; x < cortes; x++) {	
					if (x == xx && y == yy) {
						pieza [x, y] = new Pieza (x, y, new Rectangle (texture.Width + 5, texture.Height + 5, anchoCorte - 5, altoCorte - 5), true);
					} else {
						pieza [x, y] = new Pieza (x, y, new Rectangle (anchoCorte * x + 5, altoCorte * y + 5, anchoCorte - 5, altoCorte - 5), false);
					}
				}
			}
		}

		public Vector2 obtenerUbicacion (int x, int y)
		{
			return new Vector2 ((float)(anchoCorte * x), (float)(altoCorte * y));
		}



		protected void mesclarPiezas ()
		{	
			//Precargar en vector auxiliar

			Pieza[,] piezaAux = new Pieza[nPiezas, nPiezas];

			for (int y = 0; y < cortes; y++) {	
				for (int x = 0; x < cortes; x++) {				
					piezaAux [x, y] = pieza [x, y];
				}
			} 

			//Mesclar piezas
			for (int y = 0; y < cortes; y++) {	
				for (int x = 0; x < cortes; x++) {				
					pieza [x, y] = piezaAux [y, x];
				}
			} 
		}

	}
}
