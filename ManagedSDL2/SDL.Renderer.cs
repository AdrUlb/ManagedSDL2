using System;
using System.Drawing;
using static Native.SDL;
using static Native.SDL.SDL_RendererFlags;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public class Renderer : IDisposable
		{
			internal readonly IntPtr SdlRendererPtr;

			public (float X, float Y) Scale
			{
				get
				{
					SDL_RenderGetScale(SdlRendererPtr, out var scaleX, out var scaleY);
					return (scaleX, scaleY);
				}

				set => _ = SDL_RenderSetScale(SdlRendererPtr, value.X, value.Y);
			}
			public Renderer(Window window, int deviceIndex = -1, bool accelerated = true)
			{
				SdlRendererPtr = SDL_CreateRenderer(window.SdlWindowPtr, deviceIndex, accelerated ? SDL_RENDERER_ACCELERATED : SDL_RENDERER_SOFTWARE);
			}

			private void SetColor(Color color) => _ = SDL_SetRenderDrawColor(SdlRendererPtr, color.R, color.G, color.B, color.A);

			public void SetDrawBlendMode(BlendMode mode) => _ = SDL_SetRenderDrawBlendMode(SdlRendererPtr, (SDL_BlendMode)mode);

			public void DrawPoint(Color color, int x, int y)
			{
				SetColor(color);
				_ = SDL_RenderDrawPoint(SdlRendererPtr, x, y);
			}

			public void Copy(Texture texture, Rectangle? srcRect = null, Rectangle? dstRect = null)
			{
				var sdlSrcRect = new SDL_Rect();
				var sdlDstRect = new SDL_Rect();

				if (srcRect != null)
					sdlSrcRect = Util.CreateSDLRect(srcRect.Value);
				if (dstRect != null)
					sdlDstRect = Util.CreateSDLRect(dstRect.Value);

				if (srcRect != null && dstRect != null)
					_ = SDL_RenderCopy(SdlRendererPtr, texture.SdlTexturePtr, ref sdlSrcRect, ref sdlDstRect);
				else if (srcRect != null)
					_ = SDL_RenderCopy(SdlRendererPtr, texture.SdlTexturePtr, ref sdlSrcRect, IntPtr.Zero);
				else if (dstRect != null)
					_ = SDL_RenderCopy(SdlRendererPtr, texture.SdlTexturePtr, IntPtr.Zero, ref sdlDstRect);
				else
					_ = SDL_RenderCopy(SdlRendererPtr, texture.SdlTexturePtr, IntPtr.Zero, IntPtr.Zero);
			}

			public void FillRect(Color color, Rectangle rect)
			{
				SetColor(color);

				var sdlRect = Util.CreateSDLRect(rect);

				_ = SDL_RenderFillRect(SdlRendererPtr, ref sdlRect);
			}

			public void Clear(Color color)
			{
				SetColor(color);
				_ = SDL_RenderClear(SdlRendererPtr);
			}

			public void Clear() => Clear(Color.Black);

			public void Present() => SDL_RenderPresent(SdlRendererPtr);

			bool disposed = false;

			public void Dispose()
			{
				if (disposed)
					return;

				GC.SuppressFinalize(this);
				SDL_DestroyRenderer(SdlRendererPtr);

				disposed = true;
			}
		}
	}
}
