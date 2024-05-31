$CONF_BASE = "./conf"

function Set-ConfigValue {
    param ($Key, $Value)

    if (-not(Test-Path -Path $CONF_BASE)) {
        New-Item -ItemType Directory -Path $CONF_BASE
    }

    echo $Value > "$CONF_BASE/$Key"
}

function Get-ConfigValue {
    param (
        $Key
    )

    $configPath = Join-Path -Path $CONF_BASE -ChildPath $Key
    if (-not(Test-Path -Path $configPath -PathType Leaf)) {
        return $null
    }

    return (Get-Content -Path $configPath -Raw).Trim()
}