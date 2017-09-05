SET MSBUILD="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"

REM %MSBUILD% build-core.proj /target:TeamCityBuild;NuGetPack /property:Configuration=Release;PatchVersion=41
REM %MSBUILD% build.proj /target:TeamCityBuild;NuGetPack /property:Configuration=Release;PatchVersion=9

msbuild /p:Configuration=Release ..\src\Stripe.sln
