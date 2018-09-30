$ErrorActionPreference = "Stop"

trap {
    write-output $_
    exit 1
}
$testsPath = ".\Tests"
$testProjects = Get-ChildItem -Directory $testsPath -Filter *.UnitTests -Recurse -Name

Write-host "Using test projects:"
Write-host $testProjects -separator "`n"

$exitCode = 0

foreach($testProject in $testProjects)
{ 
    try {
        Push-Location $testsPath"\$($testProject)"
		
        dotnet test
        
        if (!$?){
            $exitCode = $LASTEXITCODE
        }

        Pop-Location
    }catch [Exception]{
        Write-output $_.Exception.Message
        exit 1
    }
}

exit $exitCode

