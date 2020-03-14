$packageJson = Get-Content 'package.json' | Out-String | ConvertFrom-Json
$dependencies = $packageJson.dependencies | Get-Member -MemberType NoteProperty | Select-Object -ExpandProperty Name

foreach ($d in $dependencies) {
    Invoke-Expression -Command 'cmd.exe /c "npm list $d || npm i $d"'
}