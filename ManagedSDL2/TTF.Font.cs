using System;
using System.Drawing;
using static Native.SDL;
using static Native.SDL_ttf;

namespace ManagedSDL2
{
	public partial class TTF
	{
		public class Font : IDisposable
		{
			IntPtr ttfFontPtr;
			public Font(string file, int pointSize)
			{
				ttfFontPtr = TTF_OpenFont(file, pointSize);
			}

			public SDL.Texture RenderText(SDL.Renderer renderer, string text, Color color, bool preferSpeedOverQuality)
			{
				if (preferSpeedOverQuality)
					TTF_SetFontHinting(ttfFontPtr, TTF_HINTING_NORMAL);
				else
					TTF_SetFontHinting(ttfFontPtr, TTF_HINTING_LIGHT_SUBPIXEL);

				// Render text to surface
				var sdlColor = Util.CreateSDLColor(color);
				IntPtr surface = preferSpeedOverQuality ? TTF_RenderText_Solid(ttfFontPtr, text, sdlColor) : TTF_RenderText_Blended(ttfFontPtr, text, sdlColor);

				// Create texture from surface
				IntPtr texture = SDL_CreateTextureFromSurface(renderer.SdlRendererPtr, surface);

				// Free the surface
				SDL_FreeSurface(surface);

				// Return the texture
				return new SDL.Texture(texture);
			}

			public SDL.Texture RenderText(SDL.Renderer renderer, string text, Color color, uint wrapLength, bool preferSpeedOverQuality)
			{
				if (preferSpeedOverQuality)
					TTF_SetFontHinting(ttfFontPtr, TTF_HINTING_NORMAL);
				else
					TTF_SetFontHinting(ttfFontPtr, TTF_HINTING_LIGHT_SUBPIXEL);

				// Render text to surface
				var sdlColor = Util.CreateSDLColor(color);
				IntPtr surface = preferSpeedOverQuality ? TTF_RenderText_Solid_Wrapped(ttfFontPtr, text, sdlColor, wrapLength) : TTF_RenderText_Blended_Wrapped(ttfFontPtr, text, sdlColor, wrapLength);

				// Create texture from surface
				IntPtr texture = SDL_CreateTextureFromSurface(renderer.SdlRendererPtr, surface);

				// Free the surface
				SDL_FreeSurface(surface);

				// Return the texture
				return new SDL.Texture(texture);
			}

			public void Dispose()
			{
				TTF_CloseFont(ttfFontPtr);
			}
		}
	}
}
