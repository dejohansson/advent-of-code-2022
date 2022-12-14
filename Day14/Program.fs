open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let paths =
        (File.ReadAllLines filename)
        |> Seq.map (fun x ->
            x.Split(" -> ")
            |> Seq.map (fun x ->
                let split = x.Split(",")
                (int split[0], int split[1])))

    let grid1 = Array2D.create 600 600 '.'
    let grid2 = Array2D.create 800 600 '.'
    let mutable floorY = 2

    let printSample (grid: char[,]) =
        if filename.Contains("sample") then
            for y in 0..13 do
                for x in 480..520 do
                    printf "%c" grid[x, y]

                printfn ""

            printfn ""

    paths
    |> Seq.iter (fun x ->
        x
        |> Seq.pairwise
        |> Seq.iter (fun (((x1: int), (y1: int)), (x2, y2)) ->
            let minX, maxX, minY, maxY = min x1 x2, max x1 x2, min y1 y2, max y1 y2

            if floorY < maxY + 2 then
                floorY <- maxY + 2

            grid1[minX..maxX, minY..maxY] <-
                Array2D.create (1 + maxX - minX) (1 + maxY - minY) '#'

            grid2[minX..maxX, minY..maxY] <-
                Array2D.create (1 + maxX - minX) (1 + maxY - minY) '#'))

    let mutable sandCount1 = 0
    let mutable fellIntoAbyss = false

    while not fellIntoAbyss do
        let mutable atRest = false
        let mutable x, y = 500, 0

        while not fellIntoAbyss && not atRest do
            if y >= grid1.GetLength(1) - 1 then
                fellIntoAbyss <- true
            else if grid1[x, y + 1] = '.' then
                y <- y + 1
            else if grid1[x - 1, y + 1] = '.' then
                y <- y + 1
                x <- x - 1
            else if grid1[x + 1, y + 1] = '.' then
                y <- y + 1
                x <- x + 1
            else
                grid1[x, y] <- 'o'
                atRest <- true

        if not fellIntoAbyss then
            sandCount1 <- sandCount1 + 1

        if sandCount1 % 5 = 0 || sandCount1 = 24 then
            printSample grid1

    let mutable sourceBlocked = false
    let mutable sandCount2 = 0
    grid2[*, floorY] <- Array.create (grid2.GetLength(0)) '#'

    while not sourceBlocked do
        let mutable atRest = false
        let mutable x, y = 500, 0

        while not sourceBlocked && not atRest do
            if grid2[x, y + 1] = '.' then
                y <- y + 1
            else if grid2[x - 1, y + 1] = '.' then
                y <- y + 1
                x <- x - 1
            else if grid2[x + 1, y + 1] = '.' then
                y <- y + 1
                x <- x + 1
            else if x = 500 && y = 0 then
                grid2[x, y] <- 'o'
                sourceBlocked <- true
            else
                grid2[x, y] <- 'o'
                atRest <- true

        sandCount2 <- sandCount2 + 1

        if sandCount2 % 5 = 0 || sandCount2 = 93 then
            printSample grid2

    printfn $"Part 1: {sandCount1}"
    printfn $"Part 2: {sandCount2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
