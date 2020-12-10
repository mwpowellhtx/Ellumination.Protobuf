@echo off

setlocal

rem We do not publish the API key as part of the script itself.
if "%my_nuget_api_key%"=="" (
    echo You are prohibited from making these sorts of changes.
    goto :fini
)

rem Default list delimiter is Comma (,).
:redefine_delim
if "%delim%" == "" (
    set delim=,
)
rem Redefine the delimiter when a Dot (.) is discovered.
rem Anticipates potentially accepting Delimiter as a command line arg.
if "%delim%" == "." (
    set delim=
    goto :redefine_delim
)

rem Go ahead and pre-seed the Projects up front.
:set_projects
set projects=
rem Setup All Projects
set all_projects=Ellumination.Protobuf
set all_projects=%all_projects%%delim%Ellumination.Protobuf.Antlr
set all_projects=%all_projects%%delim%Ellumination.Protobuf.Descriptors
rem Setup Protobuf Projects
set proto_projects=Ellumination.Protobuf
rem Setup Antlr Projects
set antlr_projects=Ellumination.Protobuf.Antlr
rem Setup Descriptor Projects
set descriptor_projects=Ellumination.Protobuf.Descriptors

:parse_args

rem Done parsing the args.
if "%1" == "" (
    goto :fini_args
)

:set_destination
if "%1" == "--local" (
    set dest=local
    goto :next_arg
)
if "%1" == "--nuget" (
    set dest=nuget
    goto :next_arg
)

:set_drive_letter
if "%1" == "--drive" (
    set drive_letter=%2
    shift
    goto :next_arg
)
if "%1" == "--drive-letter" (
    set drive_letter=%2
    shift
    goto :next_arg
)

:set_dry_run
if "%1" == "--dry" (
    set dry=true
    goto :next_arg
)
if "%1" == "--dry-run" (
    set dry=true
    goto :next_arg
)
if "%1" == "--no-dry" (
    set dry=false
    goto :next_arg
)
if "%1" == "--no-dry-run" (
    set dry=false
    goto :next_arg
)

:set_should_pause
if "%1" == "--pause" (
    set should_pause=1
    goto :next_arg
)
if "%1" == "--no-pause" (
    set should_pause=0
    goto :next_arg
)

:set_config
if "%1" == "--config" (
    set config=%2
    shift
    goto :next_arg
)

:add_antlr_projects
if "%1" == "--proto" (
    if "%projects%" == "" (
        set projects=%proto_projects%
    ) else (
        set projects=%projects%%delim%%proto_projects%
    )
	goto :next_arg
)
if "%1" == "--protobuf" (
    if "%projects%" == "" (
        set projects=%proto_projects%
    ) else (
        set projects=%projects%%delim%%proto_projects%
    )
	goto :next_arg
)

:add_antlr_projects
if "%1" == "--antlr" (
    if "%projects%" == "" (
        set projects=%antlr_projects%
    ) else (
        set projects=%projects%%delim%%antlr_projects%
    )
	goto :next_arg
)

:add_descriptor_project
if "%1" == "--descriptor" (
    if "%projects%" == "" (
        set projects=%descriptor_projects%
    ) else (
        set projects=%projects%%delim%%descriptor_projects%
    )
	goto :next_arg
)

:add_all_projects
if "%1" == "--all" (
    rem Prepare to publish All Projects.
    set projects=%all_projects%
    goto :next_arg
)

:add_project
if "%1" == "--project" (
    rem Add a Project to the Projects list.
    if "%projects%" == "" (
        set projects=%2
    ) else (
        set projects=%projects%%delim%%2
    )
    shift
    goto :next_arg
)

:next_arg

shift

goto :parse_args

:fini_args

:verify_args

:verify_projects

if "%projects%" == "" (
    rem In which case, there is nothing else to do.
    echo Nothing to process.
    goto :fini
)

:verify_config
if "%config%" == "" (
    rem Assumes Release Configuration when not otherwise specified.
    set config=Release
)

:verify_destination
if "%dest%" == "" set dest=local

:verify_should_pause
if "%should_pause%" == "" set should_pause=0

:verify_driver_letter
if "%driver_letter%" == "" set driver_letter=E:

:publish_projects

:set_vars
set xcopy_exe=xcopy.exe
rem Have to re-set the options for whatever reason.
set xcopy_opts=
set xcopy_opts=%xcopy_opts% /Y /F
set xcopy_destination_dir=%driver_letter%\Dev\NuGet\local\packages

rem Expecting NuGet to be found in the System Path.
set nuget_exe=NuGet.exe
set nuget_push_verbosity=detailed
set nuget_push_source=https://api.nuget.org/v3/index.json

rem Ditto clearing the value first.
set nuget_push_opts=
set nuget_push_opts=%nuget_push_opts% %my_nuget_api_key%
set nuget_push_opts=%nuget_push_opts% -Verbosity %nuget_push_verbosity%
set nuget_push_opts=%nuget_push_opts% -NonInteractive
set nuget_push_opts=%nuget_push_opts% -Source %nuget_push_source%

rem Do the main areas here.
pushd ..\..

if not "%projects%" == "" (
    echo Processing '%config%' configuration for '%projects%' ...
)
:next_project
if not "%projects%" == "" (
    for /f "tokens=1* delims=%delim%" %%p in ("%projects%") do (
        if "%dest%" equ "local" (
            call :copy_local %%p
        )
        if "%dest%" equ "nuget" (
            call :publish_nuget %%p
        )
        set projects=%%q
        goto :next_project
    )
)

popd

goto :fini

:copy_local
for %%f in ("%1\bin\%config%\%1.*.nupkg") do (
    if "%dry%" equ "true" (
        echo Dry run: %xcopy_exe% "%%f" "%xcopy_destination_dir%" %xcopy_opts%
    ) else (
        echo Running: %xcopy_exe% "%%f" "%xcopy_destination_dir%" %xcopy_opts%
        %xcopy_exe% "%%f" "%xcopy_destination_dir%" %xcopy_opts%
    )
)
exit /b

:publish_nuget
for %%f in ("%1\bin\%config%\%1.*.nupkg") do (
    if "%dry%" equ "true" (
        echo Dry run: %nuget_exe% push "%%f" %nuget_push_opts%
    ) else (
        echo Running: %nuget_exe% push "%%f" %nuget_push_opts%
        %nuget_exe% push "%%f" %nuget_push_opts%
    )
)
exit /b

:fini

if "%should_pause%" equ "1" pause

endlocal
