cls
csc /nologo /target:library /out:crow.dll crow.cs
csc /nologo /reference:crow.dll /out:test.exe test.cs