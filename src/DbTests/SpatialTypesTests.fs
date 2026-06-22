module SpatialTypesTests

open System
open Expecto
open Microsoft.SqlServer.Types
open Swensen.Unquote


let private geometryPoint x y srid = SqlGeometry.Point(x, y, srid)


let private geographyPoint latitude longitude srid =
    SqlGeography.Point(latitude, longitude, srid)


let private sameGeometry (expected: SqlGeometry) (actual: SqlGeometry) =
    actual.STSrid.Value = expected.STSrid.Value && actual.STEquals(expected).IsTrue


let private sameGeography (expected: SqlGeography) (actual: SqlGeography) =
    actual.STSrid.Value = expected.STSrid.Value && actual.STEquals(expected).IsTrue


[<Tests>]
let tests =
    if not (OperatingSystem.IsWindows()) then
        testList "Spatial type tests" [
            testCase "Runtime spatial tests require Windows native spatial assets"
            <| fun () ->
                skiptest
                    "Microsoft.SqlServer.Types spatial runtime values require Windows native assets in the pinned package version."
        ]
    else
        testList "Spatial type tests" [

            testCase "Table DTO exposes geometry and geography columns"
            <| fun () ->
                let shape = geometryPoint 1. 2. 0
                let nullableShape = geometryPoint 3. 4. 0
                let location = geographyPoint 47. 9. 4326
                let nullableLocation = geographyPoint 48. 10. 4326

                let dto: DbGen.TableDtos.dbo.TableWithSpatialTypes = {
                    Key = 1
                    Shape = shape
                    NullableShape = Some nullableShape
                    Location = location
                    NullableLocation = Some nullableLocation
                }

                test <@ dto.Shape |> sameGeometry shape @>
                test <@ dto.NullableShape |> Option.exists (sameGeometry nullableShape) @>
                test <@ dto.Location |> sameGeography location @>
                test <@ dto.NullableLocation |> Option.exists (sameGeography nullableLocation) @>


            testList (nameof DbGen.Procedures.dbo.ProcWithSpatialTypes) [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSpatialTypes_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let shape = geometryPoint 1. 2. 0
                            let location = geographyPoint 47. 9. 4326
                            let nullableLocation = geographyPoint 48. 10. 4326

                            let res =
                                DbGen.Procedures.dbo.ProcWithSpatialTypes
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        shape = shape,
                                        nullableShape = None,
                                        location = location,
                                        nullableLocation = Some nullableLocation
                                    )
                                |> exec

                            test <@ res.Value.shape |> Option.exists (sameGeometry shape) @>
                            test <@ res.Value.nullableShape = None @>
                            test <@ res.Value.location |> Option.exists (sameGeography location) @>
                            test <@ res.Value.nullableLocation |> Option.exists (sameGeography nullableLocation) @>
                    )
            ]


            testList (nameof DbGen.Procedures.dbo.ProcWithSpatialTypesFromTvp) [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSpatialTypesFromTvp_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let shape = geometryPoint 1. 2. 0
                            let nullableShape = geometryPoint 3. 4. 0
                            let location = geographyPoint 47. 9. 4326

                            let res =
                                DbGen.Procedures.dbo.ProcWithSpatialTypesFromTvp
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        ``params`` = [
                                            DbGen.TableTypes.dbo.SpatialTypesTableType.create (
                                                shape = shape,
                                                nullableShape = Some nullableShape,
                                                location = location,
                                                nullableLocation = None
                                            )
                                        ]
                                    )
                                |> exec

                            test <@ res.Value.shape |> sameGeometry shape @>
                            test <@ res.Value.nullableShape |> Option.exists (sameGeometry nullableShape) @>
                            test <@ res.Value.location |> sameGeography location @>
                            test <@ res.Value.nullableLocation = None @>
                    )
            ]


            testList (nameof DbGen.Scripts.SpatialTypesTempTable) [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Scripts.SpatialTypesTempTable_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let shape = geometryPoint 1. 2. 0
                            let location = geographyPoint 47. 9. 4326
                            let nullableLocation = geographyPoint 48. 10. 4326

                            let res =
                                DbGen.Scripts.SpatialTypesTempTable
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        spatialTypesTempTable = [
                                            DbGen.Scripts.SpatialTypesTempTable.SpatialTypesTempTable.create (
                                                shape = shape,
                                                nullableShape = None,
                                                location = location,
                                                nullableLocation = Some nullableLocation
                                            )
                                        ]
                                    )
                                |> exec

                            test <@ res.Value.Shape |> sameGeometry shape @>
                            test <@ res.Value.NullableShape = None @>
                            test <@ res.Value.Location |> sameGeography location @>
                            test <@ res.Value.NullableLocation |> Option.exists (sameGeography nullableLocation) @>
                    )
            ]
        ]
