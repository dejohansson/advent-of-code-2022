open System.IO

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"
    let rucksacks = File.ReadAllLines filename
    printfn $"Read {rucksacks.Length} lines from {filename}\n"
    let items = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"

    let sum =
        rucksacks
        |> Array.map (fun rucksack ->
            let firstCompartment = rucksack |> Seq.take (rucksack.Length / 2) |> Set.ofSeq

            let secondCompartment =
                rucksack |> Seq.skip (rucksack.Length / 2) |> Set.ofSeq

            Set.intersect firstCompartment secondCompartment |> Seq.head |> items.IndexOf)
        |> Array.sum

    printfn $"Part 1: {sum}"

    let sum2 =
        rucksacks
        |> Seq.chunkBySize 3
        |> Seq.fold
            (fun sum group ->
                sum
                + (Set.intersectMany (
                    group |> Array.map (fun rucksack -> Set.ofSeq rucksack)
                   )
                   |> Seq.head
                   |> (items.IndexOf: char -> int)))
            0

    printfn $"Part 2: {sum2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
