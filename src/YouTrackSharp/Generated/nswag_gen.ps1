#TODO all this sequence should most likely be moved to nuke
# requires $Env:JQ_DIR to be set to location of jq-win64.exe binary
# requires $Env:NSWAG_DIR to be set to root of NSwagStudio installation
#
# additional manual steps:
# - delete `Id` property from all `BundleProjectCustomField` subclasses in `YouTrackSharp.Api.Generated.cs`
# - delete `value` from `StateMachineIssueCustomField` in `openapi.json`

$nswag_file = "$PSScriptRoot\YouTrackSharp.Api.nswag"
$nswag_patched_file = "$PSScriptRoot\nswag.json"
$openapi_file = "$PSScriptRoot\openapi.json"
$openapi_patch_file = "$PSScriptRoot\openapi_patch.json"
$jqscript = "$PSScriptRoot\patch.jq"
$cs_file = "$PSScriptRoot\YouTrackSharp.Api.Generated.cs"

$nswag = (Get-Content $nswag_file | ConvertFrom-Json)

$openapi = (& "${Env:JQ_DIR}\jq-win64.exe" -f $jqscript $openapi_file $openapi_patch_file) | Out-String

$nswag.documentGenerator.fromDocument | Add-Member -name "json" -value $openapi -MemberType NoteProperty

$nswag | ConvertTo-Json -depth 100 | Out-File $nswag_patched_file

dotnet "${Env:NSWAG_DIR}/NetCore31/dotnet-nswag.dll" run $nswag_patched_file

rm $nswag_patched_file

(Get-Content $cs_file) |
    %{$_ -replace ", Required = Newtonsoft.Json.Required.DisallowNull",""} |
    %{$_ -replace "AdminProjectsPostAsync\(string template", "AdminProjectsPostAsync__FromTemplate(string template"} |
    %{$_ -replace "AgilesPostAsync\(string template", "AgilesPostAsync__FromTemplate(string template"} |
    %{$_ -replace "IssuesPostAsync\(string draftId", "IssuesPostAsync__FromDraft(string draftId"} |
    %{$_ -replace "IssuesCommentsPostAsync\(string id, string draftId", "IssuesCommentsPostAsync__FromDraft(string id, string draftId"} |
    %{$_ -replace "ArticlesPostAsync\(string draftId", "ArticlesPostAsync__FromDraft(string draftId"} |
    %{$_ -replace "ArticlesCommentsPostAsync\(string id, string draftId", "ArticlesCommentsPostAsync__FromDraft(string id, string draftId"} |
    Set-Content $cs_file