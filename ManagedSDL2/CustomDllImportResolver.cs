using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedSDL2
{
	static class CustomDllImportResolver
	{
		static Assembly executingAssembly => Assembly.GetExecutingAssembly();

		static IntPtr LoadNativeLibraryFromResource(string name)
		{
			var resourceNames = executingAssembly.GetManifestResourceNames();
			var resourceName = resourceNames.FirstOrDefault(str => str.EndsWith("." + name));

			if (resourceName == null)
				return IntPtr.Zero;

			using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

			if (s == null || s == Stream.Null)
				return IntPtr.Zero;

			var tempFile = Path.GetTempFileName();

			using var fs = File.OpenWrite(tempFile);

			s.CopyTo(fs);

			if (NativeLibrary.TryLoad(tempFile, out var handle))
				return handle;
			else
				return IntPtr.Zero;
		}

		static IntPtr LoadNativeLibraryFromFile(string path)
		{
			if (NativeLibrary.TryLoad(path, out IntPtr handle))
				return handle;

			return IntPtr.Zero;
		}

		static IntPtr LoadNativeLibrary(string libName)
		{
			string platformLibName = libName;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				platformLibName = $"Native_Windows_{RuntimeInformation.ProcessArchitecture}_{libName}.dll";
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				platformLibName = $"Native_Linux_{RuntimeInformation.ProcessArchitecture}_lib{libName}.so";
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				platformLibName = $"Native_OSX_{RuntimeInformation.ProcessArchitecture}_lib{libName}.dylib";

			var handle = LoadNativeLibraryFromResource(platformLibName);

			if (handle != IntPtr.Zero)
				return handle;

			handle = LoadNativeLibraryFromFile(platformLibName);

			if (handle != IntPtr.Zero)
				return handle;

			return LoadNativeLibrary(libName);
		}

		static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
		{
			return LoadNativeLibrary(libraryName);
		}

		public static void Enable() => NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
	}
}
