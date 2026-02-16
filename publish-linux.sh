#!/bin/bash

# Build standalone executable for Linux
echo "Building WindowTabsFree for Linux (standalone)..."

cd "$(dirname "$0")"

# Clean previous builds
echo "Cleaning previous builds..."
rm -rf ./publish/linux-x64

# Publish the UI application
echo "Publishing UI application..."
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:PublishTrimmed=false \
    -o ./publish/linux-x64

if [ $? -eq 0 ]; then
    echo ""
    echo "✓ Build successful!"
    echo "Executable location: ./publish/linux-x64/WindowTabsFree.UI"
    echo ""
    echo "To run the application:"
    echo "  chmod +x ./publish/linux-x64/WindowTabsFree.UI"
    echo "  ./publish/linux-x64/WindowTabsFree.UI"
    echo ""
    echo "Requirements:"
    echo "  - X11 display server (DISPLAY environment variable must be set)"
    echo "  - libX11.so.6 library installed"
    echo ""
else
    echo "✗ Build failed!"
    exit 1
fi
