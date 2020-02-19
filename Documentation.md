# Documentation
## File format
The file works as such:

    - Entry keys and values are seprated by a `:`
    - Entries are separated by a new lines

```
ISO: AT
Capital: Wien
Geography:
    LargestPeak: Alps
    Climate:
        Primary: Dfb
        Secondary: Dfc
        Tertiary: ET
    :
    Continent: Europe
    Landlocked: true
:
```

## Usage
A new object must be declared to interact with a crow file.

```
using(crow foo = new crow("filename.crow")) {
    _code goes here_
}
```

### Find
This function will return the value associated with the 

```
using(crow foo = new crow("filename.crow")) {
    string SearchVal = Find("Geography.LargestPeak");
    //Output: 'Alps'

    string SearchVal = Find("Geography.Climate.Quaternary");
    //Output: ':ERR_NOT_FOUND:'

    string SearchVal = Find("Geography.Climate");
    //Output: ':ERR_CONTAINER:'
}
```

### Update entry
```

```

### Add and insert entry
```

```

### Delete entry
```

```

### Add container
```

```

### Delete container
```

```

## Indentation
Indentations are enabled by default with each indent being a quadruple space.

```
using(crow foo = new crow("filename.crow")) {
    //Remove indentations
    foo.IndentationType = false;

    //Set indentations again and to double space instead of default
    foo.IndentationType = true;
    foo.IndentationString = "  ";
}
```