# Building the Godot Editor with Large World Coordinates (double precision vectors)
Gnome Space Program uses Godot Large World Coordinates (double precision vectors), which require building the editor from source.
It will not build on the standard Godot binaries, which are built with single precision vectors.
Don't worry, it's not that complicated! Just follow the instructions.

## Relevant Godot documentaion (the authoritative source):
These instructions are simplified from the Godot engine documentation, linked here:
https://docs.godotengine.org/en/stable/engine_details/development/compiling/index.html
https://docs.godotengine.org/en/stable/engine_details/development/compiling/compiling_with_dotnet.html
https://docs.godotengine.org/en/stable/tutorials/physics/large_world_coordinates.html

## Linux x86-64
### Compiling Godot
See [Godot's documentation](https://docs.godotengine.org/en/stable/engine_details/development/compiling/compiling_for_linuxbsd.html) for more details on build options.

1) Install dotnet 8.0 or higher. See https://dotnet.microsoft.com/en-us/download.

2) Install dependencies:
Go to [Godot's documentation](https://docs.godotengine.org/en/stable/engine_details/development/compiling/compiling_for_linuxbsd.html#distro-specific-one-liners) and choose the appropriate commands for your distro.

3) Clone source for Godot 4.5.1 (or whatever version is listed above):
`git clone https://github.com/godotengine/godot.git -b 4.5.1-stable --depth 1`

4) Navigate to the root of the source code. Replace the platform value with your platform, or omit to target the host platform.
`scons platform=linuxbsd precision=double module_mono_enabled=yes production=yes`
On pressing return, this starts building Godot immediately. It may take a while.

5) Build export templates. These may take a while as well.
`scons platform=linuxbsd target=template_debug precision=double module_mono_enabled=yes`
`scons platform=linuxbsd target=template_release precision=double module_mono_enabled=yes`

### Dotnet Setup
1) Generate glue - Stay at the root of the source code
`<godot_binary> --headless --generate-mono-glue modules/mono/glue`

In my case, that's:
`./bin/godot.linuxbsd.editor.double.x86_64.mono --headless --generate-mono-glue modules/mono/glue`

2) Create an empty directory for a local NuGet repository.
(for example, Godot recommends ~/MyLocalNugetSource) - replace <my_local_source> with this path.
`mkdir <my_local_source>`

3) Register this directory with dotnet:
`dotnet nuget add source <my_local_source> --name MyLocalNugetSource`

4) Build managed libraries, exporting their NuGet packages
`./modules/mono/build_scripts/build_assemblies.py --godot-output-dir ./bin --push-nupkgs-local <my_local_source> --precision=double`

## Windows x86-64
Refer to [Godot's documentation](https://docs.godotengine.org/en/stable/engine_details/development/compiling/compiling_for_windows.html) for Windows build instructions.