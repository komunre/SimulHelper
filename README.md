# SimulHelper
SimulHelper is a library, that supposed to simplify your simulations creation. For examples see [unit tests](https://github.com/komunre/SimulHelper/blob/master/SimulationExample/UnitTest1.cs)

# How to use it?
Add submodule to solution folder and add link of SimulHelper to your project.

You need to use these usings a lot:
```cs
using SimulHelper;
using SimulHelper.Math;
using SkiaSharp;
```

# What is the difference?
When you develop some simulation, you may need some strong and big base. And so, this library serves it.

SimulHelper simplifies ticking procedure using `SimulationSystem` inherited classes. Also ticker simplifies creation, when one system depends on another. You always can call `Resolve` to find system and get/wrtie info from/to it.

With it, graph building being shortened into basically three lines `AddLine`, `TranslateLineH` and `Save` (If you use one line. If more - per line there will be one `AddLine` and probably one `TranslateLineH`).

# Current state
This library is abandoned, but may be revived soon.
