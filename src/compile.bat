REM cls
csc /nologo /target:library /out:crow.dll crow.cs
csc /nologo /reference:crow.dll,Chronos.dll /out:test.exe test.cs
copy crow.dll ..\bin\crow.dll