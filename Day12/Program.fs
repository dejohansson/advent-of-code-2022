open System.IO
open System.Collections.Generic
open System

let find2dPoint (value: 'a) (array: 'a[,]) =
    let rec go x y =
        if x >= array.GetLength 0 then go 0 (y + 1)
        elif array.[x, y] = value then x, y
        else go (x + 1) y

    go 0 0

let findAll2dPoints (value: 'a) (array: 'a[,]) =
    let rec go x y =
        if y >= array.GetLength 1 then
            [||]
        else if x >= array.GetLength 0 then
            go 0 (y + 1)
        else
            Array.append
                (go (x + 1) y)
                (if array.[x, y] = value then [| (x, y) |] else [||])

    go 0 0

let directions = [| (1, 0); (-1, 0); (0, 1); (0, -1) |]

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let map = array2D (File.ReadAllLines filename)
    let start = find2dPoint 'S' map
    let x, y = find2dPoint 'E' map
    let height, width = (Array2D.length1 map), (Array2D.length2 map)
    let dists = new Dictionary<string, int>()

    let valid (visited: bool[,]) aPrev bPrev a b =
        a >= 0
        && a < height
        && b >= 0
        && b < width
        && not visited[a, b]
        && (map[aPrev, bPrev] = 'S'
            || int (if map[a, b] = 'E' then 'z' else map[a, b]) - int map[aPrev, bPrev]
               <= 1)

    let rec getDist i j currentDist (visited: bool[,]) =
        visited[i, j] <- true
        let exists, value = dists.TryGetValue($"{i},{j}-{currentDist}")

        let d =
            if exists then
                value
            else if i = x && j = y then
                currentDist
            else
                let mutable shortestDist = Int32.MaxValue

                for (dI, dJ) in directions do
                    let nextI, nextJ = i + dI, j + dJ

                    if valid visited i j nextI nextJ then
                        let nextDist = getDist nextI nextJ (currentDist + 1) visited

                        if nextDist < shortestDist then
                            shortestDist <- nextDist

                dists.Add($"{i},{j}-{currentDist}", shortestDist)
                shortestDist

        visited[i, j] <- false
        d

    let mutable minDist =
        getDist (fst start) (snd start) 0 (Array2D.create height width false)

    printfn $"Part 1: {minDist}"

    for potentialStart in findAll2dPoints 'a' map do
        let dist =
            getDist
                (fst potentialStart)
                (snd potentialStart)
                0
                (Array2D.create height width false)

        if dist < minDist then
            minDist <- dist

    printfn $"Part 2: {minDist}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
