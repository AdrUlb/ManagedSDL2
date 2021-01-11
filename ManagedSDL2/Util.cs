using System.Drawing;
using static Native.SDL;

namespace ManagedSDL2
{
	static class Util
	{
		public static SDL_Rect CreateSDLRect(int x, int y, int width, int height) => new SDL_Rect() { x = x, y = y, w = width, h = height };
		public static SDL_Rect CreateSDLRect(Rectangle rect) => CreateSDLRect(rect.X, rect.Y, rect.Width, rect.Height);
		public static SDL_Color CreateSDLColor(Color color) => new SDL_Color() { r = color.R, g = color.G, b = color.B, a = color.A };
	}
}
