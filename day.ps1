$dayNum = $args[0]
Write-Information "Creating project for Day $dayNum"

Invoke-Expression -Command "dotnet new aoc-fsharp -o Day$dayNum"
Invoke-Expression -Command "dotnet sln add ./Day$dayNum/Day$dayNum.fsproj"