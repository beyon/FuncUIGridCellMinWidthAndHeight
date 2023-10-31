# Demo project for Avalonia.FuncUI minWidth/minHeight of cells in Grid

## Problems
- Setting minWidth and/or minHeight on the "cells" directely of a Grid doesn't work in Avalonia, at least not currently (see: https://github.com/AvaloniaUI/Avalonia/issues/7637).
- You can't specify minHeight and minHeight when specifying rowDefinition/columnDefinition in string form, instead you have to use ColumnDefinitions and RowDefinitions objects instead.
- Using the ColumnDefinitions, RowDefinitions, ColumnDefinition, RowDefinition objects in FuncUI results in verbose code that doesn't fit so nicely with the FuncUI DSL


    ```fsharp
    //Column definitions
    let col1 = ColumnDefinition()
    let col2 = ColumnDefinition()
    let col3 = ColumnDefinition()
    col1.Width <- GridLength(1.0, GridUnitType.Star)
    col1.MinWidth <- 64.0
    col2.Width <- GridLength(4.0, GridUnitType.Pixel)
    col2.MinWidth <- 4.0
    col3.Width <- GridLength(3.0, GridUnitType.Star)
    col3.MinWidth <- 64.0
    let colDefs = ColumnDefinitions()
    colDefs.Add(col1)
    colDefs.Add(col2)
    colDefs.Add(col3)
    //Row definitions
    let row1 = RowDefinition()
    let row2 = RowDefinition()
    let row3 = RowDefinition()
    row1.Height <- GridLength(1.0, GridUnitType.Star)
    row1.MinHeight <- 64.0
    row2.Height <- GridLength(4.0, GridUnitType.Pixel)
    row2.MinHeight <- 4.0
    row3.Height <- GridLength(1.5, GridUnitType.Star)
    row3.MinHeight <- 64.0
    let rowDefs = RowDefinitions()
    rowDefs.Add(row1)
    rowDefs.Add(row2)
    rowDefs.Add(row3)
    Grid.create [
        Grid.columnDefinitions colDefs
        Grid.rowDefinitions rowDefs
        Grid.children [
                ...
    ```
## Proposal
Extend the FuncUI DSL with support for ColumnDefinitions and RowDefinitions, this repository offers one possible implementation of such DSL extensions.

Code using DSL with Grid.columnDefinition.create and Grid.rowDefinitons.create:
```fsharp
Grid.create [
    Grid.columnDefinitions [
        ColumnDefinition.create(GridLength(1.0, GridUnitType.Star), minWidth = 64.)
        ColumnDefinition.create(GridLength(4.0, GridUnitType.Pixel), minWidth = 4.)
        ColumnDefinition.create(GridLength(3.0, GridUnitType.Star), minWidth = 64.)
    ]
    Grid.rowDefinitions [
        ColumnDefinition.create(GridLength(1.0, GridUnitType.Star), minHeight = 64.)
        ColumnDefinition.create(GridLength(4.0, GridUnitType.Pixel), minHeight = 4.)
        ColumnDefinition.create(GridLength(1.5, GridUnitType.Star), minHeight = 64.)
    ]
    Grid.children [
        ...

```

Code using DSL with shorthand forms for the different types of rows/columns:
```fsharp
Grid.create [
    Grid.columnDefinitions [
        ColumnDefinition.createStar(1.0, minWidth=64.0)
        ColumnDefinition.createPixel(4.0, minWidth=4.0)
        ColumnDefinition.createStar(3.0, minWidth=64.0)
    ]
    Grid.rowDefinitions [
        RowDefinition.createStar(1.0, minHeight=64.0)
        RowDefinition.createPixel(4.0, minHeight=4.0)
        RowDefinition.createStar(1.5, minHeight=64.0)
    ]
    Grid.children [
        ...

```

Another example of string syntax vs proposed DSL.

String syntax:
```fsharp
Grid.columnDefinitions "200,2*,*"
Grid.rowDefinitions "400,Auto"
```

DSL:
```fsharp
Grid.columnDefinitions [
    ColumnDefinition.create(GridLength(200., GridUnitType.Pixel))
    ColumnDefinition.create(GridLength(2., GridUnitType.Star))
    ColumnDefinition.create(GridLength(1., GridUnitType.Star))
]
Grid.rowDefinitions [
    ColumnDefinition.create(GridLength(400., GridUnitType.Pixel))
    ColumnDefinition.create(GridLength(0., GridUnitType.Auto))
]
```
Shorthand DSL:
```fsharp
Grid.create [
    Grid.columnDefinitions [
        ColumnDefinition.createPixel(200.)
        ColumnDefinition.createStar(2.)
        ColumnDefinition.createStar() //same as .createStar(1.0) or .create()
    ]
    Grid.rowDefinitions [
        RowDefinition.createPixel(400.)
        RowDefinition.createAuto()
    ]
```

## Questions
- Is the .create(GridLength()) form needed? Shorthand DSL provides same functionality and at least to me much easier to read and write.
- An alternative and/or addition could be a .create(gridlength: CellSize) where CellSize could be:
    ```fsharp
    type CellSize =
    | Auto
    | Pixel of float
    | Star of float
    ```
    or
    ```fsharp
    type CellSize =
    | Auto
    | Pixel of float
    | Proportional of float
    ```
    ```fsharp
    RowDefinition.create(CellSize.Auto)
    RowDefinition.create(CellSize.Pixel 200.)
    RowDefinition.create(CellSize.Star 1.5)
    ```