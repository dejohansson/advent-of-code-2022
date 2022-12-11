open System.IO
open System.Collections.Generic
open System.Linq

type Monkey =
    { Items: Queue<uint64>
      Items2: Queue<uint64>
      Operation: uint64 -> uint64
      Test: uint64 -> int
      Divisor: uint64
      mutable Inspections: uint64
      mutable Inspections2: uint64 }

let rec gcd a b =
    match (a, b) with
    | (x, y) when x = y -> x
    | (x, y) when x > y -> gcd (x - y) y
    | (x, y) -> gcd x (y - x)

let lcm a b = a * b / (gcd a b)

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = Array.tryItem 0 argv |> Option.defaultValue "./input.txt"

    let monkeys =
        (File.ReadAllText filename).Split("\r\n\r\n")
        |> Array.map (fun x ->
            let rows = x.Split("\r\n")
            let divisor = uint64 rows[3].[21..]

            { Items = new Queue<uint64>((rows[1].[17..].Split(", ")) |> Seq.map uint64)
              Items2 = new Queue<uint64>((rows[1].[17..].Split(", ")) |> Seq.map uint64)
              Operation =
                (fun x ->
                    match rows[2].[23], rows[2].[25..] with
                    | '+', "old" -> x + x
                    | '+', _ as (_, num) -> x + uint64 num
                    | '*', "old" -> x * x
                    | '*', _ as (_, num) -> x * uint64 num
                    | _ -> failwith $"Unknown operation {rows[2].[23..]}")
              Test =
                (fun x ->
                    if x % divisor = 0UL then
                        int rows[4].[29..]
                    else
                        int rows[5].[30..])
              Divisor = divisor
              Inspections = 0UL
              Inspections2 = 0UL })

    let monkeyLcm = monkeys |> Seq.fold (fun res m -> lcm res m.Divisor) 1UL

    for i in 1..10000 do
        for monkey in monkeys do
            if i <= 20 then
                for _ in 1 .. monkey.Items.Count do
                    let newItem = monkey.Operation(monkey.Items.Dequeue()) / 3UL
                    monkeys[ monkey.Test newItem ].Items.Enqueue(newItem)
                    monkey.Inspections <- monkey.Inspections + 1UL

            for _ in 1 .. monkey.Items2.Count do
                let newItem2 = monkey.Operation(monkey.Items2.Dequeue() % monkeyLcm)
                monkeys[ monkey.Test newItem2 ].Items2.Enqueue(newItem2)
                monkey.Inspections2 <- monkey.Inspections2 + 1UL

    let part1 =
        monkeys
            .Select(fun x -> x.Inspections)
            .OrderByDescending(fun x -> x)
            .Take(2)
            .Aggregate(fun x y -> x * y)

    let part2 =
        monkeys
            .Select(fun x -> x.Inspections2)
            .OrderByDescending(fun x -> x)
            .Take(2)
            .Aggregate(fun x y -> x * y)

    printfn $"Part 1: {part1}"
    printfn $"Part 2: {part2}"
    stopWatch.Stop()
    printfn $"Completed in {stopWatch.Elapsed.TotalMilliseconds} ms"
    0
