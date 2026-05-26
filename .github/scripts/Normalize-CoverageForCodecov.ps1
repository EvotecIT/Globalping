param(
    [string] $Path = "."
)

$coverageFiles = Get-ChildItem -LiteralPath $Path -Recurse -Filter 'coverage.cobertura.xml' -File

foreach ($coverageFile in $coverageFiles) {
    [xml] $document = Get-Content -LiteralPath $coverageFile.FullName

    $branchRateNodes = $document.SelectNodes('//*[@branch-rate]')
    foreach ($node in $branchRateNodes) {
        [void] $node.RemoveAttribute('branch-rate')
    }

    $branchSummaryNodes = $document.SelectNodes('//*[@branches-covered or @branches-valid]')
    foreach ($node in $branchSummaryNodes) {
        [void] $node.RemoveAttribute('branches-covered')
        [void] $node.RemoveAttribute('branches-valid')
    }

    $branchLineNodes = $document.SelectNodes('//line[@branch or @condition-coverage]')
    foreach ($node in $branchLineNodes) {
        [void] $node.RemoveAttribute('branch')
        [void] $node.RemoveAttribute('condition-coverage')
    }

    $conditionNodes = @($document.SelectNodes('//conditions'))
    foreach ($node in $conditionNodes) {
        [void] $node.ParentNode.RemoveChild($node)
    }

    $document.Save($coverageFile.FullName)
    Write-Host "Normalized $($coverageFile.FullName)"
}
