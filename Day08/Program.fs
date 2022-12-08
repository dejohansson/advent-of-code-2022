open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let treeHeights =
        (File.ReadAllLines filename)
        |> Array.map (fun l ->
            l |> Seq.map System.Globalization.CharUnicodeInfo.GetDigitValue)
        |> array2D

    let gridSize = Array2D.length1 treeHeights
    let visibleTrees = Array2D.zeroCreate<int> gridSize gridSize

    let setVisibleTrees seq1 seq2 =
        for x in seq1 do
            let mutable horizontalHeight = -1
            let mutable verticalHeight = -1

            for y in seq2 do
                if treeHeights[x, y] > horizontalHeight then
                    Array2D.set visibleTrees x y 1
                    horizontalHeight <- treeHeights[x, y]

                if treeHeights[y, x] > verticalHeight then
                    Array2D.set visibleTrees y x 1
                    verticalHeight <- treeHeights[y, x]

    let forwards = seq { 0 .. gridSize - 1 }
    setVisibleTrees forwards forwards
    setVisibleTrees forwards (Seq.rev forwards)
    let mutable part1 = 0
    Array2D.iteri (fun x y _ -> part1 <- part1 + visibleTrees[x, y]) visibleTrees

    let rec look height dx dy x y =
        let nextX, nextY = x + dx, y + dy

        if nextX < 0 || nextX >= gridSize || nextY < 0 || nextY >= gridSize then
            0
        else if treeHeights[nextX, nextY] >= height then
            1
        else
            1 + look height dx dy nextX nextY

    let scenicScore x y =
        look treeHeights[x, y] 1 0 x y
        * look treeHeights[x, y] -1 0 x y
        * look treeHeights[x, y] 0 1 x y
        * look treeHeights[x, y] 0 -1 x y

    let mutable part2 = 0

    Array2D.iteri
        (fun x y _ ->
            let s = scenicScore x y

            if s > part2 then
                part2 <- s)
        treeHeights

    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
