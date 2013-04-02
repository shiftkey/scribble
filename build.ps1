$version = "v4.0.30319"

# build the solution from scratch
. $env:windir\Microsoft.NET\Framework\$version\MSBuild.exe build.proj /t:Test /ToolsVersion:4.0 /p:configuration=Release /m /p:BUILD_NUMBER=$build_number /m /v:M /nr:false
