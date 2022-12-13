open System.IO

type PacketData =
    | Integer of int
    | List of PacketData[]

type SearchResult =
    | Correct
    | Incorrect
    | Continue

let rec parsePacket (str: string) : PacketData =
    if str[0] <> '[' && str[str.Length - 1] <> '}' then
        int str |> Integer
    else if str = "[]" then
        List Array.empty
    else
        let mutable parts: List<string> = []
        let mutable p = 1
        let mutable depth = 0

        for i in 0 .. str.Length - 1 do
            match str[i] with
            | '[' -> depth <- depth + 1
            | ']' ->
                if depth <= 1 then
                    parts <- parts @ [ str[p .. i - 1] ]

                depth <- depth - 1
            | ',' ->
                if depth = 1 then
                    parts <- parts @ [ str[p .. i - 1] ]
                    p <- i + 1
            | _ -> ()

        parts |> Seq.map parsePacket |> Array.ofSeq |> List

let rec verifyOrder (left: PacketData) (right: PacketData) =
    let rec verifyLists left right =
        match left, right with
        | List l, List r ->
            match l.Length, r.Length with
            | 0, 0 -> Continue
            | 0, _ -> Correct
            | _, 0 -> Incorrect
            | lenL, lenR ->
                let res = verifyOrder l[0] r[0]

                if (lenL > 1 || lenR > 1) && res = Continue then
                    verifyLists (List l[1..]) (List r[1..])
                else
                    res
        | _ -> failwith "Thats not a List :/"

    match (left, right) with
    | Integer l, Integer r ->
        if l < r then Correct
        else if l > r then Incorrect
        else Continue
    | List l, Integer r -> verifyOrder left (List [| right |])
    | Integer l, List r -> verifyOrder (List [| left |]) right
    | List l, List r -> verifyLists (List l) (List r)

let comparePackets a b =
    match verifyOrder a b with
    | Correct -> -1
    | Incorrect -> 1
    | Continue -> 0

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let packetPairs =
        (File.ReadAllText filename).Split("\r\n\r\n")
        |> Seq.map (fun x ->
            let split = x.Split("\r\n")
            parsePacket split[0], parsePacket split[1])

    let part1 =
        packetPairs
        |> Seq.mapi (fun i (left, right) ->
            let res = verifyOrder left right
            if res = Correct then i + 1 else 0)
        |> Seq.sum

    let allPackets =
        packetPairs
        |> Seq.collect (fun (left, right) -> [ left; right ])
        |> Seq.append [| List [| List [| Integer 2 |] |] |]
        |> Seq.append [| List [| List [| Integer 6 |] |] |]

    let part2 =
        allPackets
        |> Seq.sortWith comparePackets
        |> Seq.mapi (fun i packet ->
            match packet with
            | List [| List [| Integer 2 |] |] -> i + 1
            | List [| List [| Integer 6 |] |] -> i + 1
            | _ -> 1)
        |> Seq.fold (fun res x -> res * x) 1

    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
