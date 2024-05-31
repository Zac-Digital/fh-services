# Name: Setup local NuGet source
# Description: Sets up a local NuGet source used for local FH package development.

. lib/conf.ps1

$SOURCE_NAME = "fh_local"

dotnet nuget add source $SOURCE_NAME -n $SOURCE_NAME

"Done!"