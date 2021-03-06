function Build($solutionPath, $mode)
{
    Write-Host 'Building' $solutionPath 'in' $mode
    $rebuildModeParameter = "/Rebuild " + $mode
    $p = Start-Process -FilePath 'C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe' -ArgumentList $solutionPath,$rebuildModeParameter -PassThru
    $null = $p.WaitForExit(-1)
    Write-Host 'Build completed!'
}