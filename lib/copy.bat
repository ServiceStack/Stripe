REM SET BUILD=Debug
SET BUILD=Release

COPY ..\..\ServiceStack\src\ServiceStack.Interfaces\bin\%BUILD%\net45\ServiceStack.Interfaces.* net45
COPY ..\..\ServiceStack\src\ServiceStack.Interfaces\bin\%BUILD%\netstandard2.0\ServiceStack.Interfaces.* netstandard2.0

COPY ..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\net45\ServiceStack.Text.* net45
COPY ..\..\ServiceStack.Text\src\ServiceStack.Text\bin\%BUILD%\netstandard2.0\ServiceStack.Text.* netstandard2.0

COPY ..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\net45\ServiceStack.Client.* net45
COPY ..\..\ServiceStack\src\ServiceStack.Client\bin\%BUILD%\netstandard2.0\ServiceStack.Client.* netstandard2.0
