#!/bin/bash

# ========================================
# Build Single Plugin
# Usage: ./Build-Plugin.sh PluginName
# ========================================

if [ -z "$1" ]; then
    echo ""
    echo "[ERROR] Plugin name required!"
    echo ""
    echo "Usage: ./Build-Plugin.sh PluginName"
    echo "Example: ./Build-Plugin.sh MyAwesomePlugin"
    echo ""
    exit 1
fi

PLUGIN_NAME="$1"

echo ""
echo "========================================"
echo " Building Plugin: $PLUGIN_NAME"
echo "========================================"
echo ""

# Step 1: Check if VEO3.SDK is built (both configs needed)
echo "[1/4] Checking/Building VEO3.SDK..."
cd VEO3.SDK
if [ ! -f "bin/Debug/net8.0-windows/VEO3.SDK.dll" ]; then
    echo "  Building Debug..."
    dotnet build -c Debug
    if [ $? -ne 0 ]; then
        echo "[ERROR] Failed to build VEO3.SDK Debug!"
        cd ..
        exit 1
    fi
fi
if [ ! -f "bin/Release/net8.0-windows/VEO3.SDK.dll" ]; then
    echo "  Building Release..."
    dotnet build -c Release
    if [ $? -ne 0 ]; then
        echo "[ERROR] Failed to build VEO3.SDK Release!"
        cd ..
        exit 1
    fi
fi
echo "  VEO3.SDK ready (Debug + Release)"
cd ..

# Step 2: Check if VEO3 main project is built
if [ ! -f "subphimv1/bin/Debug/net8.0-windows/subphimv1.dll" ]; then
    echo ""
    echo "[2/4] VEO3 main project not built yet. Building now..."
    cd subphimv1
    dotnet build -c Debug
    if [ $? -ne 0 ]; then
        echo ""
        echo "[ERROR] Failed to build VEO3 main project!"
        cd ..
        exit 1
    fi
    echo "[SUCCESS] VEO3 main project built!"
    cd ..
else
    echo "[2/4] VEO3 main project already built."
fi

# Step 3: Build Plugin
echo ""
echo "[3/4] Building $PLUGIN_NAME..."

if [ ! -d "VEO3_SDK_Examples/$PLUGIN_NAME" ]; then
    echo ""
    echo "[ERROR] Plugin directory not found: VEO3_SDK_Examples/$PLUGIN_NAME"
    echo ""
    echo "Available plugins:"
    ls -1 VEO3_SDK_Examples/
    echo ""
    exit 1
fi

cd "VEO3_SDK_Examples/$PLUGIN_NAME"
dotnet build -c Release
if [ $? -ne 0 ]; then
    echo ""
    echo "[ERROR] Failed to build $PLUGIN_NAME!"
    echo "Please check the error messages above."
    cd ../..
    exit 1
fi
echo "[SUCCESS] $PLUGIN_NAME built successfully!"
cd ../..

# Step 4: Deploy Plugin
echo ""
echo "[4/4] Deploying $PLUGIN_NAME..."

# Create Plugins directory if not exists
mkdir -p "subphimv1/bin/Debug/net8.0-windows/Plugins"
echo "Plugins directory ready"

# Copy plugin DLL
cp "VEO3_SDK_Examples/$PLUGIN_NAME/bin/Release/net8.0-windows/$PLUGIN_NAME.dll" "subphimv1/bin/Debug/net8.0-windows/Plugins/"
if [ $? -ne 0 ]; then
    echo "[ERROR] Failed to copy $PLUGIN_NAME.dll"
    echo "Source: VEO3_SDK_Examples/$PLUGIN_NAME/bin/Release/net8.0-windows/$PLUGIN_NAME.dll"
    exit 1
fi

echo ""
echo "========================================"
echo " BUILD COMPLETE!"
echo "========================================"
echo ""
echo "$PLUGIN_NAME.dll has been deployed to:"
echo "  subphimv1/bin/Debug/net8.0-windows/Plugins/"
echo ""
echo "You can now run VEO3 to test the plugin:"
echo "  cd subphimv1/bin/Debug/net8.0-windows"
echo "  ./subphimv1"
echo ""
