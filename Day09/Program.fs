open System.IO
open System.Collections.Generic

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let motions =
        File.ReadAllLines filename |> Seq.map (fun line -> (line[0], line[2..] |> int))

    let knots = new List<int * int>([| for i in 1..10 -> (0, 0) |])
    let knot2Positions = new HashSet<string>([| "0,0" |])
    let knot10Positions = new HashSet<string>([| "0,0" |])

    let rec move knot x y =
        knots[knot] <- (x, y)

        if knot < 9 then
            let dX = fst knots[knot] - fst knots[knot + 1]
            let dY = snd knots[knot] - snd knots[knot + 1]

            if dX > 1 && dY > 1 then
                move (knot + 1) (fst knots[knot + 1] + 1) (snd knots[knot + 1] + 1)
            else if dX > 1 && dY < -1 then
                move (knot + 1) (fst knots[knot + 1] + 1) (snd knots[knot + 1] - 1)
            else if dX < -1 && dY > 1 then
                move (knot + 1) (fst knots[knot + 1] - 1) (snd knots[knot + 1] + 1)
            else if dX < -1 && dY < -1 then
                move (knot + 1) (fst knots[knot + 1] - 1) (snd knots[knot + 1] - 1)
            else if dX > 1 then
                move (knot + 1) (fst knots[knot + 1] + 1) (snd knots[knot])
            else if dX < -1 then
                move (knot + 1) (fst knots[knot + 1] - 1) (snd knots[knot])
            else if dY > 1 then
                move (knot + 1) (fst knots[knot]) (snd knots[knot + 1] + 1)
            else if dY < -1 then
                move (knot + 1) (fst knots[knot]) (snd knots[knot + 1] - 1)

        if knot = 1 then
            knot2Positions.Add($"{fst knots[1]},{snd knots[1]}") |> ignore
        else if knot = 9 then
            knot10Positions.Add($"{fst knots[9]},{snd knots[9]}") |> ignore

    Seq.iter
        (fun motion ->
            for _ in seq { 1 .. snd motion } do
                match fst motion with
                | 'R' -> move 0 (fst knots[0] + 1) (snd knots[0])
                | 'L' -> move 0 (fst knots[0] - 1) (snd knots[0])
                | 'U' -> move 0 (fst knots[0]) (snd knots[0] + 1)
                | 'D' -> move 0 (fst knots[0]) (snd knots[0] - 1)
                | _ -> failwith $"Unknown direction {fst motion}")
        motions

    printfn $"Part 1: {knot2Positions.Count}"
    printfn $"Part 2: {knot10Positions.Count}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
