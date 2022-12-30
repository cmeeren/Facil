module Config

open Microsoft.Extensions.Configuration


let private config =
    ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build()

let private requireConfigValue path =
    let x = config.GetValue<string> path

    if isNull x then
        failwith $"Missing required config value '%s{path}'"
    else
        x

let connStr = requireConfigValue "connectionString"
