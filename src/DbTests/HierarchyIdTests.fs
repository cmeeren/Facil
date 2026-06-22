module HierarchyIdTests

open System.Data.SqlTypes
open Expecto
open Microsoft.SqlServer.Types
open Swensen.Unquote


let private hierarchyId path = SqlHierarchyId.Parse(SqlString(path))


let private hierarchyPath (value: SqlHierarchyId) = value.ToString()


[<Tests>]
let tests =
    testList "HierarchyId tests" [

        testCase "Table DTO exposes hierarchyid columns"
        <| fun () ->
            let dto: DbGen.TableDtos.dbo.TableWithHierarchyId = {
                Key = 1
                Path = hierarchyId "/"
                NullablePath = Some(hierarchyId "/1/")
            }

            test <@ hierarchyPath dto.Path = "/" @>
            test <@ dto.NullablePath |> Option.map hierarchyPath = Some "/1/" @>


        testList (nameof DbGen.Procedures.dbo.ProcWithHierarchyId) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithHierarchyId_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithHierarchyId
                                .WithConnection(Config.connStr)
                                .WithParameters(path = hierarchyId "/1/", nullablePath = None)
                            |> exec

                        test <@ res.Value.path |> Option.map hierarchyPath = Some "/1/" @>
                        test <@ res.Value.nullablePath = None @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithHierarchyIdFromTvp) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithHierarchyIdFromTvp_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithHierarchyIdFromTvp
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.HierarchyIdTableType.create (
                                            path = hierarchyId "/",
                                            nullablePath = Some(hierarchyId "/1/")
                                        )
                                    ]
                                )
                            |> exec

                        test <@ hierarchyPath res.Value.path = "/" @>
                        test <@ res.Value.nullablePath |> Option.map hierarchyPath = Some "/1/" @>
                )
        ]


        testList (nameof DbGen.Scripts.HierarchyIdTempTable) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.HierarchyIdTempTable_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.HierarchyIdTempTable
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    hierarchyIdTempTable = [
                                        DbGen.Scripts.HierarchyIdTempTable.HierarchyIdTempTable.create (
                                            path = hierarchyId "/1/",
                                            nullablePath = None
                                        )
                                    ]
                                )
                            |> exec

                        test <@ hierarchyPath res.Value.Path = "/1/" @>
                        test <@ res.Value.NullablePath = None @>
                )
        ]
    ]
