open System.IO
open System.Collections.Generic

let Y = 2000000
let SearchSpace = 4000000
let manhattanDist (x1, y1) (x2, y2) = abs (x1 - x2) + abs (y1 - y2)

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let sensorBeaconPairs =
        (File.ReadAllLines filename)
        |> Seq.map (fun x ->
            let split = x.Split()

            ((int (split[2][2 .. split[2].Length - 2]),
              int (split[3][2 .. split[3].Length - 2])),
             (int (split[8][2 .. split[8].Length - 2]),
              int (split[9][2 .. split[9].Length - 1]))))

    let beaconsOnY =
        HashSet<int>(
            sensorBeaconPairs
            |> Seq.filter (fun (_, (_, y)) -> y = Y)
            |> Seq.map (fun (_, (x, _)) -> x)
        )

    let nonBeaconPositionsOnY = new HashSet<int>()

    sensorBeaconPairs
    |> Seq.iter (fun (sensor, beacon) ->
        let pairDist = manhattanDist sensor beacon
        let yDist = manhattanDist sensor (fst sensor, Y)

        for i in 0 .. pairDist - yDist do
            nonBeaconPositionsOnY.Add(fst sensor + i) |> ignore
            nonBeaconPositionsOnY.Add(fst sensor - i) |> ignore)

    nonBeaconPositionsOnY.ExceptWith(beaconsOnY)
    printfn $"Part 1: {nonBeaconPositionsOnY.Count}"
    printfn $"Part 2: "
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
