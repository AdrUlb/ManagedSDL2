using static Native.SDL_ttf;
namespace ManagedSDL2
{
	public partial class TTF
	{
		public class Font
		{
			public Font(string file, int pointSize)
			{
				TTF_OpenFont("assets/fonts/OpenSans/OpenSans-Regular.ttf", 14);
			}
		}
	}
}
