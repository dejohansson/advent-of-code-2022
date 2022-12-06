open System.IO

[<EntryPoint>]
let main argv =
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let lines = File.ReadAllLines filename
    printfn $"Read {lines.Length} lines from {filename}\n"

    let score1 =
        lines
        |> Array.map (fun l ->
            let plays = ("ABC".IndexOf(l[0]) + 1, "XYZ".IndexOf(l[2]) + 1)

            snd plays
            + match plays with
              | o, y when (o = 3 && y = 1) -> 6
              | o, y when (o = 1 && y = 3) -> 0
              | o, y when o < y -> 6
              | o, y when o = y -> 3
              | _ -> 0)
        |> Array.sum

    printfn $"Part 1: {score1}"

    let score2 =
        lines
        |> Array.map (fun l ->
            let plays = ("ABC".IndexOf(l[0]), "XYZ".IndexOf(l[2]) * 3)

            snd plays
            + match plays with
              | o, 0 -> (((o - 1) % 3) + 3) % 3
              | o, 3 -> o
              | o, 6 -> (o + 1) % 3
              | _ -> raise <| new System.Exception("Opps :(")
            + 1)
        |> Array.sum

    printfn $"Part 2: {score2}"
    0
