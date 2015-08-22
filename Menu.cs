using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Android.Content;

namespace RompecabezasCCA
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Menu : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		// Global variables
		enum BState
		{
			HOVER,
			UP,
			JUST_RELEASED,
			DOWN
		}

		const int NUMBER_OF_BUTTONS = 3, 
		EASY_BUTTON_INDEX = 0,
		MEDIUM_BUTTON_INDEX = 1,
		HARD_BUTTON_INDEX = 2,
		BUTTON_HEIGHT = 200,
		BUTTON_WIDTH = 600;
		Color background_color;
		Color[] button_color = new Color[NUMBER_OF_BUTTONS];
		Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
		BState[] button_state = new BState[NUMBER_OF_BUTTONS];
		Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
		double[] button_timer = new double[NUMBER_OF_BUTTONS];
		//mouse pressed and mouse just pressed
		bool mpressed, prev_mpressed = false;
		//mouse location in window
		int mx, my;
		double frame_time;

		Texture2D fondo;

		public Menu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 812;
            graphics.PreferredBackBufferHeight = 456;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
			int x = Window.ClientBounds.Width/2 -  BUTTON_WIDTH / 2 + 300;
			int y = Window.ClientBounds.Height/2 - NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT - (NUMBER_OF_BUTTONS%2)*BUTTON_HEIGHT/2 + 25;
			for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
			{
				button_state[i] = BState.UP;
				button_color[i] = Color.White;
				button_timer[i] = 0.0;
				button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
				y += BUTTON_HEIGHT;
			}
			IsMouseVisible = true;
			background_color = Color.CornflowerBlue;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			fondo = this.Content.Load<Texture2D>("Juego");

			button_texture[EASY_BUTTON_INDEX] =  Content.Load<Texture2D>("facil");
			button_texture[MEDIUM_BUTTON_INDEX] = Content.Load<Texture2D>("medio");
			button_texture[HARD_BUTTON_INDEX] =  Content.Load<Texture2D>("dificil");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

			// get elapsed frame time in seconds
			frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

			// update mouse variables

			TouchCollection touchCollection = TouchPanel.GetState ();

			foreach (TouchLocation tl in touchCollection) {
				if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) {					
					mx = (int) tl.Position.X;
					my = (int) tl.Position.Y;

					prev_mpressed = mpressed;
					mpressed = tl.State == TouchLocationState.Pressed;
					update_buttons();
				}
			}



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			spriteBatch.Draw (fondo,  new Vector2(0,0), null, Color.White, 0f, Vector2.Zero, new Vector2 (1f, 1f), SpriteEffects.None, 0f);
			for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
				spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);
			spriteBatch.End();

            base.Draw(gameTime);
        }



		// wrapper for hit_image_alpha taking Rectangle and Texture
		bool hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
		{
			return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /  rect.Width, tex.Height * (y - rect.Y) / rect.Height);
		}

		// wraps hit_image then determines if hit a transparent part of image 
		bool hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
		{
			if (hit_image(tx, ty, tex, x, y))
			{
				if ((x - (int)tx) + (y - (int)ty) * tex.Width < tex.Width * tex.Height)
				{
					return true;
				}
			}
			return false;
		}

		// determine if x,y is within rectangle formed by texture located at tx,ty
		bool hit_image(float tx, float ty, Texture2D tex, int x, int y)
		{
			return (x >= tx &&
				x <= tx + tex.Width &&
				y >= ty &&
				y <= ty + tex.Height);
		}

		// determine state and color of button
		void update_buttons()
		{
			for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
			{

				if (hit_image_alpha(button_rectangle[i], button_texture[i], mx, my))
				{
					button_timer[i] = 0.0;
					if (mpressed) {
						// mouse is currently down
						button_state[i] = BState.DOWN;
						button_color[i] = Color.Blue;
					}
					else 
						if (!mpressed && prev_mpressed)	{
							// mouse was just released
							if (button_state[i] == BState.DOWN)
							{
								// button i was just down
								button_state[i] = BState.JUST_RELEASED;
							}
						}
						else {
							button_state[i] = BState.HOVER;
							button_color[i] = Color.LightBlue;
						}
				} 
				else {
					button_state[i] = BState.UP;
					if (button_timer[i] > 0) {
						button_timer[i] = button_timer[i] - frame_time;
					}
					else {
						button_color[i] = Color.White;
					}
				}

				if (button_state[i] == BState.JUST_RELEASED) {				
					take_action_on_button (i);
				}
			}
		}





		void take_action_on_button(int i)
		{
			this.Exit ();
			switch (i)
			{
			case EASY_BUTTON_INDEX:		
				
				new JuegoFacil ().Start ();
				break;
			case MEDIUM_BUTTON_INDEX:
				new JuegoMedio ().Start ();
				break;
			case HARD_BUTTON_INDEX:
				new JuegoDificil ().Start ();
				break;
			default:
				break;
			}
		}
    }
}
