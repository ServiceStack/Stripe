SET MSBUILD="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"

%MSBUILD% build.proj /target:TeamCityBuild;NuGetPack /property:Configuration=Release;RELEASE=true;PatchVersion=0;PatchCoreVersion=0
