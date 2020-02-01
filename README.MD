# CROW
CROW is a file format usable in C# inspired by [Deco](https://github.com/Enhex/Deco) for faster data entry and retrieval than XML or JSON.

## Usage
To invoke crow files in code initialize a new *crow* variable like so:
```
using(crow var = new crow("[filename].crow")) {
   [code goes here]
}
```

The following code would be used to 
Find:
```
    var.Find("foo.bar")
```

Update
```
    var.Update("foo.bar", "NewVar")
```

## Compiling
Simply compile `crow.cs` as a library or execute the following command:
>csc /target:library /out:crow.dll crow.cs

## License
CROW is under the Zero Clause BSD license.