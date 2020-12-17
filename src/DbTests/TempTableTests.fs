module TempTableTests

open Expecto
open Hedgehog
open Microsoft.Data.SqlClient


[<Tests>]
let tests =
  testList "TempTable tests" [
    testCase "Load and read from the temp table" <| fun () ->
      use conn = new SqlConnection(Config.connStr)

      let res =
        DbGen.Scripts.SQL.TempTable
          .WithConnection(conn)
          .BulkLoadTempTable([{| Id = 1; Name = "name" |}])
          .Execute()
        |> Seq.toList


      Expect.equal res.Length 1 "Wrong length"
      let head = res.[0]

      Expect.equal head.Id 1 "Wrong Id"
      Expect.equal head.Name "name" "Wrong Name"
  ]
