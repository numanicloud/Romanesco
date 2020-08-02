function pack([string]$path) {
    Write-Host "==== ${path} をパッケージ化します" -ForegroundColor Cyan;
    dotnet build $_ -c Debug
    dotnet pack $_ -c Debug -o bin\nupkgs\ --version-suffix $ver --include-source -v minimal;
    sleep 2
}

$CommonProjects1 = "Annotations","Styles"
$CommonProjects2 = "Extensibility","Model","ViewModel","View"
$BuiltinPlugins = ".Model",".Test",".ViewModel",".View",""

$path1 = $CommonProjects1 | foreach { "Common\Romanesco.${_}\Romanesco.${_}.csproj" }
$path2 = $CommonProjects2 | foreach { "Common\Romanesco.Common.${_}\Romanesco.Common.${_}.csproj" }
$path3 = $BuiltinPlugins | foreach { "BuiltinPlugin\Romanesco.BuiltinPlugin${_}\Romanesco.BuiltinPlugin${_}.csproj" }

$paths = $path1 + $path2 + $path3

$ver = cat bin/nupkgs/version
$ver = [System.Int32]::Parse($ver) + 1

rm bin/nupkgs/*.nupkg
$paths | foreach { pack $_ }

echo $ver > bin/nupkgs/version