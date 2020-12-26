﻿using System;
using System.Drawing;
using static Native.SDL;
using static Native.SDL.SDL_RendererFlags;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public class Renderer : IDisposable
		{
			readonly IntPtr sdlRendererPtr;

			public (float X, float Y) Scale
			{
				get
				{
					SDL_RenderGetScale(sdlRendererPtr, out var scaleX, out var scaleY);
					return (scaleX, scaleY);
				}

				set => _ = SDL_RenderSetScale(sdlRendererPtr, value.X, value.Y);
			}
			public Renderer(Window window, int deviceIndex = -1, bool accelerated = true)
			{
				sdlRendererPtr = SDL_CreateRenderer(window.SdlWindowPtr, deviceIndex, accelerated ? SDL_RENDERER_ACCELERATED : SDL_RENDERER_SOFTWARE);
			}

			private void SetColor(Color color) => _ = SDL_SetRenderDrawColor(sdlRendererPtr, color.R, color.G, color.B, color.A);

			public void SetDrawBlendMode(BlendMode mode) => _ = SDL_SetRenderDrawBlendMode(sdlRendererPtr, (SDL_BlendMode)mode);

			public void DrawPoint(Color color, int x, int y)
			{
				SetColor(color);
				_ = SDL_RenderDrawPoint(sdlRendererPtr, x, y);
			}

			public void Clear(Color color)
			{
				SetColor(color);
				_ = SDL_RenderClear(sdlRendererPtr);
			}

			public void Clear() => Clear(Color.Black);

			public void Present() => SDL_RenderPresent(sdlRendererPtr);

			bool disposed = false;

			public void Dispose()
			{
				if (disposed)
					return;

				GC.SuppressFinalize(this);
				SDL_DestroyRenderer(sdlRendererPtr);

				disposed = true;
			}
		}
	}
}
