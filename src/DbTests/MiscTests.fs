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
          allExecuteMethodsAsSingle<DbGen.Scripts.DynamicSqlWithDeclaration_Executable, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let res =
                  DbGen.Scripts.DynamicSqlWithDeclaration
                    .WithConnection(Config.connStr)
                    .WithParameters(col1Filter = "test2")
                  |> exec
                test <@ res.Value.TableCol1 = "test2" @>
                test <@ res.Value.TableCol2 = Some 2 @>
          )
      ]


      testList "Dynamic SQL works as expected with config declaration" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Scripts.DynamicSqlWithoutDeclaration_Executable, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let res =
                  DbGen.Scripts.DynamicSqlWithoutDeclaration
                    .WithConnection(Config.connStr)
                    .WithParameters(col1Filter = "test2")
                  |> exec
                test <@ res.Value.TableCol1 = "test2" @>
                test <@ res.Value.TableCol2 = Some 2 @>
          )
      ]

      testList "Truncation behavior" [

        // This functionality is incidental, not a requirement; the test is here to pick
        // up potentially breaking changes, in order to ensure that any change is
        // deliberate.
        testCase "Normal parameters that are too long are silently truncated" <| fun () ->
          let res =
            DbGen.Procedures.dbo.ProcWithLengthTypes
              .WithConnection(Config.connStr)
              .WithParameters(
                binary = [| 1uy; 2uy; 3uy; 4uy |],
                char = "1234",
                nchar = "1234",
                nvarchar = "1234",
                varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                varchar = "1234"
              )
              .ExecuteSingle()

          test <@ res.Value.binary = Some [| 1uy; 2uy; 3uy |] @>
          test <@ res.Value.char = Some "123" @>
          test <@ res.Value.nchar = Some "123" @>
          test <@ res.Value.nvarchar = Some "123" @>
          test <@ res.Value.varbinary = Some [| 1uy; 2uy; 3uy |] @>
          test <@ res.Value.varchar = Some "123" @>


        // This functionality is incidental, not a requirement; the test is here to pick
        // up potentially breaking changes, in order to ensure that any change is
        // deliberate.
        testCase "TVP parameters that are too long are silently truncated" <| fun () ->
          let res =
            DbGen.Procedures.dbo.ProcWithLengthTypesFromTvp
              .WithConnection(Config.connStr)
              .WithParameters([
                DbGen.TableTypes.dbo.LengthTypes.create(
                  binary = [| 1uy; 2uy; 3uy; 4uy |],
                  char = "1234",
                  nchar = "1234",
                  nvarchar = "1234",
                  varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                  varchar = "1234"
                )
              ])
              .ExecuteSingle()

          test <@ res.Value.binary = [| 1uy; 2uy; 3uy |] @>
          test <@ res.Value.char = "123" @>
          test <@ res.Value.nchar = "123" @>
          test <@ res.Value.nvarchar = "123" @>
          test <@ res.Value.varbinary = [| 1uy; 2uy; 3uy |] @>
          test <@ res.Value.varchar = "123" @>


        // This functionality is incidental, not a requirement; the test is here to pick
        // up potentially breaking changes, in order to ensure that any change is
        // deliberate.
        testCase "Temp table parameters that are too long raise exceptions" <| fun () ->
          let run () =
            DbGen.Scripts.TempTableWithLengthTypes
              .WithConnection(Config.connStr)
              .WithParameters([
                DbGen.Scripts.TempTableWithLengthTypes.tempTableWithLengthTypes.create(
                  binary = [| 1uy; 2uy; 3uy; 4uy |],
                  char = "1234",
                  nchar = "1234",
                  nvarchar = "1234",
                  varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                  varchar = "1234"
                )
              ])
              .ExecuteSingle()
              |> ignore
          Expect.throws run "Should throw"


      ]


      testCase "Compile-time script param inheritance test" <| fun () ->
        DbGen.Scripts.ParamInheritance
          .WithConnection(Config.connStr)
          .WithParameters(
            col1 = Some 1,
            col2 = 1
          )
          |> ignore


      testCase "Compile-time sproc column inheritance test" <| fun () ->
        let f () =
          let res =
            DbGen.Procedures.dbo.ProcColumnInheritance
              .WithConnection(Config.connStr)
              .ExecuteSingle()
          res.Value.Foo |> ignore<int>
          res.Value.Bar |> ignore<int>
        ignore f


      testCase "Compile-time script column inheritance test" <| fun () ->
        let f () =
          let res =
            DbGen.Scripts.ColumnInheritance
              .WithConnection(Config.connStr)
              .ExecuteSingle()
          res.Value.Foo |> ignore<int>
          res.Value.Bar |> ignore<int>
        ignore f


      testCase "Compile-time table DTO column inheritance test" <| fun () ->
        let f () =
          let (x: DbGen.TableDtos.dbo.TableDtoColumnInheritance) = Unchecked.defaultof<_>
          x.Foo |> ignore<int>
          x.Bar |> ignore<int>
        ignore f


  ]
