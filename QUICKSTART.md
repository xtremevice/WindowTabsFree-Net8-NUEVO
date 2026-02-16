# Quick Start Guide - How to Run WindowTabsFree from Terminal

This guide shows you how to run WindowTabsFree directly from your terminal.

## ⚠️ IMPORTANT: You Must Be in the Project Directory

All commands assume you're in the `WindowTabsFree` directory. Verify first:

```bash
pwd                    # Check where you are
cd WindowTabsFree     # Navigate to project (if needed)
git status            # Verify it's a git repository
ls src/               # You should see the projects
```

If you see errors like:
- `fatal: not a git repository` 
- `Project file does not exist`

→ **You're not in the correct directory.** Use `cd WindowTabsFree` first.

---

## Option 1: Run in Development Mode (Fastest)

### Prerequisites
- .NET 8 SDK installed
- Be in the project directory (see IMPORTANT section above)

### Steps

**Run the application directly:**
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

That's it! The application will open in a window.

---

## Option 2: Build and Run Standalone Executable (For Distribution)

### On Linux

1. **Build the standalone executable:**
```bash
chmod +x publish-linux.sh
./publish-linux.sh
```

2. **Run the executable:**
```bash
./publish/linux-x64/WindowTabsFree.UI
```

### On Windows

1. **Build the standalone executable:**
```powershell
dotnet publish src/WindowTabsFree.UI/WindowTabsFree.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64
```

2. **Run the executable:**
```powershell
.\publish\win-x64\WindowTabsFree.UI.exe
```

---

## Option 3: Just Build (Without Running)

```bash
dotnet build WindowTabsFreeNet.sln
```

Then run manually:
```bash
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj
```

---

## Verify .NET 8 is Installed

```bash
dotnet --version
```

You should see version 8.x.x. If you don't have .NET 8, download it from:
- https://dotnet.microsoft.com/download/dotnet/8.0

---

## Test Console (Optional)

If you just want to test that everything works without opening the UI:

```bash
dotnet run --project src/WindowTabsFree.TestConsole/WindowTabsFree.TestConsole.csproj
```

---

## Troubleshooting

### Error: ".NET SDK not found"
**Solution:** Install .NET 8 SDK from https://dotnet.microsoft.com/download

### Error on Linux: "X11 display not available"
**Solution:** Make sure you're running from a graphical environment with X11:
```bash
echo $DISPLAY
```
If it's empty, you need to be in a graphical session.

### Error: "libX11.so.6 not found" (Linux)
**Solution:** Install the X11 library:
```bash
# Ubuntu/Debian
sudo apt-get install libx11-6

# Fedora/RHEL
sudo dnf install libX11
```

---

## Most Commonly Used Commands Summary

```bash
# Run directly (development)
dotnet run --project src/WindowTabsFree.UI/WindowTabsFree.UI.csproj

# Build standalone executable for Linux
./publish-linux.sh
./publish/linux-x64/WindowTabsFree.UI

# Just build the project
dotnet build WindowTabsFreeNet.sln

# Check .NET version
dotnet --version
```

---

## More Information

- **Full README:** README_NET8.md
- **Technical Documentation:** IMPLEMENTATION_SUMMARY.md
- **Security:** SECURITY_SUMMARY.md
