module TempTableTests

open Expecto
open Hedgehog

[<Tests>]
let tests =
  testList "TempTable tests" [
    testCase "Load and read from the temp table with dto" <| fun () ->
      let data =
        seq {
          {| Id = 1; Name = "name" |}
        }

      let res =
        DbGen.Scripts.SQL.TempTable
          .WithConnection(Config.connStr)
          .WithParameters({| data = data |})
          .Execute()
        |> Seq.toList


      Expect.equal res.Length 1 "Wrong length"
      let head = res.[0]

      Expect.equal head.Id 1 "Wrong Id"
      Expect.equal head.Name "name" "Wrong Name"

    testCase "Load and read from the temp table" <| fun () ->
      let data =
        seq {
          DbGen.Scripts.SQL.TempTabledata(Id = 1, Name = "name")
        }

      let res =
        DbGen.Scripts.SQL.TempTable
          .WithConnection(Config.connStr)
          .WithParameters(data = data)
          .Execute()
        |> Seq.toList


      Expect.equal res.Length 1 "Wrong length"
      let head = res.[0]

      Expect.equal head.Id 1 "Wrong Id"
      Expect.equal head.Name "name" "Wrong Name"
  ]
