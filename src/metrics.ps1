$violations = @()

Get-ChildItem -Recurse -Filter *.Metrics.xml | ForEach-Object {
    $file = $_.FullName
    $xml = [xml](Get-Content $file)
    $complex = $xml.SelectNodes("//Member[@CyclomaticComplexity]")
    foreach ($m in $complex) {
        if ([int]$m.CyclomaticComplexity -gt 10) {
            $violations += "$($m.Name) in $file has CyclomaticComplexity=$($m.CyclomaticComplexity)"
        }
    }
}

if ($violations.Count -gt 0) {
    Write-Host "❌ Cyclomatic complexity exceeded (limit=10)"
    $violations | ForEach-Object { Write-Host $_ }
    exit 1
} else {
    Write-Host "✅ Cyclomatic complexity within limits"
}
