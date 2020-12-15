module MiscTests

open Expecto
open Microsoft.Data.SqlClient
open Swensen.Unquote


[<Tests>]
let tests =
  testList "Misc tests" [
    
      testSequenced <| testList "Execution is resilient against runtime column order changes in result sets" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcToBeModified, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->

                let revertColumnOrder () =
                  use conn = new SqlConnection(Config.connStr)
                  conn.Open ()
                  use cmd = conn.CreateCommand()
                  cmd.CommandText <- "
                    ALTER PROCEDURE [dbo].[ProcToBeModified]
                    AS

                    SELECT
                      Foo = 1,
                      Bar = 'test'
                  "
                  cmd.ExecuteNonQuery () |> ignore

                let changeColumnOrder () =
                  use conn = new SqlConnection(Config.connStr)
                  conn.Open ()
                  use cmd = conn.CreateCommand()
                  cmd.CommandText <- "
                    ALTER PROCEDURE [dbo].[ProcToBeModified]
                    AS

                    SELECT
                      Bar = 'test',
                      Foo = 1
                  "
                  cmd.ExecuteNonQuery () |> ignore

                let test () =
                  let res =
                    DbGen.Procedures.dbo.ProcToBeModified
                      .WithConnection(Config.connStr)
                    |> exec
                  test <@ res.Value.Foo = 1 @>
                  test <@ res.Value.Bar = "test" @>

                try
                  revertColumnOrder ()  // Just to be safe
                  test ()
                  changeColumnOrder ()
                  test ()
                finally
                  revertColumnOrder ()
          )
      ]


      testList "Dynamic SQL works as expected with explicit declaration" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Scripts.SQL.DynamicSqlWithDeclaration_Executable, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let res =
                  DbGen.Scripts.SQL.DynamicSqlWithDeclaration
                    .WithConnection(Config.connStr)
                    .WithParameters(col1Filter = "test2")
                  |> exec
                test <@ res.Value.TableCol1 = "test2" @>
                test <@ res.Value.TableCol2 = Some 2 @>
          )
      ]


      testList "Dynamic SQL works as expected with config declaration" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Scripts.SQL.DynamicSqlWithoutDeclaration_Executable, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let res =
                  DbGen.Scripts.SQL.DynamicSqlWithoutDeclaration
                    .WithConnection(Config.connStr)
                    .WithParameters(col1Filter = "test2")
                  |> exec
                test <@ res.Value.TableCol1 = "test2" @>
                test <@ res.Value.TableCol2 = Some 2 @>
          )
      ]

  ]
