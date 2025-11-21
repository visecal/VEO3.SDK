@echo off
REM ========================================
REM Build Single Plugin
REM Usage: Build-Plugin.bat PluginName
REM ========================================

if "%~1"=="" (
    echo.
    echo [ERROR] Plugin name required!
    echo.
    echo Usage: Build-Plugin.bat PluginName
    echo Example: Build-Plugin.bat MyAwesomePlugin
    echo.
    pause
    exit /b 1
)

set PLUGIN_NAME=%~1

echo.
echo ========================================
echo  Building Plugin: %PLUGIN_NAME%
echo ========================================
echo.

REM Step 1: Check if VEO3.SDK is built (both configs needed)
echo [1/4] Checking/Building VEO3.SDK...
cd VEO3.SDK
if not exist "bin\Debug\net8.0-windows\VEO3.SDK.dll" (
    echo   Building Debug...
    dotnet build -c Debug
    if %ERRORLEVEL% NEQ 0 (
        echo [ERROR] Failed to build VEO3.SDK Debug!
        cd ..
        pause
        exit /b 1
    )
)
if not exist "bin\Release\net8.0-windows\VEO3.SDK.dll" (
    echo   Building Release...
    dotnet build -c Release
    if %ERRORLEVEL% NEQ 0 (
        echo [ERROR] Failed to build VEO3.SDK Release!
        cd ..
        pause
        exit /b 1
    )
)
echo   VEO3.SDK ready (Debug + Release)
cd ..

REM Step 2: Check if VEO3 main project is built
if not exist "subphimv1\bin\Debug\net8.0-windows\subphimv1.dll" (
    echo.
    echo [2/4] VEO3 main project not built yet. Building now...
    cd subphimv1
    dotnet build -c Debug
    if %ERRORLEVEL% NEQ 0 (
        echo.
        echo [ERROR] Failed to build VEO3 main project!
        cd ..
        pause
        exit /b 1
    )
    echo [SUCCESS] VEO3 main project built!
    cd ..
) else (
    echo [2/4] VEO3 main project already built.
)

REM Step 3: Build Plugin
echo.
echo [3/4] Building %PLUGIN_NAME%...

if not exist "VEO3_SDK_Examples\%PLUGIN_NAME%" (
    echo.
    echo [ERROR] Plugin directory not found: VEO3_SDK_Examples\%PLUGIN_NAME%
    echo.
    echo Available plugins:
    dir /b /ad VEO3_SDK_Examples
    echo.
    pause
    exit /b 1
)

cd VEO3_SDK_Examples\%PLUGIN_NAME%
dotnet build -c Release
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Failed to build %PLUGIN_NAME%!
    echo Please check the error messages above.
    cd ..\..
    pause
    exit /b 1
)
echo [SUCCESS] %PLUGIN_NAME% built successfully!
cd ..\..

REM Step 4: Deploy Plugin
echo.
echo [4/4] Deploying %PLUGIN_NAME%...

REM Create Plugins directory if not exists
if not exist "subphimv1\bin\Debug\net8.0-windows\Plugins" (
    mkdir "subphimv1\bin\Debug\net8.0-windows\Plugins"
    echo Created Plugins directory
)

REM Copy plugin DLL
copy /Y "VEO3_SDK_Examples\%PLUGIN_NAME%\bin\Release\net8.0-windows\%PLUGIN_NAME%.dll" "subphimv1\bin\Debug\net8.0-windows\Plugins\" >nul
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to copy %PLUGIN_NAME%.dll
    echo Source: VEO3_SDK_Examples\%PLUGIN_NAME%\bin\Release\net8.0-windows\%PLUGIN_NAME%.dll
    pause
    exit /b 1
)

echo.
echo ========================================
echo  BUILD COMPLETE!
echo ========================================
echo.
echo %PLUGIN_NAME%.dll has been deployed to:
echo   subphimv1\bin\Debug\net8.0-windows\Plugins\
echo.
echo You can now run VEO3 to test the plugin:
echo   cd subphimv1\bin\Debug\net8.0-windows
echo   subphimv1.exe
echo.
pause
