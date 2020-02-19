# Documentation
## File format
The file works as such:

    - Entry keys and values are seprated by a ':'
    - Entries are separated by a new lines
    - Containers start and end with ':'
    - Null entries cannot exists

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
    //Throws unhandled exception 'Path not found'

    string SearchVal = Find("Geography.Climate");
    //Throws unhandled exception 'Path is a container'
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
This function will delete a container and its content whether or not it is empty.

```

```

## Indentation
Indentations are enabled by default for human readability with each indent being a quadruple space. It is possible to disable them and they will not be written when adding new entries.

```
using(crow foo = new crow("filename.crow")) {
    //Remove indentations
    foo.IndentationType = false;

    //Set indentations again and to double space instead of default
    foo.IndentationType = true;
    foo.IndentationString = "  ";
}
```