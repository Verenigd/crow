# Documentation
## File format
The file works as such:

    - Entry keys and values are seprated by a ':'
    - Entries are separated by a new lines
    - Containers start and end with ':'
    - Null entries cannot exists

The documentation will assume the file being invoked looks like this.

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
This function will return the entry value associated with the path requested.

```
using(crow foo = new crow("filename.crow")) {
    string SearchVal = foo.Find("Geography.LargestPeak");
    //Output: 'Alps'

    string SearchVal = foo.Find("Geography.Climate.Quaternary");
    //Throws unhandled exception 'Path not found'

    string SearchVal = foo.Find("Geography.Climate");
    //Throws unhandled exception 'Path is a container'
}
```

### Update entry
```

```

### Add and insert entry
Entries can be inserted at the begining of the list or container or added at the end of the list or container using the following functions. By default the `EntryAdd()` function will insert, not append.

```
using(crow foo = new crow("filename.crow")) {
    foo.EntryAdd("Language", "German");
    //Inserts new entry with key "Language" and value "German" at the first position available.

    foo.EntryAdd("Geography.Climate.Winter", "Harsh", 1);
    //Appends new entry with key "Winter" and value "Harsh" at the end of the Climate container.
}
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