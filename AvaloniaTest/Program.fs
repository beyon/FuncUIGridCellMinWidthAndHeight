namespace CounterApp

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout

[<AutoOpen>]
module ColumnDefinition =  
    open Avalonia.Controls
    
    module private Internals =
        let make (width: GridLength option) (minWidth: float option) (maxWidth: float option) =
            let columnDefinition = ColumnDefinition()
            match width with
            | Some w -> columnDefinition.Width <- w
            | None -> columnDefinition.Width <- GridLength(0.0, GridUnitType.Auto)
            minWidth |> Option.iter (fun minW -> columnDefinition.MinWidth <- minW)
            maxWidth |> Option.iter (fun maxW -> columnDefinition.MaxWidth <- maxW)
            columnDefinition

    type ColumnDefinition with
        static member create(?width: GridLength, ?minWidth: float, ?maxWidth: float) =
            Internals.make width minWidth maxWidth
        
        /// <summary>
        /// Creates a ColumnDefinition with column width in pixels
        /// </summary>
        /// <param name="width">Desired column width in pixels</param>
        /// <param name="minWidth">Optional minimum column width in pixels</param>
        /// <param name="maxWidth">Optional maximum column width in pixels</param>
        static member createPixel(width: float, ?minWidth: float, ?maxWidth: float) =
            Internals.make (Some(GridLength(width, GridUnitType.Pixel))) minWidth maxWidth

        /// <summary>
        /// Creates a ColumnDefinition with proportional width
        /// </summary>
        /// <param name="value">Factor used for dividing the width among proportional columns</param>
        /// <param name="minWidth">Optional minimum column width in pixels</param>
        /// <param name="maxWidth">Optional maximum column width in pixels</param>
        static member createStar(?value: float, ?minWidth: float, ?maxWidth: float) =
            Internals.make (Some(GridLength(defaultArg value 1.0, GridUnitType.Star))) minWidth maxWidth

        /// <summary>
        /// Creates a ColumnDefinition with automatic column width
        /// </summary>
        /// <param name="minWidth">Optional minimum column width in pixels</param>
        /// <param name="maxWidth">Optional maximum column width in pixels</param>
        static member createAuto(?minWidth: float, ?maxWidth: float) =
            Internals.make None minWidth maxWidth
            

[<AutoOpen>]
module RowDefinition =  
    open Avalonia.Controls
    
    module private Internals =
        let make (height: GridLength option) (minHeight: float option) (maxHeight: float option) =
            let rowDefinition = RowDefinition()
            match height with
            | Some h -> rowDefinition.Height <- h
            | None -> rowDefinition.Height <- GridLength(0.0, GridUnitType.Auto)
            minHeight |> Option.iter (fun minH -> rowDefinition.MinHeight <- minH)
            maxHeight |> Option.iter (fun maxH -> rowDefinition.MaxHeight <- maxH)
            rowDefinition

    type RowDefinition with
        static member create(?height: GridLength, ?minHeight: float, ?maxHeight: float) =
            Internals.make height minHeight maxHeight

        /// <summary>
        /// Creates a RowDefintion with row height in pixels
        /// </summary>
        /// <param name="height">Desired row height in pixels</param>
        /// <param name="minHeight">Optional minimum row height in pixels</param>
        /// <param name="maxHeight">Optional maximum row height in pixels</param>
        static member createPixel(height: float, ?minHeight: float, ?maxHeight: float) =
            Internals.make (Some(GridLength(height, GridUnitType.Pixel))) minHeight maxHeight

        /// <summary>
        /// Creates a RowDefintion with proportional height
        /// </summary>
        /// <param name="value">Factor used for dividing the height among proportional rows</param>
        /// <param name="minHeight">Optional minimum row height in pixels</param>
        /// <param name="maxHeight">Optional maximum row height in pixels</param>
        static member createStar(?value: float, ?minHeight: float, ?maxHeight: float) =
            Internals.make (Some(GridLength(defaultArg value 1.0, GridUnitType.Star))) minHeight maxHeight

        /// <summary>
        /// Creates a RowDefintion with automatic row height
        /// </summary>
        /// <param name="minHeight">Optional minimum row height in pixels</param>
        /// <param name="maxHeight">Optional maximum row height in pixels</param>
        static member createAuto(?minHeight: float, ?maxHeight: float) =
            Internals.make None minHeight maxHeight

[<AutoOpen>]
module Grid =
    type Grid with
        /// <summary>
        /// Add a list of column definitions to the grid
        /// </summary>
        /// <param name="columns">List of ColumnDefinition defining how the column widths should be divided</param>
        static member columnDefinitions (columns: ColumnDefinition list) =
            let colDefs = ColumnDefinitions()
            columns
            |> List.iter (fun column -> colDefs.Add(column))
            Grid.columnDefinitions colDefs

        /// <summary>
        /// Add a list of row definitions to the grid
        /// </summary>
        /// <param name="rows">List of RowDefinition defining how the row heights should be divided</param>
        static member rowDefinitions (rows: RowDefinition list) =
            let rowDefs = RowDefinitions()
            rows
            |> List.iter (fun row -> rowDefs.Add(row))
            Grid.rowDefinitions rowDefs

module Main =

    let view () =
        Component(fun ctx ->
            let gridText text = 
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text text
                ]
            let tabGridCellsWithMinWidthAndHeightNotWorking :IView =
                    Grid.create [
                        Grid.columnDefinitions "1*,4,3*"
                        Grid.rowDefinitions "1*,4,1.5*"
                        Grid.children [
                            Border.create [
                                Grid.column 0
                                Grid.row 0
                                Border.minWidth 64.0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "A")
                            ]
                            Border.create [
                                Grid.column 2
                                Grid.row 0
                                Border.minWidth 64.0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "B")
                            ]
                            Border.create [
                                Grid.column 0
                                Grid.row 2
                                Border.minWidth 64.0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "C")
                            ]
                            Border.create [
                                Grid.column 2
                                Grid.row 2
                                Border.minWidth 64.0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "D")
                            ]
                            GridSplitter.create [
                                Grid.column 1
                                Grid.rowSpan 3
                                GridSplitter.horizontalAlignment HorizontalAlignment.Center
                                GridSplitter.verticalAlignment VerticalAlignment.Stretch
                            ]
                            GridSplitter.create [
                                Grid.row 1
                                Grid.columnSpan 3
                                GridSplitter.horizontalAlignment HorizontalAlignment.Stretch
                                GridSplitter.verticalAlignment VerticalAlignment.Center
                            ]
                        ]
                    ]
            let tabGridCellsWithMinWidthAndHeightExperimentalDSL :IView =
                    Grid.create [
                        Grid.columnDefinitions [
                            ColumnDefinition.create(GridLength(1.0, GridUnitType.Star), minWidth = 64.)
                            ColumnDefinition.create(GridLength(4.0, GridUnitType.Pixel), minWidth = 4.)
                            ColumnDefinition.create(GridLength(3.0, GridUnitType.Star), minWidth = 64.)
                        ]
                        Grid.rowDefinitions [
                            RowDefinition.createStar(1.0, minHeight=64.0)
                            RowDefinition.createPixel(4.0, minHeight=4.0)
                            RowDefinition.createStar(1.5, minHeight=64.0)
                        ]
                        Grid.children [
                            Border.create [
                                Grid.column 0
                                Grid.row 0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "A")
                            ]
                            Border.create [
                                Grid.column 2
                                Grid.row 0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "B")
                            ]
                            Border.create [
                                Grid.column 0
                                Grid.row 2
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "C")
                            ]
                            Border.create [
                                Grid.column 2
                                Grid.row 2
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "D")
                            ]
                            GridSplitter.create [
                                Grid.column 1
                                Grid.rowSpan 3
                                GridSplitter.horizontalAlignment HorizontalAlignment.Center
                                GridSplitter.verticalAlignment VerticalAlignment.Stretch
                            ]
                            GridSplitter.create [
                                Grid.row 1
                                Grid.columnSpan 3
                                GridSplitter.horizontalAlignment HorizontalAlignment.Stretch
                                GridSplitter.verticalAlignment VerticalAlignment.Center
                            ]
                        ]
                    ]
            let tabGridCellsWithMinWidthAndHeightCurrent :IView =
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
                            Border.create [
                                Grid.column 0
                                Grid.row 0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "A")
                            ]
                            Border.create [
                                Grid.column 2
                                Grid.row 0
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "B")
                            ]
                            Border.create [
                                Grid.column 0
                                Grid.row 2
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "C")
                            ]
                            Border.create [
                                Grid.column 2
                                Grid.row 2
                                Border.background Media.Colors.AliceBlue
                                Border.borderThickness 2
                                Border.borderBrush (Media.Colors.Black.ToString())
                                Border.child (gridText "D")
                            ]
                            GridSplitter.create [
                                Grid.column 1
                                Grid.rowSpan 3
                                GridSplitter.horizontalAlignment HorizontalAlignment.Center
                                GridSplitter.verticalAlignment VerticalAlignment.Stretch
                            ]
                            GridSplitter.create [
                                Grid.row 1
                                Grid.columnSpan 3
                                GridSplitter.horizontalAlignment HorizontalAlignment.Stretch
                                GridSplitter.verticalAlignment VerticalAlignment.Center
                            ]
                        ]
                    ]
            TabControl.create [
                TabControl.tabStripPlacement Dock.Top
                TabControl.viewItems [
                    TabItem.create [
                        TabItem.header "GridCellsMinWidthAndHeightNotWorking"
                        TabItem.content tabGridCellsWithMinWidthAndHeightNotWorking
                    ]
                    TabItem.create [
                        TabItem.header "GridCellsMinWidthAndHeightWithExperimentalDSL"
                        TabItem.content tabGridCellsWithMinWidthAndHeightExperimentalDSL
                    ]
                    TabItem.create [
                        TabItem.header "GridCellsMinWidthAndHeightCurrent"
                        TabItem.content tabGridCellsWithMinWidthAndHeightCurrent
                    ]
                ]
            ]
        )

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Light

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
