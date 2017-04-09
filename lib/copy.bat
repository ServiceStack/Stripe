REM SET BUILD=Debug
SET BUILD=Release

COPY "..\..\ServiceStack\src\ServiceStack.Interfaces\bin\%BUILD%\portable40-net45+sl5+win8+wp8+wpa81\ServiceStack.Interfaces.*" pcl

COPY ..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\net45\ServiceStack.Client.* net45
COPY ..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\netstandard1.1\ServiceStack.Client.* netstandard1.1
COPY ..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\netstandard1.6\ServiceStack.Client.* netstandard1.6
COPY "..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\portable45-net45+win8\ServiceStack.Client.*" pcl

COPY ..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\net45\ServiceStack.Text.* net45
COPY ..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\netstandard1.1\ServiceStack.Text.* netstandard1.1
COPY ..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\netstandard1.3\ServiceStack.Text.* netstandard1.3
COPY "..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\portable45-net45+win8\ServiceStack.Text.*" pcl

