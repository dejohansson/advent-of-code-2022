open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let instructions = (File.ReadAllLines filename) |> Seq.map (fun x -> x.Split())
    let crt = Array2D.create 40 6 '.'
    crt[0, 0] <- '#'

    let cycle (c, xReg, sum) =
        let nextC = c + 1

        if nextC % 40 >= xReg && nextC % 40 <= xReg + 2 then
            crt[(nextC - 1) % 40, (nextC - 1) / 40] <- '#'

        if (nextC - 20) % 40 = 0 then
            nextC, xReg, sum + nextC * xReg
        else
            nextC, xReg, sum

    let _, _, signalStrength =
        Seq.fold
            (fun (cN, xReg, sum) (instr: string[]) ->
                match instr[0] with
                | "noop" -> cycle (cN, xReg, sum)
                | "addx" ->
                    let c2, xReg2, sum2 = cycle (cN, xReg, sum)
                    cycle (c2, xReg2 + int instr[1], sum2)
                | _ -> (failwithf "Unknown instruction %A" instr))
            (1, 1, 0)
            instructions

    printfn $"Part 1: {signalStrength}"
    printfn $"Part 2:"

    for row in 0 .. crt.GetLength(1) - 1 do
        printfn "%s" (System.String crt[*, row])

    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
