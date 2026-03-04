[CmdletBinding()]

param([Parameter(HelpMessage='Uninstall before installing')]
    [ValidateNotNullOrEmpty()]
    [switch]
    $reinstall)

if ($reinstall -eq $true)
{
    &.\uninstall.ps1
}

dotnet build

dotnet tool install --global --add-source src/Benday.SqlUtils.SqlUtilCli/bin/Debug Benday.SqlUtil
