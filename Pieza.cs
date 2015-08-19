using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RompecabezasCCA
{
	public class Pieza
	{
		//Atributos
		bool vacio;
		Rectangle sourceRectangle;

		int ordenX;
		int ordenY;

		//Constructor
		public Pieza(){
		}

		public Pieza(int ordenX, int ordenY, Rectangle sourceRectangle, bool vacio){
			this.ordenX = ordenX;
			this.ordenY = ordenY;
			this.sourceRectangle = sourceRectangle;
			this.vacio = vacio;
		}

		public Rectangle getSourceRectangle(){
			return sourceRectangle;
		}

		public bool esOrdenCorrecto(int x, int y){
			if (ordenX == x && ordenY == y)
				return true;
			else
				return false;
		}
	}
}
